namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>R Role, CRU Claim</summary>
    public interface IRoleRepository
    {
        /// <summary>List Role { UserRole } > UserList</summary>
        Task<List<Role>> Get();

        /// <summary>List Role { UserRole , RoleClaim } > RoleList</summary>
        Task<List<Role>> Get1();

        Task<Role> Find(string id);

        Task<Role> Update(Role role);

        Task<List<RoleClaim>> GetClaim();

        Task<List<RoleClaim>> GetClaim1(List<string> roleIds);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _appDbContext;

        public RoleRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Role>> Get()
        {
            return await _appDbContext.Roles.Include(x => x.UserRole).ToListAsync();
        }

        public async Task<List<Role>> Get1()
        {
            return await _appDbContext.Roles.Include(x => x.UserRole).Include(x => x.RoleClaim).ToListAsync();
        }

        public async Task<Role> Find(string id)
        {
            return (await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Id == id || x.Name == id))!;
        }

        public async Task<Role> Update(Role role)
        {
            var rowsAffected = await _appDbContext.Roles.Where(x => x.Id == role.Id).ExecuteUpdateAsync(y => y.SetProperty(z => z.Name, role.Name));

            if (rowsAffected == 0) throw new DbUpdateConcurrencyException();

            foreach (var x in role.RoleClaim!)
            {
                var rowsAffecteds = await _appDbContext.RoleClaims.Where(y => y.Id == x.Id && y.ClaimType == x.ClaimType).ExecuteUpdateAsync(y => y.SetProperty(z => z.ClaimValue, x.ClaimValue));
                if (rowsAffecteds == 0) throw new DbUpdateConcurrencyException();
            }

            return role;
        }

        public async Task<List<RoleClaim>> GetClaim()
        {
            return await _appDbContext.RoleClaims.ToListAsync();
        }

        public async Task<List<RoleClaim>> GetClaim1(List<string> roleIds)
        {
            List<RoleClaim> roleClaim = new();
            foreach (var x in roleIds)
            {
                roleClaim.AddRange(await _appDbContext.RoleClaims.Where(y => y.RoleId == x).ToListAsync());
            }
            return roleClaim;
        }
    }
}