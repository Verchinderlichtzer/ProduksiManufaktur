namespace ProduksiManufaktur.Models
{
    public class ProduksiDetailOverhead
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }

        public string ProduksiId { get; set; } = null!;
        public int OverheadId { get; set; }

        public int Biaya { get; set; }

        public Produksi? Produksi { get; set; }
        public Overhead? Overhead { get; set; }
    }
}