namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Produksi, R ProduksiDetail</summary>
    public interface IProduksiService
    {
        /// <summary>List Produksi { Id, Tanggal, Jumlah, Total, Barang { Nama, SatuanProduksi } } > ProduksiList</summary>
        Task<List<Produksi>> Get();

        /// <summary>Produksi { Id, BarangId, InputTanggal, InputWaktu, Jumlah, Keterangan, BiayaJasa, BiayaOverhead, Version, Total, Barang { Id, Nama, SatuanProduksi, Stok, Version }, List ProduksiDetailBahan { Id, ProduksiId, BahanId, Jumlah, JumlahSebelum, Bahan { Id, Nama, SatuanProduksi, Stok, Version } }, List ProduksiDetailJasa { Id, ProduksiId, KaryawanId, Biaya, Karyawan { Id, Nama, PekerjaanId, Pekerjaan { Id, Nama } } }, List ProduksiDetailOverhead { Id, ProduksiId, OverheadId, Biaya, Overhead { Id, Nama } } } > ProduksiForm</summary>
        Task<Produksi> Find(string id);

        /// <summary>Produksi { Tanggal, Jumlah, Keterangan, BiayaJasa, BiayaOverhead, Barang { Nama, SatuanProduksi }, ProduksiDetailBahan { Jumlah, Bahan { Nama, SatuanProduksi } }, ProduksiDetailJasa { Biaya, Karyawan { Nama, Pekerjaan { Nama } } }, ProduksiDetailOverhead { Biaya, Overhead { Nama } } } > ProduksiInfo</summary>
        Task<Produksi> Find1(string id);

        Task<Produksi> Create(Produksi produksi);

        Task<Produksi> Update(Produksi produksi);

        Task<bool> Delete(string id);

        /// <summary>List ProduksiDetailBahan</summary>
        Task<List<ProduksiDetailBahan>> GetDetailBahan();

        /// <summary>List ProduksiDetailBahan { Id, ProduksiId, BahanId, Jumlah, JumlahSebelum, Bahan { Id, Nama, SatuanProduksi, Stok, Version } }</summary>
        Task<List<ProduksiDetailBahan>> FindDetailBahan(string produksiId);

        /// <summary>List ProduksiDetailJasa</summary>
        Task<List<ProduksiDetailJasa>> GetDetailJasa();

        /// <summary>List ProduksiDetailJasa { Id, ProduksiId, KaryawanId, Biaya, Karyawan { Id, Nama, PekerjaanId, Pekerjaan { Id, Nama } } }</summary>
        Task<List<ProduksiDetailJasa>> FindDetailJasa(string produksiId);

        /// <summary>List ProduksiDetailOverhead</summary>
        Task<List<ProduksiDetailOverhead>> GetDetailOverhead();

        /// <summary>List ProduksiDetailOverhead { Id, ProduksiId, OverheadId, Biaya, Overhead { Id, Nama } }</summary>
        Task<List<ProduksiDetailOverhead>> FindDetailOverhead(string produksiId);

        /// <summary>Produksi { Barang { Id, Nama, SatuanProduksi, Stok, Version }, List ProduksiDetailBahan { Id, ProduksiId, BahanId, Jumlah, JumlahSebelum, Bahan { Id, Nama, SatuanProduksi, Stok, Version } }, List ProduksiDetailJasa { Id, ProduksiId, KaryawanId, Biaya, Karyawan { Id, Nama, PekerjaanId, Pekerjaan { Id, Nama } } }, List ProduksiDetailOverhead { Id, ProduksiId, OverheadId, Biaya, Overhead { Id, Nama } } } > ProduksiForm</summary>
        Task<Produksi> RefreshDetail(string produksiId, List<string> bahanIds, List<string> karyawanIds, List<int> OverheadIds);
    }

    public class ProduksiService : IProduksiService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public ProduksiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Produksi>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/produksi");
            return JsonSerializer.Deserialize<List<Produksi>>(jsonString)!;
        }

        public async Task<Produksi> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/produksi/{id}");
            return JsonSerializer.Deserialize<Produksi>(jsonString)!;
        }

        public async Task<Produksi> Find1(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/produksi/f/1/{id}");
            return JsonSerializer.Deserialize<Produksi>(jsonString)!;
        }

        public async Task<Produksi> Create(Produksi produksi)
        {
            var response = await _httpClient.PostAsJsonAsync("api/produksi", JsonSerializer.Serialize(produksi), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Produksi>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Produksi> Update(Produksi produksi)
        {
            var response = await _httpClient.PutAsJsonAsync("api/produksi", JsonSerializer.Serialize(produksi), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Produksi>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/produksi/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ProduksiDetailBahan>> GetDetailBahan()
        {
            var jsonString = await _httpClient.GetStringAsync("api/produksi/detailbahan");
            return JsonSerializer.Deserialize<List<ProduksiDetailBahan>>(jsonString)!;
        }

        public async Task<List<ProduksiDetailBahan>> FindDetailBahan(string produksiId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/produksi/detailbahan/{produksiId}");
            return JsonSerializer.Deserialize<List<ProduksiDetailBahan>>(jsonString)!;
        }

        public async Task<List<ProduksiDetailJasa>> GetDetailJasa()
        {
            var jsonString = await _httpClient.GetStringAsync("api/produksi/detailjasa");
            return JsonSerializer.Deserialize<List<ProduksiDetailJasa>>(jsonString)!;
        }

        public async Task<List<ProduksiDetailJasa>> FindDetailJasa(string produksiId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/produksi/detailjasa/{produksiId}");
            return JsonSerializer.Deserialize<List<ProduksiDetailJasa>>(jsonString)!;
        }

        public async Task<List<ProduksiDetailOverhead>> GetDetailOverhead()
        {
            var jsonString = await _httpClient.GetStringAsync("api/produksi/detailoverhead");
            return JsonSerializer.Deserialize<List<ProduksiDetailOverhead>>(jsonString)!;
        }

        public async Task<List<ProduksiDetailOverhead>> FindDetailOverhead(string produksiId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/produksi/detailoverhead/{produksiId}");
            return JsonSerializer.Deserialize<List<ProduksiDetailOverhead>>(jsonString)!;
        }

        public async Task<Produksi> RefreshDetail(string produksiId, List<string> bahanIds, List<string> karyawanIds, List<int> OverheadIds)
        {
            //var jsonString = await _httpClient.GetStringAsync($"api/produksi/detail/refresh/{produksiId}");
            //return JsonSerializer.Deserialize<Produksi>(jsonString)!;
            var response = await _httpClient.PostAsJsonAsync($"api/produksi/detail/refresh/{produksiId}", JsonSerializer.Serialize(new { bahanIds, karyawanIds, OverheadIds }), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Produksi>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }
    }
}