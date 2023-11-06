namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Barang, CRUD PerubahanStokBarang</summary>
    public interface IBarangRepository
    {
        /// <summary>List Barang > BarangList, PerubahanStokBarangList, PenjualanForm, FormulasiForm, ProduksiForm</summary>
        Task<List<Barang>> Get();

        /// <summary>Barang > BarangForm</summary>
        Task<Barang> Find(string id);

        Task<Barang> Create(Barang barang);

        Task<Barang> Update(Barang barang);

        Task<bool> Deletable(string id);

        Task Delete(string id);

        /// <summary>List BarangSatuan { Id, BarangId, Nama, Ukuran, Harga, KonversiStok, Barang { Nama, SatuanProduksi, Stok, Version } } > PenjualanFormList</summary>
        Task<List<BarangSatuan>> GetBarangSatuan();

        /// <summary>List BarangSatuan > BarangList</summary>
        Task<List<BarangSatuan>> FindBarangSatuan(string barangId);

        Task<bool> DeletableBarangSatuan(int id);

        Task<bool> CekStokBarang();

        /// <summary>List PerubahanStokBarang { Id, Tanggal, Jenis, Jumlah, Keterangan, Barang { Nama } } > PerubahanStokBarangList</summary>
        Task<List<PerubahanStokBarang>> GetPerubahanStok();

        /// <summary>PerubahanStokBarang { Id, BarangId, InputTanggal, InputWaktu, Jenis, JenisSebelum, Jumlah, JumlahSebelum, Keterangan, Barang { Id, Nama, Stok, SatuanProduksi, Version } } > PerubahanStokBarangForm</summary>
        Task<PerubahanStokBarang> FindPerubahanStok(int id);

        Task<PerubahanStokBarang> CreatePerubahanStok(PerubahanStokBarang perubahanStokBarang);

        Task<PerubahanStokBarang> UpdatePerubahanStok(PerubahanStokBarang perubahanStokBarang);

        Task<bool> DeletablePerubahanStok(int id);

        Task DeletePerubahanStok(int id);
    }

    public class BarangRepository : IBarangRepository
    {
        private readonly AppDbContext _appDbContext;

        public BarangRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Barang>> Get()
        {
            return await _appDbContext.Barang.OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<Barang> Find(string id)
        {
            return (await _appDbContext.Barang.FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public async Task<Barang> Create(Barang barang)
        {
            barang.Id = GenerateId(_appDbContext.Barang.Select(x => x.Id), 4, "BRG");
            barang.SatuanProduksi = barang.SatuanProduksi.ToLower();
            barang.Stok = barang.StokAwal;

            List<BarangSatuan> barangSatuan = barang.BarangSatuan!;
            barangSatuan.ForEach(x => x.BarangId = barang.Id);
            var result = await _appDbContext.Barang.AddAsync(barang);
            await _appDbContext.BarangSatuan.AddRangeAsync(barangSatuan);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Barang> Update(Barang barang)
        {
            Barang model = await _appDbContext.Barang.Include(x => x.BarangSatuan).FirstAsync(x => x.Id == barang.Id && x.Version.SequenceEqual(barang.Version));
            model.Nama = barang.Nama;
            model.SatuanProduksi = barang.SatuanProduksi;
            model.StokMinimal = barang.StokMinimal;

            _appDbContext.BarangSatuan.RemoveRange(await _appDbContext.BarangSatuan.Where(x => x.BarangId == barang.Id).ToListAsync());
            await _appDbContext.BarangSatuan.AddRangeAsync(barang.BarangSatuan!);

            await _appDbContext.SaveChangesAsync();

            return barang;
        }

        public async Task<bool> Deletable(string id)
        {
            return await _appDbContext.Barang.AnyAsync(x => x.Id == id && !x.Formulasi!.Any() && !x.BarangSatuan!.Any(y => y.PenjualanDetail!.Any()) && !x.Produksi!.Any());
        }

        public async Task Delete(string id)
        {
            await _appDbContext.Barang.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<BarangSatuan>> GetBarangSatuan()
        {
            return await _appDbContext.BarangSatuan.Include(x => x.Barang).Where(x => x.Barang!.Stok > 0).Select(x => new BarangSatuan
            {
                Id = x.Id,
                BarangId = x.BarangId,
                Nama = x.Nama,
                Ukuran = x.Ukuran,
                Harga = x.Harga,
                KonversiStok = x.KonversiStok,
                Barang = new Barang
                {
                    Nama = x.Barang!.Nama,
                    SatuanProduksi = x.Barang!.SatuanProduksi,
                    Stok = x.Barang!.Stok,
                    Version = x.Barang!.Version
                }
            }).ToListAsync();
        }

        public async Task<List<BarangSatuan>> FindBarangSatuan(string barangId)
        {
            return await _appDbContext.BarangSatuan.Where(x => x.BarangId == barangId).ToListAsync();
        }

        public async Task<bool> DeletableBarangSatuan(int id)
        {
            return await _appDbContext.BarangSatuan.AnyAsync(x => x.Id == id && !x.PenjualanDetail!.Any());
        }

        public async Task<bool> CekStokBarang()
        {
            List<Barang> barang = await _appDbContext.Barang
                .Include(x => x.Produksi)
                .Include(x => x.PerubahanStokBarang)
                .Include(x => x.BarangSatuan!).ThenInclude(x => x.PenjualanDetail)
                .Include(x => x.BarangSatuan!).ThenInclude(x => x.ReturPenjualanDetail)
                .ToListAsync();

            var stokYgBenar = barang.ConvertAll(x => new Barang
            {
                Id = x.Id,
                Stok = x.StokAwal - x.BarangSatuan!.Sum(y => y.PenjualanDetail!.Sum(z => z.Jumlah * y.KonversiStok)) + x.Produksi!.Sum(y => y.Jumlah) + x.BarangSatuan!.Sum(y => y.ReturPenjualanDetail!.Sum(z => z.Jumlah * y.KonversiStok)) - x.PerubahanStokBarang!.Where(y => y.Jenis == "Pengurangan").Sum(y => y.Jumlah) + x.PerubahanStokBarang!.Where(y => y.Jenis == "Penambahan").Sum(y => y.Jumlah)
            });

            bool konsisten = true;
            foreach (var item in barang)
            {
                decimal stokBenar = stokYgBenar.First(x => x.Id == item.Id).Stok;
                if (item.Stok != stokBenar)
                {
                    item.Stok = stokBenar;
                    konsisten = false;
                    if (stokBenar < 0) throw new DbUpdateException();
                }
            }
            await _appDbContext.SaveChangesAsync();

            return konsisten;
        }

        public async Task<List<PerubahanStokBarang>> GetPerubahanStok()
        {
            return await _appDbContext.PerubahanStokBarang.Include(x => x.Barang).Select(x => new PerubahanStokBarang
            {
                Id = x.Id,
                Tanggal = x.Tanggal,
                Jenis = x.Jenis,
                Jumlah = x.Jumlah,
                Keterangan = x.Keterangan,
                Barang = new Barang { Nama = x.Barang!.Nama }
            }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<PerubahanStokBarang> FindPerubahanStok(int id)
        {
            PerubahanStokBarang perubahanStokBarang = (await _appDbContext.PerubahanStokBarang.Include(x => x.Barang).FirstOrDefaultAsync(x => x.Id == id))!;
            return perubahanStokBarang is null ? null! : new PerubahanStokBarang
            {
                Id = perubahanStokBarang.Id,
                BarangId = perubahanStokBarang.BarangId,
                InputTanggal = perubahanStokBarang.Tanggal.Date,
                InputWaktu = perubahanStokBarang.Tanggal.TimeOfDay,
                Jenis = perubahanStokBarang.Jenis,
                JenisSebelum = perubahanStokBarang.Jenis,
                Jumlah = perubahanStokBarang.Jumlah,
                JumlahSebelum = perubahanStokBarang.Jumlah,
                Keterangan = perubahanStokBarang.Keterangan,
                Barang = new Barang
                {
                    Id = perubahanStokBarang.Barang!.Id,
                    Nama = perubahanStokBarang.Barang!.Nama,
                    Stok = perubahanStokBarang.Barang!.Stok,
                    SatuanProduksi = perubahanStokBarang.Barang!.SatuanProduksi,
                    Version = perubahanStokBarang.Barang!.Version
                }
            };
        }

        public async Task<PerubahanStokBarang> CreatePerubahanStok(PerubahanStokBarang perubahanStokBarang)
        {
            perubahanStokBarang.Id = GenerateId(_appDbContext.PerubahanStokBarang.Select(x => x.Id));
            perubahanStokBarang.Tanggal = (DateTime)(perubahanStokBarang.InputTanggal + perubahanStokBarang.InputWaktu)!;

            var barang = await _appDbContext.Barang.FirstAsync(x => x.Id == perubahanStokBarang.BarangId && x.Version.SequenceEqual(perubahanStokBarang.Barang!.Version));
            if (perubahanStokBarang.Jenis == "Pengurangan")
                barang.Stok -= perubahanStokBarang.Jumlah;
            else
                barang.Stok += perubahanStokBarang.Jumlah;

            if (barang.Stok < 0) throw new DbUpdateException();

            Nullify(perubahanStokBarang);

            var result = await _appDbContext.PerubahanStokBarang.AddAsync(perubahanStokBarang);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<PerubahanStokBarang> UpdatePerubahanStok(PerubahanStokBarang perubahanStokBarang)
        {
            PerubahanStokBarang model = await _appDbContext.PerubahanStokBarang.FirstAsync(x => x.Id == perubahanStokBarang.Id);

            Barang barangLama = await _appDbContext.Barang.FirstAsync(x => x.Id == model.BarangId);
            Barang barangBaru = await _appDbContext.Barang.FirstAsync(x => x.Id == perubahanStokBarang.BarangId && x.Version.SequenceEqual(perubahanStokBarang.Barang!.Version));

            if (model.Jenis == "Pengurangan")
                barangLama.Stok += model.Jumlah;
            else
                barangLama.Stok -= model.Jumlah;

            if (perubahanStokBarang.Jenis == "Pengurangan")
                barangBaru.Stok -= perubahanStokBarang.Jumlah;
            else
                barangBaru.Stok += perubahanStokBarang.Jumlah;

            model.Jenis = perubahanStokBarang.Jenis;
            model.Jumlah = perubahanStokBarang.Jumlah;
            model.BarangId = perubahanStokBarang.BarangId;
            model.Tanggal = (DateTime)(perubahanStokBarang.InputTanggal + perubahanStokBarang.InputWaktu)!;
            model.Keterangan = perubahanStokBarang.Keterangan;

            if (barangLama.Stok < 0 || barangBaru.Stok < 0) throw new DbUpdateException();

            Nullify(perubahanStokBarang);

            await _appDbContext.SaveChangesAsync();

            return perubahanStokBarang;
        }

        public async Task<bool> DeletablePerubahanStok(int id)
        {
            return await _appDbContext.PerubahanStokBarang.AnyAsync(x => x.Id == id && (x.Jenis == "Pengurangan" || (x.Jenis == "Penambahan" && x.Jumlah <= x.Barang!.Stok)));
        }

        public async Task DeletePerubahanStok(int id)
        {
            var result = await _appDbContext.PerubahanStokBarang.FirstAsync(x => x.Id == id);
            var barang = await _appDbContext.Barang.FirstAsync(y => y.Id == result.BarangId);
            if (result.Jenis == "Pengurangan")
                barang.Stok += result.Jumlah;
            else
                barang.Stok -= result.Jumlah;

            if (barang.Stok < 0) throw new DbUpdateException();

            _appDbContext.PerubahanStokBarang.Remove(result);
            await _appDbContext.SaveChangesAsync();
        }
    }
}