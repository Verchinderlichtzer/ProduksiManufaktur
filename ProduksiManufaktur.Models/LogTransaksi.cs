namespace ProduksiManufaktur.Models
{
    public class LogTransaksi
    {
        /// <summary>No Auto Generate</summary>
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public DateTime Tanggal { get; set; }
        public string Entitas { get; set; } = string.Empty;
        public string EntitasId { get; set; } = string.Empty;
        public string Keterangan { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}