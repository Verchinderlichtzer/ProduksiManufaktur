namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Pekerjaan</summary>
    public interface IPekerjaanRepository
    {
        /// <summary>List Pekerjaan { Id, Nama, JumlahKaryawan } > PekerjaanList, KaryawanList</summary>
        Task<List<Pekerjaan>> Get();

        /// <summary>List Pekerjaan { Id, Nama } > PekerjaanForm</summary>
        Task<List<Pekerjaan>> Get1();

        /// <summary>Pekerjaan { Id, Nama } > PekerjaanForm</summary>
        Task<Pekerjaan> Find(int id);

        /// <summary>Pekerjaan { Id, Nama, List Karyawan { Id, Nama, Telepon, Email, Upah } } > PekerjaanInfo</summary>
        Task<Pekerjaan> Find1(int id);

        Task<Pekerjaan> Create(Pekerjaan pekerjaan);

        Task<Pekerjaan> Update(Pekerjaan pekerjaan);

        Task<bool> Deletable(int id);

        Task Delete(int id);
    }

    public class PekerjaanRepository : IPekerjaanRepository
    {
        private readonly AppDbContext _appDbContext;

        public PekerjaanRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Pekerjaan>> Get()
        {
            return await _appDbContext.Pekerjaan.Include(x => x.Karyawan).Select(x => new Pekerjaan
            {
                Id = x.Id,
                Nama = x.Nama,
                JumlahKaryawan = x.Karyawan!.Count
            }).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<Pekerjaan>> Get1()
        {
            return await _appDbContext.Pekerjaan.Select(x => new Pekerjaan { Id = x.Id, Nama = x.Nama }).ToListAsync();
        }

        public async Task<Pekerjaan> Find(int id)
        {
            return (await _appDbContext.Pekerjaan.FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public async Task<Pekerjaan> Find1(int id)
        {
            Pekerjaan pekerjaan = await _appDbContext.Pekerjaan.Include(x => x.Karyawan).FirstAsync(x => x.Id == id);
            return new Pekerjaan
            {
                Id = pekerjaan.Id,
                Nama = pekerjaan.Nama,
                Karyawan = pekerjaan.Karyawan!.ConvertAll(x => new Karyawan
                {
                    Id = x.Id,
                    Nama = x.Nama,
                    Telepon = x.Telepon,
                    Email = x.Email,
                    Upah = x.Upah
                })
            };
        }

        public async Task<Pekerjaan> Create(Pekerjaan pekerjaan)
        {
            Nullify(pekerjaan);

            var result = await _appDbContext.Pekerjaan.AddAsync(pekerjaan);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Pekerjaan> Update(Pekerjaan pekerjaan)
        {
            await _appDbContext.Pekerjaan.Where(x => x.Id == pekerjaan.Id).ExecuteUpdateAsync(y => y
                .SetProperty(z => z.Nama, pekerjaan.Nama));
            return pekerjaan;
        }

        public async Task<bool> Deletable(int id)
        {
            return await _appDbContext.Pekerjaan.AnyAsync(x => x.Id == id && !x.Karyawan!.Any());
        }

        public async Task Delete(int id)
        {
            await _appDbContext.Pekerjaan.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}