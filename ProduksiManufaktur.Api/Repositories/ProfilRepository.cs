namespace ProduksiManufaktur.Api.Repositories
{
    public interface IProfilRepository
    {
        Task<Profil> Get();

        Task<Profil> Update(Profil profil);
    }

    public class ProfilRepository : IProfilRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProfilRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Profil> Get()
        {
            return (await _appDbContext.Profil.ToArrayAsync())[0];
        }

        public async Task<Profil> Update(Profil profil)
        {
            await _appDbContext.Profil.Where(x => x.Id == profil.Id).ExecuteUpdateAsync(y => y
                .SetProperty(z => z.Nama, profil.Nama)
                .SetProperty(z => z.Alamat, profil.Alamat)
                .SetProperty(z => z.Telepon, profil.Telepon)
                .SetProperty(z => z.Fax, profil.Fax)
                .SetProperty(z => z.Email, profil.Email)
                .SetProperty(z => z.Website, profil.Website)
                .SetProperty(z => z.Pengurus, profil.Pengurus)
                .SetProperty(z => z.Jabatan, profil.Jabatan)
                .SetProperty(z => z.Logo, profil.Logo));
            return profil;
        }
    }
}