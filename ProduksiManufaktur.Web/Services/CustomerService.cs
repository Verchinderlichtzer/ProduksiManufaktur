namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Customer</summary>
    public interface ICustomerService
    {
        /// <summary>List Customer > CustomerList</summary>
        Task<List<Customer>> Get();

        /// <summary>List Customer { Telepon, Fax, Email } > CustomerForm</summary>
        Task<List<Customer>> Get1();

        /// <summary>List Customer { Id, Nama } > PenjualanForm</summary>
        Task<List<Customer>> Get2();

        /// <summary>Customer > CustomerForm</summary>
        Task<Customer> Find(string id);

        Task<Customer> Create(Customer customer);

        Task<Customer> Update(Customer customer);

        Task<bool> Deletable(string id);

        Task<bool> Delete(string id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public CustomerService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Customer>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/customer");
            return JsonSerializer.Deserialize<List<Customer>>(jsonString)!;
        }

        public async Task<List<Customer>> Get1()
        {
            var jsonString = await _httpClient.GetStringAsync("api/customer/g/1");
            return JsonSerializer.Deserialize<List<Customer>>(jsonString)!;
        }

        public async Task<List<Customer>> Get2()
        {
            var jsonString = await _httpClient.GetStringAsync("api/customer/g/2");
            return JsonSerializer.Deserialize<List<Customer>>(jsonString)!;
        }

        public async Task<Customer> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/customer/{id}");
            return JsonSerializer.Deserialize<Customer>(jsonString)!;
        }

        public async Task<Customer> Create(Customer customer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customer", JsonSerializer.Serialize(customer), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Customer>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Customer> Update(Customer customer)
        {
            var response = await _httpClient.PutAsJsonAsync("api/customer", JsonSerializer.Serialize(customer), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Customer>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/customer/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/customer/{id}");
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}