namespace ProduksiManufaktur.Web.Services
{
    /// <summary>R Role, CRU Claim</summary>
    public interface IRoleService
    {
        /// <summary>List Role { UserRole } > UserList</summary>
        Task<List<Role>> Get();

        /// <summary>List Role { UserRole , RoleClaim } > RoleList</summary>
        Task<List<Role>> Get1();

        Task<Role> Find(string id);

        Task<Role> Update(Role role);

        Task<List<RoleClaim>> GetClaim();
    }

    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public RoleService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Role>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/role");
            return JsonSerializer.Deserialize<List<Role>>(jsonString)!;
        }

        public async Task<List<Role>> Get1()
        {
            var jsonString = await _httpClient.GetStringAsync("api/role/q/1");
            return JsonSerializer.Deserialize<List<Role>>(jsonString)!;
        }

        public async Task<Role> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/role/{id}");
            return JsonSerializer.Deserialize<Role>(jsonString)!;
        }

        public async Task<Role> Update(Role role)
        {
            var response = await _httpClient.PutAsJsonAsync("api/role", JsonSerializer.Serialize(role), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Role>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<List<RoleClaim>> GetClaim()
        {
            var jsonString = await _httpClient.GetStringAsync("api/role/claim");
            return JsonSerializer.Deserialize<List<RoleClaim>>(jsonString)!;
        }
    }
}