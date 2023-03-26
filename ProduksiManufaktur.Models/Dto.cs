namespace ProduksiManufaktur.Models
{
    public class EmailDto
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = "Konfirmasi Email - Produksi Manufaktur";
        public string Body { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class JumlahItemDto
    {
        public string Nama { get; set; } = string.Empty;
        public decimal Jumlah { get; set; }
        public string Satuan { get; set; } = string.Empty;
        public int Total { get; set; }
    }

    public class GrafikDto
    {
        public int No { get; set; }
        public DateTime Tanggal { get; set; }
        public double Nominal { get; set; }
    }

    public class BarangPopulerDto
    {
        public string Label { get; set; } = string.Empty;
        public double Data { get; set; }
    }

    public class EntitasDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
    }
}