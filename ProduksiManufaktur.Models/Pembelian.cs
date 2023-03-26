namespace ProduksiManufaktur.Models
{
    public class Pembelian
    {
        public string Id { get; set; } = string.Empty;
        public string SupplierId { get; set; } = null!;

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

        public Supplier? Supplier { get; set; }

        /// <summary>1 : 1</summary>
        public ReturPembelian? ReturPembelian { get; set; }

        public List<PembelianDetail>? PembelianDetail { get; set; }
        public List<TransaksiPembelian>? TransaksiPembelian { get; set; }
    }
}