namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Customer</summary>
    public interface ICustomerRepository
    {
        /// <summary>List Customer > CustomerList</summary>
        Task<List<Customer>> Get();

        /// <summary>List Customer { Telepon, Fax, Email } > CustomerForm</summary>
        Task<List<Customer>> Get1();

        /// <summary>List Customer { Id, Nama } > PenjualanForm</summary>
        Task<List<Customer>> Get2();

        /// <summary>Customer > CustomerForm</summary>
        Task<Customer> Find(string id);

        Task<Customer> Create(Customer customer);

        Task<Customer> Update(Customer customer);

        Task<bool> Deletable(string id);

        Task Delete(string id);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _appDbContext;

        public CustomerRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Customer>> Get()
        {
            return await _appDbContext.Customer.OrderBy(x => x.Nama).ToListAsync();
        }

        public async Task<List<Customer>> Get1()
        {
            return await _appDbContext.Customer.Select(x => new Customer
            {
                Telepon = x.Telepon,
                Fax = x.Fax,
                Email = x.Email
            }).ToListAsync();
        }

        public async Task<List<Customer>> Get2()
        {
            return await _appDbContext.Customer.Select(x => new Customer
            {
                Id = x.Id,
                Nama = x.Nama
            }).ToListAsync();
        }

        public async Task<Customer> Find(string id)
        {
            return (await _appDbContext.Customer.FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public async Task<Customer> Create(Customer customer)
        {
            customer.Id = GenerateId(_appDbContext.Customer.Select(x => x.Id), 4, "CST");
            var result = await _appDbContext.Customer.AddAsync(customer);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Customer> Update(Customer customer)
        {
            await _appDbContext.Customer.Where(x => x.Id == customer.Id).ExecuteUpdateAsync(y => y
                .SetProperty(z => z.Nama, customer.Nama)
                .SetProperty(z => z.Alamat, customer.Alamat)
                .SetProperty(z => z.Telepon, customer.Telepon)
                .SetProperty(z => z.Fax, customer.Fax)
                .SetProperty(z => z.Email, customer.Email));
            return customer;
        }

        public async Task<bool> Deletable(string id)
        {
            return await _appDbContext.Customer.AnyAsync(x => x.Id == id && !x.Penjualan!.Any());
        }

        public async Task Delete(string id)
        {
            await _appDbContext.Customer.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}