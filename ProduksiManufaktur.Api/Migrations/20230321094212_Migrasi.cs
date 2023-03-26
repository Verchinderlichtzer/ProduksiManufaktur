using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProduksiManufaktur.Api.Migrations
{
    /// <inheritdoc />
    public partial class Migrasi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bahan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SatuanProduksi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StokAwal = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Stok = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    StokMinimal = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bahan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Barang",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SatuanProduksi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StokAwal = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Stok = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    StokMinimal = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telepon = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Overhead",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Overhead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pekerjaan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pekerjaan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telepon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pengurus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jabatan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telepon = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransaksiLain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jenis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nominal = table.Column<int>(type: "int", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaksiLain", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TempatLahir = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TanggalLahir = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BahanSatuan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BahanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ukuran = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Harga = table.Column<int>(type: "int", nullable: false),
                    KonversiStok = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BahanSatuan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BahanSatuan_Bahan_BahanId",
                        column: x => x.BahanId,
                        principalTable: "Bahan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerubahanStokBahan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BahanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jenis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerubahanStokBahan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerubahanStokBahan_Bahan_BahanId",
                        column: x => x.BahanId,
                        principalTable: "Bahan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BarangSatuan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarangId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ukuran = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Harga = table.Column<int>(type: "int", nullable: false),
                    KonversiStok = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarangSatuan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarangSatuan_Barang_BarangId",
                        column: x => x.BarangId,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Formulasi",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BarangId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formulasi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formulasi_Barang_BarangId",
                        column: x => x.BarangId,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerubahanStokBarang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BarangId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jenis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JenisSebelum = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerubahanStokBarang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerubahanStokBarang_Barang_BarangId",
                        column: x => x.BarangId,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produksi",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BarangId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BiayaJasa = table.Column<int>(type: "int", nullable: false),
                    BiayaOverhead = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produksi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produksi_Barang_BarangId",
                        column: x => x.BarangId,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Penjualan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<int>(type: "int", nullable: false),
                    PPN = table.Column<int>(type: "int", nullable: false),
                    Terbayar = table.Column<int>(type: "int", nullable: false),
                    MetodeBayar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JatuhTempo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penjualan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penjualan_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Karyawan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PekerjaanId = table.Column<int>(type: "int", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TempatLahir = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TanggalLahir = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telepon = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Upah = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karyawan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Karyawan_Pekerjaan_PekerjaanId",
                        column: x => x.PekerjaanId,
                        principalTable: "Pekerjaan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pembelian",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<int>(type: "int", nullable: false),
                    PPN = table.Column<int>(type: "int", nullable: false),
                    Terbayar = table.Column<int>(type: "int", nullable: false),
                    MetodeBayar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JatuhTempo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pembelian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pembelian_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogTransaksi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Entitas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntitasId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTransaksi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogTransaksi_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormulasiDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FormulasiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BahanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulasiDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormulasiDetail_Bahan_BahanId",
                        column: x => x.BahanId,
                        principalTable: "Bahan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulasiDetail_Formulasi_FormulasiId",
                        column: x => x.FormulasiId,
                        principalTable: "Formulasi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProduksiDetailBahan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProduksiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BahanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduksiDetailBahan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProduksiDetailBahan_Bahan_BahanId",
                        column: x => x.BahanId,
                        principalTable: "Bahan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProduksiDetailBahan_Produksi_ProduksiId",
                        column: x => x.ProduksiId,
                        principalTable: "Produksi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProduksiDetailOverhead",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProduksiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OverheadId = table.Column<int>(type: "int", nullable: false),
                    Biaya = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduksiDetailOverhead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProduksiDetailOverhead_Overhead_OverheadId",
                        column: x => x.OverheadId,
                        principalTable: "Overhead",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProduksiDetailOverhead_Produksi_ProduksiId",
                        column: x => x.ProduksiId,
                        principalTable: "Produksi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PenjualanDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PenjualanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BarangSatuanId = table.Column<int>(type: "int", nullable: false),
                    MinJumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Harga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenjualanDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PenjualanDetail_BarangSatuan_BarangSatuanId",
                        column: x => x.BarangSatuanId,
                        principalTable: "BarangSatuan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PenjualanDetail_Penjualan_PenjualanId",
                        column: x => x.PenjualanId,
                        principalTable: "Penjualan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturPenjualan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PenjualanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GrandTotal = table.Column<int>(type: "int", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturPenjualan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturPenjualan_Penjualan_PenjualanId",
                        column: x => x.PenjualanId,
                        principalTable: "Penjualan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransaksiPenjualan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PenjualanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nominal = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaksiPenjualan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaksiPenjualan_Penjualan_PenjualanId",
                        column: x => x.PenjualanId,
                        principalTable: "Penjualan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProduksiDetailJasa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProduksiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KaryawanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Biaya = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduksiDetailJasa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProduksiDetailJasa_Karyawan_KaryawanId",
                        column: x => x.KaryawanId,
                        principalTable: "Karyawan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProduksiDetailJasa_Produksi_ProduksiId",
                        column: x => x.ProduksiId,
                        principalTable: "Produksi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PembelianDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PembelianId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BahanSatuanId = table.Column<int>(type: "int", nullable: false),
                    MinJumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Harga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PembelianDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PembelianDetail_BahanSatuan_BahanSatuanId",
                        column: x => x.BahanSatuanId,
                        principalTable: "BahanSatuan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PembelianDetail_Pembelian_PembelianId",
                        column: x => x.PembelianId,
                        principalTable: "Pembelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturPembelian",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PembelianId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GrandTotal = table.Column<int>(type: "int", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturPembelian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturPembelian_Pembelian_PembelianId",
                        column: x => x.PembelianId,
                        principalTable: "Pembelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransaksiPembelian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PembelianId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nominal = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaksiPembelian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaksiPembelian_Pembelian_PembelianId",
                        column: x => x.PembelianId,
                        principalTable: "Pembelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturPenjualanDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BarangSatuanId = table.Column<int>(type: "int", nullable: false),
                    ReturPenjualanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaxJumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Harga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturPenjualanDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturPenjualanDetail_BarangSatuan_BarangSatuanId",
                        column: x => x.BarangSatuanId,
                        principalTable: "BarangSatuan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturPenjualanDetail_ReturPenjualan_ReturPenjualanId",
                        column: x => x.ReturPenjualanId,
                        principalTable: "ReturPenjualan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturPembelianDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BahanSatuanId = table.Column<int>(type: "int", nullable: false),
                    ReturPembelianId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaxJumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Harga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturPembelianDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturPembelianDetail_BahanSatuan_BahanSatuanId",
                        column: x => x.BahanSatuanId,
                        principalTable: "BahanSatuan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturPembelianDetail_ReturPembelian_ReturPembelianId",
                        column: x => x.ReturPembelianId,
                        principalTable: "ReturPembelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Bahan",
                columns: new[] { "Id", "Nama", "SatuanProduksi", "Stok", "StokAwal", "StokMinimal" },
                values: new object[,]
                {
                    { "BHN0001", "Tepung Terigu", "gram", 0m, 0m, 15000m },
                    { "BHN0002", "Telur", "butir", 0m, 0m, 48m },
                    { "BHN0003", "Gula Pasir", "gram", 0m, 0m, 8000m },
                    { "BHN0004", "Garam", "gram", 0m, 0m, 1000m },
                    { "BHN0005", "Ragi Instan", "gram", 0m, 0m, 200m },
                    { "BHN0006", "Baking Powder", "gram", 0m, 0m, 160m },
                    { "BHN0007", "Tepung Maizena", "gram", 0m, 0m, 1500m },
                    { "BHN0008", "Tepung Tapioka", "gram", 0m, 0m, 1400m },
                    { "BHN0009", "Vanili Bubuk", "gram", 0m, 0m, 100m },
                    { "BHN0010", "Cokelat Bubuk", "gram", 0m, 0m, 750m },
                    { "BHN0011", "Margarin", "gram", 0m, 0m, 1250m },
                    { "BHN0012", "Soda Kue", "gram", 0m, 0m, 80m },
                    { "BHN0016", "Tepung Beras", "gram", 0m, 0m, 2000m },
                    { "BHN0018", "Susu Bubuk Full Cream", "gram", 0m, 0m, 1500m },
                    { "BHN0019", "Selai Cokelat", "gram", 0m, 0m, 400m },
                    { "BHN0022", "Mentega (Butter)", "gram", 0m, 0m, 1000m },
                    { "BHN0023", "Selai Nanas", "gram", 0m, 0m, 400m },
                    { "BHN0028", "Susu Cair Full Cream", "ml", 0m, 0m, 3000m },
                    { "BHN0029", "Keju Cheddar", "gram", 0m, 0m, 1200m },
                    { "BHN0032", "Gula Halus", "gram", 0m, 0m, 1600m },
                    { "BHN0033", "Minyak Kelapa Sawit", "ml", 0m, 0m, 2000m },
                    { "BHN0035", "Air Mineral", "ml", 0m, 0m, 10000m },
                    { "BHN0036", "Pasta Cokelat", "gram", 0m, 0m, 650m },
                    { "BHN0038", "Kental Manis", "gram", 0m, 0m, 800m },
                    { "BHN0039", "Santan Kelapa Cair", "ml", 0m, 0m, 600m },
                    { "BHN0040", "Pasta Pandan", "ml", 0m, 0m, 500m },
                    { "BHN0041", "Meises", "gram", 0m, 0m, 850m },
                    { "BHN0044", "Cokelat Chip", "gram", 0m, 0m, 600m },
                    { "BHN0047", "Santan Kelapa Bubuk", "gram", 0m, 0m, 400m },
                    { "BHN0048", "Pengembang Kue (Emulsifier)", "gram", 0m, 0m, 130m },
                    { "BHN0049", "Pewarna Makanan", "ml", 0m, 0m, 100m },
                    { "BHN0056", "Tepung Sagu", "gram", 0m, 0m, 1500m },
                    { "BHN0066", "Keju Edam", "gram", 0m, 0m, 1400m }
                });

            migrationBuilder.InsertData(
                table: "Barang",
                columns: new[] { "Id", "Nama", "SatuanProduksi", "Stok", "StokAwal", "StokMinimal" },
                values: new object[,]
                {
                    { "BRG0006", "Roti Tawar", "pcs", 0m, 0m, 14m },
                    { "BRG0010", "Roti Sobek Isi Cokelat Keju", "pcs", 0m, 0m, 13m },
                    { "BRG0013", "Bolu Kukus", "pcs", 0m, 0m, 14m },
                    { "BRG0017", "Donat", "pcs", 0m, 0m, 15m },
                    { "BRG0024", "Kukis Cokelat", "pcs", 0m, 0m, 25m },
                    { "BRG0025", "Nastar", "pcs", 0m, 0m, 27m },
                    { "BRG0026", "Kue Putri Salju", "pcs", 0m, 0m, 28m },
                    { "BRG0028", "Sagu Keju", "pcs", 0m, 0m, 30m },
                    { "BRG0029", "Kastengel", "pcs", 0m, 0m, 30m }
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Alamat", "Email", "Fax", "Nama", "Telepon" },
                values: new object[,]
                {
                    { "CST0001", "Surabaya", null, null, "Customer A", "084277593471" },
                    { "CST0002", "Cirebon", null, null, "Customer B", "081154285364" },
                    { "CST0003", "Tangerang", null, null, "Customer C", "082535846940" },
                    { "CST0004", "Bandung", "exemail12@gmail.com", "123456", "Customer D", "081152953535" },
                    { "CST0005", "Malang", "test32@gmail.com", null, "Customer E", "08237583473" }
                });

            migrationBuilder.InsertData(
                table: "Overhead",
                columns: new[] { "Id", "Nama" },
                values: new object[,]
                {
                    { 1, "Biaya listrik" },
                    { 2, "Biaya angkut" },
                    { 3, "Biaya bahan penolong" },
                    { 4, "Biaya tenaga kerja tidak langsung" },
                    { 5, "Biaya kemasan" },
                    { 6, "Biaya lain-lain" }
                });

            migrationBuilder.InsertData(
                table: "Pekerjaan",
                columns: new[] { "Id", "Nama" },
                values: new object[,]
                {
                    { 1, "Koki" },
                    { 2, "Produksi" },
                    { 3, "Penjual" },
                    { 4, "Sopir" }
                });

            migrationBuilder.InsertData(
                table: "Profil",
                columns: new[] { "Id", "Alamat", "Email", "Fax", "Jabatan", "Logo", "Nama", "Pengurus", "Telepon", "Website" },
                values: new object[] { 1, "Perumahan Bumi Anggrek Blok K No 80", "example@gmail.com", "021-1234567", "Direktur", new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 100, 0, 0, 0, 100, 8, 6, 0, 0, 0, 112, 226, 149, 84, 0, 0, 0, 6, 98, 75, 71, 68, 0, 255, 0, 255, 0, 255, 160, 189, 167, 147, 0, 0, 10, 162, 73, 68, 65, 84, 88, 9, 237, 89, 11, 108, 20, 199, 25, 254, 118, 207, 175, 123, 27, 165, 6, 167, 160, 220, 57, 6, 82, 251, 82, 74, 3, 77, 170, 4, 8, 9, 45, 152, 135, 160, 165, 32, 40, 9, 82, 160, 77, 41, 81, 16, 145, 32, 85, 164, 160, 210, 208, 170, 141, 210, 39, 65, 73, 77, 130, 137, 82, 212, 16, 10, 109, 10, 4, 2, 152, 135, 18, 170, 10, 20, 53, 53, 21, 198, 20, 4, 28, 111, 3, 6, 3, 198, 128, 169, 239, 182, 255, 204, 221, 222, 195, 183, 119, 183, 187, 119, 134, 243, 121, 78, 51, 59, 179, 255, 252, 255, 191, 255, 124, 223, 205, 204, 238, 12, 32, 126, 2, 1, 129, 128, 64, 64, 32, 32, 16, 16, 8, 8, 4, 4, 2, 2, 1, 129, 128, 64, 64, 32, 32, 16, 16, 8, 8, 4, 4, 2, 2, 1, 129, 128, 64, 64, 32, 32, 16, 16, 8, 100, 25, 1, 41, 203, 254, 122, 164, 59, 175, 215, 87, 163, 72, 88, 66, 193, 15, 165, 108, 167, 108, 38, 181, 147, 81, 131, 164, 224, 231, 126, 127, 227, 118, 170, 155, 74, 189, 158, 16, 207, 131, 85, 19, 160, 200, 91, 76, 161, 151, 196, 136, 72, 169, 49, 75, 74, 65, 18, 159, 189, 70, 172, 40, 242, 82, 246, 175, 92, 248, 124, 5, 102, 78, 185, 31, 197, 197, 178, 169, 190, 119, 116, 4, 241, 193, 71, 231, 176, 162, 206, 15, 69, 146, 126, 6, 192, 212, 40, 49, 247, 116, 122, 90, 190, 36, 34, 163, 138, 245, 101, 250, 228, 114, 211, 100, 48, 123, 70, 228, 12, 34, 148, 213, 1, 165, 58, 84, 26, 191, 246, 122, 66, 20, 160, 137, 193, 182, 126, 83, 51, 58, 131, 116, 199, 110, 76, 100, 102, 187, 110, 227, 249, 176, 165, 116, 40, 92, 49, 92, 208, 31, 196, 176, 77, 94, 25, 228, 218, 26, 98, 201, 43, 116, 77, 116, 230, 90, 107, 203, 209, 62, 165, 125, 247, 211, 188, 255, 16, 253, 59, 251, 155, 112, 17, 49, 161, 241, 245, 185, 172, 224, 7, 102, 23, 116, 230, 136, 98, 96, 133, 200, 244, 234, 91, 174, 72, 56, 95, 230, 46, 193, 190, 55, 38, 25, 2, 228, 177, 151, 55, 163, 229, 122, 7, 100, 72, 229, 39, 78, 28, 188, 96, 200, 184, 139, 114, 175, 95, 67, 84, 60, 20, 69, 234, 195, 234, 110, 91, 33, 43, 12, 101, 183, 173, 136, 235, 171, 62, 248, 141, 201, 139, 32, 36, 2, 156, 84, 202, 170, 78, 19, 132, 56, 195, 132, 0, 157, 220, 7, 243, 99, 54, 11, 66, 84, 228, 44, 157, 124, 132, 184, 172, 161, 127, 187, 42, 214, 83, 186, 194, 36, 230, 252, 8, 241, 84, 248, 246, 120, 188, 190, 93, 122, 58, 117, 239, 117, 100, 254, 239, 118, 89, 141, 79, 89, 170, 141, 100, 9, 141, 178, 76, 250, 210, 221, 95, 234, 163, 209, 115, 94, 27, 56, 33, 234, 122, 96, 4, 212, 232, 186, 147, 57, 33, 98, 202, 82, 145, 87, 66, 96, 170, 211, 143, 42, 214, 83, 70, 167, 185, 32, 39, 85, 143, 77, 50, 29, 65, 72, 4, 25, 133, 131, 233, 48, 49, 101, 69, 109, 66, 164, 70, 92, 154, 168, 8, 66, 162, 160, 113, 66, 76, 77, 89, 246, 240, 186, 163, 128, 251, 136, 186, 52, 94, 19, 132, 132, 49, 147, 16, 250, 119, 155, 155, 178, 194, 132, 32, 52, 202, 194, 46, 77, 21, 221, 189, 168, 39, 13, 138, 189, 129, 65, 65, 240, 164, 191, 113, 76, 82, 165, 187, 218, 160, 148, 179, 199, 45, 120, 103, 31, 22, 176, 138, 70, 174, 28, 80, 134, 157, 181, 139, 120, 203, 152, 31, 255, 6, 199, 207, 180, 128, 201, 150, 206, 155, 76, 178, 253, 148, 67, 31, 151, 84, 49, 157, 238, 25, 33, 20, 113, 78, 189, 129, 209, 62, 148, 149, 98, 194, 223, 127, 255, 34, 190, 54, 104, 0, 171, 166, 204, 187, 106, 23, 71, 218, 15, 28, 57, 173, 214, 197, 148, 165, 34, 145, 133, 146, 131, 233, 118, 112, 94, 12, 185, 115, 133, 109, 130, 16, 107, 136, 33, 224, 210, 40, 115, 66, 92, 246, 146, 52, 106, 137, 205, 110, 123, 136, 68, 250, 228, 226, 62, 18, 53, 244, 75, 196, 162, 30, 194, 138, 176, 68, 169, 36, 73, 80, 255, 237, 33, 177, 190, 171, 203, 17, 33, 145, 17, 194, 124, 233, 51, 212, 208, 202, 202, 26, 226, 243, 249, 138, 110, 180, 99, 15, 125, 149, 63, 174, 241, 12, 180, 253, 180, 150, 166, 104, 96, 78, 253, 223, 240, 121, 243, 217, 56, 149, 84, 109, 113, 138, 119, 225, 70, 38, 66, 160, 240, 80, 13, 61, 173, 192, 98, 129, 173, 164, 8, 55, 111, 223, 41, 44, 243, 249, 236, 151, 26, 27, 111, 24, 114, 16, 163, 156, 21, 66, 218, 218, 241, 18, 245, 69, 147, 140, 152, 103, 33, 144, 226, 136, 52, 85, 91, 172, 143, 238, 172, 7, 130, 65, 172, 222, 248, 79, 252, 104, 234, 72, 254, 152, 149, 127, 253, 12, 175, 191, 183, 149, 215, 245, 94, 138, 58, 44, 108, 148, 152, 38, 36, 163, 225, 197, 130, 236, 63, 112, 200, 128, 130, 64, 128, 157, 75, 59, 106, 159, 158, 140, 17, 253, 61, 76, 204, 243, 195, 107, 86, 240, 242, 224, 236, 196, 23, 201, 84, 109, 220, 232, 46, 95, 246, 158, 245, 99, 254, 238, 205, 176, 23, 23, 162, 126, 217, 88, 244, 43, 13, 173, 11, 122, 195, 24, 255, 90, 61, 142, 156, 187, 134, 160, 28, 252, 234, 233, 99, 77, 7, 245, 218, 117, 213, 147, 187, 10, 140, 222, 23, 4, 3, 111, 146, 141, 163, 198, 59, 40, 142, 12, 146, 245, 168, 52, 178, 191, 23, 53, 158, 129, 104, 239, 248, 31, 150, 173, 107, 48, 28, 187, 250, 65, 41, 41, 161, 93, 99, 195, 14, 194, 6, 25, 17, 226, 121, 176, 106, 2, 125, 220, 125, 215, 81, 88, 132, 87, 134, 143, 10, 187, 236, 185, 197, 226, 225, 35, 97, 43, 40, 196, 182, 47, 206, 226, 211, 131, 205, 134, 58, 162, 110, 193, 19, 30, 252, 92, 197, 144, 113, 140, 178, 105, 66, 188, 94, 111, 9, 20, 121, 57, 243, 53, 111, 200, 163, 248, 146, 213, 198, 170, 113, 249, 145, 190, 95, 198, 176, 126, 253, 227, 100, 234, 77, 170, 54, 85, 231, 110, 151, 229, 54, 7, 230, 83, 95, 216, 115, 151, 125, 216, 128, 59, 157, 65, 86, 213, 149, 213, 17, 130, 240, 22, 140, 46, 35, 13, 37, 211, 139, 186, 34, 57, 22, 1, 202, 192, 193, 125, 238, 195, 236, 170, 161, 26, 174, 129, 63, 141, 251, 158, 166, 156, 9, 83, 181, 177, 246, 123, 149, 103, 87, 127, 29, 155, 79, 28, 198, 145, 75, 151, 81, 183, 243, 8, 230, 215, 124, 69, 87, 40, 110, 187, 122, 210, 152, 217, 22, 188, 169, 17, 226, 241, 12, 169, 32, 50, 94, 165, 13, 57, 44, 121, 116, 52, 10, 36, 73, 87, 208, 61, 65, 137, 245, 229, 85, 234, 19, 251, 38, 121, 107, 75, 19, 206, 92, 110, 215, 21, 182, 58, 101, 17, 38, 165, 186, 12, 146, 40, 153, 34, 4, 150, 192, 111, 201, 159, 117, 98, 197, 96, 176, 169, 135, 234, 121, 149, 134, 209, 84, 59, 193, 51, 24, 183, 238, 4, 240, 171, 13, 255, 209, 213, 183, 232, 153, 72, 102, 219, 39, 134, 9, 81, 23, 114, 103, 81, 17, 94, 30, 62, 66, 87, 176, 61, 81, 233, 39, 223, 24, 1, 7, 245, 81, 239, 2, 31, 153, 178, 148, 204, 182, 224, 117, 173, 33, 94, 175, 175, 70, 145, 176, 132, 128, 29, 74, 111, 17, 118, 42, 209, 118, 231, 14, 158, 92, 95, 199, 170, 121, 159, 231, 174, 248, 7, 108, 197, 22, 84, 13, 40, 197, 130, 137, 213, 24, 233, 235, 151, 208, 103, 117, 202, 202, 116, 81, 79, 59, 66, 216, 136, 32, 50, 62, 161, 8, 158, 160, 204, 201, 160, 178, 215, 165, 155, 29, 1, 252, 235, 216, 101, 60, 247, 230, 94, 236, 109, 188, 144, 208, 255, 200, 91, 150, 140, 62, 9, 141, 6, 4, 105, 71, 136, 162, 200, 75, 217, 146, 189, 240, 249, 10, 204, 156, 114, 63, 138, 139, 211, 114, 104, 224, 241, 61, 71, 181, 163, 35, 136, 15, 62, 58, 135, 21, 117, 126, 44, 223, 113, 18, 79, 142, 157, 136, 216, 67, 170, 63, 44, 158, 73, 157, 249, 140, 222, 117, 50, 91, 67, 210, 18, 66, 100, 84, 209, 147, 48, 125, 114, 121, 175, 37, 131, 245, 159, 253, 17, 103, 208, 31, 146, 17, 114, 244, 212, 69, 38, 66, 236, 33, 213, 233, 230, 86, 46, 163, 75, 247, 190, 101, 41, 64, 19, 61, 4, 235, 55, 53, 163, 51, 197, 230, 32, 211, 201, 231, 204, 250, 190, 110, 227, 121, 222, 197, 65, 15, 244, 229, 101, 236, 165, 203, 22, 124, 108, 147, 161, 122, 250, 17, 34, 5, 95, 163, 47, 242, 45, 203, 223, 61, 1, 150, 13, 121, 207, 83, 229, 151, 102, 125, 43, 161, 103, 78, 91, 9, 100, 89, 66, 48, 168, 184, 169, 81, 166, 172, 255, 51, 159, 148, 213, 196, 12, 213, 186, 102, 121, 242, 120, 211, 86, 73, 193, 120, 106, 220, 79, 185, 147, 114, 175, 76, 5, 178, 140, 161, 15, 61, 128, 247, 151, 205, 197, 168, 71, 6, 39, 96, 192, 200, 176, 91, 139, 153, 92, 246, 122, 135, 186, 88, 197, 76, 46, 208, 99, 228, 247, 55, 110, 35, 61, 150, 169, 0, 60, 21, 62, 154, 201, 128, 239, 111, 88, 199, 239, 243, 249, 178, 118, 218, 12, 222, 189, 163, 155, 126, 201, 203, 84, 23, 23, 29, 229, 182, 181, 223, 38, 149, 219, 108, 29, 185, 74, 21, 195, 73, 23, 33, 134, 189, 230, 161, 65, 197, 164, 87, 12, 244, 138, 31, 82, 25, 208, 143, 170, 10, 66, 162, 88, 164, 172, 29, 91, 57, 45, 101, 59, 107, 156, 245, 187, 79, 177, 255, 191, 151, 168, 42, 149, 210, 197, 84, 74, 187, 134, 152, 242, 218, 75, 141, 92, 214, 34, 222, 115, 69, 49, 255, 113, 216, 173, 35, 228, 98, 99, 19, 252, 219, 183, 163, 185, 241, 16, 110, 181, 181, 193, 234, 116, 162, 252, 225, 106, 84, 140, 27, 139, 178, 234, 106, 30, 124, 186, 75, 174, 248, 72, 23, 39, 107, 87, 191, 214, 37, 139, 249, 45, 248, 110, 25, 33, 74, 32, 128, 134, 119, 87, 225, 192, 219, 111, 99, 238, 248, 81, 216, 181, 245, 207, 244, 58, 24, 228, 229, 156, 113, 163, 208, 240, 214, 31, 209, 176, 106, 53, 151, 177, 142, 104, 229, 92, 241, 161, 21, 91, 50, 153, 219, 90, 24, 110, 202, 177, 41, 235, 223, 171, 234, 224, 188, 214, 138, 61, 219, 63, 196, 51, 179, 166, 162, 188, 188, 140, 7, 202, 202, 103, 159, 153, 138, 221, 219, 214, 194, 209, 218, 130, 3, 117, 171, 185, 92, 235, 146, 43, 62, 180, 98, 75, 38, 203, 198, 22, 124, 214, 71, 8, 155, 98, 90, 27, 27, 177, 122, 229, 27, 112, 58, 237, 154, 177, 187, 92, 14, 106, 255, 53, 90, 14, 28, 192, 165, 195, 135, 19, 116, 114, 197, 71, 66, 96, 105, 4, 46, 91, 104, 13, 65, 6, 91, 240, 89, 39, 228, 84, 253, 14, 44, 124, 225, 57, 56, 28, 218, 100, 168, 125, 98, 100, 45, 124, 97, 14, 78, 238, 216, 169, 138, 34, 101, 174, 248, 136, 4, 164, 179, 226, 182, 229, 224, 148, 117, 161, 233, 48, 198, 60, 53, 66, 87, 23, 198, 60, 245, 56, 46, 54, 29, 74, 208, 205, 21, 31, 9, 129, 165, 17, 168, 139, 58, 169, 153, 222, 130, 207, 250, 91, 214, 245, 43, 87, 240, 205, 39, 38, 81, 76, 137, 201, 91, 249, 24, 23, 86, 86, 122, 176, 107, 199, 95, 208, 175, 95, 95, 180, 93, 105, 229, 178, 216, 139, 30, 31, 177, 250, 160, 45, 233, 184, 123, 186, 185, 113, 245, 42, 202, 202, 238, 195, 211, 223, 158, 142, 227, 199, 79, 145, 36, 154, 212, 56, 162, 18, 170, 105, 248, 32, 169, 161, 20, 29, 33, 230, 183, 224, 179, 78, 8, 157, 40, 194, 127, 108, 127, 66, 71, 24, 8, 93, 229, 22, 139, 140, 96, 32, 152, 160, 107, 196, 7, 51, 102, 190, 89, 25, 155, 153, 95, 230, 127, 119, 253, 250, 88, 49, 152, 110, 215, 56, 152, 2, 147, 179, 50, 147, 172, 126, 135, 144, 15, 211, 31, 134, 217, 39, 132, 162, 201, 199, 84, 57, 111, 67, 66, 183, 42, 7, 148, 97, 103, 237, 34, 126, 80, 37, 209, 48, 93, 243, 139, 31, 146, 78, 61, 232, 47, 38, 8, 33, 36, 186, 53, 157, 248, 248, 245, 164, 254, 119, 213, 46, 230, 109, 237, 183, 58, 120, 73, 179, 159, 233, 53, 68, 230, 30, 196, 37, 43, 8, 216, 74, 138, 192, 182, 233, 201, 153, 99, 216, 176, 97, 234, 43, 23, 221, 234, 79, 5, 250, 85, 245, 107, 38, 155, 143, 147, 201, 181, 60, 39, 211, 141, 149, 171, 47, 7, 90, 246, 76, 22, 171, 203, 238, 213, 172, 202, 211, 217, 171, 250, 122, 75, 73, 146, 224, 116, 148, 160, 245, 250, 77, 156, 111, 107, 115, 147, 93, 11, 101, 67, 169, 91, 8, 73, 182, 104, 38, 147, 107, 69, 156, 76, 87, 75, 174, 101, 207, 100, 90, 186, 140, 12, 45, 57, 211, 207, 70, 118, 217, 172, 156, 16, 57, 80, 200, 214, 17, 195, 132, 136, 41, 43, 27, 44, 196, 248, 112, 211, 8, 97, 183, 146, 98, 110, 63, 171, 91, 70, 8, 11, 40, 223, 178, 177, 3, 42, 128, 142, 189, 77, 45, 236, 130, 16, 157, 255, 156, 99, 43, 167, 233, 210, 124, 113, 229, 62, 124, 242, 197, 25, 210, 53, 183, 5, 95, 64, 150, 89, 77, 86, 167, 131, 127, 124, 117, 117, 170, 144, 128, 205, 223, 84, 196, 37, 171, 203, 25, 119, 207, 110, 114, 197, 7, 139, 197, 104, 142, 110, 159, 228, 200, 148, 245, 157, 247, 234, 140, 246, 33, 65, 63, 87, 124, 36, 4, 166, 67, 160, 110, 159, 208, 135, 34, 91, 212, 117, 88, 196, 171, 136, 69, 61, 30, 143, 140, 239, 28, 225, 99, 92, 192, 220, 148, 37, 8, 201, 152, 130, 120, 7, 247, 116, 202, 90, 59, 109, 70, 124, 52, 57, 118, 87, 225, 238, 131, 205, 147, 159, 229, 81, 77, 218, 184, 6, 254, 235, 87, 121, 61, 246, 162, 165, 19, 43, 91, 27, 171, 172, 163, 174, 78, 89, 180, 65, 90, 170, 67, 61, 65, 37, 163, 69, 253, 224, 236, 5, 9, 14, 115, 85, 240, 241, 148, 217, 105, 67, 211, 163, 147, 206, 73, 100, 132, 200, 48, 245, 218, 43, 166, 172, 116, 8, 27, 108, 143, 30, 227, 154, 59, 19, 17, 132, 24, 4, 60, 157, 186, 179, 36, 178, 167, 104, 106, 202, 18, 132, 164, 67, 216, 96, 187, 219, 94, 164, 90, 152, 34, 132, 182, 238, 85, 123, 237, 210, 83, 225, 219, 67, 45, 163, 33, 126, 153, 35, 160, 96, 247, 73, 127, 227, 152, 84, 142, 244, 140, 144, 140, 22, 254, 84, 15, 239, 109, 109, 138, 36, 9, 44, 123, 27, 233, 162, 191, 2, 1, 129, 128, 64, 64, 32, 32, 16, 16, 8, 8, 4, 4, 2, 2, 1, 129, 128, 64, 64, 32, 32, 16, 16, 8, 8, 4, 4, 2, 2, 1, 129, 128, 64, 64, 32, 144, 11, 8, 252, 31, 169, 202, 13, 179, 208, 197, 176, 70, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 }, "Hanif Manufaktur", "Mahdiya", "085739194810", "www.hanifmanufaktur.com" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "98b79864-fff9-46e0-9cf4-e58c8c782710", "79bbaf13-c30b-4b13-827b-2be942a11545", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Supplier",
                columns: new[] { "Id", "Alamat", "Email", "Fax", "Nama", "Telepon" },
                values: new object[,]
                {
                    { "SPL0001", "Banten", "supp@gmail.com", "987654", "Supplier A", "085377419674" },
                    { "SPL0002", "Banjarmasin", null, null, "Supplier B", "089244218647" },
                    { "SPL0003", "Solo", "coba36@gmail.com", "234567", "Supplier C", "082364664466" },
                    { "SPL0004", "Sukabumi", null, null, "Supplier D", "08235384125" },
                    { "SPL0005", "Mojokerto", "supplierlima@gmail.co.id", null, "Supplier E", "084264825547" }
                });

            migrationBuilder.InsertData(
                table: "TransaksiLain",
                columns: new[] { "Id", "Jenis", "Kategori", "Keterangan", "Nominal", "Tanggal" },
                values: new object[,]
                {
                    { 1, "Pengeluaran", "Beban Sewa", "Sewa Gedung pada bulan November 2022", 95000, new DateTime(2022, 11, 30, 16, 28, 41, 0, DateTimeKind.Unspecified) },
                    { 2, "Pengeluaran", "Beban Perbaikan dan Renovasi", "Beberapa mesin rusak", 120000, new DateTime(2022, 12, 21, 8, 15, 55, 0, DateTimeKind.Unspecified) },
                    { 3, "Pendapatan", "Pendapatan Lain-lain", "Ok", 150000, new DateTime(2023, 2, 9, 22, 24, 33, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "Alamat", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TanggalLahir", "TempatLahir", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4b35caf0-9371-447b-ab23-30ff5ef8586c", 0, "Perumahan Bumi Anggrek Blok K No 80", "67d704f0-db9f-43d9-ac27-8c1937ae7d81", "sujudihanif36@gmail.com", true, false, null, "SUJUDIHANIF36@GMAIL.COM", "SUJUDIHANIF36@GMAIL.COM", "AQAAAAIAAYagAAAAEDF80p+z0OgQH9yZ5j+z6W8bphkX52+PUhg4nz7H+9mLTqn1bdsExZqQtkeJYeQLtg==", "085739194810", true, "5ZI4DZQUJY2FPYAH2T4S4DFM7GCOBAKL", new DateTime(2002, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bekasi", false, "sujudihanif36@gmail.com" },
                    { "d6ca57a9-dbdf-48d5-867b-7e5431ea4018", 0, "Jakarta", "005e218f-6b61-4c45-9f4f-daf314279e4a", "sujudihanif@yahoo.co.id", false, false, null, "SUJUDIHANIF@YAHOO.CO.ID", "SUJUDIHANIF@YAHOO.CO.ID", "AQAAAAIAAYagAAAAEIrKzVjcLeG32l2jA5tlZ4ISF/4Y3H7F0DlJdf5Hqyj0QeXRy9Lbe8+0smI/eUmpng==", "085372842236", true, "TQ5AE6UTHWKH7DI4N32ANGT2NZWM64SS", new DateTime(2000, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Semarang", false, "sujudihanif@yahoo.co.id" }
                });

            migrationBuilder.InsertData(
                table: "BahanSatuan",
                columns: new[] { "Id", "BahanId", "Harga", "KonversiStok", "Nama", "Ukuran" },
                values: new object[,]
                {
                    { 1, "BHN0001", 140000, 10000m, "dus", "20 x 500 g" },
                    { 2, "BHN0001", 12500, 1000m, "pak", "kg" },
                    { 3, "BHN0001", 6500, 500m, "pak", "500 g" },
                    { 4, "BHN0002", 28000, 16m, "kg", "" },
                    { 5, "BHN0003", 300000, 20000m, "dus", "20 x 1 kg" },
                    { 6, "BHN0003", 13000, 1000m, "pak", "kg" },
                    { 7, "BHN0004", 5000, 500m, "pcs", "500 g" },
                    { 8, "BHN0005", 6000, 11m, "sachet", "11 g" },
                    { 9, "BHN0006", 6500, 45m, "toples", "45 g" },
                    { 10, "BHN0007", 5500, 100m, "sachet", "100 g" },
                    { 11, "BHN0008", 7000, 500m, "pcs", "500 g" },
                    { 12, "BHN0009", 7000, 20m, "botol", "20 g" },
                    { 13, "BHN0010", 25000, 500m, "sachet", "500 g" },
                    { 14, "BHN0011", 5500, 200m, "pak", "200 g" },
                    { 15, "BHN0012", 6000, 81m, "toples", "81 g" },
                    { 19, "BHN0016", 10800, 500m, "pak", "500 g" },
                    { 21, "BHN0018", 40000, 800m, "pak", "800 g" },
                    { 22, "BHN0019", 27000, 150m, "botol", "150 g" },
                    { 25, "BHN0022", 12000, 200m, "pak", "200 g" },
                    { 26, "BHN0023", 20000, 170m, "botol", "170 g" },
                    { 31, "BHN0028", 17500, 1000m, "pcs", "liter" },
                    { 32, "BHN0029", 23000, 165m, "pcs", "165 g" },
                    { 35, "BHN0032", 11000, 250m, "pak", "250 g" },
                    { 36, "BHN0033", 34000, 2000m, "pouch", "2 liter" },
                    { 38, "BHN0035", 40000, 19000m, "galon", "19 liter" },
                    { 39, "BHN0036", 100000, 500m, "toples", "500 g" },
                    { 41, "BHN0038", 12000, 370m, "kaleng", "370 g" },
                    { 42, "BHN0039", 16500, 200m, "pcs", "200 ml" },
                    { 43, "BHN0040", 8000, 60m, "botol", "60 ml" },
                    { 44, "BHN0041", 17000, 200m, "pak", "200 g" },
                    { 47, "BHN0044", 32000, 500m, "pak", "500 g" },
                    { 50, "BHN0047", 9000, 80m, "sachet", "80 g" },
                    { 51, "BHN0048", 10500, 30m, "sachet", "30 g" },
                    { 52, "BHN0049", 5000, 30m, "botol", "30 ml" },
                    { 59, "BHN0056", 13000, 500m, "pak", "500 g" },
                    { 69, "BHN0066", 33000, 250m, "pcs", "250 g" },
                    { 70, "BHN0002", 2500, 1m, "butir", "" },
                    { 71, "BHN0035", 53000, 18000m, "dus", "12 x 1,5 liter" },
                    { 72, "BHN0035", 5500, 1500m, "botol", "1,5 liter" }
                });

            migrationBuilder.InsertData(
                table: "BarangSatuan",
                columns: new[] { "Id", "BarangId", "Harga", "KonversiStok", "Nama", "Ukuran" },
                values: new object[,]
                {
                    { 7, "BRG0006", 20000, 1m, "pcs", "" },
                    { 11, "BRG0010", 20500, 1m, "pcs", "" },
                    { 14, "BRG0013", 30000, 12m, "pak", "12 pcs" },
                    { 18, "BRG0017", 30000, 12m, "box", "12 pcs" },
                    { 25, "BRG0024", 32000, 24m, "toples", "24 pcs" },
                    { 26, "BRG0025", 70000, 44m, "toples", "44 pcs" },
                    { 27, "BRG0025", 30000, 18m, "pak", "18 pcs" },
                    { 28, "BRG0026", 50000, 36m, "toples", "28 pcs" },
                    { 30, "BRG0028", 55000, 30m, "toples", "30 pcs" },
                    { 31, "BRG0029", 75000, 42m, "toples", "42 pcs" }
                });

            migrationBuilder.InsertData(
                table: "Formulasi",
                columns: new[] { "Id", "BarangId", "Jumlah" },
                values: new object[,]
                {
                    { "F0002", "BRG0025", 60m },
                    { "F0004", "BRG0029", 60m },
                    { "F0005", "BRG0026", 45m },
                    { "F0006", "BRG0017", 20m },
                    { "F0007", "BRG0024", 50m },
                    { "F0008", "BRG0006", 3m }
                });

            migrationBuilder.InsertData(
                table: "Karyawan",
                columns: new[] { "Id", "Alamat", "Email", "Nama", "PekerjaanId", "TanggalLahir", "Telepon", "TempatLahir", "Upah" },
                values: new object[,]
                {
                    { "KYN0001", "Semarang", null, "Alma", 1, new DateTime(1967, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "081153822928", "Bandung", 50000 },
                    { "KYN0002", "Cilacap", "dima1337@gmail.co.id", "Dima", 2, new DateTime(1983, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "083357463286", "Surabaya", 65000 },
                    { "KYN0003", "Madiun", "faran22@gmail.co.id", "Faran", 1, new DateTime(1988, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Makassar", 55000 },
                    { "KYN0004", "Lumajang", "erlina16@yahoo.com", "Erlina", 1, new DateTime(1976, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "087733882727", "Banten", 45000 },
                    { "KYN0005", "Jepara", "mark@gmail.com", "Mark", 4, new DateTime(1990, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "084353847756", "Magelang", 70000 }
                });

            migrationBuilder.InsertData(
                table: "RoleClaim",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Akun", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 2, "Produk", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 3, "Pekerja", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 4, "Pihak", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 5, "Overhead", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 6, "Pembelian", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 7, "Penjualan", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 8, "Produksi", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 9, "TransaksiLain", "W2", "98b79864-fff9-46e0-9cf4-e58c8c782710" },
                    { 10, "Report", "W1", "98b79864-fff9-46e0-9cf4-e58c8c782710" }
                });

            migrationBuilder.InsertData(
                table: "UserClaim",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "Akun", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 2, "Produk", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 3, "Pekerja", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 4, "Pihak", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 5, "Overhead", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 6, "Pembelian", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 7, "Penjualan", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 8, "Produksi", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 9, "TransaksiLain", "S2", "4b35caf0-9371-447b-ab23-30ff5ef8586c" },
                    { 10, "Report", "S1", "4b35caf0-9371-447b-ab23-30ff5ef8586c" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "98b79864-fff9-46e0-9cf4-e58c8c782710", "4b35caf0-9371-447b-ab23-30ff5ef8586c" });

            migrationBuilder.InsertData(
                table: "FormulasiDetail",
                columns: new[] { "Id", "BahanId", "FormulasiId", "Jumlah" },
                values: new object[,]
                {
                    { 11, "BHN0011", "F0002", 125m },
                    { 12, "BHN0022", "F0002", 125m },
                    { 13, "BHN0032", "F0002", 75m },
                    { 14, "BHN0009", "F0002", 2.5m },
                    { 15, "BHN0002", "F0002", 3m },
                    { 16, "BHN0001", "F0002", 350m },
                    { 17, "BHN0007", "F0002", 50m },
                    { 18, "BHN0018", "F0002", 50m },
                    { 19, "BHN0023", "F0002", 50m },
                    { 20, "BHN0029", "F0002", 35m },
                    { 21, "BHN0033", "F0002", 2.5m },
                    { 36, "BHN0011", "F0004", 300m },
                    { 37, "BHN0066", "F0004", 100m },
                    { 38, "BHN0029", "F0004", 100m },
                    { 39, "BHN0001", "F0004", 350m },
                    { 40, "BHN0007", "F0004", 50m },
                    { 41, "BHN0018", "F0004", 20m },
                    { 42, "BHN0002", "F0004", 2m },
                    { 43, "BHN0002", "F0005", 2m },
                    { 44, "BHN0022", "F0005", 200m },
                    { 45, "BHN0011", "F0005", 300m },
                    { 46, "BHN0032", "F0005", 100m },
                    { 47, "BHN0066", "F0005", 50m },
                    { 48, "BHN0029", "F0005", 50m },
                    { 49, "BHN0001", "F0005", 650m },
                    { 50, "BHN0018", "F0005", 60m },
                    { 51, "BHN0007", "F0005", 65m },
                    { 52, "BHN0001", "F0006", 1000m },
                    { 53, "BHN0005", "F0006", 3m },
                    { 54, "BHN0003", "F0006", 150m },
                    { 55, "BHN0035", "F0006", 125m },
                    { 56, "BHN0011", "F0006", 20m },
                    { 57, "BHN0004", "F0006", 3m },
                    { 58, "BHN0041", "F0006", 100m },
                    { 59, "BHN0002", "F0006", 4m },
                    { 60, "BHN0022", "F0007", 200m },
                    { 61, "BHN0009", "F0007", 4m },
                    { 62, "BHN0002", "F0007", 2m },
                    { 63, "BHN0032", "F0007", 175m },
                    { 64, "BHN0001", "F0007", 250m },
                    { 65, "BHN0010", "F0007", 20m },
                    { 66, "BHN0006", "F0007", 4m },
                    { 67, "BHN0044", "F0007", 175m },
                    { 68, "BHN0001", "F0008", 600m },
                    { 69, "BHN0018", "F0008", 200m },
                    { 70, "BHN0032", "F0008", 100m },
                    { 71, "BHN0011", "F0008", 100m },
                    { 72, "BHN0005", "F0008", 10m },
                    { 73, "BHN0002", "F0008", 4m },
                    { 74, "BHN0048", "F0008", 5m },
                    { 75, "BHN0004", "F0008", 3m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BahanSatuan_BahanId_Nama_Ukuran",
                table: "BahanSatuan",
                columns: new[] { "BahanId", "Nama", "Ukuran" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BarangSatuan_BarangId_Nama_Ukuran",
                table: "BarangSatuan",
                columns: new[] { "BarangId", "Nama", "Ukuran" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email",
                table: "Customer",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Fax",
                table: "Customer",
                column: "Fax",
                unique: true,
                filter: "[Fax] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Telepon",
                table: "Customer",
                column: "Telepon",
                unique: true,
                filter: "[Telepon] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Formulasi_BarangId",
                table: "Formulasi",
                column: "BarangId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulasiDetail_BahanId",
                table: "FormulasiDetail",
                column: "BahanId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulasiDetail_FormulasiId_BahanId",
                table: "FormulasiDetail",
                columns: new[] { "FormulasiId", "BahanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Karyawan_Email",
                table: "Karyawan",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Karyawan_PekerjaanId",
                table: "Karyawan",
                column: "PekerjaanId");

            migrationBuilder.CreateIndex(
                name: "IX_Karyawan_Telepon",
                table: "Karyawan",
                column: "Telepon",
                unique: true,
                filter: "[Telepon] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LogTransaksi_UserId",
                table: "LogTransaksi",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Overhead_Nama",
                table: "Overhead",
                column: "Nama",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pekerjaan_Nama",
                table: "Pekerjaan",
                column: "Nama",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pembelian_SupplierId",
                table: "Pembelian",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PembelianDetail_BahanSatuanId",
                table: "PembelianDetail",
                column: "BahanSatuanId");

            migrationBuilder.CreateIndex(
                name: "IX_PembelianDetail_PembelianId_BahanSatuanId",
                table: "PembelianDetail",
                columns: new[] { "PembelianId", "BahanSatuanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Penjualan_CustomerId",
                table: "Penjualan",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PenjualanDetail_BarangSatuanId",
                table: "PenjualanDetail",
                column: "BarangSatuanId");

            migrationBuilder.CreateIndex(
                name: "IX_PenjualanDetail_PenjualanId_BarangSatuanId",
                table: "PenjualanDetail",
                columns: new[] { "PenjualanId", "BarangSatuanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerubahanStokBahan_BahanId",
                table: "PerubahanStokBahan",
                column: "BahanId");

            migrationBuilder.CreateIndex(
                name: "IX_PerubahanStokBarang_BarangId",
                table: "PerubahanStokBarang",
                column: "BarangId");

            migrationBuilder.CreateIndex(
                name: "IX_Produksi_BarangId",
                table: "Produksi",
                column: "BarangId");

            migrationBuilder.CreateIndex(
                name: "IX_ProduksiDetailBahan_BahanId",
                table: "ProduksiDetailBahan",
                column: "BahanId");

            migrationBuilder.CreateIndex(
                name: "IX_ProduksiDetailBahan_ProduksiId_BahanId",
                table: "ProduksiDetailBahan",
                columns: new[] { "ProduksiId", "BahanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProduksiDetailJasa_KaryawanId",
                table: "ProduksiDetailJasa",
                column: "KaryawanId");

            migrationBuilder.CreateIndex(
                name: "IX_ProduksiDetailJasa_ProduksiId_KaryawanId",
                table: "ProduksiDetailJasa",
                columns: new[] { "ProduksiId", "KaryawanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProduksiDetailOverhead_OverheadId",
                table: "ProduksiDetailOverhead",
                column: "OverheadId");

            migrationBuilder.CreateIndex(
                name: "IX_ProduksiDetailOverhead_ProduksiId_OverheadId",
                table: "ProduksiDetailOverhead",
                columns: new[] { "ProduksiId", "OverheadId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturPembelian_PembelianId",
                table: "ReturPembelian",
                column: "PembelianId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturPembelianDetail_BahanSatuanId",
                table: "ReturPembelianDetail",
                column: "BahanSatuanId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturPembelianDetail_ReturPembelianId",
                table: "ReturPembelianDetail",
                column: "ReturPembelianId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturPenjualan_PenjualanId",
                table: "ReturPenjualan",
                column: "PenjualanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturPenjualanDetail_BarangSatuanId",
                table: "ReturPenjualanDetail",
                column: "BarangSatuanId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturPenjualanDetail_ReturPenjualanId",
                table: "ReturPenjualanDetail",
                column: "ReturPenjualanId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Email",
                table: "Supplier",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Fax",
                table: "Supplier",
                column: "Fax",
                unique: true,
                filter: "[Fax] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Telepon",
                table: "Supplier",
                column: "Telepon",
                unique: true,
                filter: "[Telepon] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransaksiPembelian_PembelianId",
                table: "TransaksiPembelian",
                column: "PembelianId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaksiPenjualan_PenjualanId",
                table: "TransaksiPenjualan",
                column: "PenjualanId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormulasiDetail");

            migrationBuilder.DropTable(
                name: "LogTransaksi");

            migrationBuilder.DropTable(
                name: "PembelianDetail");

            migrationBuilder.DropTable(
                name: "PenjualanDetail");

            migrationBuilder.DropTable(
                name: "PerubahanStokBahan");

            migrationBuilder.DropTable(
                name: "PerubahanStokBarang");

            migrationBuilder.DropTable(
                name: "ProduksiDetailBahan");

            migrationBuilder.DropTable(
                name: "ProduksiDetailJasa");

            migrationBuilder.DropTable(
                name: "ProduksiDetailOverhead");

            migrationBuilder.DropTable(
                name: "Profil");

            migrationBuilder.DropTable(
                name: "ReturPembelianDetail");

            migrationBuilder.DropTable(
                name: "ReturPenjualanDetail");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "TransaksiLain");

            migrationBuilder.DropTable(
                name: "TransaksiPembelian");

            migrationBuilder.DropTable(
                name: "TransaksiPenjualan");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "Formulasi");

            migrationBuilder.DropTable(
                name: "Karyawan");

            migrationBuilder.DropTable(
                name: "Overhead");

            migrationBuilder.DropTable(
                name: "Produksi");

            migrationBuilder.DropTable(
                name: "BahanSatuan");

            migrationBuilder.DropTable(
                name: "ReturPembelian");

            migrationBuilder.DropTable(
                name: "BarangSatuan");

            migrationBuilder.DropTable(
                name: "ReturPenjualan");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Pekerjaan");

            migrationBuilder.DropTable(
                name: "Bahan");

            migrationBuilder.DropTable(
                name: "Pembelian");

            migrationBuilder.DropTable(
                name: "Barang");

            migrationBuilder.DropTable(
                name: "Penjualan");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}