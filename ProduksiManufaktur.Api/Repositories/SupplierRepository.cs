namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Supplier</summary>
    public interface ISupplierRepository
    {
        /// <summary>List Supplier > SupplierList</summary>
        Task<List<Supplier>> Get();

        /// <summary>List Supplier { Telepon, Fax, Email } > SupplierForm</summary>
        Task<List<Supplier>> Get1();

        /// <summary>List Supplier { Id, Nama } > PembelianForm</summary>
        Task<List<Supplier>> Get2();

        /// <summary>Supplier > SupplierForm</summary>
        Task<Supplier> Find(string id);

        Task<Supplier> Create(Supplier supplier);

        Task<Supplier> Update(Supplier supplier);

        Task<bool> Deletable(string id);

        Task Delete(string id);
    }

    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _appDbContext;

        public SupplierRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Supplier>> Get()
        {
            return await _appDbContext.Supplier.OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<Supplier>> Get1()
        {
            return await _appDbContext.Supplier.Select(x => new Supplier
            {
                Telepon = x.Telepon,
                Fax = x.Fax,
                Email = x.Email
            }).ToListAsync();
        }

        public async Task<List<Supplier>> Get2()
        {
            return await _appDbContext.Supplier.Select(x => new Supplier
            {
                Id = x.Id,
                Nama = x.Nama
            }).ToListAsync();
        }

        public async Task<Supplier> Find(string id)
        {
            return await _appDbContext.Supplier.FirstAsync(x => x.Id == id);
        }

        public async Task<Supplier> Create(Supplier supplier)
        {
            supplier.Id = GenerateId(_appDbContext.Supplier.Select(x => x.Id), 4, "SPL");
            var result = await _appDbContext.Supplier.AddAsync(supplier);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Supplier> Update(Supplier supplier)
        {
            await _appDbContext.Supplier.Where(x => x.Id == supplier.Id).ExecuteUpdateAsync(y => y
                .SetProperty(z => z.Nama, supplier.Nama)
                .SetProperty(z => z.Alamat, supplier.Alamat)
                .SetProperty(z => z.Telepon, supplier.Telepon)
                .SetProperty(z => z.Fax, supplier.Fax)
                .SetProperty(z => z.Email, supplier.Email));
            return supplier;
        }

        public async Task<bool> Deletable(string id)
        {
            return await _appDbContext.Supplier.AnyAsync(x => x.Id == id && !x.Pembelian!.Any());
        }

        public async Task Delete(string id)
        {
            await _appDbContext.Supplier.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}