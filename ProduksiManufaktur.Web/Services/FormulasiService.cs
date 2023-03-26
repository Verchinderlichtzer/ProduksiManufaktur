namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Formulasi, R FormulasiDetail</summary>
    public interface IFormulasiService
    {
        /// <summary>List Formulasi { Id, Jumlah, Barang { Nama, SatuanProduksi } } > FormulasiList</summary>
        Task<List<Formulasi>> Get();

        /// <summary>Formulasi { Id, BarangId, Jumlah, Barang { Nama, SatuanProduksi }, List FormulasiDetail { FormulasiId, BahanId, Jumlah, Bahan { Nama, SatuanProduksi } } } > FormulasiForm</summary>
        Task<Formulasi> Find(string id);

        /// <summary>List Formulasi { Id, Jumlah, Barang { SatuanProduksi } } } > ProduksiForm</summary>
        Task<List<Formulasi>> Find1(string barangId);

        /// <summary>Formulasi { Jumlah, List FormulasiDetail { Jumlah, Bahan { Id, Nama, Stok, SatuanProduksi, Version } } } > ProduksiForm</summary>
        Task<Formulasi> Find2(string id);

        Task<Formulasi> Create(Formulasi formulasi);

        Task<Formulasi> Update(Formulasi formulasi);

        Task<bool> Delete(string id);

        Task<List<FormulasiDetail>> GetDetail();
    }

    public class FormulasiService : IFormulasiService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public FormulasiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Formulasi>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/formulasi");
            return JsonSerializer.Deserialize<List<Formulasi>>(jsonString)!;
        }

        public async Task<Formulasi> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/formulasi/{id}");
            return JsonSerializer.Deserialize<Formulasi>(jsonString)!;
        }

        public async Task<List<Formulasi>> Find1(string barangId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/formulasi/f/1/{barangId}");
            return JsonSerializer.Deserialize<List<Formulasi>>(jsonString)!;
        }

        public async Task<Formulasi> Find2(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/formulasi/f/2/{id}");
            return JsonSerializer.Deserialize<Formulasi>(jsonString)!;
        }

        public async Task<Formulasi> Create(Formulasi formulasi)
        {
            var response = await _httpClient.PostAsJsonAsync("api/formulasi", JsonSerializer.Serialize(formulasi), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Formulasi>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Formulasi> Update(Formulasi formulasi)
        {
            var response = await _httpClient.PutAsJsonAsync("api/formulasi", JsonSerializer.Serialize(formulasi), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Formulasi>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/formulasi/{id}");
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public async Task<List<FormulasiDetail>> GetDetail()
        {
            var jsonString = await _httpClient.GetStringAsync("api/formulasi/detail");
            return JsonSerializer.Deserialize<List<FormulasiDetail>>(jsonString)!;
        }
    }
}