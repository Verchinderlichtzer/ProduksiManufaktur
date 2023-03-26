namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Supplier</summary>
    public interface ISupplierService
    {
        /// <summary>List Supplier > SupplierList</summary>
        Task<List<Supplier>> Get();

        /// <summary>List Supplier { Telepon, Fax, Email } > SupplierForm</summary>
        Task<List<Supplier>> Get1();

        /// <summary>List Supplier { Id, Nama } > PembelianForm</summary>
        Task<List<Supplier>> Get2();

        /// <summary>Supplier > SupplierForm</summary>
        Task<Supplier> Find(string id);

        Task<Supplier> Create(Supplier supplier);

        Task<Supplier> Update(Supplier supplier);

        Task<bool> Deletable(string id);

        Task<bool> Delete(string id);
    }

    public class SupplierService : ISupplierService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public SupplierService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Supplier>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/supplier");
            return JsonSerializer.Deserialize<List<Supplier>>(jsonString)!;
        }

        public async Task<List<Supplier>> Get1()
        {
            var jsonString = await _httpClient.GetStringAsync("api/supplier/g/1");
            return JsonSerializer.Deserialize<List<Supplier>>(jsonString)!;
        }

        public async Task<List<Supplier>> Get2()
        {
            var jsonString = await _httpClient.GetStringAsync("api/supplier/g/2");
            return JsonSerializer.Deserialize<List<Supplier>>(jsonString)!;
        }

        public async Task<Supplier> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/supplier/{id}");
            return JsonSerializer.Deserialize<Supplier>(jsonString)!;
        }

        public async Task<Supplier> Create(Supplier supplier)
        {
            var response = await _httpClient.PostAsJsonAsync("api/supplier", JsonSerializer.Serialize(supplier), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Supplier>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Supplier> Update(Supplier supplier)
        {
            var response = await _httpClient.PutAsJsonAsync("api/supplier", JsonSerializer.Serialize(supplier), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Supplier>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/supplier/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/supplier/{id}");
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}