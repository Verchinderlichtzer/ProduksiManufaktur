namespace ProduksiManufaktur.Models
{
    public class FormulasiDetail
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }

        public string FormulasiId { get; set; } = null!;
        public string BahanId { get; set; } = null!;

        public decimal Jumlah { get; set; } = 1;

        public Formulasi? Formulasi { get; set; }
        public Bahan? Bahan { get; set; }
    }
}