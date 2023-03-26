namespace ProduksiManufaktur.Models
{
    public class Bahan
    {
        public string Id { get; set; } = string.Empty;

        public string Nama { get; set; } = string.Empty;
        public string SatuanProduksi { get; set; } = string.Empty;
        public decimal StokAwal { get; set; }
        public decimal Stok { get; set; }
        public decimal StokMinimal { get; set; }
        public byte[] Version { get; set; } = null!;

        public List<BahanSatuan>? BahanSatuan { get; set; }
        public List<ProduksiDetailBahan>? ProduksiDetailBahan { get; set; }
        public List<FormulasiDetail>? FormulasiDetail { get; set; }
        public List<PerubahanStokBahan>? PerubahanStokBahan { get; set; }
    }
}