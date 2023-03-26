namespace ProduksiManufaktur.Models
{
    public class Barang
    {
        public string Id { get; set; } = string.Empty;

        public string Nama { get; set; } = string.Empty;
        public string SatuanProduksi { get; set; } = string.Empty;
        public decimal StokAwal { get; set; }
        public decimal Stok { get; set; }
        public decimal StokMinimal { get; set; }
        public byte[] Version { get; set; } = null!;

        public List<BarangSatuan>? BarangSatuan { get; set; }
        public List<Produksi>? Produksi { get; set; }
        public List<Formulasi>? Formulasi { get; set; }
        public List<PerubahanStokBarang>? PerubahanStokBarang { get; set; }
    }
}