namespace ProduksiManufaktur.Models
{
    public class ProduksiDetailBahan
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }
        public string ProduksiId { get; set; } = null!;
        public string BahanId { get; set; } = null!;

        public decimal Jumlah { get; set; } = 1;

        /// <summary>Ignore</summary>
        public decimal JumlahSebelum { get; set; }

        /// <summary>Ignore</summary>
        public decimal StokAkhir { get; set; }

        /// <summary>Ignore</summary>
        public decimal JumlahTerkunci { get; set; }

        public Produksi? Produksi { get; set; }
        public Bahan? Bahan { get; set; }
    }
}