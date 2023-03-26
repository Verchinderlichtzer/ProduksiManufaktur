namespace ProduksiManufaktur.Models
{
    public class Karyawan
    {
        public string Id { get; set; } = string.Empty;
        public int PekerjaanId { get; set; }

        public string Nama { get; set; } = string.Empty;
        public string TempatLahir { get; set; } = string.Empty;
        public DateTime TanggalLahir { get; set; }
        public string Alamat { get; set; } = string.Empty;

        /// <summary>Unique</summary>
        public string? Telepon { get; set; }

        /// <summary>Unique</summary>
        public string? Email { get; set; }

        public int Upah { get; set; }

        /// <summary>Ignore</summary>
        public DateTime? InputTanggalLahir { get; set; } = DateTime.Now.Date;

        public Pekerjaan? Pekerjaan { get; set; }
        public List<ProduksiDetailJasa>? ProduksiDetailJasa { get; set; }
    }
}