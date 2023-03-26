namespace ProduksiManufaktur.Web.Services
{
    /// <summary>RU User, CRU UserClaim, CR LogTransaksi</summary>
    public interface IUserService
    {
        /// <summary>List User { Id, Email, PhoneNumber, Alamat, TempatLahir, TanggalLahir, Roles, EmailConfirmed, SecurityStamp, ConcurrencyStamp } > UserListBase</summary>
        Task<List<User>> Get();

        /// <summary>User { Id, UserName, Email, PhoneNumber, Alamat, TempatLahir, TanggalLahir, EmailConfirmed, SecurityStamp, ConcurrencyStamp, PasswordHash, List UserRole { Role { List RoleClaim } } } > UserListBase</summary>
        Task<User> Find(string id);

        Task<User> Update(User user);

        Task<bool> Deletable(string id);

        /// <summary>List UserClaim</summary>
        Task<List<UserClaim>> GetClaim();

        Task<User> UpdatesClaim(User user);

        /// <summary>List LogTransaksi { Tanggal, Entitas, EntitasId, Keterangan, User { Email } } > LogTransaksiList</summary>
        Task<List<LogTransaksi>> GetLog();

        Task<LogTransaksi> CreateLog(LogTransaksi LogTransaksi);

        Task<bool> DeletesLog();
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<User>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/user");
            return JsonSerializer.Deserialize<List<User>>(jsonString)!;
        }

        public async Task<User> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/user/{id}");
            return JsonSerializer.Deserialize<User>(jsonString)!;
        }

        public async Task<User> Update(User user)
        {
            var response = await _httpClient.PutAsJsonAsync("api/user", JsonSerializer.Serialize(user), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/user/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<List<UserClaim>> GetClaim()
        {
            var jsonString = await _httpClient.GetStringAsync("api/user/claim");
            return JsonSerializer.Deserialize<List<UserClaim>>(jsonString)!;
        }

        public async Task<User> UpdatesClaim(User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/claim/{user.Id}", JsonSerializer.Serialize(user.UserClaim), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<List<LogTransaksi>> GetLog()
        {
            var jsonString = await _httpClient.GetStringAsync("api/user/log");
            return JsonSerializer.Deserialize<List<LogTransaksi>>(jsonString)!;
        }

        public async Task<LogTransaksi> CreateLog(LogTransaksi logTransaksi)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/log", JsonSerializer.Serialize(logTransaksi), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<LogTransaksi>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeletesLog()
        {
            var response = await _httpClient.DeleteAsync("api/user/log");
            return response.IsSuccessStatusCode;
        }
    }
}