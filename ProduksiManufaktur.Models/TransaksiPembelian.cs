namespace ProduksiManufaktur.Models
{
    public class TransaksiPembelian
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }
        public string PembelianId { get; set; } = null!;

        public DateTime Tanggal { get; set; }
        public string Keterangan { get; set; } = string.Empty;
        public int Nominal { get; set; }
        public byte[] Version { get; set; } = null!;

        /// <summary>Ignore</summary>
        public DateTime? InputTanggal { get; set; } = DateTime.Now.Date;

        /// <summary>Ignore</summary>
        public TimeSpan? InputWaktu { get; set; } = DateTime.Now.TimeOfDay;

        /// <summary>Ignore</summary>
        public int NominalSebelum { get; set; }

        /// <summary>Ignore</summary>
        public int Balance { get; set; }

        /// <summary>Ignore</summary>
        public string BalanceLabel { get; set; } = "Sisa";

        public Pembelian? Pembelian { get; set; }
    }
}