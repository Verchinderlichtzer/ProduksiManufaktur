global using Microsoft.AspNetCore.Components;
global using Microsoft.JSInterop;
global using MudBlazor;
global using ProduksiManufaktur.Models;
global using ProduksiManufaktur.Web.Services;
global using ProduksiManufaktur.Web.Shared;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using static ProduksiManufaktur.Web.Shared.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(options => options.Conventions.AuthorizePage("/_Host"));
builder.Services.AddServerSideBlazor();
builder.Services.AddLocalization();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.PreventDuplicates = false;
});

builder.Services.AddDbContextPool<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Conn"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(999);
    options.Cookie.MaxAge = options.ExpireTimeSpan;
    options.SlidingExpiration = false;
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
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.EmailConfirmationTokenProvider = "Default";
    options.Tokens.PasswordResetTokenProvider = "Default";
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(999);
}).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders()
.AddTokenProvider("Default", typeof(DataProtectorTokenProvider<User>)).Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(1));

#region AddServices

// User = User, UserClaim
// Role = Role, RoleClaim
// Bahan = Bahan, PerubahanStokBahan
// Barang = Barang, PerubahanStokBarang
// Formulasi = Formulasi, FormulasiDetail
// Pembelian = Pembelian, PembelianDetail, TransaksiPembelian, ReturPembelian, ReturPembelianDetail
// Penjualan = Penjualan, PenjualanDetail, TransaksiPenjualan, ReturPenjualan, ReturPenjualanDetail
// Produksi = Produksi, ProduksiDetailBahan, ProduksiDetailJasa, ProduksiDetailOverhead, TransaksiProduksi
builder.Services.AddHttpClient<IAccountService, AccountService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IUserService, UserService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IRoleService, RoleService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IBahanService, BahanService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IBarangService, BarangService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IFormulasiService, FormulasiService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IPekerjaanService, PekerjaanService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IKaryawanService, KaryawanService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<ISupplierService, SupplierService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<ICustomerService, CustomerService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IOverheadService, OverheadService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IPembelianService, PembelianService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IPenjualanService, PenjualanService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IProduksiService, ProduksiService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<ITransaksiLainService, TransaksiLainService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IProfilService, ProfilService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<IIndexService, IndexService>(client => client.BaseAddress = new("https://localhost:7017/"));
builder.Services.AddHttpClient<ILaporanService, LaporanService>(client => client.BaseAddress = new("https://localhost:7017/"));

#endregion AddServices

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();
app.UseRequestLocalization("id-ID");

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();