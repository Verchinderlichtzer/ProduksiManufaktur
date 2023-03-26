namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Karyawan</summary>
    public interface IKaryawanRepository
    {
        /// <summary>List Karyawan { Id, PekerjaanId, Nama, TempatLahir, TanggalLahir, Alamat, Telepon, Email, Upah, Pekerjaan { Id, Nama } } > KaryawanList</summary>
        Task<List<Karyawan>> Get();

        /// <summary>List Karyawan { Telepon, Email } > KaryawanForm</summary>
        Task<List<Karyawan>> Get1();

        /// <summary>List Karyawan { Id, Nama, Upah, Pekerjaan { Nama } } > ProduksiForm</summary>
        Task<List<Karyawan>> Get2();

        /// <summary>Karyawan { Id, PekerjaanId, Nama, TempatLahir, TanggalLahir, Alamat, Telepon, Email, Upah, Pekerjaan { Id, Nama } } > KaryawanForm</summary>
        Task<Karyawan> Find(string id);

        Task<Karyawan> Create(Karyawan karyawan);

        Task<Karyawan> Update(Karyawan karyawan);

        Task<bool> Deletable(string id);

        Task Delete(string id);
    }

    public class KaryawanRepository : IKaryawanRepository
    {
        private readonly AppDbContext _appDbContext;

        public KaryawanRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Karyawan>> Get()
        {
            return await _appDbContext.Karyawan.Include(x => x.Pekerjaan).OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<Karyawan>> Get1()
        {
            return await _appDbContext.Karyawan.Select(x => new Karyawan { Telepon = x.Telepon, Email = x.Email }).ToListAsync();
        }

        public async Task<List<Karyawan>> Get2()
        {
            return await _appDbContext.Karyawan.Select(x => new Karyawan { Id = x.Id, Nama = x.Nama, Upah = x.Upah, Pekerjaan = new Pekerjaan { Nama = x.Pekerjaan!.Nama } }).ToListAsync();
        }

        public async Task<Karyawan> Find(string id)
        {
            return (await _appDbContext.Karyawan.Include(x => x.Pekerjaan).FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public async Task<Karyawan> Create(Karyawan karyawan)
        {
            Nullify(karyawan);

            karyawan.Id = GenerateId(_appDbContext.Karyawan.Select(x => x.Id), 4, "KYN");
            karyawan.TanggalLahir = (DateTime)karyawan.InputTanggalLahir!;

            var result = await _appDbContext.Karyawan.AddAsync(karyawan);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Karyawan> Update(Karyawan karyawan)
        {
            await _appDbContext.Karyawan.Where(x => x.Id == karyawan.Id).ExecuteUpdateAsync(y => y
                .SetProperty(z => z.Nama, karyawan.Nama)
                .SetProperty(z => z.TempatLahir, karyawan.TempatLahir)
                .SetProperty(z => z.TanggalLahir, (DateTime)karyawan.InputTanggalLahir!)
                .SetProperty(z => z.Alamat, karyawan.Alamat)
                .SetProperty(z => z.Telepon, karyawan.Telepon)
                .SetProperty(z => z.Email, karyawan.Email)
                .SetProperty(z => z.PekerjaanId, karyawan.PekerjaanId)
                .SetProperty(z => z.Upah, karyawan.Upah));

            return karyawan;
        }

        public async Task<bool> Deletable(string id)
        {
            return await _appDbContext.Karyawan.AnyAsync(x => x.Id == id && !x.ProduksiDetailJasa!.Any());
        }

        public async Task Delete(string id)
        {
            await _appDbContext.Karyawan.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}