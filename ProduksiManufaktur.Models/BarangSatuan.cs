namespace ProduksiManufaktur.Models
{
    public class BarangSatuan
    {
        /// <summary>Auto Generate</summary>
        public int Id { get; set; }
        public string BarangId { get; set; } = null!;

        public string Nama { get; set; } = string.Empty;
        public string Ukuran { get; set; } = string.Empty;
        public int Harga { get; set; }
        public decimal KonversiStok { get; set; }

        public List<PenjualanDetail>? PenjualanDetail { get; set; }
        public List<ReturPenjualanDetail>? ReturPenjualanDetail { get; set; }
        public Barang? Barang { get; set; }
    }
}