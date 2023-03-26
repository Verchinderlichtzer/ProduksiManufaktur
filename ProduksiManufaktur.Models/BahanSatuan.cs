namespace ProduksiManufaktur.Models
{
    public class BahanSatuan
    {
        /// <summary>Auto Generate</summary>
        public int Id { get; set; }

        public string BahanId { get; set; } = null!;

        public string Nama { get; set; } = string.Empty;
        public string Ukuran { get; set; } = string.Empty;
        public int Harga { get; set; }
        public decimal KonversiStok { get; set; }

        public List<PembelianDetail>? PembelianDetail { get; set; }
        public List<ReturPembelianDetail>? ReturPembelianDetail { get; set; }
        public Bahan? Bahan { get; set; }
    }
}