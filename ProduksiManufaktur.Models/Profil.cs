namespace ProduksiManufaktur.Models
{
    public class Profil
    {
        /// <summary>One Only</summary>
        public int Id { get; set; }

        public string Nama { get; set; } = string.Empty;
        public string Alamat { get; set; } = string.Empty;
        public string Telepon { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Pengurus { get; set; } = string.Empty;
        public string Jabatan { get; set; } = string.Empty;
        public byte[]? Logo { get; set; }
    }
}