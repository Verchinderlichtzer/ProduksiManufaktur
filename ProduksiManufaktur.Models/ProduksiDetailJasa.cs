namespace ProduksiManufaktur.Models
{
    public class ProduksiDetailJasa
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }

        public string ProduksiId { get; set; } = null!;
        public string KaryawanId { get; set; } = null!;

        public int Biaya { get; set; }

        public Produksi? Produksi { get; set; }
        public Karyawan? Karyawan { get; set; }
    }
}