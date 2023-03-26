namespace ProduksiManufaktur.Models
{
    public class Pekerjaan
    {
        /// <summary>Auto Generate</summary>
        public int Id { get; set; }

        /// <summary>Unique</summary>
        public string Nama { get; set; } = string.Empty;

        /// <summary>Ignore</summary>
        public int JumlahKaryawan { get; set; }

        public List<Karyawan>? Karyawan { get; set; }
    }
}