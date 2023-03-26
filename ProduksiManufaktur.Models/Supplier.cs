namespace ProduksiManufaktur.Models
{
    public class Supplier
    {
        public string Id { get; set; } = string.Empty;

        public string Nama { get; set; } = string.Empty;
        public string Alamat { get; set; } = string.Empty;

        /// <summary>Unique</summary>
        public string? Telepon { get; set; }

        /// <summary>Unique</summary>
        public string? Fax { get; set; }

        /// <summary>Unique</summary>
        public string? Email { get; set; }

        public List<Pembelian>? Pembelian { get; set; }
    }
}