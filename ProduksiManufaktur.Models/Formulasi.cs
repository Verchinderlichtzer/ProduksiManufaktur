namespace ProduksiManufaktur.Models
{
    public class Formulasi
    {
        public string Id { get; set; } = string.Empty;
        public string BarangId { get; set; } = null!;

        public decimal Jumlah { get; set; }

        public Barang? Barang { get; set; }
        public List<FormulasiDetail>? FormulasiDetail { get; set; }
    }
}