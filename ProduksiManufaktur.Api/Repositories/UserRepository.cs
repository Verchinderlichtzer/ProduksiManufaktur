namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>RU User, CRU UserClaim, CR LogTransaksi</summary>
    public interface IUserRepository
    {
        /// <summary>List User { Id, Email, PhoneNumber, Alamat, TempatLahir, TanggalLahir, Roles, EmailConfirmed, SecurityStamp, ConcurrencyStamp } > UserListBase</summary>
        Task<List<User>> Get();

        /// <summary>User { Id, UserName, Email, PhoneNumber, Alamat, TempatLahir, TanggalLahir, EmailConfirmed, SecurityStamp, ConcurrencyStamp, PasswordHash, List UserRole { Role { List RoleClaim } } } > UserListBase</summary>
        Task<User> Find(string id);

        /// <summary>User > AccountController</summary>
        Task<User> Find1(string id);

        Task<User> Update(User user);

        Task<bool> Deletable(string id);

        /// <summary>List UserClaim</summary>
        Task<List<UserClaim>> GetClaim();

        Task<User> UpdatesClaim(string userId, List<UserClaim> userClaim);

        /// <summary>List LogTransaksi { Tanggal, Entitas, EntitasId, Keterangan, User { Email } } > LogTransaksiList</summary>
        Task<List<LogTransaksi>> GetLog();

        /// <summary>LogTransaksi</summary>
        Task<LogTransaksi> FindLog(int id);

        Task<LogTransaksi> CreateLog(LogTransaksi logTransaksi);

        Task DeletesLog();
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<User>> Get()
        {
            return await _appDbContext.Users.Include(x => x.UserClaim).Include(x => x.UserRole!).ThenInclude(x => x.Role).Select(x => new User
            {
                Id = x.Id,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Alamat = x.Alamat,
                TempatLahir = x.TempatLahir,
                TanggalLahir = x.TanggalLahir,
                Roles = string.Join(", ", x.UserRole!.Select(x => x.Role!.Name)),
                EmailConfirmed = x.EmailConfirmed,
                SecurityStamp = x.SecurityStamp,
                ConcurrencyStamp = x.ConcurrencyStamp,
                UserClaim = x.UserClaim
            }).ToListAsync();
        }

        public async Task<User> Find(string id)
        {
            return (await _appDbContext.Users.Include(x => x.UserRole!).ThenInclude(x => x.Role!).ThenInclude(x => x.RoleClaim).FirstOrDefaultAsync(x => x.Id == id || x.Email == id))!;
        }

        public async Task<User> Find1(string id)
        {
            return (await _appDbContext.Users.Include(x => x.UserRole!).FirstOrDefaultAsync(x => x.Id == id || x.Email == id))!;
        }

        public async Task<User> Update(User user)
        {
            User model = await _appDbContext.Users.FirstAsync(x => x.Id == user.Id);
            model.TempatLahir = user.TempatLahir;
            model.TanggalLahir = (DateTime)user.InputTanggalLahir!;
            model.Alamat = user.Alamat;

            if (user.EmailConfirmed && user.UserRole is not null)
            {
                _appDbContext.UserRoles.RemoveRange(await _appDbContext.UserRoles.Where(x => x.UserId == user.Id).ToListAsync());
                foreach (var x in user.UserRole!)
                    x.RoleId = (await _appDbContext.Roles.FirstAsync(y => y.Name == x.RoleName)).Id;

                var userRole = Nullifies(user.UserRole!);
                await _appDbContext.UserRoles.AddRangeAsync(userRole);
            }

            Nullify(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> Deletable(string id)
        {
            return await _appDbContext.Users.AnyAsync(x => x.Id == id && !x.LogTransaksi!.Any());
        }

        public async Task<List<UserClaim>> GetClaim()
        {
            return await _appDbContext.UserClaims.ToListAsync();
        }

        public async Task<User> UpdatesClaim(string userId, List<UserClaim> userClaim)
        {
            Nullifies(userClaim);

            await _appDbContext.UserClaims.Where(x => x.UserId == userId).ExecuteDeleteAsync();

            await _appDbContext.UserClaims.AddRangeAsync(userClaim);
            await _appDbContext.SaveChangesAsync();

            return await _appDbContext.Users.FirstAsync(x => x.Id == userId);
        }

        public async Task<List<LogTransaksi>> GetLog()
        {
            return await _appDbContext.LogTransaksi.Include(x => x.User).Select(x => new LogTransaksi
            {
                Tanggal = x.Tanggal,
                Entitas = x.Entitas,
                EntitasId = x.EntitasId,
                Keterangan = x.Keterangan,
                User = new User { Email = x.User!.Email }
            }).OrderByDescending(x => x.Tanggal).ToListAsync();
        }

        public async Task<LogTransaksi> FindLog(int id)
        {
            return await _appDbContext.LogTransaksi.FirstAsync(x => x.Id == id);
        }

        public async Task<LogTransaksi> CreateLog(LogTransaksi logTransaksi)
        {
            logTransaksi.Id = GenerateId(_appDbContext.LogTransaksi.Select(x => x.Id));

            var result = await _appDbContext.LogTransaksi.AddAsync(logTransaksi);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeletesLog()
        {
            await _appDbContext.LogTransaksi.ExecuteDeleteAsync();
        }
    }
}