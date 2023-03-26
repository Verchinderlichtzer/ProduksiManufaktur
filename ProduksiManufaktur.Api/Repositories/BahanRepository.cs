namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Bahan, CRUD PerubahanStokBahan</summary>
    public interface IBahanRepository
    {
        /// <summary>List Bahan > BahanList, PerubahanStokBahanList, PembelianForm, FormulasiForm</summary>
        Task<List<Bahan>> Get();

        /// <summary>List Bahan { Id, Nama, SatuanProduksi, Stok, Version } > ProduksiForm</summary>
        Task<List<Bahan>> Get1();

        /// <summary>Bahan > BahanForm</summary>
        Task<Bahan> Find(string id);

        Task<Bahan> Create(Bahan bahan);

        Task<Bahan> Update(Bahan bahan);

        Task<bool> Deletable(string id);

        Task Delete(string id);

        /// <summary>List BahanSatuan { Id, BahanId, Nama, Ukuran, Harga, KonversiStok, Bahan { Nama, SatuanProduksi, Stok, Version } } > PembelianFormList</summary>
        Task<List<BahanSatuan>> GetBahanSatuan();

        /// <summary>List BahanSatuan > BahanList</summary>
        Task<List<BahanSatuan>> FindBahanSatuan(string bahanId);

        Task<bool> DeletableBahanSatuan(int id);

        /// <summary>List PerubahanStokBahan { Id, Tanggal, Jenis, Jumlah, Keterangan, Bahan { Nama } } > PerubahanStokBahanList</summary>
        Task<List<PerubahanStokBahan>> GetPerubahanStok();

        /// <summary>PerubahanStokBahan { Id, BahanId, InputTanggal, InputWaktu, Jenis, JenisSebelum, Jumlah, JumlahSebelum, Keterangan, Bahan { Id, Nama, Stok, SatuanProduksi, Version } } > PerubahanStokBahanForm</summary>
        Task<PerubahanStokBahan> FindPerubahanStok(int id);

        Task<PerubahanStokBahan> CreatePerubahanStok(PerubahanStokBahan perubahanStokBahan);

        Task<PerubahanStokBahan> UpdatePerubahanStok(PerubahanStokBahan perubahanStokBahan);

        Task<bool> DeletablePerubahanStok(int id);

        Task DeletePerubahanStok(int id);
    }

    public class BahanRepository : IBahanRepository
    {
        private readonly AppDbContext _appDbContext;

        public BahanRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Bahan>> Get()
        {
            return await _appDbContext.Bahan.OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<Bahan>> Get1()
        {
            return await _appDbContext.Bahan.Where(x => x.Stok > 0).Select(x => new Bahan
            {
                Id = x.Id,
                Nama = x.Nama,
                SatuanProduksi = x.SatuanProduksi,
                Stok = x.Stok,
                Version = x.Version
            }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<Bahan> Find(string id)
        {
            return (await _appDbContext.Bahan.FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public async Task<Bahan> Create(Bahan bahan)
        {
            bahan.Id = GenerateId(_appDbContext.Bahan.Select(x => x.Id), 4, "BHN");
            bahan.SatuanProduksi = bahan.SatuanProduksi.ToLower();
            bahan.Stok = bahan.StokAwal;

            List<BahanSatuan> bahanSatuan = bahan.BahanSatuan!;
            bahanSatuan.ForEach(x => x.BahanId = bahan.Id);
            var result = await _appDbContext.Bahan.AddAsync(bahan);
            await _appDbContext.BahanSatuan.AddRangeAsync(bahanSatuan);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Bahan> Update(Bahan bahan)
        {
            Bahan model = await _appDbContext.Bahan.Include(x => x.BahanSatuan).FirstAsync(x => x.Id == bahan.Id && x.Version.SequenceEqual(bahan.Version));
            model.Nama = bahan.Nama;
            model.SatuanProduksi = bahan.SatuanProduksi;
            model.StokMinimal = bahan.StokMinimal;

            _appDbContext.BahanSatuan.RemoveRange(await _appDbContext.BahanSatuan.Where(x => x.BahanId == bahan.Id).ToListAsync());
            await _appDbContext.BahanSatuan.AddRangeAsync(bahan.BahanSatuan!);

            await _appDbContext.SaveChangesAsync();

            return bahan;
        }

        public async Task<bool> Deletable(string id)
        {
            return await _appDbContext.Bahan.AnyAsync(x => x.Id == id && !x.FormulasiDetail!.Any() && !x.BahanSatuan!.Any(y => y.PembelianDetail!.Any()) && !x.ProduksiDetailBahan!.Any());
        }

        public async Task Delete(string id)
        {
            await _appDbContext.Bahan.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<BahanSatuan>> GetBahanSatuan()
        {
            return await _appDbContext.BahanSatuan.Include(x => x.Bahan).Select(x => new BahanSatuan
            {
                Id = x.Id,
                BahanId = x.BahanId,
                Nama = x.Nama,
                Ukuran = x.Ukuran,
                Harga = x.Harga,
                KonversiStok = x.KonversiStok,
                Bahan = new Bahan
                {
                    Nama = x.Bahan!.Nama,
                    SatuanProduksi = x.Bahan!.SatuanProduksi,
                    Stok = x.Bahan!.Stok,
                    Version = x.Bahan!.Version
                }
            }).ToListAsync();
        }

        public async Task<List<BahanSatuan>> FindBahanSatuan(string bahanId)
        {
            return await _appDbContext.BahanSatuan.Where(x => x.BahanId == bahanId).ToListAsync();
        }

        public async Task<bool> DeletableBahanSatuan(int id)
        {
            return await _appDbContext.BahanSatuan.AnyAsync(x => x.Id == id && !x.PembelianDetail!.Any());
        }

        public async Task<List<PerubahanStokBahan>> GetPerubahanStok()
        {
            return await _appDbContext.PerubahanStokBahan.Include(x => x.Bahan).Select(x => new PerubahanStokBahan
            {
                Id = x.Id,
                Tanggal = x.Tanggal,
                Jenis = x.Jenis,
                Jumlah = x.Jumlah,
                Keterangan = x.Keterangan,
                Bahan = new Bahan { Nama = x.Bahan!.Nama }
            }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<PerubahanStokBahan> FindPerubahanStok(int id)
        {
            PerubahanStokBahan perubahanStokBahan = (await _appDbContext.PerubahanStokBahan.Include(x => x.Bahan).FirstOrDefaultAsync(x => x.Id == id))!;
            return perubahanStokBahan is null ? null! : new PerubahanStokBahan
            {
                Id = perubahanStokBahan.Id,
                BahanId = perubahanStokBahan.BahanId,
                InputTanggal = perubahanStokBahan.Tanggal.Date,
                InputWaktu = perubahanStokBahan.Tanggal.TimeOfDay,
                Jenis = perubahanStokBahan.Jenis,
                JenisSebelum = perubahanStokBahan.Jenis,
                Jumlah = perubahanStokBahan.Jumlah,
                JumlahSebelum = perubahanStokBahan.Jumlah,
                Keterangan = perubahanStokBahan.Keterangan,
                Bahan = new Bahan
                {
                    Id = perubahanStokBahan.Bahan!.Id,
                    Nama = perubahanStokBahan.Bahan!.Nama,
                    Stok = perubahanStokBahan.Bahan!.Stok,
                    SatuanProduksi = perubahanStokBahan.Bahan!.SatuanProduksi,
                    Version = perubahanStokBahan.Bahan!.Version
                }
            };
        }

        public async Task<PerubahanStokBahan> CreatePerubahanStok(PerubahanStokBahan perubahanStokBahan)
        {
            perubahanStokBahan.Id = GenerateId(_appDbContext.PerubahanStokBahan.Select(x => x.Id));
            perubahanStokBahan.Tanggal = (DateTime)(perubahanStokBahan.InputTanggal + perubahanStokBahan.InputWaktu)!;

            var bahan = await _appDbContext.Bahan.FirstAsync(x => x.Id == perubahanStokBahan.BahanId && x.Version.SequenceEqual(perubahanStokBahan.Bahan!.Version));
            if (perubahanStokBahan.Jenis == "Pengurangan")
                bahan.Stok -= perubahanStokBahan.Jumlah;
            else
                bahan.Stok += perubahanStokBahan.Jumlah;

            if (bahan.Stok < 0) throw new DbUpdateException();

            Nullify(perubahanStokBahan);

            var result = await _appDbContext.PerubahanStokBahan.AddAsync(perubahanStokBahan);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<PerubahanStokBahan> UpdatePerubahanStok(PerubahanStokBahan perubahanStokBahan)
        {
            PerubahanStokBahan model = await _appDbContext.PerubahanStokBahan.FirstAsync(x => x.Id == perubahanStokBahan.Id);

            Bahan bahanLama = await _appDbContext.Bahan.FirstAsync(x => x.Id == model.BahanId);
            Bahan bahanBaru = await _appDbContext.Bahan.FirstAsync(x => x.Id == perubahanStokBahan.BahanId && x.Version.SequenceEqual(perubahanStokBahan.Bahan!.Version));

            if (model.Jenis == "Pengurangan")
                bahanLama.Stok += model.Jumlah;
            else
                bahanLama.Stok -= model.Jumlah;

            if (perubahanStokBahan.Jenis == "Pengurangan")
                bahanBaru.Stok -= perubahanStokBahan.Jumlah;
            else
                bahanBaru.Stok += perubahanStokBahan.Jumlah;

            model.Jenis = perubahanStokBahan.Jenis;
            model.Jumlah = perubahanStokBahan.Jumlah;
            model.BahanId = perubahanStokBahan.BahanId;
            model.Tanggal = (DateTime)(perubahanStokBahan.InputTanggal + perubahanStokBahan.InputWaktu)!;
            model.Keterangan = perubahanStokBahan.Keterangan;

            if (bahanLama.Stok < 0 || bahanBaru.Stok < 0) throw new DbUpdateException();

            Nullify(perubahanStokBahan);

            await _appDbContext.SaveChangesAsync();

            return perubahanStokBahan;
        }

        public async Task<bool> DeletablePerubahanStok(int id)
        {
            return await _appDbContext.PerubahanStokBahan.AnyAsync(x => x.Id == id && (x.Jenis == "Pengurangan" || (x.Jenis == "Penambahan" && x.Jumlah <= x.Bahan!.Stok)));
        }

        public async Task DeletePerubahanStok(int id)
        {
            var result = await _appDbContext.PerubahanStokBahan.FirstAsync(x => x.Id == id);
            var bahan = await _appDbContext.Bahan.FirstAsync(y => y.Id == result.BahanId);
            if (result.Jenis == "Pengurangan")
                bahan.Stok += result.Jumlah;
            else
                bahan.Stok -= result.Jumlah;

            if (bahan.Stok < 0) throw new DbUpdateException();

            _appDbContext.PerubahanStokBahan.Remove(result);
            await _appDbContext.SaveChangesAsync();
        }
    }
}