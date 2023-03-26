namespace ProduksiManufaktur.Models
{
    public class Produksi
    {
        public string Id { get; set; } = string.Empty;
        public string BarangId { get; set; } = null!;

        public DateTime Tanggal { get; set; }
        public decimal Jumlah { get; set; } = 1;
        public string Keterangan { get; set; } = string.Empty;
        public int BiayaJasa { get; set; }
        public int BiayaOverhead { get; set; }
        public byte[] Version { get; set; } = null!;

        /// <summary>Ignore</summary>
        public DateTime? InputTanggal { get; set; } = DateTime.Now.Date;

        /// <summary>Ignore</summary>
        public TimeSpan? InputWaktu { get; set; } = DateTime.Now.TimeOfDay;

        /// <summary>Ignore</summary>
        public decimal JumlahSebelum { get; set; }

        /// <summary>Ignore</summary>
        public decimal StokAkhir { get; set; }

        /// <summary>Ignore</summary>
        public int Total { get; set; }

        /// <summary>Ignore</summary>
        public decimal JumlahTerkunci { get; set; }

        public Barang? Barang { get; set; }
        public List<ProduksiDetailJasa>? ProduksiDetailJasa { get; set; }
        public List<ProduksiDetailBahan>? ProduksiDetailBahan { get; set; }
        public List<ProduksiDetailOverhead>? ProduksiDetailOverhead { get; set; }
    }
}