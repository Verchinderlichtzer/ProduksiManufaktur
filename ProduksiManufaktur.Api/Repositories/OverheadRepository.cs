namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Overhead</summary>
    public interface IOverheadRepository
    {
        /// <summary>List Overhead > OverheadList, OverheadForm, ProduksiForm</summary>
        Task<List<Overhead>> Get();

        /// <summary>Overhead > OverheadForm</summary>
        Task<Overhead> Find(int id);

        Task<Overhead> Create(Overhead overhead);

        Task<Overhead> Update(Overhead overhead);

        Task<bool> Deletable(int id);

        Task Delete(int id);
    }

    public class OverheadRepository : IOverheadRepository
    {
        private readonly AppDbContext _appDbContext;

        public OverheadRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Overhead>> Get()
        {
            return await _appDbContext.Overhead.OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<Overhead> Find(int id)
        {
            return (await _appDbContext.Overhead.FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public async Task<Overhead> Create(Overhead overhead)
        {
            var result = await _appDbContext.Overhead.AddAsync(overhead);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Overhead> Update(Overhead overhead)
        {
            await _appDbContext.Overhead.Where(x => x.Id == overhead.Id).ExecuteUpdateAsync(y => y
                .SetProperty(z => z.Nama, overhead.Nama));
            return overhead;
        }

        public async Task<bool> Deletable(int id)
        {
            return await _appDbContext.Overhead.AnyAsync(x => x.Id == id && !x.ProduksiDetailOverhead!.Any());
        }

        public async Task Delete(int id)
        {
            await _appDbContext.Overhead.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}