namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Penjualan, R PenjualanDetail, CRUD TransaksiPenjualan, CRUD ReturPenjualan, R ReturPenjualanDetail</summary>
    public interface IPenjualanService
    {
        /// <summary>List Penjualan { Id, Tanggal, Status, GrandTotal, Customer { Nama } } > PenjualanList</summary>
        Task<List<Penjualan>> Get();

        /// <summary>List Penjualan { Id, Tanggal } > ReturPenjualanForm (Untuk autocomplete, memilih penjualan yang belum retur untuk diretur)</summary>
        Task<List<Penjualan>> Get1();

        /// <summary>Penjualan { Id, CustomerId, InputTanggal, InputWaktu, JatuhTempo, PPN, Keterangan, Version, HariJatuhTempo, Terbayar, Customer { Id, Nama }, List PenjualanDetail { Id, PenjualanId, BarangSatuanId, MinJumlah, Jumlah, Harga, JumlahSebelum, StokAkhir, Total, BarangSatuan { BarangId, Nama, Ukuran, Harga, KonversiStok, Barang { Nama, SatuanProduksi, Stok, Version } } } } > PenjualanForm (Untuk autocomplete, memilih penjualan yang ingin diedit)</summary>
        Task<Penjualan> Find(string id);

        /// <summary>Penjualan { Id, Tanggal, JatuhTempo, HariJatuhTempo, GrandTotal, Terbayar, Sisa, Status, Version, List TransaksiPenjualan { Id, PenjualanId, Tanggal, Keterangan, Nominal, Version, InputTanggal, InputWaktu, NominalSebelum, Balance } } > TransaksiPenjualanForm (Memuat semua transaksi pada penjualan tertentu)</summary>
        Task<Penjualan> Find1(string id);

        /// <summary>Penjualan { Tanggal, Subtotal, PPN, Terbayar, MetodeBayar, Status, JatuhTempo, Keterangan, HariJatuhTempo, GrandTotal, Sisa, Customer { Nama }, List PenjualanDetail { Jumlah, Harga, Total, BarangSatuan { Nama, Ukuran, Barang { Nama } } } } > PenjualanInfo</summary>
        Task<Penjualan> Find2(string id);

        Task<Penjualan> Create(Penjualan penjualan);

        Task<Penjualan> Update(Penjualan penjualan);

        Task<bool> Deletable(string id);

        Task<bool> Delete(string id);

        /// <summary>List PenjualanDetail</summary>
        Task<List<PenjualanDetail>> GetDetail();

        /// <summary>List PenjualanDetail { BarangSatuanId, Harga, Jumlah, BarangSatuan { Id, Nama, Ukuran, Barang { Nama, Stok, Version } } } > ReturPenjualanForm (Untuk PilihPenjualan, PenjualanDetail akan dimasukkan ke ReturDetail)</summary>
        Task<List<PenjualanDetail>> FindDetail(string penjualanId);

        /// <summary>List BarangSatuan { Id, Nama, Ukuran, Harga, KonversiStok, Barang { Nama, SatuanProduksi, Version } } > PenjualanForm</summary>
        Task<List<BarangSatuan>> RefreshDetail(string id, List<int> barangSatuanIds);

        Task<bool> DeletableDetail(string penjualanId, int barangSatuanId);

        /// <summary>List TransaksiPenjualan</summary>
        Task<List<TransaksiPenjualan>> GetTransaksi();

        /// <summary>TransaksiPenjualan</summary>
        Task<TransaksiPenjualan> FindTransaksi(int id);

        Task<TransaksiPenjualan> CreateTransaksi(TransaksiPenjualan transaksiPenjualan);

        Task<TransaksiPenjualan> UpdateTransaksi(TransaksiPenjualan transaksiPenjualan);

        Task<bool> DeleteTransaksi(int id);

        /// <summary>List ReturPenjualan { Id, Tanggal, Keterangan, GrandTotal, Penjualan { Customer { Nama } } } > ReturPenjualanList</summary>
        Task<List<ReturPenjualan>> GetRetur();

        /// <summary>ReturPenjualan { Id, PenjualanId, Keterangan, Version, InputTanggal, InputWaktu, GrandTotal, Penjualan, List ReturPenjualanDetail { BarangSatuanId, Harga, Jumlah, MaxJumlah, Total, BarangSatuan { Id, BarangId, Nama, Ukuran, Barang { Nama, SatuanProduksi, Stok, Version } } } } > ReturPenjualanForm</summary>
        Task<ReturPenjualan> FindRetur(string id);

        /// <summary>ReturPenjualan { PenjualanId, Tanggal, GrandTotal, Keterangan, List ReturPenjualanDetail { BarangSatuanId, Jumlah, Harga, Total, BarangSatuan { Nama, Ukuran, Barang { Nama } } } } > ReturPenjualanInfo</summary>
        Task<ReturPenjualan> FindRetur1(string id);

        Task<ReturPenjualan> CreateRetur(ReturPenjualan returPenjualan);

        Task<ReturPenjualan> UpdateRetur(ReturPenjualan returPenjualan);

        Task<bool> DeleteRetur(string id);

        /// <summary>List ReturPenjualanDetail</summary>
        Task<List<ReturPenjualanDetail>> GetReturDetail();

        /// <summary>List BarangSatuan { Id, BarangId, Nama, Ukuran, Barang { Nama, SatuanProduksi, Stok, Version } } > ReturPenjualanForm</summary>
        Task<List<BarangSatuan>> RefreshReturDetail(string returId);
    }

    public class PenjualanService : IPenjualanService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public PenjualanService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Penjualan>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/penjualan");
            return JsonSerializer.Deserialize<List<Penjualan>>(jsonString)!;
        }

        public async Task<List<Penjualan>> Get1()
        {
            var jsonString = await _httpClient.GetStringAsync("api/penjualan/g/1");
            return JsonSerializer.Deserialize<List<Penjualan>>(jsonString)!;
        }

        public async Task<Penjualan> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/{id}");
            return JsonSerializer.Deserialize<Penjualan>(jsonString)!;
        }

        public async Task<Penjualan> Find1(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/f/1/{id}");
            return JsonSerializer.Deserialize<Penjualan>(jsonString)!;
        }

        public async Task<Penjualan> Find2(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/f/2/{id}");
            return JsonSerializer.Deserialize<Penjualan>(jsonString)!;
        }

        public async Task<Penjualan> Create(Penjualan penjualan)
        {
            var response = await _httpClient.PostAsJsonAsync("api/penjualan", JsonSerializer.Serialize(penjualan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Penjualan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Penjualan> Update(Penjualan penjualan)
        {
            var response = await _httpClient.PutAsJsonAsync("api/penjualan", JsonSerializer.Serialize(penjualan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Penjualan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/penjualan/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<PenjualanDetail>> GetDetail()
        {
            var jsonString = await _httpClient.GetStringAsync("api/penjualan/detail");
            return JsonSerializer.Deserialize<List<PenjualanDetail>>(jsonString)!;
        }

        public async Task<List<PenjualanDetail>> FindDetail(string penjualanid)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/detail/{penjualanid}");
            return JsonSerializer.Deserialize<List<PenjualanDetail>>(jsonString)!;
        }

        public async Task<List<BarangSatuan>> RefreshDetail(string id, List<int> barangSatuanIds)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/penjualan/detail/refresh/{id}", JsonSerializer.Serialize(barangSatuanIds), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<List<BarangSatuan>>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeletableDetail(string penjualanId, int barangSatuanId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/detail/deletable/{penjualanId}/{barangSatuanId}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<List<TransaksiPenjualan>> GetTransaksi()
        {
            var jsonString = await _httpClient.GetStringAsync("api/penjualan/transaksi");
            return JsonSerializer.Deserialize<List<TransaksiPenjualan>>(jsonString)!;
        }

        public async Task<TransaksiPenjualan> FindTransaksi(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/transaksi/{id}");
            return JsonSerializer.Deserialize<TransaksiPenjualan>(jsonString)!;
        }

        public async Task<TransaksiPenjualan> CreateTransaksi(TransaksiPenjualan transaksiPenjualan)
        {
            var response = await _httpClient.PostAsJsonAsync("api/penjualan/transaksi", JsonSerializer.Serialize(transaksiPenjualan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<TransaksiPenjualan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<TransaksiPenjualan> UpdateTransaksi(TransaksiPenjualan transaksiPenjualan)
        {
            var response = await _httpClient.PutAsJsonAsync("api/penjualan/transaksi", JsonSerializer.Serialize(transaksiPenjualan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<TransaksiPenjualan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeleteTransaksi(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/penjualan/transaksi/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ReturPenjualan>> GetRetur()
        {
            var jsonString = await _httpClient.GetStringAsync("api/penjualan/retur");
            return JsonSerializer.Deserialize<List<ReturPenjualan>>(jsonString)!;
        }

        public async Task<ReturPenjualan> FindRetur(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/retur/{id}");
            return JsonSerializer.Deserialize<ReturPenjualan>(jsonString)!;
        }

        public async Task<ReturPenjualan> FindRetur1(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/retur/f/1/{id}");
            return JsonSerializer.Deserialize<ReturPenjualan>(jsonString)!;
        }

        public async Task<ReturPenjualan> CreateRetur(ReturPenjualan returPenjualan)
        {
            var response = await _httpClient.PostAsJsonAsync("api/penjualan/retur", JsonSerializer.Serialize(returPenjualan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<ReturPenjualan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<ReturPenjualan> UpdateRetur(ReturPenjualan returPenjualan)
        {
            var response = await _httpClient.PutAsJsonAsync("api/penjualan/retur", JsonSerializer.Serialize(returPenjualan), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<ReturPenjualan>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeleteRetur(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/penjualan/retur/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ReturPenjualanDetail>> GetReturDetail()
        {
            var jsonString = await _httpClient.GetStringAsync("api/penjualan/retur/detail");
            return JsonSerializer.Deserialize<List<ReturPenjualanDetail>>(jsonString)!;
        }

        public async Task<List<BarangSatuan>> RefreshReturDetail(string returId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/penjualan/retur/detail/{returId}");
            return JsonSerializer.Deserialize<List<BarangSatuan>>(jsonString)!;
        }
    }
}