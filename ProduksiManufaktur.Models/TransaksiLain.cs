namespace ProduksiManufaktur.Models
{
    public class TransaksiLain
    {
        /// <summary>Auto Generate</summary>
        public int Id { get; set; }

        public DateTime Tanggal { get; set; }
        public string Jenis { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
        public int Nominal { get; set; }
        public string Keterangan { get; set; } = string.Empty;

        /// <summary>Ignore</summary>
        public DateTime? InputTanggal { get; set; } = DateTime.Now.Date;

        /// <summary>Ignore</summary>
        public TimeSpan? InputWaktu { get; set; } = DateTime.Now.TimeOfDay;
    }
}