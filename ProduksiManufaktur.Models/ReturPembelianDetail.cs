namespace ProduksiManufaktur.Models
{
    public class ReturPembelianDetail
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }
        public int BahanSatuanId { get; set; }
        public string ReturPembelianId { get; set; } = null!;

        public decimal MaxJumlah { get; set; }
        public decimal Jumlah { get; set; }
        public int Harga { get; set; }

        /// <summary>Ignore</summary>
        public int Total { get; set; }

        public BahanSatuan? BahanSatuan { get; set; }
        public ReturPembelian? ReturPembelian { get; set; }
    }
}