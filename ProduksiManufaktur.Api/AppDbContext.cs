using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProduksiManufaktur.Api
{
    public class AppDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region DbSet

        public DbSet<LogTransaksi> LogTransaksi { get; set; }
        public DbSet<Bahan> Bahan { get; set; }
        public DbSet<BahanSatuan> BahanSatuan { get; set; }
        public DbSet<PerubahanStokBahan> PerubahanStokBahan { get; set; }
        public DbSet<Barang> Barang { get; set; }
        public DbSet<BarangSatuan> BarangSatuan { get; set; }
        public DbSet<PerubahanStokBarang> PerubahanStokBarang { get; set; }
        public DbSet<Formulasi> Formulasi { get; set; }
        public DbSet<FormulasiDetail> FormulasiDetail { get; set; }
        public DbSet<Pekerjaan> Pekerjaan { get; set; }
        public DbSet<Karyawan> Karyawan { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Overhead> Overhead { get; set; }
        public DbSet<Pembelian> Pembelian { get; set; }
        public DbSet<PembelianDetail> PembelianDetail { get; set; }
        public DbSet<TransaksiPembelian> TransaksiPembelian { get; set; }
        public DbSet<Penjualan> Penjualan { get; set; }
        public DbSet<PenjualanDetail> PenjualanDetail { get; set; }
        public DbSet<TransaksiPenjualan> TransaksiPenjualan { get; set; }
        public DbSet<Produksi> Produksi { get; set; }
        public DbSet<ProduksiDetailBahan> ProduksiDetailBahan { get; set; }
        public DbSet<ProduksiDetailJasa> ProduksiDetailJasa { get; set; }
        public DbSet<ProduksiDetailOverhead> ProduksiDetailOverhead { get; set; }
        public DbSet<TransaksiLain> TransaksiLain { get; set; }
        public DbSet<ReturPembelian> ReturPembelian { get; set; }
        public DbSet<ReturPembelianDetail> ReturPembelianDetail { get; set; }
        public DbSet<ReturPenjualan> ReturPenjualan { get; set; }
        public DbSet<ReturPenjualanDetail> ReturPenjualanDetail { get; set; }
        public DbSet<Profil> Profil { get; set; }

        #endregion DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Model Configuration

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.Ignore(x => x.InputTanggalLahir);
                e.Ignore(x => x.Password);
                e.Ignore(x => x.PasswordBaru);
                e.Ignore(x => x.KonfirmasiPasswordBaru);
                e.Ignore(x => x.Roles);
            });

            modelBuilder.Entity<UserClaim>(e =>
            {
                e.ToTable("UserClaim");
                e.HasOne(x => x.User).WithMany(x => x.UserClaim).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<UserRole>(e =>
            {
                e.ToTable("UserRole");
                e.HasOne(x => x.User).WithMany(x => x.UserRole).HasForeignKey(x => x.UserId);
                e.HasOne(x => x.Role).WithMany(x => x.UserRole).HasForeignKey(x => x.RoleId);
                e.Ignore(x => x.RoleName);
            });

            modelBuilder.Entity<UserLogin>(e =>
            {
                e.ToTable("UserLogin");
                e.HasOne(x => x.User).WithMany(x => x.UserLogin).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<UserToken>(e =>
            {
                e.ToTable("UserToken");
                e.HasOne(x => x.User).WithMany(x => x.UserToken).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("Role");
                e.Ignore(x => x.ClaimNoAccess);
                e.Ignore(x => x.ClaimRead);
                e.Ignore(x => x.ClaimWrite);
                e.Ignore(x => x.JumlahUser);
            });

            modelBuilder.Entity<RoleClaim>(e =>
            {
                e.ToTable("RoleClaim");
                e.HasOne(x => x.Role).WithMany(x => x.RoleClaim).HasForeignKey(x => x.RoleId);
            });

            modelBuilder.Entity<LogTransaksi>(e =>
            {
                e.HasOne(x => x.User).WithMany(x => x.LogTransaksi).HasForeignKey(x => x.UserId);
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.Tanggal).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Bahan>(e =>
            {
                e.Property(x => x.StokAwal).HasPrecision(9, 2);
                e.Property(x => x.Stok).HasPrecision(9, 2);
                e.Property(x => x.StokMinimal).HasPrecision(9, 2);
                e.Property(x => x.Version).IsRowVersion();
            });

            modelBuilder.Entity<BahanSatuan>(e =>
            {
                e.HasOne(x => x.Bahan).WithMany(x => x.BahanSatuan).HasForeignKey(x => x.BahanId);
                e.Property(x => x.KonversiStok).HasPrecision(9, 2);
                e.HasIndex(x => new { x.BahanId, x.Nama, x.Ukuran }).IsUnique();
            });

            modelBuilder.Entity<PerubahanStokBahan>(e =>
            {
                e.HasOne(x => x.Bahan).WithMany(x => x.PerubahanStokBahan).HasForeignKey(x => x.BahanId);
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
                e.Ignore(x => x.JenisSebelum);
                e.Ignore(x => x.JumlahSebelum);
            });

            modelBuilder.Entity<Barang>(e =>
            {
                e.Property(x => x.StokAwal).HasPrecision(9, 2);
                e.Property(x => x.Stok).HasPrecision(9, 2);
                e.Property(x => x.StokMinimal).HasPrecision(9, 2);
                e.Property(x => x.Version).IsRowVersion();
            });

            modelBuilder.Entity<BarangSatuan>(e =>
            {
                e.HasOne(x => x.Barang).WithMany(x => x.BarangSatuan).HasForeignKey(x => x.BarangId);
                e.Property(x => x.KonversiStok).HasPrecision(9, 2);
                e.HasIndex(x => new { x.BarangId, x.Nama, x.Ukuran }).IsUnique();
            });

            modelBuilder.Entity<PerubahanStokBarang>(e =>
            {
                e.HasOne(x => x.Barang).WithMany(x => x.PerubahanStokBarang).HasForeignKey(x => x.BarangId);
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
                e.Ignore(x => x.JumlahSebelum);
            });

            modelBuilder.Entity<Formulasi>(e =>
            {
                e.HasOne(x => x.Barang).WithMany(x => x.Formulasi).HasForeignKey(x => x.BarangId);
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
            });

            modelBuilder.Entity<FormulasiDetail>(e =>
            {
                e.HasOne(x => x.Formulasi).WithMany(x => x.FormulasiDetail).HasForeignKey(x => x.FormulasiId);
                e.HasOne(x => x.Bahan).WithMany(x => x.FormulasiDetail).HasForeignKey(x => x.BahanId);
                e.HasIndex(x => new { x.FormulasiId, x.BahanId }).IsUnique();
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
            });

            modelBuilder.Entity<Pekerjaan>(e =>
            {
                e.HasIndex(x => x.Nama).IsUnique();
                e.Ignore(x => x.JumlahKaryawan);
            });

            modelBuilder.Entity<Karyawan>(e =>
            {
                e.HasOne(x => x.Pekerjaan).WithMany(x => x.Karyawan).HasForeignKey(x => x.PekerjaanId);
                e.HasIndex(x => x.Telepon).IsUnique();
                e.HasIndex(x => x.Email).IsUnique();
                e.Ignore(x => x.InputTanggalLahir);
            });

            modelBuilder.Entity<Supplier>(e =>
            {
                e.HasIndex(x => x.Telepon).IsUnique();
                e.HasIndex(x => x.Fax).IsUnique();
                e.HasIndex(x => x.Email).IsUnique();
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.HasIndex(x => x.Telepon).IsUnique();
                e.HasIndex(x => x.Fax).IsUnique();
                e.HasIndex(x => x.Email).IsUnique();
            });

            modelBuilder.Entity<Overhead>(e => e.HasIndex(x => x.Nama).IsUnique());

            modelBuilder.Entity<Pembelian>(e =>
            {
                e.HasOne(x => x.Supplier).WithMany(x => x.Pembelian).HasForeignKey(x => x.SupplierId);
                e.Property(x => x.Version).IsRowVersion();
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
                e.Ignore(x => x.HariJatuhTempo);
                e.Ignore(x => x.GrandTotal);
                e.Ignore(x => x.Sisa);
            });

            modelBuilder.Entity<PembelianDetail>(e =>
            {
                e.HasOne(x => x.Pembelian).WithMany(x => x.PembelianDetail).HasForeignKey(x => x.PembelianId);
                e.HasOne(x => x.BahanSatuan).WithMany(x => x.PembelianDetail).HasForeignKey(x => x.BahanSatuanId);
                e.HasIndex(x => new { x.PembelianId, x.BahanSatuanId }).IsUnique();
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.MinJumlah).HasPrecision(9, 2);
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
                e.Ignore(x => x.NamaBahan);
                e.Ignore(x => x.JumlahSebelum);
                e.Ignore(x => x.StokAkhir);
                e.Ignore(x => x.Total);
            });

            modelBuilder.Entity<TransaksiPembelian>(e =>
            {
                e.HasOne(x => x.Pembelian).WithMany(x => x.TransaksiPembelian).HasForeignKey(x => x.PembelianId);
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.Version).IsRowVersion();
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
                e.Ignore(x => x.NominalSebelum);
                e.Ignore(x => x.Balance);
                e.Ignore(x => x.BalanceLabel);
            });

            modelBuilder.Entity<Penjualan>(e =>
            {
                e.HasOne(x => x.Customer).WithMany(x => x.Penjualan).HasForeignKey(x => x.CustomerId);
                e.Property(x => x.Version).IsRowVersion();
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
                e.Ignore(x => x.HariJatuhTempo);
                e.Ignore(x => x.GrandTotal);
                e.Ignore(x => x.Sisa);
            });

            modelBuilder.Entity<PenjualanDetail>(e =>
            {
                e.HasOne(x => x.Penjualan).WithMany(x => x.PenjualanDetail).HasForeignKey(x => x.PenjualanId);
                e.HasOne(x => x.BarangSatuan).WithMany(x => x.PenjualanDetail).HasForeignKey(x => x.BarangSatuanId);
                e.HasIndex(x => new { x.PenjualanId, x.BarangSatuanId }).IsUnique();
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.MinJumlah).HasPrecision(9, 2);
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
                e.Ignore(x => x.NamaBarang);
                e.Ignore(x => x.JumlahSebelum);
                e.Ignore(x => x.StokAkhir);
                e.Ignore(x => x.Total);
            });

            modelBuilder.Entity<TransaksiPenjualan>(e =>
            {
                e.HasOne(x => x.Penjualan).WithMany(x => x.TransaksiPenjualan).HasForeignKey(x => x.PenjualanId);
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.Version).IsRowVersion();
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
                e.Ignore(x => x.NominalSebelum);
                e.Ignore(x => x.Balance);
                e.Ignore(x => x.BalanceLabel);
            });

            modelBuilder.Entity<Produksi>(e =>
            {
                e.HasOne(x => x.Barang).WithMany(x => x.Produksi).HasForeignKey(x => x.BarangId);
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
                e.Property(x => x.Version).IsRowVersion();
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
                e.Ignore(x => x.JumlahSebelum);
                e.Ignore(x => x.StokAkhir);
                e.Ignore(x => x.Total);
                e.Ignore(x => x.JumlahTerkunci);
            });

            modelBuilder.Entity<ProduksiDetailBahan>(e =>
            {
                e.HasOne(x => x.Produksi).WithMany(x => x.ProduksiDetailBahan).HasForeignKey(x => x.ProduksiId);
                e.HasOne(x => x.Bahan).WithMany(x => x.ProduksiDetailBahan).HasForeignKey(x => x.BahanId);
                e.HasIndex(x => new { x.ProduksiId, x.BahanId }).IsUnique();
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
                e.Ignore(x => x.JumlahSebelum);
                e.Ignore(x => x.StokAkhir);
                e.Ignore(x => x.JumlahTerkunci);
            });

            modelBuilder.Entity<ProduksiDetailJasa>(e =>
            {
                e.HasOne(x => x.Produksi).WithMany(x => x.ProduksiDetailJasa).HasForeignKey(x => x.ProduksiId);
                e.HasOne(x => x.Karyawan).WithMany(x => x.ProduksiDetailJasa).HasForeignKey(x => x.KaryawanId);
                e.HasIndex(x => new { x.ProduksiId, x.KaryawanId }).IsUnique();
                e.Property(x => x.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProduksiDetailOverhead>(e =>
            {
                e.HasOne(x => x.Produksi).WithMany(x => x.ProduksiDetailOverhead).HasForeignKey(x => x.ProduksiId);
                e.HasOne(x => x.Overhead).WithMany(x => x.ProduksiDetailOverhead).HasForeignKey(x => x.OverheadId);
                e.HasIndex(x => new { x.ProduksiId, x.OverheadId }).IsUnique();
                e.Property(x => x.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<TransaksiLain>(e =>
            {
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
            });

            modelBuilder.Entity<ReturPembelian>(e =>
            {
                e.HasOne(x => x.Pembelian).WithOne(x => x.ReturPembelian).HasForeignKey<ReturPembelian>(x => x.PembelianId);
                e.Property(x => x.Version).IsRowVersion();
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
            });

            modelBuilder.Entity<ReturPembelianDetail>(e =>
            {
                e.HasOne(x => x.ReturPembelian).WithMany(x => x.ReturPembelianDetail).HasForeignKey(x => x.ReturPembelianId);
                e.HasOne(x => x.BahanSatuan).WithMany(x => x.ReturPembelianDetail).HasForeignKey(x => x.BahanSatuanId);
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.MaxJumlah).HasPrecision(9, 2);
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
                e.Ignore(x => x.Total);
            });

            modelBuilder.Entity<ReturPenjualan>(e =>
            {
                e.HasOne(x => x.Penjualan).WithOne(x => x.ReturPenjualan).HasForeignKey<ReturPenjualan>(x => x.PenjualanId);
                e.Property(x => x.Version).IsRowVersion();
                e.Ignore(x => x.InputTanggal);
                e.Ignore(x => x.InputWaktu);
            });

            modelBuilder.Entity<ReturPenjualanDetail>(e =>
            {
                e.HasOne(x => x.ReturPenjualan).WithMany(x => x.ReturPenjualanDetail).HasForeignKey(x => x.ReturPenjualanId);
                e.HasOne(x => x.BarangSatuan).WithMany(x => x.ReturPenjualanDetail).HasForeignKey(x => x.BarangSatuanId);
                e.Property(x => x.Id).ValueGeneratedNever();
                e.Property(x => x.MaxJumlah).HasPrecision(9, 2);
                e.Property(x => x.Jumlah).HasPrecision(9, 2);
                e.Ignore(x => x.Total);
            });

            #endregion Model Configuration

            //#region Data Initializer

            //modelBuilder.Entity<User>().HasData(
            //new User
            //{
            //    Id = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    Alamat = "Perumahan Bumi Anggrek Blok K No 80",
            //    TempatLahir = "Bekasi",
            //    TanggalLahir = new DateTime(2002, 6, 11),
            //    UserName = "sujudihanif36@gmail.com",
            //    NormalizedUserName = "SUJUDIHANIF36@GMAIL.COM",
            //    Email = "sujudihanif36@gmail.com",
            //    NormalizedEmail = "SUJUDIHANIF36@GMAIL.COM",
            //    EmailConfirmed = true,
            //    PasswordHash = "AQAAAAIAAYagAAAAEDF80p+z0OgQH9yZ5j+z6W8bphkX52+PUhg4nz7H+9mLTqn1bdsExZqQtkeJYeQLtg==",
            //    SecurityStamp = "5ZI4DZQUJY2FPYAH2T4S4DFM7GCOBAKL",
            //    ConcurrencyStamp = "67d704f0-db9f-43d9-ac27-8c1937ae7d81",
            //    PhoneNumber = "085739194810",
            //    PhoneNumberConfirmed = true,
            //    TwoFactorEnabled = false,
            //    LockoutEnabled = false,
            //    AccessFailedCount = 0
            //},
            ////new User
            ////{
            ////    Id = "53399911-ca6c-426f-bfb3-ac49ea29e020",
            ////    Alamat = "Bandung",
            ////    TempatLahir = "Surabaya",
            ////    TanggalLahir = new DateTime(2001, 4, 22),
            ////    UserName = "sujudihanif9@gmail.com",
            ////    NormalizedUserName = "SUJUDIHANIF9@GMAIL.COM",
            ////    Email = "sujudihanif9@gmail.com",
            ////    NormalizedEmail = "SUJUDIHANIF9@GMAIL.COM",
            ////    EmailConfirmed = true,
            ////    PasswordHash = "AQAAAAIAAYagAAAAEBiGPo90IEhE+ZDugGocqhp4dlUbWCS58DttBu0HTlbF1e8WpjtqPYjJvkf2rAeFEQ==",
            ////    SecurityStamp = "JBGNWUZUJOB24OPZJCBZV6A74CAK6MGM",
            ////    ConcurrencyStamp = "6caad73a-17b8-4091-a61f-57347251ec4b",
            ////    PhoneNumber = "084722659281",
            ////    PhoneNumberConfirmed = true,
            ////    TwoFactorEnabled = false,
            ////    LockoutEnabled = false,
            ////    AccessFailedCount = 0
            ////},
            //new User
            //{
            //    Id = "d6ca57a9-dbdf-48d5-867b-7e5431ea4018",
            //    Alamat = "Jakarta",
            //    TempatLahir = "Semarang",
            //    TanggalLahir = new DateTime(2000, 2, 5),
            //    UserName = "sujudihanif@yahoo.co.id",
            //    NormalizedUserName = "SUJUDIHANIF@YAHOO.CO.ID",
            //    Email = "sujudihanif@yahoo.co.id",
            //    NormalizedEmail = "SUJUDIHANIF@YAHOO.CO.ID",
            //    EmailConfirmed = false,
            //    PasswordHash = "AQAAAAIAAYagAAAAEIrKzVjcLeG32l2jA5tlZ4ISF/4Y3H7F0DlJdf5Hqyj0QeXRy9Lbe8+0smI/eUmpng==",
            //    SecurityStamp = "TQ5AE6UTHWKH7DI4N32ANGT2NZWM64SS",
            //    ConcurrencyStamp = "005e218f-6b61-4c45-9f4f-daf314279e4a",
            //    PhoneNumber = "085372842236",
            //    PhoneNumberConfirmed = true,
            //    TwoFactorEnabled = false,
            //    LockoutEnabled = false,
            //    AccessFailedCount = 0
            //});

            //modelBuilder.Entity<UserClaim>().HasData(
            //new UserClaim
            //{
            //    Id = 1,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Akun",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 2,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Produk",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 3,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Pekerja",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 4,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Pihak",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 5,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Overhead",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 6,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Pembelian",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 7,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Penjualan",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 8,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Produksi",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 9,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "TransaksiLain",
            //    ClaimValue = "S2"
            //},
            //new UserClaim
            //{
            //    Id = 10,
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    ClaimType = "Report",
            //    ClaimValue = "S1"
            //});

            //modelBuilder.Entity<Role>().HasData(
            //new Role
            //{
            //    Id = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    Name = "Admin",
            //    NormalizedName = "ADMIN",
            //    ConcurrencyStamp = "79bbaf13-c30b-4b13-827b-2be942a11545"
            //});

            //modelBuilder.Entity<RoleClaim>().HasData(
            //new RoleClaim
            //{
            //    Id = 1,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Akun",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 2,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Produk",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 3,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Pekerja",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 4,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Pihak",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 5,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Overhead",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 6,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Pembelian",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 7,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Penjualan",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 8,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Produksi",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 9,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "TransaksiLain",
            //    ClaimValue = "W2"
            //},
            //new RoleClaim
            //{
            //    Id = 10,
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710",
            //    ClaimType = "Report",
            //    ClaimValue = "W1"
            //});

            //modelBuilder.Entity<UserRole>().HasData(
            //new UserRole
            //{
            //    UserId = "4b35caf0-9371-447b-ab23-30ff5ef8586c",
            //    RoleId = "98b79864-fff9-46e0-9cf4-e58c8c782710"
            //});

            //modelBuilder.Entity<Profil>().HasData(
            //new Profil
            //{
            //    Id = 1,
            //    Nama = "Hanif Manufaktur",
            //    Alamat = "Perumahan Bumi Anggrek Blok K No 80",
            //    Telepon = "085739194810",
            //    Fax = "021-1234567",
            //    Email = "example@gmail.com",
            //    Website = "www.hanifmanufaktur.com",
            //    Pengurus = "Mahdiya",
            //    Jabatan = "Direktur",
            //    Logo = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAYAAABw4pVUAAAABmJLR0QA/wD/AP+gvaeTAAAKoklEQVRYCe1ZC2wUxxn+ds+vexulBqeg3DkGUvtSSgNNqgQICS2Yh6ClICgJUqBNKVEQkSBVpKDS0KqN0idBSU2CiVLUEAptCgQCmIcSqgoUNTUVxhQEHG8DBgPGgKnvtv/M3d7Dt3e3u3eG83lOMzuz//z/v/98383M7gwgfgIBgYBAQCAgEBAICAQEAgIBgYBAQCAgEBAICAQEAgIBgYBAQCAgEBAIZBkBKcv+eqQ7r9dXo0hYQsEPpWynbCa1k1GDpODnfn/jdqqbSr2eEM+DVROgyFtMoZfEiEipMUtKQRKfvUasKPJS9q9c+HwFZk65H8XFsqm+d3QE8cFH57Cizg9Fkn4GwNQoMfd0elq+JCKjivVl+uRy02Qwe0bkDCKU1QGlOlQav/Z6QhSgicG2flMzOoN0x25MZGa7buP5sKV0KFwxXNAfxLBNXhnk2hpiySt0TXTmWmvL0T6lfffTvP8Q/Tv7m3ARMaHx9bms4AdmF3TmiGJghcj06luuSDhf5i7BvjcmGQLksZc3o+V6B2RI5SdOHLxgyLiLcq9fQ1Q8FEXqw+puWyErDGW3rYjrqz74jcmLICQCnFTKqk4ThDjDhACd3AfzYzYLQlTkLJ18hLisoX+7KtZTusIk5vwI8VT49ni8vl16OnXvdWT+73ZZjU9Zqo1kCY2yTPrS3V/qo9FzXhs4Iep6YATU6LqTOSFiylKRV0JgqtOPKtZTRqe5ICdVj00yHUFIBBmFg+kwMWVFbUKkRlyaqAhCoqBxQkxNWfbwuqOA+4i6NF4ThIQxkxD6d5ubssKEIDTKwi5NFd29qCcNir2BQUHwpL9xTFKlu9qglLPHLXhnHxawikauHFCGnbWLeMuYH/8Gx8+0gMmWzptMsv2UQx+XVDGd7hkhFHFOvYHRPpSVYsLff/8ivjZoAKumzLtqF0faDxw5rdbFlKUikYWSg+l2cF4MuXOFbYIQa4gh4NIoc0Jc9pI0aonNbnuIRPrk4j4SNfRLxKIewoqwRKkkSVD/7SGxvqvLESGREcJ86TPU0MrKGuLz+YputGMPfZU/rvEMtP20lqZoYE793/B589k4lVRtcYp34UYmQqDwUA09rcBiga2kCDdv3yks8/nslxobbxhyEKOcFULa2vES9UWTjJhnIZDiiDRVW6yP7qwHgkGs3vhP/GjqSP6YlX/9DK+/t5XX9V6KOixslJgmJKPhxYLsP3DIgIJAgJ1LO2qfnowR/T1MzPPDa1bw8uDsxBfJVG3c6C5f9p71Y/7uzbAXF6J+2Vj0Kw2tC3rDGP9aPY6cu4agHPzq6WNNB/XaddWTuwqM3hcEA2+SjaPGOyiODJL1qDSyvxc1noFo7/gflq1rMBy7+kEpKaFdY8MOwgYZEeJ5sGoCfdx911FYhFeGjwq77LnF4uEjYSsoxLYvzuLTg82GOqJuwRMe/FzFkHGMsmlCvF5vCRR5OfM1b8ij+JLVxqpx+ZG+X8awfv3jZOpNqjZV526X5TYH5lNf2HOXfdiAO51BVtWV1RGC8BaMLiMNJdOLuiI5FgHKwMF97sPsqqEaroE/jfueppwJU7Wx9nuVZ1d/HZtPHMaRS5dRt/MI5td8RVcobrt60pjZFrypEeLxDKkgMl6lDTkseXQ0CiRJV9A9QYn15VXqE/smeWtLE85cbtcVtjplESalugySKJkiBJbAb8mfdWLFYLCph+p5lYbRVDvBMxi37gTwqw3/0dW36JlIZtsnhglRF3JnURFeHj5CV7A9Uekn3xgBB/VR7wIfmbKUzLbgda0hXq+vRpGwhIAdSm8RdirRducOnlxfx6p5n+eu+AdsxRZUDSjFgonVGOnrl9BndcrKdFFPO0LYiCAyPqEInqDMyaCy16WbHQH869hlPPfmXuxtvJDQ/8hblow+CY0GBGlHiKLIS9mSvfD5Csyccj+Ki9NyaODxPUe1oyOIDz46hxV1fizfcRJPjp2I2EOqPyyeSZ35jN51MltD0hJCZFTRkzB9cnmvJYP1n/0RZ9AfkhFy9NRFJkLsIdXp5lYuo0v3vmUpQBM9BOs3NaMzxeYg08nnzPq+buN53sVBD/TlZeylyxZ8bJOhevoRIgVfoy/yLcvfPQGWDXnPU+WXZn0roWdOWwlkWUIwqLipUaas/zOflNXEDNW6ZnnyeNNWScF4atxPuZNyr0wFsoyhDz2A95fNxahHBidgwMiwW4uZXPZ6h7pYxUwu0GPk9zduIz2WqQA8FT6ayYDvb1jH7/P5snbaDN69o5t+yctUFxcd5ba13yaV22wduUoVw0kXIYa95qFBxaRXDPSKH1IZ0I+qCkKiWKSsHVs5LWU7a5z1u0+x/7+XqCqV0sVUSruGmPLaS41c1iLec0Ux/3HYrSPkYmMT/Nu3o7nxEG61tcHqdKL84WpUjBuLsupqHny6S674SBcna1e/1iWL+S34bhkhSiCAhndX4cDbb2Pu+FHYtfXP9DoY5OWccaPQ8NYf0bBqNZexjmjlXPGhFVsymdtaGG7KsSnr36vq4LzWij3bP8Qzs6aivLyMB8rKZ5+Zit3b1sLR2oIDdau5XOuSKz60Yksmy8YWfNZHCJtiWhsbsXrlG3A67Zqxu1wOav81Wg4cwKXDhxN0csVHQmBpBC5baA1BBlvwWSfkVP0OLHzhOTgc2mSofWJkLXxhDk7u2KmKImWu+IgEpLPituXglHWh6TDGPDVCVxfGPPU4LjYdStDNFR8JgaURqIs6qZnegs/6W9b1K1fwzScmUUyJyVv5GBdWVnqwa8df0K9fX7RdaeWy2IseH7H6oC3puHu6uXH1KsrK7sPT356O48dPkSSa1DiiEqpp+CCpoRQdIea34LNOCJ0own9sf0JHGAhd5RaLjGAgmKBrxAczZr5ZGZuZX+Z/d/36WDGYbtc4mAKTszKTrH6HkA/TH4bZJ4SiycdUOW9DQrcqB5RhZ+0iflAl0TBd84sfkk496C8mCCEkujWd+Pj1pP531S7mbe23OnhJs5/pNUTmHsQlKwjYSorAtunJmWPYsGHqKxfd6k8F+lX1ayabj5PJtTwn042Vqy8HWvZMFqvL7tWsytPZq/p6S0mS4HSUoPX6TZxva3OTXQtlQ6lbCEm2aCaTa0WcTFdLrmXPZFq6jAwtOdPPRnbZrJwQOVDI1hHDhIgpKxssxPhw0whht5Jibj+rW0YICyjfsrEDKoCOvU0t7IIQnf+cYyun6dJ8ceU+fPLFGdI1twVfQJZZTVang398dXWqkIDN31TEJavLGXfPbnLFB4vFaI5un+TIlPWd9+qM9iFBP1d8JASmQ6Bun9CHIlvUdVjEq4hFPR6PjO8c4WNcwNyUJQjJmIJ4B/d0ylo7bUZ8NDl2V+Hug82Tn+VRTdq4Bv7rV3k99qKlEytbG6uso65OWbRBWqpDPUElo0X94OwFCQ5zVfDxlNlpQ9Ojk85JZITIMPXaK6asdAgbbI8e45o7ExGEGAQ8nbqzJLKnaGrKEoSkQ9hgu9tepFqYIoS27lV77dJT4dtDLaMhfpkjoGD3SX/jmFSO9IyQjBb+VA/vbW2KJAksexvpor8CAYGAQEAgIBAQCAgEBAICAYGAQEAgIBAQCAgEBAICAYGAQEAgkAsI/B+pyg2z0MWwRgAAAABJRU5ErkJggg==")
            //});

            //modelBuilder.Entity<Bahan>().HasData(
            //new Bahan
            //{
            //    Id = "BHN0001",
            //    Nama = "Tepung Terigu",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 15000
            //},
            //new Bahan
            //{
            //    Id = "BHN0002",
            //    Nama = "Telur",
            //    SatuanProduksi = "butir",
            //    StokMinimal = 48
            //},
            //new Bahan
            //{
            //    Id = "BHN0003",
            //    Nama = "Gula Pasir",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 8000
            //},
            //new Bahan
            //{
            //    Id = "BHN0004",
            //    Nama = "Garam",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1000
            //},
            //new Bahan
            //{
            //    Id = "BHN0005",
            //    Nama = "Ragi Instan",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 200
            //},
            //new Bahan
            //{
            //    Id = "BHN0006",
            //    Nama = "Baking Powder",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 160
            //},
            //new Bahan
            //{
            //    Id = "BHN0007",
            //    Nama = "Tepung Maizena",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1500
            //},
            //new Bahan
            //{
            //    Id = "BHN0008",
            //    Nama = "Tepung Tapioka",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1400
            //},
            //new Bahan
            //{
            //    Id = "BHN0009",
            //    Nama = "Vanili Bubuk",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 100
            //},
            //new Bahan
            //{
            //    Id = "BHN0010",
            //    Nama = "Cokelat Bubuk",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 750
            //},
            //new Bahan
            //{
            //    Id = "BHN0011",
            //    Nama = "Margarin",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1250
            //},
            //new Bahan
            //{
            //    Id = "BHN0012",
            //    Nama = "Soda Kue",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 80
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0013",
            ////    Nama = "Whipping Cream",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 700
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0014",
            ////    Nama = "Kismis",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 250
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0015",
            ////    Nama = "Kacang Almond",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 200
            ////},
            //new Bahan
            //{
            //    Id = "BHN0016",
            //    Nama = "Tepung Beras",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 2000
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0017",
            ////    Nama = "Tepung Sorgum",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 800
            ////},
            //new Bahan
            //{
            //    Id = "BHN0018",
            //    Nama = "Susu Bubuk Full Cream",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1500
            //},
            //new Bahan
            //{
            //    Id = "BHN0019",
            //    Nama = "Selai Cokelat",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 400
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0020",
            ////    Nama = "Apel",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1500
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0021",
            ////    Nama = "Sprinkles",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1000
            ////},
            //new Bahan
            //{
            //    Id = "BHN0022",
            //    Nama = "Mentega (Butter)",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1000
            //},
            //new Bahan
            //{
            //    Id = "BHN0023",
            //    Nama = "Selai Nanas",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 400
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0024",
            ////    Nama = "Selai Stroberi",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 400
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0025",
            ////    Nama = "Selai Blueberry",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 400
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0026",
            ////    Nama = "Selai Srikaya",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 400
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0027",
            ////    Nama = "Selai Kacang",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 400
            ////},
            //new Bahan
            //{
            //    Id = "BHN0028",
            //    Nama = "Susu Cair Full Cream",
            //    SatuanProduksi = "ml",
            //    StokMinimal = 3000
            //},
            //new Bahan
            //{
            //    Id = "BHN0029",
            //    Nama = "Keju Cheddar",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1200
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0030",
            ////    Nama = "Kayu Manis",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 200
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0031",
            ////    Nama = "Madu",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 400
            ////},
            //new Bahan
            //{
            //    Id = "BHN0032",
            //    Nama = "Gula Halus",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1600
            //},
            //new Bahan
            //{
            //    Id = "BHN0033",
            //    Nama = "Minyak Kelapa Sawit",
            //    SatuanProduksi = "ml",
            //    StokMinimal = 2000
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0034",
            ////    Nama = "Dark Chocolate",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 500
            ////},
            //new Bahan
            //{
            //    Id = "BHN0035",
            //    Nama = "Air Mineral",
            //    SatuanProduksi = "ml",
            //    StokMinimal = 10000
            //},
            //new Bahan
            //{
            //    Id = "BHN0036",
            //    Nama = "Pasta Cokelat",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 650
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0037",
            ////    Nama = "Tepung Custard",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1700
            ////},
            //new Bahan
            //{
            //    Id = "BHN0038",
            //    Nama = "Kental Manis",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 800
            //},
            //new Bahan
            //{
            //    Id = "BHN0039",
            //    Nama = "Santan Kelapa Cair",
            //    SatuanProduksi = "ml",
            //    StokMinimal = 600
            //},
            //new Bahan
            //{
            //    Id = "BHN0040",
            //    Nama = "Pasta Pandan",
            //    SatuanProduksi = "ml",
            //    StokMinimal = 500
            //},
            //new Bahan
            //{
            //    Id = "BHN0041",
            //    Nama = "Meises",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 850
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0042",
            ////    Nama = "Gula Cokelat",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 500
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0043",
            ////    Nama = "Mayones",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 450
            ////},
            //new Bahan
            //{
            //    Id = "BHN0044",
            //    Nama = "Cokelat Chip",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 600
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0045",
            ////    Nama = "Bubuk Matcha",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 400
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0046",
            ////    Nama = "Tepung Mocaf",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1500
            ////},
            //new Bahan
            //{
            //    Id = "BHN0047",
            //    Nama = "Santan Kelapa Bubuk",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 400
            //},
            //new Bahan
            //{
            //    Id = "BHN0048",
            //    Nama = "Pengembang Kue (Emulsifier)",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 130
            //},
            //new Bahan
            //{
            //    Id = "BHN0049",
            //    Nama = "Pewarna Makanan",
            //    SatuanProduksi = "ml",
            //    StokMinimal = 100
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0050",
            ////    Nama = "Gula Merah",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 800
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0051",
            ////    Nama = "Tape",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 400
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0052",
            ////    Nama = "Labu Kuning",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1300
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0053",
            ////    Nama = "Tepung Kanji",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1500
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0054",
            ////    Nama = "Daun Pandan",
            ////    SatuanProduksi = "lembar",
            ////    StokMinimal = 10
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0055",
            ////    Nama = "Ubi Ungu",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1000
            ////},
            //new Bahan
            //{
            //    Id = "BHN0056",
            //    Nama = "Tepung Sagu",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1500
            //},
            ////new Bahan
            ////{
            ////    Id = "BHN0057",
            ////    Nama = "Tepung Ketan Putih",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1500
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0058",
            ////    Nama = "Kentang",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1000
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0059",
            ////    Nama = "Pisang",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1000
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0060",
            ////    Nama = "Stroberi",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 650
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0061",
            ////    Nama = "Daun Suji",
            ////    SatuanProduksi = "lembar",
            ////    StokMinimal = 10
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0062",
            ////    Nama = "Kelapa Parut",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 350
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0063",
            ////    Nama = "Tepung Ketan Hitam",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1500
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0064",
            ////    Nama = "Kacang Tanah",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 400
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0065",
            ////    Nama = "Pasta Black Forest",
            ////    SatuanProduksi = "ml",
            ////    StokMinimal = 160
            ////}
            //new Bahan
            //{
            //    Id = "BHN0066",
            //    Nama = "Keju Edam",
            //    SatuanProduksi = "gram",
            //    StokMinimal = 1400
            //}
            ////new Bahan
            ////{
            ////    Id = "BHN0067",
            ////    Nama = "Tepung Roti (Panir)",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 800
            ////},
            ////new Bahan
            ////{
            ////    Id = "BHN0068",
            ////    Nama = "Tepung Gandum Utuh",
            ////    SatuanProduksi = "gram",
            ////    StokMinimal = 1000
            ////}
            //);

            //modelBuilder.Entity<BahanSatuan>().HasData(
            //new BahanSatuan
            //{
            //    Id = 1,
            //    BahanId = "BHN0001",
            //    Nama = "dus",
            //    Ukuran = "20 x 500 g",
            //    Harga = 140000,
            //    KonversiStok = 10000
            //},
            //new BahanSatuan
            //{
            //    Id = 2,
            //    BahanId = "BHN0001",
            //    Nama = "pak",
            //    Ukuran = "kg",
            //    Harga = 12500,
            //    KonversiStok = 1000
            //},
            //new BahanSatuan
            //{
            //    Id = 3,
            //    BahanId = "BHN0001",
            //    Nama = "pak",
            //    Ukuran = "500 g",
            //    Harga = 6500,
            //    KonversiStok = 500
            //},
            //new BahanSatuan
            //{
            //    Id = 4,
            //    BahanId = "BHN0002",
            //    Nama = "kg",
            //    Harga = 28000,
            //    KonversiStok = 16
            //},
            //new BahanSatuan
            //{
            //    Id = 5,
            //    BahanId = "BHN0003",
            //    Nama = "dus",
            //    Ukuran = "20 x 1 kg",
            //    Harga = 300000,
            //    KonversiStok = 20000
            //},
            //new BahanSatuan
            //{
            //    Id = 6,
            //    BahanId = "BHN0003",
            //    Nama = "pak",
            //    Ukuran = "kg",
            //    Harga = 13000,
            //    KonversiStok = 1000
            //},
            //new BahanSatuan
            //{
            //    Id = 7,
            //    BahanId = "BHN0004",
            //    Nama = "pcs",
            //    Ukuran = "500 g",
            //    Harga = 5000,
            //    KonversiStok = 500
            //},
            //new BahanSatuan
            //{
            //    Id = 8,
            //    BahanId = "BHN0005",
            //    Nama = "sachet",
            //    Ukuran = "11 g",
            //    Harga = 6000,
            //    KonversiStok = 11
            //},
            //new BahanSatuan
            //{
            //    Id = 9,
            //    BahanId = "BHN0006",
            //    Nama = "toples",
            //    Ukuran = "45 g",
            //    Harga = 6500,
            //    KonversiStok = 45
            //},
            //new BahanSatuan
            //{
            //    Id = 10,
            //    BahanId = "BHN0007",
            //    Nama = "sachet",
            //    Ukuran = "100 g",
            //    Harga = 5500,
            //    KonversiStok = 100
            //},
            //new BahanSatuan
            //{
            //    Id = 11,
            //    BahanId = "BHN0008",
            //    Nama = "pcs",
            //    Ukuran = "500 g",
            //    Harga = 7000,
            //    KonversiStok = 500
            //},
            //new BahanSatuan
            //{
            //    Id = 12,
            //    BahanId = "BHN0009",
            //    Nama = "botol",
            //    Ukuran = "20 g",
            //    Harga = 7000,
            //    KonversiStok = 20
            //},
            //new BahanSatuan
            //{
            //    Id = 13,
            //    BahanId = "BHN0010",
            //    Nama = "sachet",
            //    Ukuran = "500 g",
            //    Harga = 25000,
            //    KonversiStok = 500
            //},
            //new BahanSatuan
            //{
            //    Id = 14,
            //    BahanId = "BHN0011",
            //    Nama = "pak",
            //    Ukuran = "200 g",
            //    Harga = 5500,
            //    KonversiStok = 200
            //},
            //new BahanSatuan
            //{
            //    Id = 15,
            //    BahanId = "BHN0012",
            //    Nama = "toples",
            //    Ukuran = "81 g",
            //    Harga = 6000,
            //    KonversiStok = 81
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 16,
            ////    BahanId = "BHN0013",
            ////    Nama = "pak",
            ////    Ukuran = "200 g",
            ////    Harga = 35000,
            ////    KonversiStok = 200
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 17,
            ////    BahanId = "BHN0014",
            ////    Nama = "pak",
            ////    Ukuran = "kg",
            ////    Harga = 107000,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 18,
            ////    BahanId = "BHN0015",
            ////    Nama = "pouch",
            ////    Ukuran = "100 g",
            ////    Harga = 19000,
            ////    KonversiStok = 100
            ////},
            //new BahanSatuan
            //{
            //    Id = 19,
            //    BahanId = "BHN0016",
            //    Nama = "pak",
            //    Ukuran = "500 g",
            //    Harga = 10800,
            //    KonversiStok = 500
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 20,
            ////    BahanId = "BHN0017",
            ////    Nama = "pak",
            ////    Ukuran = "kg",
            ////    Harga = 36000,
            ////    KonversiStok = 1000
            ////},
            //new BahanSatuan
            //{
            //    Id = 21,
            //    BahanId = "BHN0018",
            //    Nama = "pak",
            //    Ukuran = "800 g",
            //    Harga = 40000,
            //    KonversiStok = 800
            //},
            //new BahanSatuan
            //{
            //    Id = 22,
            //    BahanId = "BHN0019",
            //    Nama = "botol",
            //    Ukuran = "150 g",
            //    Harga = 27000,
            //    KonversiStok = 150
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 23,
            ////    BahanId = "BHN0020",
            ////    Nama = "kg",
            ////    Harga = 40000,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 24,
            ////    BahanId = "BHN0021",
            ////    Nama = "pak",
            ////    Ukuran = "100 g",
            ////    Harga = 10000,
            ////    KonversiStok = 100
            ////},
            //new BahanSatuan
            //{
            //    Id = 25,
            //    BahanId = "BHN0022",
            //    Nama = "pak",
            //    Ukuran = "200 g",
            //    Harga = 12000,
            //    KonversiStok = 200
            //},
            //new BahanSatuan
            //{
            //    Id = 26,
            //    BahanId = "BHN0023",
            //    Nama = "botol",
            //    Ukuran = "170 g",
            //    Harga = 20000,
            //    KonversiStok = 170
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 27,
            ////    BahanId = "BHN0024",
            ////    Nama = "botol",
            ////    Ukuran = "170 g",
            ////    Harga = 24000,
            ////    KonversiStok = 170
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 28,
            ////    BahanId = "BHN0025",
            ////    Nama = "botol",
            ////    Ukuran = "170 g",
            ////    Harga = 29000,
            ////    KonversiStok = 170
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 29,
            ////    BahanId = "BHN0026",
            ////    Nama = "botol",
            ////    Ukuran = "250 g",
            ////    Harga = 28500,
            ////    KonversiStok = 250
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 30,
            ////    BahanId = "BHN0027",
            ////    Nama = "botol",
            ////    Ukuran = "150 g",
            ////    Harga = 30000,
            ////    KonversiStok = 150
            ////},
            //new BahanSatuan
            //{
            //    Id = 31,
            //    BahanId = "BHN0028",
            //    Nama = "pcs",
            //    Ukuran = "liter",
            //    Harga = 17500,
            //    KonversiStok = 1000
            //},
            //new BahanSatuan
            //{
            //    Id = 32,
            //    BahanId = "BHN0029",
            //    Nama = "pcs",
            //    Ukuran = "165 g",
            //    Harga = 23000,
            //    KonversiStok = 165
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 33,
            ////    BahanId = "BHN0030",
            ////    Nama = "pak",
            ////    Ukuran = "250 g",
            ////    Harga = 14000,
            ////    KonversiStok = 250
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 34,
            ////    BahanId = "BHN0031",
            ////    Nama = "jerigen",
            ////    Ukuran = "kg",
            ////    Harga = 50000,
            ////    KonversiStok = 1000
            ////},
            //new BahanSatuan
            //{
            //    Id = 35,
            //    BahanId = "BHN0032",
            //    Nama = "pak",
            //    Ukuran = "250 g",
            //    Harga = 11000,
            //    KonversiStok = 250
            //},
            //new BahanSatuan
            //{
            //    Id = 36,
            //    BahanId = "BHN0033",
            //    Nama = "pouch",
            //    Ukuran = "2 liter",
            //    Harga = 34000,
            //    KonversiStok = 2000
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 37,
            ////    BahanId = "BHN0034",
            ////    Nama = "pcs",
            ////    Ukuran = "kg",
            ////    Harga = 70000,
            ////    KonversiStok = 1000
            ////},
            //new BahanSatuan
            //{
            //    Id = 38,
            //    BahanId = "BHN0035",
            //    Nama = "galon",
            //    Ukuran = "19 liter",
            //    Harga = 40000,
            //    KonversiStok = 19000
            //},
            //new BahanSatuan
            //{
            //    Id = 39,
            //    BahanId = "BHN0036",
            //    Nama = "toples",
            //    Ukuran = "500 g",
            //    Harga = 100000,
            //    KonversiStok = 500
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 40,
            ////    BahanId = "BHN0037",
            ////    Nama = "kaleng",
            ////    Ukuran = "300 g",
            ////    Harga = 30500,
            ////    KonversiStok = 300
            ////},
            //new BahanSatuan
            //{
            //    Id = 41,
            //    BahanId = "BHN0038",
            //    Nama = "kaleng",
            //    Ukuran = "370 g",
            //    Harga = 12000,
            //    KonversiStok = 370
            //},
            //new BahanSatuan
            //{
            //    Id = 42,
            //    BahanId = "BHN0039",
            //    Nama = "pcs",
            //    Ukuran = "200 ml",
            //    Harga = 16500,
            //    KonversiStok = 200
            //},
            //new BahanSatuan
            //{
            //    Id = 43,
            //    BahanId = "BHN0040",
            //    Nama = "botol",
            //    Ukuran = "60 ml",
            //    Harga = 8000,
            //    KonversiStok = 60
            //},
            //new BahanSatuan
            //{
            //    Id = 44,
            //    BahanId = "BHN0041",
            //    Nama = "pak",
            //    Ukuran = "200 g",
            //    Harga = 17000,
            //    KonversiStok = 200
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 45,
            ////    BahanId = "BHN0042",
            ////    Nama = "pak",
            ////    Ukuran = "kg",
            ////    Harga = 26500,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 46,
            ////    BahanId = "BHN0043",
            ////    Nama = "sachet",
            ////    Ukuran = "250 g",
            ////    Harga = 10000,
            ////    KonversiStok = 250
            ////},
            //new BahanSatuan
            //{
            //    Id = 47,
            //    BahanId = "BHN0044",
            //    Nama = "pak",
            //    Ukuran = "500 g",
            //    Harga = 32000,
            //    KonversiStok = 500
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 48,
            ////    BahanId = "BHN0045",
            ////    Nama = "pouch",
            ////    Ukuran = "50 g",
            ////    Harga = 30000,
            ////    KonversiStok = 50
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 49,
            ////    BahanId = "BHN0046",
            ////    Nama = "pak",
            ////    Ukuran = "500 g",
            ////    Harga = 16000,
            ////    KonversiStok = 500
            ////},
            //new BahanSatuan
            //{
            //    Id = 50,
            //    BahanId = "BHN0047",
            //    Nama = "sachet",
            //    Ukuran = "80 g",
            //    Harga = 9000,
            //    KonversiStok = 80
            //},
            //new BahanSatuan
            //{
            //    Id = 51,
            //    BahanId = "BHN0048",
            //    Nama = "sachet",
            //    Ukuran = "30 g",
            //    Harga = 10500,
            //    KonversiStok = 30
            //},
            //new BahanSatuan
            //{
            //    Id = 52,
            //    BahanId = "BHN0049",
            //    Nama = "botol",
            //    Ukuran = "30 ml",
            //    Harga = 5000,
            //    KonversiStok = 30
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 53,
            ////    BahanId = "BHN0050",
            ////    Nama = "pak",
            ////    Ukuran = "kg",
            ////    Harga = 21000,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 54,
            ////    BahanId = "BHN0051",
            ////    Nama = "toples",
            ////    Ukuran = "kg",
            ////    Harga = 15000,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 55,
            ////    BahanId = "BHN0052",
            ////    Nama = "kg",
            ////    Harga = 21000,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 56,
            ////    BahanId = "BHN0053",
            ////    Nama = "pak",
            ////    Ukuran = "500 g",
            ////    Harga = 10000,
            ////    KonversiStok = 500
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 57,
            ////    BahanId = "BHN0054",
            ////    Nama = "lembar",
            ////    KonversiStok = 1
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 58,
            ////    BahanId = "BHN0055",
            ////    Nama = "kg",
            ////    Harga = 12500,
            ////    KonversiStok = 1000
            ////},
            //new BahanSatuan
            //{
            //    Id = 59,
            //    BahanId = "BHN0056",
            //    Nama = "pak",
            //    Ukuran = "500 g",
            //    Harga = 13000,
            //    KonversiStok = 500
            //},
            ////new BahanSatuan
            ////{
            ////    Id = 60,
            ////    BahanId = "BHN0057",
            ////    Nama = "pak",
            ////    Ukuran = "500 g",
            ////    Harga = 12000,
            ////    KonversiStok = 500
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 61,
            ////    BahanId = "BHN0058",
            ////    Nama = "kg",
            ////    Harga = 17500,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 62,
            ////    BahanId = "BHN0059",
            ////    Nama = "kg",
            ////    Harga = 15000,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 63,
            ////    BahanId = "BHN0060",
            ////    Nama = "pak",
            ////    Ukuran = "110 g",
            ////    Harga = 10000,
            ////    KonversiStok = 110
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 64,
            ////    BahanId = "BHN0061",
            ////    Nama = "lembar",
            ////    KonversiStok = 1
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 65,
            ////    BahanId = "BHN0062",
            ////    Nama = "pak",
            ////    Ukuran = "175 g",
            ////    Harga = 10000,
            ////    KonversiStok = 175
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 66,
            ////    BahanId = "BHN0063",
            ////    Nama = "pak",
            ////    Ukuran = "250 g",
            ////    Harga = 10000,
            ////    KonversiStok = 250
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 67,
            ////    BahanId = "BHN0064",
            ////    Nama = "kg",
            ////    Harga = 8500,
            ////    KonversiStok = 1000
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 68,
            ////    BahanId = "BHN0065",
            ////    Nama = "botol",
            ////    Ukuran = "55 ml",
            ////    Harga = 7000,
            ////    KonversiStok = 55
            ////},
            //new BahanSatuan
            //{
            //    Id = 69,
            //    BahanId = "BHN0066",
            //    Nama = "pcs",
            //    Ukuran = "250 g",
            //    Harga = 33000,
            //    KonversiStok = 250
            //},
            //new BahanSatuan
            //{
            //    Id = 70,
            //    BahanId = "BHN0002",
            //    Nama = "butir",
            //    Harga = 2500,
            //    KonversiStok = 1
            //},
            //new BahanSatuan
            //{
            //    Id = 71,
            //    BahanId = "BHN0035",
            //    Nama = "dus",
            //    Ukuran = "12 x 1,5 liter",
            //    Harga = 53000,
            //    KonversiStok = 18000
            //},
            //new BahanSatuan
            //{
            //    Id = 72,
            //    BahanId = "BHN0035",
            //    Nama = "botol",
            //    Ukuran = "1,5 liter",
            //    Harga = 5500,
            //    KonversiStok = 1500
            //}
            ////new BahanSatuan
            ////{
            ////    Id = 73,
            ////    BahanId = "BHN0067",
            ////    Nama = "pak",
            ////    Ukuran = "200 g",
            ////    Harga = 9000,
            ////    KonversiStok = 200
            ////},
            ////new BahanSatuan
            ////{
            ////    Id = 74,
            ////    BahanId = "BHN0068",
            ////    Nama = "pak",
            ////    Ukuran = "1 kg",
            ////    Harga = 40000,
            ////    KonversiStok = 1000
            ////}
            //);

            //modelBuilder.Entity<Barang>().HasData(
            ////new Barang
            ////{
            ////    Id = "BRG0001",
            ////    Nama = "Pukis Tape",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 9
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0002",
            ////    Nama = "Pukis Pandan Cokelat",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 8
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0003",
            ////    Nama = "Lapis Pelangi",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 7
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0004",
            ////    Nama = "Lapis Ubi Ungu",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 8
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0005",
            ////    Nama = "Kue Lumpur Labu Kuning",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 8
            ////},
            //new Barang
            //{
            //    Id = "BRG0006",
            //    Nama = "Roti Tawar",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 14
            //},
            ////new Barang
            ////{
            ////    Id = "BRG0007",
            ////    Nama = "Kue Lumpur Pisang",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 11
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0008",
            ////    Nama = "Kue Lumpur Pandan Keju",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 10
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0009",
            ////    Nama = "Kue Lumpur Kentang",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 9
            ////},
            //new Barang
            //{
            //    Id = "BRG0010",
            //    Nama = "Roti Sobek Isi Cokelat Keju",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 13
            //},
            ////new Barang
            ////{
            ////    Id = "BRG0011",
            ////    Nama = "Roti Sobek Pisang",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 12
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0012",
            ////    Nama = "Bola-Bola Cokelat",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 16
            ////},
            //new Barang
            //{
            //    Id = "BRG0013",
            //    Nama = "Bolu Kukus",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 14
            //},
            ////new Barang
            ////{
            ////    Id = "BRG0014",
            ////    Nama = "Bolu Pisang",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 11
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0015",
            ////    Nama = "Bolu Pandan",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 9
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0016",
            ////    Nama = "Black Forest",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 2
            ////},
            //new Barang
            //{
            //    Id = "BRG0017",
            //    Nama = "Donat",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 15
            //},
            ////new Barang
            ////{
            ////    Id = "BRG0018",
            ////    Nama = "Banana Roll Cake",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 7
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0019",
            ////    Nama = "Kue Putu",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 18
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0020",
            ////    Nama = "Dadar Gulung",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 19
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0021",
            ////    Nama = "Nagasari",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 20
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0022",
            ////    Nama = "Kue Cucur",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 22
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0023",
            ////    Nama = "Klepon",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 21
            ////},
            //new Barang
            //{
            //    Id = "BRG0024",
            //    Nama = "Kukis Cokelat",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 25
            //},
            //new Barang
            //{
            //    Id = "BRG0025",
            //    Nama = "Nastar",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 27
            //},
            //new Barang
            //{
            //    Id = "BRG0026",
            //    Nama = "Kue Putri Salju",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 28
            //},
            ////new Barang
            ////{
            ////    Id = "BRG0027",
            ////    Nama = "Kue Kacang Almond",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 25
            ////},
            //new Barang
            //{
            //    Id = "BRG0028",
            //    Nama = "Sagu Keju",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 30
            //},
            //new Barang
            //{
            //    Id = "BRG0029",
            //    Nama = "Kastengel",
            //    SatuanProduksi = "pcs",
            //    StokMinimal = 30
            //}
            ////new Barang
            ////{
            ////    Id = "BRG0030",
            ////    Nama = "Kue Semprit",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 14
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0031",
            ////    Nama = "Pie Apel",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 5
            ////},
            ////new Barang
            ////{
            ////    Id = "BRG0032",
            ////    Nama = "Matcha Pastry",
            ////    SatuanProduksi = "pcs",
            ////    StokMinimal = 8
            ////}
            //);

            //modelBuilder.Entity<BarangSatuan>().HasData(
            ////new BarangSatuan
            ////{
            ////    Id = 1,
            ////    BarangId = "BRG0001",
            ////    Nama = "pak",
            ////    Ukuran = "20 pcs",
            ////    Harga = 35000,
            ////    KonversiStok = 20
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 2,
            ////    BarangId = "BRG0001",
            ////    Nama = "pcs",
            ////    Harga = 2000,
            ////    KonversiStok = 1
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 3,
            ////    BarangId = "BRG0002",
            ////    Nama = "pak",
            ////    Ukuran = "30 pcs",
            ////    Harga = 48000,
            ////    KonversiStok = 30
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 4,
            ////    BarangId = "BRG0003",
            ////    Nama = "pak",
            ////    Ukuran = "20 pcs",
            ////    Harga = 38500,
            ////    KonversiStok = 20
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 5,
            ////    BarangId = "BRG0004",
            ////    Nama = "pak",
            ////    Ukuran = "18 pcs",
            ////    Harga = 41000,
            ////    KonversiStok = 18
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 6,
            ////    BarangId = "BRG0005",
            ////    Nama = "pak",
            ////    Ukuran = "12 pcs",
            ////    Harga = 28500,
            ////    KonversiStok = 12
            ////},
            //new BarangSatuan
            //{
            //    Id = 7,
            //    BarangId = "BRG0006",
            //    Nama = "pcs",
            //    Harga = 20000,
            //    KonversiStok = 1
            //},
            ////new BarangSatuan
            ////{
            ////    Id = 8,
            ////    BarangId = "BRG0007",
            ////    Nama = "pak",
            ////    Ukuran = "20 pcs",
            ////    Harga = 37500,
            ////    KonversiStok = 20
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 9,
            ////    BarangId = "BRG0008",
            ////    Nama = "pak",
            ////    Ukuran = "20 pcs",
            ////    Harga = 43500,
            ////    KonversiStok = 20
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 10,
            ////    BarangId = "BRG0009",
            ////    Nama = "pak",
            ////    Ukuran = "20 pcs",
            ////    Harga = 45000,
            ////    KonversiStok = 20
            ////},
            //new BarangSatuan
            //{
            //    Id = 11,
            //    BarangId = "BRG0010",
            //    Nama = "pcs",
            //    Harga = 20500,
            //    KonversiStok = 1
            //},
            ////new BarangSatuan
            ////{
            ////    Id = 12,
            ////    BarangId = "BRG0011",
            ////    Nama = "pcs",
            ////    Harga = 23000,
            ////    KonversiStok = 1
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 13,
            ////    BarangId = "BRG0012",
            ////    Nama = "pak",
            ////    Ukuran = "18 pcs",
            ////    Harga = 35000,
            ////    KonversiStok = 18
            ////},
            //new BarangSatuan
            //{
            //    Id = 14,
            //    BarangId = "BRG0013",
            //    Nama = "pak",
            //    Ukuran = "12 pcs",
            //    Harga = 30000,
            //    KonversiStok = 12
            //},
            ////new BarangSatuan
            ////{
            ////    Id = 15,
            ////    BarangId = "BRG0014",
            ////    Nama = "pak",
            ////    Ukuran = "14 pcs",
            ////    Harga = 33000,
            ////    KonversiStok = 14
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 16,
            ////    BarangId = "BRG0015",
            ////    Nama = "pak",
            ////    Ukuran = "16 pcs",
            ////    Harga = 34500,
            ////    KonversiStok = 16
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 17,
            ////    BarangId = "BRG0016",
            ////    Nama = "pcs",
            ////    Harga = 70000,
            ////    KonversiStok = 1
            ////},
            //new BarangSatuan
            //{
            //    Id = 18,
            //    BarangId = "BRG0017",
            //    Nama = "box",
            //    Ukuran = "12 pcs",
            //    Harga = 30000,
            //    KonversiStok = 12
            //},
            ////new BarangSatuan
            ////{
            ////    Id = 19,
            ////    BarangId = "BRG0018",
            ////    Nama = "pcs",
            ////    Harga = 22000,
            ////    KonversiStok = 1
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 20,
            ////    BarangId = "BRG0019",
            ////    Nama = "pak",
            ////    Ukuran = "18 pcs",
            ////    Harga = 40000,
            ////    KonversiStok = 18
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 21,
            ////    BarangId = "BRG0020",
            ////    Nama = "pak",
            ////    Ukuran = "20 pcs",
            ////    Harga = 34000,
            ////    KonversiStok = 20
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 22,
            ////    BarangId = "BRG0021",
            ////    Nama = "pak",
            ////    Ukuran = "32 pcs",
            ////    Harga = 36500,
            ////    KonversiStok = 32
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 23,
            ////    BarangId = "BRG0022",
            ////    Nama = "pak",
            ////    Ukuran = "18 pcs",
            ////    Harga = 26000,
            ////    KonversiStok = 18
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 24,
            ////    BarangId = "BRG0023",
            ////    Nama = "pak",
            ////    Ukuran = "18 pcs",
            ////    Harga = 30000,
            ////    KonversiStok = 18
            ////},
            //new BarangSatuan
            //{
            //    Id = 25,
            //    BarangId = "BRG0024",
            //    Nama = "toples",
            //    Ukuran = "24 pcs",
            //    Harga = 32000,
            //    KonversiStok = 24
            //},
            //new BarangSatuan
            //{
            //    Id = 26,
            //    BarangId = "BRG0025",
            //    Nama = "toples",
            //    Ukuran = "44 pcs",
            //    Harga = 70000,
            //    KonversiStok = 44
            //},
            //new BarangSatuan
            //{
            //    Id = 27,
            //    BarangId = "BRG0025",
            //    Nama = "pak",
            //    Ukuran = "18 pcs",
            //    Harga = 30000,
            //    KonversiStok = 18
            //},
            //new BarangSatuan
            //{
            //    Id = 28,
            //    BarangId = "BRG0026",
            //    Nama = "toples",
            //    Ukuran = "28 pcs",
            //    Harga = 50000,
            //    KonversiStok = 36
            //},
            ////new BarangSatuan
            ////{
            ////    Id = 29,
            ////    BarangId = "BRG0027",
            ////    Nama = "pak",
            ////    Ukuran = "14 pcs",
            ////    Harga = 29500,
            ////    KonversiStok = 14
            ////},
            //new BarangSatuan
            //{
            //    Id = 30,
            //    BarangId = "BRG0028",
            //    Nama = "toples",
            //    Ukuran = "30 pcs",
            //    Harga = 55000,
            //    KonversiStok = 30
            //},
            //new BarangSatuan
            //{
            //    Id = 31,
            //    BarangId = "BRG0029",
            //    Nama = "toples",
            //    Ukuran = "42 pcs",
            //    Harga = 75000,
            //    KonversiStok = 42
            //}
            ////new BarangSatuan
            ////{
            ////    Id = 32,
            ////    BarangId = "BRG0030",
            ////    Nama = "pak",
            ////    Ukuran = "18 pcs",
            ////    Harga = 36000,
            ////    KonversiStok = 18
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 33,
            ////    BarangId = "BRG0031",
            ////    Nama = "pcs",
            ////    Harga = 55000,
            ////    KonversiStok = 1
            ////},
            ////new BarangSatuan
            ////{
            ////    Id = 34,
            ////    BarangId = "BRG0032",
            ////    Nama = "pak",
            ////    Ukuran = "20 pcs",
            ////    Harga = 60000,
            ////    KonversiStok = 20
            ////}
            //);

            //modelBuilder.Entity<Formulasi>().HasData(
            ////new Formulasi
            ////{
            ////    Id = "F0001",
            ////    BarangId = "BRG0016",
            ////    Jumlah = 1
            ////},
            //new Formulasi
            //{
            //    Id = "F0002",
            //    BarangId = "BRG0025",
            //    Jumlah = 60
            //},
            ////new Formulasi
            ////{
            ////    Id = "F0003",
            ////    BarangId = "BRG0018",
            ////    Jumlah = 5
            ////}
            //new Formulasi
            //{
            //    Id = "F0004",
            //    BarangId = "BRG0029",
            //    Jumlah = 60
            //},
            //new Formulasi
            //{
            //    Id = "F0005",
            //    BarangId = "BRG0026",
            //    Jumlah = 45
            //},
            //new Formulasi
            //{
            //    Id = "F0006",
            //    BarangId = "BRG0017",
            //    Jumlah = 20
            //},
            //new Formulasi
            //{
            //    Id = "F0007",
            //    BarangId = "BRG0024",
            //    Jumlah = 50
            //},
            //new Formulasi
            //{
            //    Id = "F0008",
            //    BarangId = "BRG0006",
            //    Jumlah = 3
            //});

            //modelBuilder.Entity<FormulasiDetail>().HasData(
            ////new FormulasiDetail
            ////{
            ////    Id = 1,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0001",
            ////    Jumlah = 120m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 2,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0002",
            ////    Jumlah = 7m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 3,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0032",
            ////    Jumlah = 200m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 4,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0048",
            ////    Jumlah = 10m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 5,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0010",
            ////    Jumlah = 30m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 6,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0036",
            ////    Jumlah = 10m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 7,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0011",
            ////    Jumlah = 150m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 8,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0013",
            ////    Jumlah = 120m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 9,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0025",
            ////    Jumlah = 50m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 10,
            ////    FormulasiId = "F0001",
            ////    BahanId = "BHN0034",
            ////    Jumlah = 200m,
            ////},
            //new FormulasiDetail
            //{
            //    Id = 11,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0011",
            //    Jumlah = 125m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 12,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0022",
            //    Jumlah = 125m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 13,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0032",
            //    Jumlah = 75m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 14,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0009",
            //    Jumlah = 2.5m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 15,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0002",
            //    Jumlah = 3m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 16,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0001",
            //    Jumlah = 350m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 17,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0007",
            //    Jumlah = 50m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 18,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0018",
            //    Jumlah = 50m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 19,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0023",
            //    Jumlah = 50m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 20,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0029",
            //    Jumlah = 35m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 21,
            //    FormulasiId = "F0002",
            //    BahanId = "BHN0033",
            //    Jumlah = 2.5m,
            //},
            ////new FormulasiDetail
            ////{
            ////    Id = 22,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0002",
            ////    Jumlah = 4m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 23,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0003",
            ////    Jumlah = 50m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 24,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0048",
            ////    Jumlah = 5m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 25,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0001",
            ////    Jumlah = 60m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 26,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0007",
            ////    Jumlah = 10m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 27,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0018",
            ////    Jumlah = 10m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 28,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0022",
            ////    Jumlah = 70m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 29,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0040",
            ////    Jumlah = 2.5m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 30,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0009",
            ////    Jumlah = 1.5m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 31,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0059",
            ////    Jumlah = 600m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 32,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0033",
            ////    Jumlah = 5m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 33,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0041",
            ////    Jumlah = 70m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 34,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0029",
            ////    Jumlah = 60m,
            ////},
            ////new FormulasiDetail
            ////{
            ////    Id = 35,
            ////    FormulasiId = "F0003",
            ////    BahanId = "BHN0034",
            ////    Jumlah = 100m,
            ////}
            //new FormulasiDetail
            //{
            //    Id = 36,
            //    FormulasiId = "F0004",
            //    BahanId = "BHN0011",
            //    Jumlah = 300m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 37,
            //    FormulasiId = "F0004",
            //    BahanId = "BHN0066",
            //    Jumlah = 100m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 38,
            //    FormulasiId = "F0004",
            //    BahanId = "BHN0029",
            //    Jumlah = 100m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 39,
            //    FormulasiId = "F0004",
            //    BahanId = "BHN0001",
            //    Jumlah = 350m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 40,
            //    FormulasiId = "F0004",
            //    BahanId = "BHN0007",
            //    Jumlah = 50m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 41,
            //    FormulasiId = "F0004",
            //    BahanId = "BHN0018",
            //    Jumlah = 20m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 42,
            //    FormulasiId = "F0004",
            //    BahanId = "BHN0002",
            //    Jumlah = 2m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 43,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0002",
            //    Jumlah = 2m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 44,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0022",
            //    Jumlah = 200m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 45,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0011",
            //    Jumlah = 300m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 46,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0032",
            //    Jumlah = 100m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 47,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0066",
            //    Jumlah = 50m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 48,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0029",
            //    Jumlah = 50m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 49,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0001",
            //    Jumlah = 650m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 50,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0018",
            //    Jumlah = 60m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 51,
            //    FormulasiId = "F0005",
            //    BahanId = "BHN0007",
            //    Jumlah = 65m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 52,
            //    FormulasiId = "F0006",
            //    BahanId = "BHN0001",
            //    Jumlah = 1000m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 53,
            //    FormulasiId = "F0006",
            //    BahanId = "BHN0005",
            //    Jumlah = 3m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 54,
            //    FormulasiId = "F0006",
            //    BahanId = "BHN0003",
            //    Jumlah = 150m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 55,
            //    FormulasiId = "F0006",
            //    BahanId = "BHN0035",
            //    Jumlah = 125m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 56,
            //    FormulasiId = "F0006",
            //    BahanId = "BHN0011",
            //    Jumlah = 20m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 57,
            //    FormulasiId = "F0006",
            //    BahanId = "BHN0004",
            //    Jumlah = 3m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 58,
            //    FormulasiId = "F0006",
            //    BahanId = "BHN0041",
            //    Jumlah = 100m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 59,
            //    FormulasiId = "F0006",
            //    BahanId = "BHN0002",
            //    Jumlah = 4m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 60,
            //    FormulasiId = "F0007",
            //    BahanId = "BHN0022",
            //    Jumlah = 200m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 61,
            //    FormulasiId = "F0007",
            //    BahanId = "BHN0009",
            //    Jumlah = 4m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 62,
            //    FormulasiId = "F0007",
            //    BahanId = "BHN0002",
            //    Jumlah = 2m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 63,
            //    FormulasiId = "F0007",
            //    BahanId = "BHN0032",
            //    Jumlah = 175m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 64,
            //    FormulasiId = "F0007",
            //    BahanId = "BHN0001",
            //    Jumlah = 250m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 65,
            //    FormulasiId = "F0007",
            //    BahanId = "BHN0010",
            //    Jumlah = 20m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 66,
            //    FormulasiId = "F0007",
            //    BahanId = "BHN0006",
            //    Jumlah = 4m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 67,
            //    FormulasiId = "F0007",
            //    BahanId = "BHN0044",
            //    Jumlah = 175m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 68,
            //    FormulasiId = "F0008",
            //    BahanId = "BHN0001",
            //    Jumlah = 600m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 69,
            //    FormulasiId = "F0008",
            //    BahanId = "BHN0018",
            //    Jumlah = 200m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 70,
            //    FormulasiId = "F0008",
            //    BahanId = "BHN0032",
            //    Jumlah = 100m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 71,
            //    FormulasiId = "F0008",
            //    BahanId = "BHN0011",
            //    Jumlah = 100m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 72,
            //    FormulasiId = "F0008",
            //    BahanId = "BHN0005",
            //    Jumlah = 10m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 73,
            //    FormulasiId = "F0008",
            //    BahanId = "BHN0002",
            //    Jumlah = 4m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 74,
            //    FormulasiId = "F0008",
            //    BahanId = "BHN0048",
            //    Jumlah = 5m,
            //},
            //new FormulasiDetail
            //{
            //    Id = 75,
            //    FormulasiId = "F0008",
            //    BahanId = "BHN0004",
            //    Jumlah = 3m,
            //});

            //modelBuilder.Entity<Customer>().HasData(
            //new Customer
            //{
            //    Id = "CST0001",
            //    Nama = "Customer A",
            //    Alamat = "Surabaya",
            //    Telepon = "084277593471",
            //    Fax = null!,
            //    Email = null!
            //},
            //new Customer
            //{
            //    Id = "CST0002",
            //    Nama = "Customer B",
            //    Alamat = "Cirebon",
            //    Telepon = "081154285364",
            //    Fax = null!,
            //    Email = null!
            //},
            //new Customer
            //{
            //    Id = "CST0003",
            //    Nama = "Customer C",
            //    Alamat = "Tangerang",
            //    Telepon = "082535846940",
            //    Fax = null!,
            //    Email = null!
            //},
            //new Customer
            //{
            //    Id = "CST0004",
            //    Nama = "Customer D",
            //    Alamat = "Bandung",
            //    Telepon = "081152953535",
            //    Fax = "123456",
            //    Email = "exemail12@gmail.com"
            //},
            //new Customer
            //{
            //    Id = "CST0005",
            //    Nama = "Customer E",
            //    Alamat = "Malang",
            //    Telepon = "08237583473",
            //    Fax = null!,
            //    Email = "test32@gmail.com"
            //});

            //modelBuilder.Entity<Supplier>().HasData(
            //new Supplier
            //{
            //    Id = "SPL0001",
            //    Nama = "Supplier A",
            //    Alamat = "Banten",
            //    Telepon = "085377419674",
            //    Fax = "987654",
            //    Email = "supp@gmail.com"
            //},
            //new Supplier
            //{
            //    Id = "SPL0002",
            //    Nama = "Supplier B",
            //    Alamat = "Banjarmasin",
            //    Telepon = "089244218647",
            //    Fax = null!,
            //    Email = null!
            //},
            //new Supplier
            //{
            //    Id = "SPL0003",
            //    Nama = "Supplier C",
            //    Alamat = "Solo",
            //    Telepon = "082364664466",
            //    Fax = "234567",
            //    Email = "coba36@gmail.com"
            //},
            //new Supplier
            //{
            //    Id = "SPL0004",
            //    Nama = "Supplier D",
            //    Alamat = "Sukabumi",
            //    Telepon = "08235384125",
            //    Fax = null!,
            //    Email = null!
            //},
            //new Supplier
            //{
            //    Id = "SPL0005",
            //    Nama = "Supplier E",
            //    Alamat = "Mojokerto",
            //    Telepon = "084264825547",
            //    Fax = null!,
            //    Email = "supplierlima@gmail.co.id"
            //});

            //modelBuilder.Entity<Overhead>().HasData(
            //new Overhead
            //{
            //    Id = 1,
            //    Nama = "Biaya listrik"
            //},
            //new Overhead
            //{
            //    Id = 2,
            //    Nama = "Biaya angkut"
            //},
            //new Overhead
            //{
            //    Id = 3,
            //    Nama = "Biaya bahan penolong"
            //},
            //new Overhead
            //{
            //    Id = 4,
            //    Nama = "Biaya tenaga kerja tidak langsung"
            //},
            //new Overhead
            //{
            //    Id = 5,
            //    Nama = "Biaya kemasan"
            //},
            //new Overhead
            //{
            //    Id = 6,
            //    Nama = "Biaya lain-lain"
            //});

            //modelBuilder.Entity<Pekerjaan>().HasData(
            //new Pekerjaan
            //{
            //    Id = 1,
            //    Nama = "Koki",
            //},
            //new Pekerjaan
            //{
            //    Id = 2,
            //    Nama = "Produksi",
            //},
            //new Pekerjaan
            //{
            //    Id = 3,
            //    Nama = "Penjual",
            //},
            //new Pekerjaan
            //{
            //    Id = 4,
            //    Nama = "Sopir",
            //});

            //modelBuilder.Entity<Karyawan>().HasData(
            //new Karyawan
            //{
            //    Id = "KYN0001",
            //    Nama = "Alma",
            //    TempatLahir = "Bandung",
            //    TanggalLahir = new DateTime(1967, 3, 26),
            //    Alamat = "Semarang",
            //    Telepon = "081153822928",
            //    Email = null!,
            //    PekerjaanId = 1,
            //    Upah = 50000
            //},
            //new Karyawan
            //{
            //    Id = "KYN0002",
            //    Nama = "Dima",
            //    TempatLahir = "Surabaya",
            //    TanggalLahir = new DateTime(1983, 5, 14),
            //    Alamat = "Cilacap",
            //    Telepon = "083357463286",
            //    Email = "dima1337@gmail.co.id",
            //    PekerjaanId = 2,
            //    Upah = 65000
            //},
            //new Karyawan
            //{
            //    Id = "KYN0003",
            //    Nama = "Faran",
            //    TempatLahir = "Makassar",
            //    TanggalLahir = new DateTime(1988, 1, 19),
            //    Alamat = "Madiun",
            //    Telepon = null!,
            //    Email = "faran22@gmail.co.id",
            //    PekerjaanId = 1,
            //    Upah = 55000
            //},
            //new Karyawan
            //{
            //    Id = "KYN0004",
            //    Nama = "Erlina",
            //    TempatLahir = "Banten",
            //    TanggalLahir = new DateTime(1976, 12, 2),
            //    Alamat = "Lumajang",
            //    Telepon = "087733882727",
            //    Email = "erlina16@yahoo.com",
            //    PekerjaanId = 1,
            //    Upah = 45000
            //},
            //new Karyawan
            //{
            //    Id = "KYN0005",
            //    Nama = "Mark",
            //    TempatLahir = "Magelang",
            //    TanggalLahir = new DateTime(1990, 8, 14),
            //    Alamat = "Jepara",
            //    Telepon = "084353847756",
            //    Email = "mark@gmail.com",
            //    PekerjaanId = 4,
            //    Upah = 70000
            //});

            //modelBuilder.Entity<TransaksiLain>().HasData(
            //new TransaksiLain
            //{
            //    Id = 1,
            //    Tanggal = new DateTime(2022, 11, 30, 16, 28, 41),
            //    Jenis = "Pengeluaran",
            //    Kategori = "Beban Sewa",
            //    Keterangan = "Sewa Gedung pada bulan November 2022",
            //    Nominal = 95000
            //},
            //new TransaksiLain
            //{
            //    Id = 2,
            //    Tanggal = new DateTime(2022, 12, 21, 8, 15, 55),
            //    Jenis = "Pengeluaran",
            //    Kategori = "Beban Perbaikan dan Renovasi",
            //    Keterangan = "Beberapa mesin rusak",
            //    Nominal = 120000
            //},
            //new TransaksiLain
            //{
            //    Id = 3,
            //    Tanggal = new DateTime(2023, 2, 9, 22, 24, 33),
            //    Jenis = "Pendapatan",
            //    Kategori = "Pendapatan Lain-lain",
            //    Keterangan = "Ok",
            //    Nominal = 150000
            //});

            //#endregion DataInitializer
        }
    }
}