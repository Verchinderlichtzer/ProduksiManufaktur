namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Karyawan</summary>
    public interface IKaryawanService
    {
        /// <summary>List Karyawan { Id, PekerjaanId, Nama, TempatLahir, TanggalLahir, Alamat, Telepon, Email, Upah, Pekerjaan { Id, Nama } } > KaryawanList</summary>
        Task<List<Karyawan>> Get();

        /// <summary>List Karyawan { Telepon, Email } > KaryawanForm</summary>
        Task<List<Karyawan>> Get1();

        /// <summary>List Karyawan { Id, Nama, Upah, Pekerjaan { Nama } } > ProduksiForm</summary>
        Task<List<Karyawan>> Get2();

        /// <summary>Karyawan { Id, PekerjaanId, Nama, TempatLahir, TanggalLahir, Alamat, Telepon, Email, Upah, Pekerjaan { Id, Nama } } > KaryawanForm</summary>
        Task<Karyawan> Find(string id);

        Task<Karyawan> Create(Karyawan karyawan);

        Task<Karyawan> Update(Karyawan karyawan);

        Task<bool> Deletable(string id);

        Task<bool> Delete(string id);
    }

    public class KaryawanService : IKaryawanService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public KaryawanService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Karyawan>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/karyawan");
            return JsonSerializer.Deserialize<List<Karyawan>>(jsonString)!;
        }

        public async Task<List<Karyawan>> Get1()
        {
            var jsonString = await _httpClient.GetStringAsync("api/karyawan/g/1");
            return JsonSerializer.Deserialize<List<Karyawan>>(jsonString)!;
        }

        public async Task<List<Karyawan>> Get2()
        {
            var jsonString = await _httpClient.GetStringAsync("api/karyawan/g/2");
            return JsonSerializer.Deserialize<List<Karyawan>>(jsonString)!;
        }

        public async Task<Karyawan> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/karyawan/{id}");
            return JsonSerializer.Deserialize<Karyawan>(jsonString)!;
        }

        public async Task<Karyawan> Create(Karyawan karyawan)
        {
            var response = await _httpClient.PostAsJsonAsync("api/karyawan", JsonSerializer.Serialize(karyawan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Karyawan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Karyawan> Update(Karyawan karyawan)
        {
            var response = await _httpClient.PutAsJsonAsync("api/karyawan", JsonSerializer.Serialize(karyawan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Karyawan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/karyawan/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/karyawan/{id}");
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}