namespace ProduksiManufaktur.Models
{
    public class ReturPenjualan
    {
        public string Id { get; set; } = string.Empty;
        public string PenjualanId { get; set; } = null!;

        public DateTime Tanggal { get; set; }
        public int GrandTotal { get; set; }
        public string Keterangan { get; set; } = string.Empty;
        public byte[] Version { get; set; } = null!;

        /// <summary>Ignore</summary>
        public DateTime? InputTanggal { get; set; } = DateTime.Now.Date;

        /// <summary>Ignore</summary>
        public TimeSpan? InputWaktu { get; set; } = DateTime.Now.TimeOfDay;

        /// <summary>1 : 1</summary>
        public Penjualan? Penjualan { get; set; }

        public List<ReturPenjualanDetail>? ReturPenjualanDetail { get; set; }
    }
}