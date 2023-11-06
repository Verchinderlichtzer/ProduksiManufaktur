using Microsoft.AspNetCore.Identity;

namespace ProduksiManufaktur.Models
{
    public class User : IdentityUser
    {
        public string Alamat { get; set; } = string.Empty;
        public string TempatLahir { get; set; } = string.Empty;
        public DateTime TanggalLahir { get; set; }

        /// <summary>Ignored</summary>
        public DateTime? InputTanggalLahir { get; set; } = DateTime.Now.Date;

        /// <summary>Ignored</summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>Ignored</summary>
        public string PasswordBaru { get; set; } = string.Empty;

        /// <summary>Ignored</summary>
        public string KonfirmasiPasswordBaru { get; set; } = string.Empty;

        /// <summary>Ignored</summary>
        public string Roles { get; set; } = string.Empty;

        public List<LogTransaksi>? LogTransaksi { get; set; }
        public List<UserClaim>? UserClaim { get; set; }
        public List<UserRole>? UserRole { get; set; }
        public List<UserLogin>? UserLogin { get; set; }
        public List<UserToken>? UserToken { get; set; }
    }

    public class UserClaim : IdentityUserClaim<string>
    {
        public User? User { get; set; }
    }

    public class UserRole : IdentityUserRole<string>
    {
        /// <summary>Ignored</summary>
        public string RoleName { get; set; } = string.Empty;

        public User? User { get; set; }
        public Role? Role { get; set; }
    }

    public class UserLogin : IdentityUserLogin<string>
    {
        public User? User { get; set; }
    }

    public class UserToken : IdentityUserToken<string>
    {
        public User? User { get; set; }
    }

    public class Role : IdentityRole
    {
        /// <summary>Ignored</summary>
        public int ClaimNoAccess { get; set; }

        /// <summary>Ignored</summary>
        public int ClaimRead { get; set; }

        /// <summary>Ignored</summary>
        public int ClaimWrite { get; set; }

        /// <summary>Ignored</summary>
        public int JumlahUser { get; set; }

        public List<UserRole>? UserRole { get; set; }
        public List<RoleClaim>? RoleClaim { get; set; }
    }

    public class RoleClaim : IdentityRoleClaim<string>
    {
        public Role? Role { get; set; }
    }
}