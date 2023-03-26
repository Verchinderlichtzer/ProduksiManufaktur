namespace ProduksiManufaktur.Web.Services
{
    /// <summary>CRUD Pembelian, R PembelianDetail, CRUD TransaksiPembelian, CRUD ReturPembelian, R ReturPembelianDetail</summary>
    public interface IPembelianService
    {
        /// <summary>List Pembelian { Id, Tanggal, Status, GrandTotal, Supplier { Nama } } > PembelianList</summary>
        Task<List<Pembelian>> Get();

        /// <summary>List Pembelian { Id, Tanggal } > ReturPembelianForm (Untuk autocomplete, memilih pembelian yang belum retur untuk diretur)</summary>
        Task<List<Pembelian>> Get1();

        /// <summary>Pembelian { Id, SupplierId, InputTanggal, InputWaktu, JatuhTempo, PPN, Keterangan, Version, HariJatuhTempo, Terbayar, Supplier { Id, Nama }, List PembelianDetail { Id, PembelianId, BahanSatuanId, MinJumlah, Jumlah, Harga, JumlahSebelum, StokAkhir, Total, BahanSatuan { BahanId, Nama, Ukuran, Harga, KonversiStok, Bahan { Nama, SatuanProduksi, Stok, Version } } } } > PembelianForm (Untuk autocomplete, memilih pembelian yang ingin diedit)</summary>
        Task<Pembelian> Find(string id);

        /// <summary>Pembelian { Id, Tanggal, JatuhTempo, HariJatuhTempo, GrandTotal, Terbayar, Sisa, Status, Version, List TransaksiPembelian { Id, PembelianId, Tanggal, Keterangan, Nominal, Version, InputTanggal, InputWaktu, NominalSebelum, Balance } } > TransaksiPembelianForm (Memuat semua transaksi pada pembelian tertentu)</summary>
        Task<Pembelian> Find1(string id);

        /// <summary>Pembelian { Tanggal, Subtotal, PPN, Terbayar, MetodeBayar, Status, JatuhTempo, Keterangan, HariJatuhTempo, GrandTotal, Sisa, Supplier { Nama }, List PembelianDetail { Jumlah, Harga, Total, BahanSatuan { Nama, Ukuran, Bahan { Nama } } } } > PembelianInfo</summary>
        Task<Pembelian> Find2(string id);

        Task<Pembelian> Create(Pembelian pembelian);

        Task<Pembelian> Update(Pembelian pembelian);

        Task<bool> Deletable(string id);

        Task<bool> Delete(string id);

        /// <summary>List PembelianDetail</summary>
        Task<List<PembelianDetail>> GetDetail();

        /// <summary>List PembelianDetail { BahanSatuanId, Harga, Jumlah, BahanSatuan { Id, Nama, Ukuran, Bahan { Nama, Stok, Version } } } > ReturPembelianForm (Untuk PilihPembelian, PembelianDetail akan dimasukkan ke ReturDetail)</summary>
        Task<List<PembelianDetail>> FindDetail(string pembelianId);

        /// <summary>List BahanSatuan { Id, Nama, Ukuran, Harga, KonversiStok, Bahan { Nama, SatuanProduksi, Version } } > PembelianForm</summary>
        Task<List<BahanSatuan>> RefreshDetail(string id, List<int> bahanSatuanIds);

        Task<bool> DeletableDetail(string pembelianId, int bahanSatuanId);

        /// <summary>List TransaksiPembelian</summary>
        Task<List<TransaksiPembelian>> GetTransaksi();

        /// <summary>TransaksiPembelian</summary>
        Task<TransaksiPembelian> FindTransaksi(int id);

        Task<TransaksiPembelian> CreateTransaksi(TransaksiPembelian transaksiPembelian);

        Task<TransaksiPembelian> UpdateTransaksi(TransaksiPembelian transaksiPembelian);

        Task<bool> DeleteTransaksi(int id);

        /// <summary>List ReturPembelian { Id, Tanggal, Keterangan, GrandTotal, Pembelian { Supplier { Nama } } } > ReturPembelianList</summary>
        Task<List<ReturPembelian>> GetRetur();

        /// <summary>ReturPembelian { Id, PembelianId, Keterangan, Version, InputTanggal, InputWaktu, GrandTotal, Pembelian, List ReturPembelianDetail { BahanSatuanId, Harga, Jumlah, MaxJumlah, Total, BahanSatuan { Id, BahanId, Nama, Ukuran, Bahan { Nama, SatuanProduksi, Stok, Version } } } } > ReturPembelianForm</summary>
        Task<ReturPembelian> FindRetur(string id);

        /// <summary>ReturPembelian { PembelianId, Tanggal, GrandTotal, Keterangan, List ReturPembelianDetail { BahanSatuanId, Jumlah, Harga, Total, BahanSatuan { Nama, Ukuran, Bahan { Nama } } } } > ReturPembelianInfo</summary>
        Task<ReturPembelian> FindRetur1(string id);

        Task<ReturPembelian> CreateRetur(ReturPembelian returPembelian);

        Task<ReturPembelian> UpdateRetur(ReturPembelian returPembelian);

        Task<bool> DeleteRetur(string id);

        /// <summary>List ReturPembelianDetail</summary>
        Task<List<ReturPembelianDetail>> GetReturDetail();

        /// <summary>List BahanSatuan { Id, BahanId, Nama, Ukuran, Bahan { Nama, SatuanProduksi, Stok, Version } } > ReturPembelianForm</summary>
        Task<List<BahanSatuan>> RefreshReturDetail(string returId);
    }

    public class PembelianService : IPembelianService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public PembelianService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<Pembelian>> Get()
        {
            var jsonString = await _httpClient.GetStringAsync("api/pembelian");
            return JsonSerializer.Deserialize<List<Pembelian>>(jsonString)!;
        }

        public async Task<List<Pembelian>> Get1()
        {
            var jsonString = await _httpClient.GetStringAsync("api/pembelian/g/1");
            return JsonSerializer.Deserialize<List<Pembelian>>(jsonString)!;
        }

        public async Task<Pembelian> Find(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/{id}");
            return JsonSerializer.Deserialize<Pembelian>(jsonString)!;
        }

        public async Task<Pembelian> Find1(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/f/1/{id}");
            return JsonSerializer.Deserialize<Pembelian>(jsonString)!;
        }

        public async Task<Pembelian> Find2(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/f/2/{id}");
            return JsonSerializer.Deserialize<Pembelian>(jsonString)!;
        }

        public async Task<Pembelian> Create(Pembelian pembelian)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pembelian", JsonSerializer.Serialize(pembelian), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Pembelian>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<Pembelian> Update(Pembelian pembelian)
        {
            var response = await _httpClient.PutAsJsonAsync("api/pembelian", JsonSerializer.Serialize(pembelian), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<Pembelian>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> Deletable(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/deletable/{id}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/pembelian/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<PembelianDetail>> GetDetail()
        {
            var jsonString = await _httpClient.GetStringAsync("api/pembelian/detail");
            return JsonSerializer.Deserialize<List<PembelianDetail>>(jsonString)!;
        }

        public async Task<List<PembelianDetail>> FindDetail(string pembelianid)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/detail/{pembelianid}");
            return JsonSerializer.Deserialize<List<PembelianDetail>>(jsonString)!;
        }

        public async Task<List<BahanSatuan>> RefreshDetail(string id, List<int> bahanSatuanIds)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/pembelian/detail/refresh/{id}", JsonSerializer.Serialize(bahanSatuanIds), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<List<BahanSatuan>>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeletableDetail(string pembelianId, int bahanSatuanId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/detail/deletable/{pembelianId}/{bahanSatuanId}");
            return JsonSerializer.Deserialize<bool>(jsonString)!;
        }

        public async Task<List<TransaksiPembelian>> GetTransaksi()
        {
            var jsonString = await _httpClient.GetStringAsync("api/pembelian/transaksi");
            return JsonSerializer.Deserialize<List<TransaksiPembelian>>(jsonString)!;
        }

        public async Task<TransaksiPembelian> FindTransaksi(int id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/transaksi/{id}");
            return JsonSerializer.Deserialize<TransaksiPembelian>(jsonString)!;
        }

        public async Task<TransaksiPembelian> CreateTransaksi(TransaksiPembelian transaksiPembelian)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pembelian/transaksi", JsonSerializer.Serialize(transaksiPembelian), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<TransaksiPembelian>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<TransaksiPembelian> UpdateTransaksi(TransaksiPembelian transaksiPembelian)
        {
            var response = await _httpClient.PutAsJsonAsync("api/pembelian/transaksi", JsonSerializer.Serialize(transaksiPembelian), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<TransaksiPembelian>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeleteTransaksi(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/pembelian/transaksi/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ReturPembelian>> GetRetur()
        {
            var jsonString = await _httpClient.GetStringAsync("api/pembelian/retur");
            return JsonSerializer.Deserialize<List<ReturPembelian>>(jsonString)!;
        }

        public async Task<ReturPembelian> FindRetur(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/retur/{id}");
            return JsonSerializer.Deserialize<ReturPembelian>(jsonString)!;
        }

        public async Task<ReturPembelian> FindRetur1(string id)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/retur/f/1/{id}");
            return JsonSerializer.Deserialize<ReturPembelian>(jsonString)!;
        }

        public async Task<ReturPembelian> CreateRetur(ReturPembelian returPembelian)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pembelian/retur", JsonSerializer.Serialize(returPembelian), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<ReturPembelian>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<ReturPembelian> UpdateRetur(ReturPembelian returPembelian)
        {
            var response = await _httpClient.PutAsJsonAsync("api/pembelian/retur", JsonSerializer.Serialize(returPembelian), _options);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<ReturPembelian>(await response.Content.ReadAsStringAsync(), _options)!;
            return null!;
        }

        public async Task<bool> DeleteRetur(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/pembelian/retur/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ReturPembelianDetail>> GetReturDetail()
        {
            var jsonString = await _httpClient.GetStringAsync("api/pembelian/retur/detail");
            return JsonSerializer.Deserialize<List<ReturPembelianDetail>>(jsonString)!;
        }

        public async Task<List<BahanSatuan>> RefreshReturDetail(string returId)
        {
            var jsonString = await _httpClient.GetStringAsync($"api/pembelian/retur/detail/{returId}");
            return JsonSerializer.Deserialize<List<BahanSatuan>>(jsonString)!;
        }
    }
}