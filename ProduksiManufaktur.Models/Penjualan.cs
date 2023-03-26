namespace ProduksiManufaktur.Models
{
    public class Penjualan
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId { get; set; } = null!;

        public DateTime Tanggal { get; set; }
        public int Subtotal { get; set; }
        public int PPN { get; set; }
        public int Terbayar { get; set; }
        public string MetodeBayar { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? JatuhTempo { get; set; } = DateTime.Now.Date.AddDays(1);
        public string Keterangan { get; set; } = string.Empty;
        public byte[] Version { get; set; } = null!;

        /// <summary>Ignore</summary>
        public DateTime? InputTanggal { get; set; } = DateTime.Now.Date;

        /// <summary>Ignore</summary>
        public TimeSpan? InputWaktu { get; set; } = DateTime.Now.TimeOfDay;

        /// <summary>Ignore</summary>
        public int HariJatuhTempo { get; set; } = 1;

        /// <summary>Ignore</summary>
        public int GrandTotal { get; set; }

        /// <summary>Ignore</summary>
        public int Sisa { get; set; }

        public Customer? Customer { get; set; }

        /// <summary>1 : 1</summary>
        public ReturPenjualan? ReturPenjualan { get; set; }

        public List<PenjualanDetail>? PenjualanDetail { get; set; }
        public List<TransaksiPenjualan>? TransaksiPenjualan { get; set; }
    }
}