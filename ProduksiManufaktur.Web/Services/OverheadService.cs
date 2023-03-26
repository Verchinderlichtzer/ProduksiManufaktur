namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Overhead</summary>
    public interface IOverheadService
    {
        /// <summary>List Overhead > OverheadList, OverheadForm, ProduksiForm</summary>
        Task<List<Overhead>> Get();

        /// <summary>Overhead > OverheadForm</summary>
        Task<Overhead> Find(int id);

        Task<Overhead> Create(Overhead overhead);

        Task<Overhead> Update(Overhead overhead);

        Task<bool> Deletable(int id);

        Task<bool> Delete(int id);
    }

    public class OverheadService : IOverheadService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public OverheadService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Overhead>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/overhead");
            return JsonSerializer.Deserialize<List<Overhead>>(jsonString)!;
        }

        public async Task<Overhead> Find(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/overhead/{id}");
            return JsonSerializer.Deserialize<Overhead>(jsonString)!;
        }

        public async Task<Overhead> Create(Overhead overhead)
        {
            var response = await _httpClient.PostAsJsonAsync("api/overhead", JsonSerializer.Serialize(overhead), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Overhead>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Overhead> Update(Overhead overhead)
        {
            var response = await _httpClient.PutAsJsonAsync("api/overhead", JsonSerializer.Serialize(overhead), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Overhead>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/overhead/{id}/deletable");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/overhead/{id}");
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}