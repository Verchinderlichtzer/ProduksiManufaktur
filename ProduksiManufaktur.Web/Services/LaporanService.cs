namespace ProduksiManufaktur.Web.Services
{
    public interface ILaporanService
    {
        /// <summary>Untuk Filter MetodeBayar dan Status didapat dari filterTerpilih</summary>
        Task<bool> Get(string laporan, List<string> filterTerpilih, string filterText, string entitas, string tanggal);

        Task<List<EntitasDto>> GetBahan();

        Task<List<EntitasDto>> GetBarang();

        Task<List<EntitasDto>> GetKaryawan();

        Task<List<EntitasDto>> GetSupplier();

        Task<List<EntitasDto>> GetCustomer();

        Task<List<EntitasDto>> GetPembelian();

        Task<List<EntitasDto>> GetReturPembelian();

        Task<List<EntitasDto>> GetPenjualan();

        Task<List<EntitasDto>> GetReturPenjualan();

        Task<List<EntitasDto>> GetProduksi();

        Task<List<EntitasDto>> GetFormulasi();
    }

    public class LaporanService : ILaporanService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _js;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public LaporanService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IJSRuntime js)
        {
            _httpClient = httpClient;
            _js = js;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
            // GetByteArrayAsync
        }

        public async Task<bool> Get(string laporan, List<string> filterTerpilih, string filterText, string entitas, string tanggal)
        {
            var response = await _httpClient.GetAsync($"api/laporan/{laporan}/filter?filterTerpilih={string.Join('.', filterTerpilih)}&filterText={filterText}&entitas={entitas}&tanggal={tanggal}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var fileStream = new MemoryStream(content);
                var fileName = response.Content.Headers.ContentDisposition!.FileName;
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await _js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }
            return response.IsSuccessStatusCode;
        }

        // https://localhost:7017/api/laporan/{laporan}/filter?filterTerpilih={string.Join('.', filterTerpilih)}&filterText={filterText}&entitas={entitas}&tanggal={tanggal}"))

        public async Task<List<EntitasDto>> GetBahan()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/bahan");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetBarang()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/barang");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetKaryawan()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/karyawan");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetSupplier()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/supplier");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetCustomer()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/customer");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetPembelian()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/pembelian");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetReturPembelian()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/retur-pembelian");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetPenjualan()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/penjualan");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetReturPenjualan()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/retur-penjualan");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetProduksi()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/produksi");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }

        public async Task<List<EntitasDto>> GetFormulasi()
        {
            var jsonString = await _httpClient.GetStringAsync("api/laporan/get/formulasi");
            return JsonSerializer.Deserialize<List<EntitasDto>>(jsonString)!;
        }
    }
}