namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Barang, CRUD PerubahanStokBarang</summary>
    public interface IBarangService
    {
        /// <summary>List Barang > BarangList, PerubahanStokBarangList, PenjualanForm, FormulasiForm, ProduksiForm</summary>
        Task<List<Barang>> Get();

        /// <summary>Barang > BarangForm</summary>
        Task<Barang> Find(string id);

        Task<Barang> Create(Barang barang);

        Task<Barang> Update(Barang barang);

        Task<bool> Deletable(string id);

        Task<bool> Delete(string id);

        /// <summary>List BarangSatuan { Id, BarangId, Nama, Ukuran, Harga, KonversiStok, Barang { Nama, SatuanProduksi, Stok, Version } } > PenjualanFormList</summary>
        Task<List<BarangSatuan>> GetBarangSatuan();

        /// <summary>List BarangSatuan > BarangList</summary>
        Task<List<BarangSatuan>> FindBarangSatuan(string barangId);

        Task<bool> DeletableBarangSatuan(int id);

        /// <summary>List PerubahanStokBarang { Id, Tanggal, Jenis, Jumlah, Keterangan, Barang { Nama } } > PerubahanStokBarangList</summary>
        Task<List<PerubahanStokBarang>> GetPerubahanStok();

        /// <summary>PerubahanStokBarang { Id, BarangId, InputTanggal, InputWaktu, Jenis, JenisSebelum, Jumlah, JumlahSebelum, Keterangan, Barang { Id, Nama, Stok, SatuanProduksi, Version } } > PerubahanStokBarangForm</summary>
        Task<PerubahanStokBarang> FindPerubahanStok(int id);

        Task<PerubahanStokBarang> CreatePerubahanStok(PerubahanStokBarang perubahanStokBarang);

        Task<PerubahanStokBarang> UpdatePerubahanStok(PerubahanStokBarang perubahanStokBarang);

        Task<bool> DeletablePerubahanStok(int id);

        Task<bool> DeletePerubahanStok(int id);
    }

    public class BarangService : IBarangService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public BarangService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Barang>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/barang");
            return JsonSerializer.Deserialize<List<Barang>>(jsonString)!;
        }

        public async Task<Barang> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/barang/{id}");
            return JsonSerializer.Deserialize<Barang>(jsonString)!;
        }

        public async Task<Barang> Create(Barang barang)
        {
            var response = await _httpClient.PostAsJsonAsync("api/barang", JsonSerializer.Serialize(barang), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Barang>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Barang> Update(Barang barang)
        {
            var response = await _httpClient.PutAsJsonAsync("api/barang", JsonSerializer.Serialize(barang), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Barang>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/barang/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/barang/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<BarangSatuan>> GetBarangSatuan()
        {
            var jsonString = await _httpClient.GetStringAsync("api/barang/barangsatuan");
            return JsonSerializer.Deserialize<List<BarangSatuan>>(jsonString)!;
        }

        public async Task<List<BarangSatuan>> FindBarangSatuan(string barangId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/barang/barangsatuan/{barangId}");
            return JsonSerializer.Deserialize<List<BarangSatuan>>(jsonString)!;
        }

        public async Task<bool> DeletableBarangSatuan(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/barang/barangsatuan/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<List<PerubahanStokBarang>> GetPerubahanStok()
        {
            var jsonString = await _httpClient.GetStringAsync("api/barang/perubahanstok");
            return JsonSerializer.Deserialize<List<PerubahanStokBarang>>(jsonString)!;
        }

        public async Task<PerubahanStokBarang> FindPerubahanStok(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/barang/perubahanstok/{id}");
            return JsonSerializer.Deserialize<PerubahanStokBarang>(jsonString)!;
        }

        public async Task<PerubahanStokBarang> CreatePerubahanStok(PerubahanStokBarang perubahanStokBarang)
        {
            var response = await _httpClient.PostAsJsonAsync("api/barang/perubahanstok", JsonSerializer.Serialize(perubahanStokBarang), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<PerubahanStokBarang>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<PerubahanStokBarang> UpdatePerubahanStok(PerubahanStokBarang perubahanStokBarang)
        {
            var response = await _httpClient.PutAsJsonAsync("api/barang/perubahanstok", JsonSerializer.Serialize(perubahanStokBarang), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<PerubahanStokBarang>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeletablePerubahanStok(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/barang/perubahanstok/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> DeletePerubahanStok(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/barang/perubahanstok/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}