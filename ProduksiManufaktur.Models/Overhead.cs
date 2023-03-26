namespace ProduksiManufaktur.Models
{
    public class Overhead
    {
        /// <summary>Auto Generate</summary>
        public int Id { get; set; }

        /// <summary>Unique</summary>
        public string Nama { get; set; } = string.Empty;

        public List<ProduksiDetailOverhead>? ProduksiDetailOverhead { get; set; }
    }
}