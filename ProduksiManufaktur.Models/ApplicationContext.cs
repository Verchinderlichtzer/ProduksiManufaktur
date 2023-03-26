using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProduksiManufaktur.Models
{
    public class ApplicationContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

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

            //modelBuilder.Entity<LogTransaksi>(e =>
            //{
            //    e.HasOne(x => x.User).WithMany(x => x.LogTransaksi).HasForeignKey(x => x.UserId);
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.Tanggal).HasDefaultValueSql("GETDATE()");
            //});

            //modelBuilder.Entity<Bahan>(e =>
            //{
            //    e.Property(x => x.StokAwal).HasPrecision(9, 2);
            //    e.Property(x => x.Stok).HasPrecision(9, 2);
            //    e.Property(x => x.StokMinimal).HasPrecision(9, 2);
            //    e.Property(x => x.Version).IsRowVersion();
            //});

            //modelBuilder.Entity<BahanSatuan>(e =>
            //{
            //    e.HasOne(x => x.Bahan).WithMany(x => x.BahanSatuan).HasForeignKey(x => x.BahanId);
            //    e.Property(x => x.KonversiStok).HasPrecision(9, 2);
            //    e.HasIndex(x => new { x.BahanId, x.Nama, x.Ukuran }).IsUnique();
            //});

            //modelBuilder.Entity<PerubahanStokBahan>(e =>
            //{
            //    e.HasOne(x => x.Bahan).WithMany(x => x.PerubahanStokBahan).HasForeignKey(x => x.BahanId);
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //    e.Ignore(x => x.JenisSebelum);
            //    e.Ignore(x => x.JumlahSebelum);
            //});

            //modelBuilder.Entity<Barang>(e =>
            //{
            //    e.Property(x => x.StokAwal).HasPrecision(9, 2);
            //    e.Property(x => x.Stok).HasPrecision(9, 2);
            //    e.Property(x => x.StokMinimal).HasPrecision(9, 2);
            //    e.Property(x => x.Version).IsRowVersion();
            //});

            //modelBuilder.Entity<BarangSatuan>(e =>
            //{
            //    e.HasOne(x => x.Barang).WithMany(x => x.BarangSatuan).HasForeignKey(x => x.BarangId);
            //    e.Property(x => x.KonversiStok).HasPrecision(9, 2);
            //    e.HasIndex(x => new { x.BarangId, x.Nama, x.Ukuran }).IsUnique();
            //});

            //modelBuilder.Entity<PerubahanStokBarang>(e =>
            //{
            //    e.HasOne(x => x.Barang).WithMany(x => x.PerubahanStokBarang).HasForeignKey(x => x.BarangId);
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //    e.Ignore(x => x.JumlahSebelum);
            //});

            //modelBuilder.Entity<Formulasi>(e =>
            //{
            //    e.HasOne(x => x.Barang).WithMany(x => x.Formulasi).HasForeignKey(x => x.BarangId);
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //});

            //modelBuilder.Entity<FormulasiDetail>(e =>
            //{
            //    e.HasOne(x => x.Formulasi).WithMany(x => x.FormulasiDetail).HasForeignKey(x => x.FormulasiId);
            //    e.HasOne(x => x.Bahan).WithMany(x => x.FormulasiDetail).HasForeignKey(x => x.BahanId);
            //    e.HasIndex(x => new { x.FormulasiId, x.BahanId }).IsUnique();
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //});

            //modelBuilder.Entity<Pekerjaan>(e =>
            //{
            //    e.HasIndex(x => x.Nama).IsUnique();
            //    e.Ignore(x => x.JumlahKaryawan);
            //});

            //modelBuilder.Entity<Karyawan>(e =>
            //{
            //    e.HasOne(x => x.Pekerjaan).WithMany(x => x.Karyawan).HasForeignKey(x => x.PekerjaanId);
            //    e.HasIndex(x => x.Telepon).IsUnique();
            //    e.HasIndex(x => x.Email).IsUnique();
            //    e.Ignore(x => x.InputTanggalLahir);
            //});

            //modelBuilder.Entity<Supplier>(e =>
            //{
            //    e.HasIndex(x => x.Telepon).IsUnique();
            //    e.HasIndex(x => x.Fax).IsUnique();
            //    e.HasIndex(x => x.Email).IsUnique();
            //});

            //modelBuilder.Entity<Customer>(e =>
            //{
            //    e.HasIndex(x => x.Telepon).IsUnique();
            //    e.HasIndex(x => x.Fax).IsUnique();
            //    e.HasIndex(x => x.Email).IsUnique();
            //});

            //modelBuilder.Entity<Overhead>(e => e.HasIndex(x => x.Nama).IsUnique());

            //modelBuilder.Entity<Pembelian>(e =>
            //{
            //    e.HasOne(x => x.Supplier).WithMany(x => x.Pembelian).HasForeignKey(x => x.SupplierId);
            //    e.Property(x => x.Version).IsRowVersion();
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //    e.Ignore(x => x.HariJatuhTempo);
            //    e.Ignore(x => x.GrandTotal);
            //    e.Ignore(x => x.Sisa);
            //});

            //modelBuilder.Entity<PembelianDetail>(e =>
            //{
            //    e.HasOne(x => x.Pembelian).WithMany(x => x.PembelianDetail).HasForeignKey(x => x.PembelianId);
            //    e.HasOne(x => x.BahanSatuan).WithMany(x => x.PembelianDetail).HasForeignKey(x => x.BahanSatuanId);
            //    e.HasIndex(x => new { x.PembelianId, x.BahanSatuanId }).IsUnique();
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.MinJumlah).HasPrecision(9, 2);
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //    e.Ignore(x => x.NamaBahan);
            //    e.Ignore(x => x.JumlahSebelum);
            //    e.Ignore(x => x.StokAkhir);
            //    e.Ignore(x => x.Total);
            //});

            //modelBuilder.Entity<TransaksiPembelian>(e =>
            //{
            //    e.HasOne(x => x.Pembelian).WithMany(x => x.TransaksiPembelian).HasForeignKey(x => x.PembelianId);
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.Version).IsRowVersion();
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //    e.Ignore(x => x.NominalSebelum);
            //    e.Ignore(x => x.Balance);
            //    e.Ignore(x => x.BalanceLabel);
            //});

            //modelBuilder.Entity<Penjualan>(e =>
            //{
            //    e.HasOne(x => x.Customer).WithMany(x => x.Penjualan).HasForeignKey(x => x.CustomerId);
            //    e.Property(x => x.Version).IsRowVersion();
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //    e.Ignore(x => x.HariJatuhTempo);
            //    e.Ignore(x => x.GrandTotal);
            //    e.Ignore(x => x.Sisa);
            //});

            //modelBuilder.Entity<PenjualanDetail>(e =>
            //{
            //    e.HasOne(x => x.Penjualan).WithMany(x => x.PenjualanDetail).HasForeignKey(x => x.PenjualanId);
            //    e.HasOne(x => x.BarangSatuan).WithMany(x => x.PenjualanDetail).HasForeignKey(x => x.BarangSatuanId);
            //    e.HasIndex(x => new { x.PenjualanId, x.BarangSatuanId }).IsUnique();
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.MinJumlah).HasPrecision(9, 2);
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //    e.Ignore(x => x.NamaBarang);
            //    e.Ignore(x => x.JumlahSebelum);
            //    e.Ignore(x => x.StokAkhir);
            //    e.Ignore(x => x.Total);
            //});

            //modelBuilder.Entity<TransaksiPenjualan>(e =>
            //{
            //    e.HasOne(x => x.Penjualan).WithMany(x => x.TransaksiPenjualan).HasForeignKey(x => x.PenjualanId);
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.Version).IsRowVersion();
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //    e.Ignore(x => x.NominalSebelum);
            //    e.Ignore(x => x.Balance);
            //    e.Ignore(x => x.BalanceLabel);
            //});

            //modelBuilder.Entity<Produksi>(e =>
            //{
            //    e.HasOne(x => x.Barang).WithMany(x => x.Produksi).HasForeignKey(x => x.BarangId);
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //    e.Property(x => x.Version).IsRowVersion();
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //    e.Ignore(x => x.JumlahSebelum);
            //    e.Ignore(x => x.StokAkhir);
            //    e.Ignore(x => x.Total);
            //    e.Ignore(x => x.JumlahTerkunci);
            //});

            //modelBuilder.Entity<ProduksiDetailBahan>(e =>
            //{
            //    e.HasOne(x => x.Produksi).WithMany(x => x.ProduksiDetailBahan).HasForeignKey(x => x.ProduksiId);
            //    e.HasOne(x => x.Bahan).WithMany(x => x.ProduksiDetailBahan).HasForeignKey(x => x.BahanId);
            //    e.HasIndex(x => new { x.ProduksiId, x.BahanId }).IsUnique();
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //    e.Ignore(x => x.JumlahSebelum);
            //    e.Ignore(x => x.StokAkhir);
            //    e.Ignore(x => x.JumlahTerkunci);
            //});

            //modelBuilder.Entity<ProduksiDetailJasa>(e =>
            //{
            //    e.HasOne(x => x.Produksi).WithMany(x => x.ProduksiDetailJasa).HasForeignKey(x => x.ProduksiId);
            //    e.HasOne(x => x.Karyawan).WithMany(x => x.ProduksiDetailJasa).HasForeignKey(x => x.KaryawanId);
            //    e.HasIndex(x => new { x.ProduksiId, x.KaryawanId }).IsUnique();
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //});

            //modelBuilder.Entity<ProduksiDetailOverhead>(e =>
            //{
            //    e.HasOne(x => x.Produksi).WithMany(x => x.ProduksiDetailOverhead).HasForeignKey(x => x.ProduksiId);
            //    e.HasOne(x => x.Overhead).WithMany(x => x.ProduksiDetailOverhead).HasForeignKey(x => x.OverheadId);
            //    e.HasIndex(x => new { x.ProduksiId, x.OverheadId }).IsUnique();
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //});

            //modelBuilder.Entity<TransaksiLain>(e =>
            //{
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //});

            //modelBuilder.Entity<ReturPembelian>(e =>
            //{
            //    e.HasOne(x => x.Pembelian).WithOne(x => x.ReturPembelian).HasForeignKey<ReturPembelian>(x => x.PembelianId);
            //    e.Property(x => x.Version).IsRowVersion();
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //});

            //modelBuilder.Entity<ReturPembelianDetail>(e =>
            //{
            //    e.HasOne(x => x.ReturPembelian).WithMany(x => x.ReturPembelianDetail).HasForeignKey(x => x.ReturPembelianId);
            //    e.HasOne(x => x.BahanSatuan).WithMany(x => x.ReturPembelianDetail).HasForeignKey(x => x.BahanSatuanId);
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.MaxJumlah).HasPrecision(9, 2);
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //    e.Ignore(x => x.Total);
            //});

            //modelBuilder.Entity<ReturPenjualan>(e =>
            //{
            //    e.HasOne(x => x.Penjualan).WithOne(x => x.ReturPenjualan).HasForeignKey<ReturPenjualan>(x => x.PenjualanId);
            //    e.Property(x => x.Version).IsRowVersion();
            //    e.Ignore(x => x.InputTanggal);
            //    e.Ignore(x => x.InputWaktu);
            //});

            //modelBuilder.Entity<ReturPenjualanDetail>(e =>
            //{
            //    e.HasOne(x => x.ReturPenjualan).WithMany(x => x.ReturPenjualanDetail).HasForeignKey(x => x.ReturPenjualanId);
            //    e.HasOne(x => x.BarangSatuan).WithMany(x => x.ReturPenjualanDetail).HasForeignKey(x => x.BarangSatuanId);
            //    e.Property(x => x.Id).ValueGeneratedNever();
            //    e.Property(x => x.MaxJumlah).HasPrecision(9, 2);
            //    e.Property(x => x.Jumlah).HasPrecision(9, 2);
            //    e.Ignore(x => x.Total);
            //});

            #endregion Model Configuration
        }
    }
}