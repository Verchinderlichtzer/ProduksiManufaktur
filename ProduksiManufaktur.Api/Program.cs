global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using ProduksiManufaktur.Api.Repositories;
global using ProduksiManufaktur.Models;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using static ProduksiManufaktur.Api.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProduksiManufaktur.Api;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    x.JsonSerializerOptions.MaxDepth = 8;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(x =>
    {
        x.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
        .WithMethods("GET", "POST", "DELETE", "PUT")
        .AllowCredentials()
        .AllowAnyHeader();
    });
});

builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorizationCore(options =>
{
    // Akun = Role, User
    // Produk = Bahan, Barang, Formulasi
    // Pekerja = Pekerjaan, Karyawan
    // Pihak = Supplier, Customer
    // Report = Laporan
    options.AddPolicy(name: "AkunRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Akun")));
    options.AddPolicy(name: "AkunWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "Akun")));
    options.AddPolicy(name: "ProdukRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Produk")));
    options.AddPolicy(name: "ProdukWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "Produk")));
    options.AddPolicy(name: "PekerjaRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Pekerja")));
    options.AddPolicy(name: "PekerjaWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "Pekerja")));
    options.AddPolicy(name: "PihakRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Pihak")));
    options.AddPolicy(name: "PihakWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "Pihak")));
    options.AddPolicy(name: "OverheadRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Overhead")));
    options.AddPolicy(name: "OverheadWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "Overhead")));
    options.AddPolicy(name: "PembelianRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Pembelian")));
    options.AddPolicy(name: "PembelianWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "Pembelian")));
    options.AddPolicy(name: "PenjualanRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Penjualan")));
    options.AddPolicy(name: "PenjualanWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "Penjualan")));
    options.AddPolicy(name: "ProduksiRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Produksi")));
    options.AddPolicy(name: "ProduksiWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "Produksi")));
    options.AddPolicy(name: "TransaksiLainRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "TransaksiLain")));
    options.AddPolicy(name: "TransaksiLainWrite", policy => policy.RequireAssertion(context => ReadWriteAccess(context, "TransaksiLain")));
    options.AddPolicy(name: "ReportRead", policy => policy.RequireAssertion(context => ReadOnlyAccess(context, "Report")));
});

#region AddRepositories

// User = User, UserClaim
// Role = Role, RoleClaim
// Bahan = Bahan, PerubahanStokBahan
// Barang = Barang, PerubahanStokBarang
// Formulasi = Formulasi, FormulasiDetail
// Pembelian = Pembelian, PembelianDetail, TransaksiPembelian, ReturPembelian, ReturPembelianDetail
// Penjualan = Penjualan, PenjualanDetail, TransaksiPenjualan, ReturPenjualan, ReturPenjualanDetail
// Produksi = Produksi, ProduksiDetailBahan, ProduksiDetailJasa, ProduksiDetailOverhead, TransaksiProduksi
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IBahanRepository, BahanRepository>();
builder.Services.AddScoped<IBarangRepository, BarangRepository>();
builder.Services.AddScoped<IFormulasiRepository, FormulasiRepository>();
builder.Services.AddScoped<IPekerjaanRepository, PekerjaanRepository>();
builder.Services.AddScoped<IKaryawanRepository, KaryawanRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOverheadRepository, OverheadRepository>();
builder.Services.AddScoped<IPembelianRepository, PembelianRepository>();
builder.Services.AddScoped<IPenjualanRepository, PenjualanRepository>();
builder.Services.AddScoped<IProduksiRepository, ProduksiRepository>();
builder.Services.AddScoped<ITransaksiLainRepository, TransaksiLainRepository>();
builder.Services.AddScoped<IProfilRepository, ProfilRepository>();
builder.Services.AddScoped<IIndexRepository, IndexRepository>();
builder.Services.AddScoped<ILaporanRepository, LaporanRepository>();

#endregion AddRepositories

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStatusCodePages(context =>
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;
    if (response.StatusCode == 401)
    {
        response.Redirect("https://localhost:7036/Account/Login");
    }
    return Task.CompletedTask;
});

app.UseStaticFiles();
app.UseRequestLocalization("id-ID");

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();