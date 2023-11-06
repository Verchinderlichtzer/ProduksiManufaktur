namespace ProduksiManufaktur.Models
{
    public class PerubahanStokBahan
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }
        public string BahanId { get; set; } = null!;

        public DateTime Tanggal { get; set; }
        public string Jenis { get; set; } = string.Empty;
        public decimal Jumlah { get; set; }
        public string Keterangan { get; set; } = string.Empty;

        /// <summary>Ignore</summary>
        public DateTime? InputTanggal { get; set; } = DateTime.Now.Date;

        /// <summary>Ignore</summary>
        public TimeSpan? InputWaktu { get; set; } = DateTime.Now.TimeOfDay;

        /// <summary>Ignore</summary>
        public string JenisSebelum { get; set; } = string.Empty;

        /// <summary>Ignore</summary>
        public decimal JumlahSebelum { get; set; }

        public Bahan? Bahan { get; set; }
    }
}