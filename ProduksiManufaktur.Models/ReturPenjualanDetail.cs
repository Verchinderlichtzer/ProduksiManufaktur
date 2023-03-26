namespace ProduksiManufaktur.Models
{
    public class ReturPenjualanDetail
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }

        public int BarangSatuanId { get; set; }
        public string ReturPenjualanId { get; set; } = null!;

        public decimal MaxJumlah { get; set; }
        public decimal Jumlah { get; set; }
        public int Harga { get; set; }

        /// <summary>Ignore</summary>
        public int Total { get; set; }

        public BarangSatuan? BarangSatuan { get; set; }
        public ReturPenjualan? ReturPenjualan { get; set; }
    }
}