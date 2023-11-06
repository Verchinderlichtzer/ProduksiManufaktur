namespace ProduksiManufaktur.Models
{
    public class PenjualanDetail
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }
        public string PenjualanId { get; set; } = null!;
        public int BarangSatuanId { get; set; }

        public decimal MinJumlah { get; set; }
        public decimal Jumlah { get; set; }
        public int Harga { get; set; }

        /// <summary>Ignore</summary>
        public string NamaBarang { get; set; } = string.Empty;

        /// <summary>Ignore</summary>
        public decimal JumlahSebelum { get; set; }

        /// <summary>Ignore</summary>
        public decimal StokAkhir { get; set; }

        /// <summary>Ignore</summary>
        public int Total { get; set; }

        public Penjualan? Penjualan { get; set; }
        public BarangSatuan? BarangSatuan { get; set; }
    }
}