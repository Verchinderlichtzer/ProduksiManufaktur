namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Formulasi, R FormulasiDetail</summary>
    public interface IFormulasiRepository
    {
        /// <summary>List Formulasi { Id, Jumlah, Barang { Nama, SatuanProduksi } } > FormulasiList</summary>
        Task<List<Formulasi>> Get();

        /// <summary>Formulasi { Id, BarangId, Jumlah, Barang { Nama, SatuanProduksi }, List FormulasiDetail { FormulasiId, BahanId, Jumlah, Bahan { Nama, SatuanProduksi } } } > FormulasiForm</summary>
        Task<Formulasi> Find(string id);

        /// <summary>List Formulasi { Id, Jumlah, Barang { SatuanProduksi } } } > ProduksiForm</summary>
        Task<List<Formulasi>> Find1(string barangId);

        /// <summary>Formulasi { Jumlah, List FormulasiDetail { Jumlah, Bahan { Id, Nama, Stok, SatuanProduksi, Version } } } > ProduksiForm</summary>
        Task<Formulasi> Find2(string id);

        Task<Formulasi> Create(Formulasi formulasi);

        Task<Formulasi> Update(Formulasi formulasi);

        Task Delete(string id);

        Task<List<FormulasiDetail>> GetDetail();
    }

    public class FormulasiRepository : IFormulasiRepository
    {
        private readonly AppDbContext _appDbContext;

        public FormulasiRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Formulasi>> Get()
        {
            return await _appDbContext.Formulasi.Include(x => x.Barang).Select(x => new Formulasi
            {
                Id = x.Id,
                Jumlah = x.Jumlah,
                Barang = new Barang { Nama = x.Barang!.Nama, SatuanProduksi = x.Barang!.SatuanProduksi }
            }).ToListAsync();
        }

        public async Task<Formulasi> Find(string id)
        {
            Formulasi formulasi = (await _appDbContext.Formulasi.Include(x => x.Barang!).Include(x => x.FormulasiDetail!).ThenInclude(x => x.Bahan!).FirstOrDefaultAsync(x => x.Id == id))!;
            return formulasi is null ? null! : new Formulasi
            {
                Id = formulasi.Id,
                BarangId = formulasi.BarangId,
                Jumlah = formulasi.Jumlah,
                Barang = new Barang
                {
                    Nama = formulasi.Barang!.Nama,
                    SatuanProduksi = formulasi.Barang!.SatuanProduksi
                },
                FormulasiDetail = formulasi.FormulasiDetail!.ConvertAll(x => new FormulasiDetail
                {
                    FormulasiId = x.FormulasiId,
                    BahanId = x.BahanId,
                    Jumlah = x.Jumlah,
                    Bahan = new Bahan
                    {
                        Nama = x.Bahan!.Nama,
                        SatuanProduksi = x.Bahan!.SatuanProduksi
                    }
                })
            };
        }

        public async Task<List<Formulasi>> Find1(string barangId)
        {
            return await _appDbContext.Formulasi.Include(x => x.Barang!).Where(x => x.BarangId == barangId).Select(x => new Formulasi { Id = x.Id, Jumlah = x.Jumlah, Barang = new Barang { SatuanProduksi = x.Barang!.SatuanProduksi } }).ToListAsync();
        }

        public async Task<Formulasi> Find2(string id)
        {
            Formulasi formulasi = await _appDbContext.Formulasi.Include(x => x.FormulasiDetail!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == id);
            return new Formulasi
            {
                Jumlah = formulasi.Jumlah,
                FormulasiDetail = formulasi.FormulasiDetail!.ConvertAll(x => new FormulasiDetail
                {
                    Jumlah = x.Jumlah,
                    Bahan = new Bahan
                    {
                        Id = x.Bahan!.Id,
                        Nama = x.Bahan!.Nama,
                        Stok = x.Bahan!.Stok,
                        SatuanProduksi = x.Bahan!.SatuanProduksi,
                        Version = x.Bahan!.Version
                    }
                })
            };
        }

        public async Task<Formulasi> Create(Formulasi formulasi)
        {
            var formulasiDetail = Nullifies(formulasi.FormulasiDetail!);
            Nullify(formulasi);

            formulasi.Id = GenerateId(_appDbContext.Formulasi.Select(x => x.Id), 4, "F");

            var idsDetail = GenerateId(_appDbContext.FormulasiDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.FormulasiDetail.Select(x => x.Id), formulasiDetail);

            for (int i = 0; i < formulasiDetail.Count; i++)
            {
                formulasiDetail[i].Id = idsDetail[i];
                formulasiDetail[i].FormulasiId = formulasi.Id;
            }

            var result = await _appDbContext.Formulasi.AddAsync(formulasi);
            await _appDbContext.FormulasiDetail.AddRangeAsync(formulasiDetail);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Formulasi> Update(Formulasi formulasi)
        {
            Formulasi model = await _appDbContext.Formulasi.Include(x => x.FormulasiDetail!).FirstAsync(x => x.Id == formulasi.Id);

            model.BarangId = formulasi.BarangId;
            model.Jumlah = formulasi.Jumlah;

            _appDbContext.FormulasiDetail.RemoveRange(await _appDbContext.FormulasiDetail.Where(x => x.FormulasiId == formulasi.Id).ToListAsync());

            var idsDetail = GenerateId(_appDbContext.FormulasiDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.FormulasiDetail.Select(x => x.Id), formulasi.FormulasiDetail!);
            for (int i = 0; i < formulasi.FormulasiDetail!.Count; i++) formulasi.FormulasiDetail[i].Id = idsDetail[i];

            var formulasiDetail = Nullifies(formulasi.FormulasiDetail!);

            await _appDbContext.FormulasiDetail.AddRangeAsync(formulasiDetail);
            await _appDbContext.SaveChangesAsync();

            return formulasi;
        }

        public async Task Delete(string id)
        {
            await _appDbContext.Formulasi.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<FormulasiDetail>> GetDetail()
        {
            return await _appDbContext.FormulasiDetail.ToListAsync();
        }
    }
}