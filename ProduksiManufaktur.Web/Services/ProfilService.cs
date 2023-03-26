namespace ProduksiManufaktur.Web.Services
{
    public interface IProfilService
    {
        Task<Profil> Get();

        Task<Profil> Update(Profil profil);
    }

    public class ProfilService : IProfilService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public ProfilService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<Profil> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/profil");
            return JsonSerializer.Deserialize<Profil>(jsonString)!;
        }

        public async Task<Profil> Update(Profil profil)
        {
            var response = await _httpClient.PutAsJsonAsync("api/profil", JsonSerializer.Serialize(profil), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Profil>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }
    }
}