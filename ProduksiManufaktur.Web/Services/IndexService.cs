namespace ProduksiManufaktur.Web.Services
{
    public interface IIndexService
    {
        /// <summary>List JumlahItemDto { Nama, Jumlah, Satuan } > Index</summary>
        Task<List<JumlahItemDto>> GetJumlahPakai();

        /// <summary>List JumlahItemDto { Nama, Jumlah, Satuan, Total } > Index</summary>
        Task<List<JumlahItemDto>> GetJumlahBeli();

        /// <summary>List JumlahItemDto { Nama, Jumlah, Satuan } > Index</summary>
        Task<List<JumlahItemDto>> GetJumlahProduksi();

        /// <summary>List JumlahItemDto { Nama, Jumlah, Satuan, Total } > Index</summary>
        Task<List<JumlahItemDto>> GetJumlahJual();

        /// <summary>List GrafikDto { Tanggal, Nominal } > Index</summary>
        Task<List<GrafikDto>> GetPengeluaran();

        /// <summary>List GrafikDto { Tanggal, Nominal } > Index</summary>
        Task<List<GrafikDto>> GetPendapatan();

        /// <summary>List Bahan { Nama, SatuanProduksi, Stok, StokMinimal } > Index</summary>
        Task<List<Bahan>> GetStokBahanMinim();

        /// <summary>List Barang { Nama, SatuanProduksi, Stok, StokMinimal } > Index</summary>
        Task<List<Barang>> GetStokBarangMinim();

        /// <summary>List Pembelian { Id, Sisa, HariJatuhTempo, JatuhTempo } > Index</summary>
        Task<List<Pembelian>> GetUtang();

        /// <summary>List Penjualan { Id, Sisa, HariJatuhTempo, JatuhTempo } > Index</summary>
        Task<List<Penjualan>> GetPiutang();

        /// <summary>List BarangPopulerDto { Label, Data } > Index</summary>
        Task<List<BarangPopulerDto>> GetBarangPopuler();
    }

    public class IndexService : IIndexService
    {
        private readonly HttpClient _httpClient;

        public IndexService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext!.Request.Cookies["api_token"]);
        }

        public async Task<List<JumlahItemDto>> GetJumlahPakai()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/jumlahpakai");
            return JsonSerializer.Deserialize<List<JumlahItemDto>>(jsonString)!;
        }

        public async Task<List<JumlahItemDto>> GetJumlahBeli()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/jumlahbeli");
            return JsonSerializer.Deserialize<List<JumlahItemDto>>(jsonString)!;
        }

        public async Task<List<JumlahItemDto>> GetJumlahProduksi()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/jumlahproduksi");
            return JsonSerializer.Deserialize<List<JumlahItemDto>>(jsonString)!;
        }

        public async Task<List<JumlahItemDto>> GetJumlahJual()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/jumlahjual");
            return JsonSerializer.Deserialize<List<JumlahItemDto>>(jsonString)!;
        }

        public async Task<List<GrafikDto>> GetPengeluaran()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/pengeluaran");
            return JsonSerializer.Deserialize<List<GrafikDto>>(jsonString)!;
        }

        public async Task<List<GrafikDto>> GetPendapatan()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/pendapatan");
            return JsonSerializer.Deserialize<List<GrafikDto>>(jsonString)!;
        }

        public async Task<List<Bahan>> GetStokBahanMinim()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/stokbahanminim");
            return JsonSerializer.Deserialize<List<Bahan>>(jsonString)!;
        }

        public async Task<List<Barang>> GetStokBarangMinim()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/stokbarangminim");
            return JsonSerializer.Deserialize<List<Barang>>(jsonString)!;
        }

        public async Task<List<Pembelian>> GetUtang()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/utang");
            return JsonSerializer.Deserialize<List<Pembelian>>(jsonString)!;
        }

        public async Task<List<Penjualan>> GetPiutang()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/piutang");
            return JsonSerializer.Deserialize<List<Penjualan>>(jsonString)!;
        }

        public async Task<List<BarangPopulerDto>> GetBarangPopuler()
        {
            var jsonString = await _httpClient.GetStringAsync("api/index/barangpopuler");
            return JsonSerializer.Deserialize<List<BarangPopulerDto>>(jsonString)!;
        }
    }
}