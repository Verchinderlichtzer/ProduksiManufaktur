namespace ProduksiManufaktur.Models
{
    public class PembelianDetail
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }
        public string PembelianId { get; set; } = null!;
        public int BahanSatuanId { get; set; }

        public decimal MinJumlah { get; set; }
        public decimal Jumlah { get; set; }
        public int Harga { get; set; }

        /// <summary>Ignore</summary>
        public string NamaBahan { get; set; } = string.Empty;

        /// <summary>Ignore</summary>
        public decimal JumlahSebelum { get; set; }

        /// <summary>Ignore</summary>
        public decimal StokAkhir { get; set; }

        /// <summary>Ignore</summary>
        public int Total { get; set; }

        public Pembelian? Pembelian { get; set; }
        public BahanSatuan? BahanSatuan { get; set; }
    }
}