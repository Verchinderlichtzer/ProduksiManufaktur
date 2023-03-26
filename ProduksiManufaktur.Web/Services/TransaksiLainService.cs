namespace ProduksiManufaktur.Web.Services
{
    public interface ITransaksiLainService
    {
        Task<List<TransaksiLain>> Get();

        Task<TransaksiLain> Find(int id);

        Task<List<TransaksiLain>> Creates(List<TransaksiLain> transaksiLain);

        Task<TransaksiLain> Update(TransaksiLain transaksiLain);

        Task<bool> Delete(int id);
    }

    public class TransaksiLainService : ITransaksiLainService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public TransaksiLainService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<TransaksiLain>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/transaksilain");
            return JsonSerializer.Deserialize<List<TransaksiLain>>(jsonString)!;
        }

        public async Task<TransaksiLain> Find(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/transaksilain/{id}");
            return JsonSerializer.Deserialize<TransaksiLain>(jsonString)!;
        }

        public async Task<List<TransaksiLain>> Creates(List<TransaksiLain> transaksiLain)
        {
            var response = await _httpClient.PostAsJsonAsync("api/transaksilain", JsonSerializer.Serialize(transaksiLain), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<List<TransaksiLain>>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<TransaksiLain> Update(TransaksiLain transaksiLain)
        {
            var response = await _httpClient.PutAsJsonAsync("api/transaksilain", JsonSerializer.Serialize(transaksiLain), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<TransaksiLain>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/transaksilain/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}