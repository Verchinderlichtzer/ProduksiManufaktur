namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Pekerjaan</summary>
    public interface IPekerjaanService
    {
        /// <summary>List Pekerjaan { Id, Nama, JumlahKaryawan } > PekerjaanList, KaryawanList</summary>
        Task<List<Pekerjaan>> Get();

        /// <summary>List Pekerjaan { Id, Nama } > PekerjaanForm</summary>
        Task<List<Pekerjaan>> Get1();

        /// <summary>Pekerjaan { Id, Nama } > PekerjaanForm</summary>
        Task<Pekerjaan> Find(int id);

        /// <summary>Pekerjaan { Id, Nama, List Karyawan { Id, Nama, Telepon, Email, Upah } } > PekerjaanInfo</summary>
        Task<Pekerjaan> Find1(int id);

        Task<Pekerjaan> Create(Pekerjaan pekerjaan);

        Task<Pekerjaan> Update(Pekerjaan pekerjaan);

        Task<bool> Deletable(int id);

        Task<bool> Delete(int id);
    }

    public class PekerjaanService : IPekerjaanService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public PekerjaanService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Pekerjaan>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/pekerjaan");
            return JsonSerializer.Deserialize<List<Pekerjaan>>(jsonString)!;
        }

        public async Task<List<Pekerjaan>> Get1()
        {
            var jsonString = await _httpClient.GetStringAsync("api/pekerjaan/q/1");
            return JsonSerializer.Deserialize<List<Pekerjaan>>(jsonString)!;
        }

        public async Task<Pekerjaan> Find(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pekerjaan/{id}");
            return JsonSerializer.Deserialize<Pekerjaan>(jsonString)!;
        }

        public async Task<Pekerjaan> Find1(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pekerjaan/{id}/q/1");
            return JsonSerializer.Deserialize<Pekerjaan>(jsonString)!;
        }

        public async Task<Pekerjaan> Create(Pekerjaan pekerjaan)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pekerjaan", JsonSerializer.Serialize(pekerjaan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Pekerjaan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Pekerjaan> Update(Pekerjaan pekerjaan)
        {
            var response = await _httpClient.PutAsJsonAsync("api/pekerjaan", JsonSerializer.Serialize(pekerjaan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Pekerjaan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pekerjaan/{id}/deletable");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/pekerjaan/{id}");
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}