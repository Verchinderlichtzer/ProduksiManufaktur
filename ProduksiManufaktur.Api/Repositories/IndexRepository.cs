namespace ProduksiManufaktur.Api.Repositories
{
    public interface IIndexRepository
    {
        /// <summary>List JumlahItemDto { Nama, Jumlah, Satuan } > Index</summary>
        Task<List<JumlahItemDto>> GetJumlahPakai();

        /// <summary>List JumlahItemDto { Nama, Jumlah, Satuan, Total } > Index</summary>
        Task<List<JumlahItemDto>> GetJumlahBeli();

        /// <summary>List JumlahItemDto { Nama, Jumlah, Satuan } > Index</summary>
        Task<List<JumlahItemDto>> GetJumlahProduksi();

        /// <summary>List JumlahItemDto { Nama, Jumlah, Satuan, Total } > Index</summary>
        Task<List<JumlahItemDto>> GetJumlahJual();

        /// <summary>List GrafikDto { Tanggal, Nominal } > Index</summary>
        Task<List<GrafikDto>> GetPengeluaran();

        /// <summary>List GrafikDto { Tanggal, Nominal } > Index</summary>
        Task<List<GrafikDto>> GetPendapatan();

        /// <summary>List Bahan { Nama, SatuanProduksi, Stok, StokMinimal } > Index</summary>
        Task<List<Bahan>> GetStokBahanMinim();

        /// <summary>List Barang { Nama, SatuanProduksi, Stok, StokMinimal } > Index</summary>
        Task<List<Barang>> GetStokBarangMinim();

        /// <summary>List Pembelian { Id, Sisa, HariJatuhTempo, JatuhTempo } > Index</summary>
        Task<List<Pembelian>> GetUtang();

        /// <summary>List Penjualan { Id, Sisa, HariJatuhTempo, JatuhTempo } > Index</summary>
        Task<List<Penjualan>> GetPiutang();

        /// <summary>List BarangPopulerDto { Label, Data } > Index</summary>
        Task<List<BarangPopulerDto>> GetBarangPopuler();
    }

    public class IndexRepository : IIndexRepository
    {
        private readonly AppDbContext _appDbContext;

        public IndexRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<JumlahItemDto>> GetJumlahPakai()
        {
            return await _appDbContext.Bahan.Include(x => x.ProduksiDetailBahan).Where(x => x.ProduksiDetailBahan!.Any(y => y.Jumlah > 0)).Select(x => new JumlahItemDto
            {
                Nama = x.Nama,
                Jumlah = x.ProduksiDetailBahan!.Sum(y => y.Jumlah),
                Satuan = x.SatuanProduksi
            }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<JumlahItemDto>> GetJumlahBeli()
        {
            var result = await _appDbContext.PembelianDetail.Include(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).ToListAsync();

            return result.GroupBy(x => new { x.BahanSatuan, x.BahanSatuan!.Bahan })
                .Select(g => new JumlahItemDto
                {
                    Nama = g.Key.Bahan!.Nama,
                    Jumlah = g.Sum(x => x.Jumlah),
                    Satuan = string.IsNullOrEmpty(g.Key.BahanSatuan!.Ukuran) ? g.Key.BahanSatuan!.Nama : $"{g.Key.BahanSatuan!.Nama} ({g.Key.BahanSatuan!.Ukuran})",
                    Total = (int)(g.Sum(x => x.Jumlah * x.Harga))
                }).OrderBy(x => x.Nama).ToList();
        }

        public async Task<List<JumlahItemDto>> GetJumlahProduksi()
        {
            return await _appDbContext.Barang.Include(x => x.Produksi).Where(x => x.Produksi!.Any(y => y.Jumlah > 0)).Select(x => new JumlahItemDto
            {
                Nama = x.Nama,
                Jumlah = x.Produksi!.Sum(y => y.Jumlah),
                Satuan = x.SatuanProduksi
            }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<JumlahItemDto>> GetJumlahJual()
        {
            var result = await _appDbContext.PenjualanDetail.Include(x => x.BarangSatuan!).ThenInclude(x => x.Barang).ToListAsync();

            return result.GroupBy(x => new { x.BarangSatuan, x.BarangSatuan!.Barang })
                .Select(g => new JumlahItemDto
                {
                    Nama = g.Key.Barang!.Nama,
                    Jumlah = g.Sum(x => x.Jumlah),
                    Satuan = string.IsNullOrEmpty(g.Key.BarangSatuan!.Ukuran) ? g.Key.BarangSatuan!.Nama : $"{g.Key.BarangSatuan!.Nama} ({g.Key.BarangSatuan!.Ukuran})",
                    Total = (int)(g.Sum(x => x.Jumlah * x.Harga))
                }).OrderBy(x => x.Nama).ToList();
        }

        public async Task<List<GrafikDto>> GetPengeluaran()
        {
            return await _appDbContext.TransaksiPembelian.Select(x => new GrafikDto
            {
                Tanggal = x.Tanggal,
                Nominal = x.Nominal
            }).ToListAsync();
        }

        public async Task<List<GrafikDto>> GetPendapatan()
        {
            return await _appDbContext.TransaksiPenjualan.Select(x => new GrafikDto
            {
                Tanggal = x.Tanggal,
                Nominal = x.Nominal
            }).ToListAsync();
        }

        public async Task<List<Bahan>> GetStokBahanMinim()
        {
            return await _appDbContext.Bahan.Where(x => x.Stok <= x.StokMinimal).Select(x => new Bahan
            {
                Nama = x.Nama,
                SatuanProduksi = x.SatuanProduksi,
                Stok = x.Stok,
                StokMinimal = x.StokMinimal
            }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<Barang>> GetStokBarangMinim()
        {
            return await _appDbContext.Barang.Where(x => x.Stok <= x.StokMinimal).Select(x => new Barang
            {
                Nama = x.Nama,
                SatuanProduksi = x.SatuanProduksi,
                Stok = x.Stok,
                StokMinimal = x.StokMinimal
            }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<Pembelian>> GetUtang()
        {
            return await _appDbContext.Pembelian.Where(x => x.Status == "Belum Lunas").Select(x => new Pembelian
            {
                Id = x.Id,
                Sisa = (int)(x.Subtotal * ((x.PPN + 100) / 100m)) - x.Terbayar,
                HariJatuhTempo = ((TimeSpan)(x.JatuhTempo - DateTime.Today)!).Days,
                JatuhTempo = x.JatuhTempo
            }).OrderBy(x => x.JatuhTempo).ToListAsync();
        }

        public async Task<List<Penjualan>> GetPiutang()
        {
            return await _appDbContext.Penjualan.Where(x => x.Status == "Belum Lunas").Select(x => new Penjualan
            {
                Id = x.Id,
                Sisa = (int)(x.Subtotal * ((x.PPN + 100) / 100m)) - x.Terbayar,
                HariJatuhTempo = ((TimeSpan)(x.JatuhTempo - DateTime.Today)!).Days,
                JatuhTempo = x.JatuhTempo
            }).OrderBy(x => x.JatuhTempo).ToListAsync();
        }

        public async Task<List<BarangPopulerDto>> GetBarangPopuler()
        {
            var result = await _appDbContext.PenjualanDetail.Include(x => x.Penjualan).Include(x => x.BarangSatuan!).ThenInclude(x => x.Barang).Where(x => x.Penjualan!.Tanggal >= DateTime.Now.AddDays(-14).Date).ToListAsync();

            return result.GroupBy(x => x.BarangSatuan!.Barang)
                .Select(g => new BarangPopulerDto
                {
                    Label = g.Key!.Nama,
                    Data = (double)g.Sum(x => x.Jumlah)
                }).OrderByDescending(x => x.Data).Take(5).ToList();
        }
    }
}