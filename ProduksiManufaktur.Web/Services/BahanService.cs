namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Bahan, CRUD PerubahanStokBahan</summary>
    public interface IBahanService
    {
        /// <summary>List Bahan > BahanList, PerubahanStokBahanList, PembelianForm, FormulasiForm</summary>
        Task<List<Bahan>> Get();

        /// <summary>List Bahan { Id, Nama, SatuanProduksi, Stok, Version } > ProduksiForm</summary>
        Task<List<Bahan>> Get1();

        /// <summary>Bahan > BahanForm</summary>
        Task<Bahan> Find(string id);

        Task<Bahan> Create(Bahan bahan);

        Task<Bahan> Update(Bahan bahan);

        Task<bool> Deletable(string id);

        Task<bool> Delete(string id);

        /// <summary>List BahanSatuan { Id, BahanId, Nama, Ukuran, Harga, KonversiStok, Bahan { Nama, SatuanProduksi, Stok, Version } } > PembelianFormList</summary>
        Task<List<BahanSatuan>> GetBahanSatuan();

        /// <summary>List BahanSatuan > BahanList</summary>
        Task<List<BahanSatuan>> FindBahanSatuan(string bahanId);

        Task<bool> DeletableBahanSatuan(int id);

        /// <summary>List PerubahanStokBahan { Id, Tanggal, Jenis, Jumlah, Keterangan, Bahan { Nama } } > PerubahanStokBahanList</summary>
        Task<List<PerubahanStokBahan>> GetPerubahanStok();

        /// <summary>PerubahanStokBahan { Id, BahanId, InputTanggal, InputWaktu, Jenis, JenisSebelum, Jumlah, JumlahSebelum, Keterangan, Bahan { Id, Nama, Stok, SatuanProduksi, Version } } > PerubahanStokBahanForm</summary>
        Task<PerubahanStokBahan> FindPerubahanStok(int id);

        Task<PerubahanStokBahan> CreatePerubahanStok(PerubahanStokBahan perubahanStokBahan);

        Task<PerubahanStokBahan> UpdatePerubahanStok(PerubahanStokBahan perubahanStokBahan);

        Task<bool> DeletablePerubahanStok(int id);

        Task<bool> DeletePerubahanStok(int id);
    }

    public class BahanService : IBahanService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public BahanService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Bahan>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/bahan");
            return JsonSerializer.Deserialize<List<Bahan>>(jsonString)!;
        }

        public async Task<List<Bahan>> Get1()
        {
            var jsonString = await _httpClient.GetStringAsync("api/bahan/g/1");
            return JsonSerializer.Deserialize<List<Bahan>>(jsonString)!;
        }

        public async Task<Bahan> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/bahan/{id}");
            return JsonSerializer.Deserialize<Bahan>(jsonString)!;
        }

        public async Task<Bahan> Create(Bahan bahan)
        {
            var response = await _httpClient.PostAsJsonAsync("api/bahan", JsonSerializer.Serialize(bahan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Bahan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Bahan> Update(Bahan bahan)
        {
            var response = await _httpClient.PutAsJsonAsync("api/bahan", JsonSerializer.Serialize(bahan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Bahan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/bahan/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/bahan/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<BahanSatuan>> GetBahanSatuan()
        {
            var jsonString = await _httpClient.GetStringAsync("api/bahan/bahansatuan");
            return JsonSerializer.Deserialize<List<BahanSatuan>>(jsonString)!;
        }

        public async Task<List<BahanSatuan>> FindBahanSatuan(string bahanId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/bahan/bahansatuan/{bahanId}");
            return JsonSerializer.Deserialize<List<BahanSatuan>>(jsonString)!;
        }

        public async Task<bool> DeletableBahanSatuan(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/bahan/bahansatuan/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<List<PerubahanStokBahan>> GetPerubahanStok()
        {
            var jsonString = await _httpClient.GetStringAsync("api/bahan/perubahanstok");
            return JsonSerializer.Deserialize<List<PerubahanStokBahan>>(jsonString)!;
        }

        public async Task<PerubahanStokBahan> FindPerubahanStok(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/bahan/perubahanstok/{id}");
            return JsonSerializer.Deserialize<PerubahanStokBahan>(jsonString)!;
        }

        public async Task<PerubahanStokBahan> CreatePerubahanStok(PerubahanStokBahan perubahanStokBahan)
        {
            var response = await _httpClient.PostAsJsonAsync("api/bahan/perubahanstok", JsonSerializer.Serialize(perubahanStokBahan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<PerubahanStokBahan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<PerubahanStokBahan> UpdatePerubahanStok(PerubahanStokBahan perubahanStokBahan)
        {
            var response = await _httpClient.PutAsJsonAsync("api/bahan/perubahanstok", JsonSerializer.Serialize(perubahanStokBahan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<PerubahanStokBahan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeletablePerubahanStok(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/bahan/perubahanstok/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> DeletePerubahanStok(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/bahan/perubahanstok/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}