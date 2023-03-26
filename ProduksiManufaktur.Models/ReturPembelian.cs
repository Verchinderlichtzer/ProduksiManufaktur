namespace ProduksiManufaktur.Models
{
    public class ReturPembelian
    {
        public string Id { get; set; } = string.Empty;
        public string PembelianId { get; set; } = null!;

        public DateTime Tanggal { get; set; }
        public int GrandTotal { get; set; }
        public string Keterangan { get; set; } = string.Empty;
        public byte[] Version { get; set; } = null!;

        /// <summary>Ignore</summary>
        public DateTime? InputTanggal { get; set; } = DateTime.Now.Date;

        /// <summary>Ignore</summary>
        public TimeSpan? InputWaktu { get; set; } = DateTime.Now.TimeOfDay;

        /// <summary>1 : 1</summary>
        public Pembelian? Pembelian { get; set; }

        public List<ReturPembelianDetail>? ReturPembelianDetail { get; set; }
    }
}