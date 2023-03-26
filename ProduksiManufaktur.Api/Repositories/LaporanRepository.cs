namespace ProduksiManufaktur.Api.Repositories
{
    public interface ILaporanRepository
    {
        Task<List<EntitasDto>> GetBahan();

        Task<List<EntitasDto>> GetBarang();

        Task<List<EntitasDto>> GetKaryawan();

        Task<List<EntitasDto>> GetSupplier();

        Task<List<EntitasDto>> GetCustomer();

        Task<List<EntitasDto>> GetPembelian();

        Task<List<EntitasDto>> GetReturPembelian();

        Task<List<EntitasDto>> GetPenjualan();

        Task<List<EntitasDto>> GetReturPenjualan();

        Task<List<EntitasDto>> GetProduksi();

        Task<List<EntitasDto>> GetFormulasi();
    }

    public class LaporanRepository : ILaporanRepository
    {
        private readonly AppDbContext _appDbContext;

        public LaporanRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<EntitasDto>> GetBahan()
        {
            List<Bahan> bahan = await _appDbContext.Bahan.OrderBy(x => x.Nama).ToListAsync();
            return bahan.ConvertAll(x => new EntitasDto { Id = x.Id, Nama = $"{x.Nama} ({x.SatuanProduksi})" });
        }

        public async Task<List<EntitasDto>> GetBarang()
        {
            List<Barang> barang = await _appDbContext.Barang.OrderBy(x => x.Nama).ToListAsync();
            return barang.ConvertAll(x => new EntitasDto { Id = x.Id, Nama = $"{x.Nama} ({x.SatuanProduksi})" });
        }

        public async Task<List<EntitasDto>> GetKaryawan()
        {
            return await _appDbContext.Karyawan.Select(x => new EntitasDto { Id = x.Id, Nama = x.Nama }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<EntitasDto>> GetSupplier()
        {
            return await _appDbContext.Supplier.Select(x => new EntitasDto { Id = x.Id, Nama = x.Nama }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<EntitasDto>> GetCustomer()
        {
            return await _appDbContext.Customer.Select(x => new EntitasDto { Id = x.Id, Nama = x.Nama }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<EntitasDto>> GetPembelian()
        {
            List<Pembelian> pembelian = await _appDbContext.Pembelian.OrderByDescending(x => x.Id).ToListAsync();
            return pembelian.ConvertAll(x => new EntitasDto { Id = x.Id, Nama = $"{x.Id} - {x.Tanggal:dd/MM/yyyy HH:mm}" });
        }

        public async Task<List<EntitasDto>> GetReturPembelian()
        {
            List<ReturPembelian> returPembelian = await _appDbContext.ReturPembelian.OrderByDescending(x => x.Id).ToListAsync();
            return returPembelian.ConvertAll(x => new EntitasDto { Id = x.Id, Nama = $"{x.Id} - {x.Tanggal:dd/MM/yyyy HH:mm}" });
        }

        public async Task<List<EntitasDto>> GetPenjualan()
        {
            List<Penjualan> penjualan = await _appDbContext.Penjualan.OrderByDescending(x => x.Id).ToListAsync();
            return penjualan.ConvertAll(x => new EntitasDto { Id = x.Id, Nama = $"{x.Id} - {x.Tanggal:dd/MM/yyyy HH:mm}" });
        }

        public async Task<List<EntitasDto>> GetReturPenjualan()
        {
            List<ReturPenjualan> returPenjualan = await _appDbContext.ReturPenjualan.OrderByDescending(x => x.Id).ToListAsync();
            return returPenjualan.ConvertAll(x => new EntitasDto { Id = x.Id, Nama = $"{x.Id} - {x.Tanggal:dd/MM/yyyy HH:mm}" });
        }

        public async Task<List<EntitasDto>> GetProduksi()
        {
            List<Produksi> produksi = await _appDbContext.Produksi.OrderByDescending(x => x.Id).ToListAsync();
            return produksi.ConvertAll(x => new EntitasDto { Id = x.Id, Nama = $"{x.Id} - {x.Tanggal:dd/MM/yyyy HH:mm}" });
        }

        public async Task<List<EntitasDto>> GetFormulasi()
        {
            return await _appDbContext.Formulasi.Select(x => new EntitasDto { Id = x.Id, Nama = x.Id }).ToListAsync();
        }
    }
}