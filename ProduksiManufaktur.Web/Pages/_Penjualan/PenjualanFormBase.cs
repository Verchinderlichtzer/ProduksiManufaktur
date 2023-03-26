using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Penjualan
{
    [Authorize(Policy = "PenjualanWrite")]
    public class PenjualanFormBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPenjualanService PenjualanService { get; set; } = null!;

        [Inject]
        protected ICustomerService CustomerService { get; set; } = null!;

        [Inject]
        protected IBarangService BarangService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected PenjualanFluentValidator validator = null!;
        protected MudForm? form = new();
        protected MudAutocomplete<BarangSatuan>? inputBarangSatuan = new();

        protected Penjualan penjualan = new() { PenjualanDetail = new() };
        protected TransaksiPenjualan transaksiPenjualan = new();
        protected List<BarangSatuan> listBarangSatuan = null!;
        protected List<Customer> listCustomer = null!;

        protected bool baru;
        protected bool loaded;
        protected bool popupTerbuka;
        protected Penjualan? result = new();
        protected string pesan = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Penjualan";
        protected Color warna = Color.Success;

        protected override async Task OnInitializedAsync()
        {
            baru = string.IsNullOrEmpty(Id);
            if (!baru && !(await Layout.AuthenticationState).User.IsInRole("Admin"))
            {
                To.NavigateTo("/penjualan");
                return;
            }
            validator = new();
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Penjualan", "/penjualan"),
                new BreadcrumbItem("Form", $"/penjualan/form/{Id}")
            };
            Layout.Refresh();

            listBarangSatuan = await BarangService.GetBarangSatuan();
            listCustomer = await CustomerService.Get();

            if (!baru)
            {
                penjualan = await PenjualanService.Find(Id);

                foreach (var item in penjualan.PenjualanDetail!)
                    listBarangSatuan.RemoveAll(x => x.Id == item.BarangSatuanId);

                Hitung();

                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {penjualan.Id}";
                warna = Color.Warning;
            }
            loaded = true;
        }

        protected void Hitung()
        {
            try
            {
                penjualan.Subtotal = (int)penjualan.PenjualanDetail!.Sum(x => x.Harga * x.Jumlah);
                penjualan.GrandTotal = (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m));

                transaksiPenjualan.Balance = Math.Abs(transaksiPenjualan.Nominal + penjualan.Terbayar - penjualan.GrandTotal);
                transaksiPenjualan.BalanceLabel = transaksiPenjualan.Nominal + penjualan.Terbayar < penjualan.GrandTotal ? "Sisa" : "Kembali";

                foreach (var item in penjualan.PenjualanDetail!)
                {
                    item.StokAkhir = item.BarangSatuan!.Barang!.Stok - penjualan.PenjualanDetail!.Where(x => x.BarangSatuan!.BarangId == item.BarangSatuan!.BarangId).Sum(x => (x.Jumlah - x.JumlahSebelum) * x.BarangSatuan!.KonversiStok);
                    item.Total = (int)(item.Jumlah * item.Harga);
                }

                if (baru && penjualan.Subtotal > 0)
                    penjualan.MetodeBayar = transaksiPenjualan.Nominal < penjualan.GrandTotal ? "Kredit" : "Tunai";
                else if (baru)
                    penjualan.MetodeBayar = string.Empty;
                if (penjualan.Subtotal > 0)
                    penjualan.Status = transaksiPenjualan.Nominal + penjualan.Terbayar < penjualan.GrandTotal ? "Belum Lunas" : "Lunas";
                else
                    penjualan.Status = string.Empty;
            }
            catch (OverflowException)
            {
                pesan = "Nominal terlalu banyak";
                result = null;
                return;
            }
            result = new();
        }

        protected void HitungJatuhTempo(DateTime? e)
        {
            if (e <= penjualan.InputTanggal) e = penjualan.InputTanggal?.AddDays(1);
            penjualan.JatuhTempo = e;
            penjualan.HariJatuhTempo = (e! - penjualan.InputTanggal?.Date)?.Days ?? 1;
        }

        protected async Task<IEnumerable<BarangSatuan>> CariBarangSatuan(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listBarangSatuan.Where(x => (x.Barang!.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Ukuran.Contains(value, StringComparison.OrdinalIgnoreCase)) && x.Barang!.Stok > x.KonversiStok).OrderBy(x => x.Barang!.Nama).ThenBy(x => x.Id));
        }

        protected void PilihBarangSatuan(BarangSatuan e)
        {
            if (e is null) return;
            penjualan.PenjualanDetail!.Add(new() { PenjualanId = penjualan.Id, BarangSatuanId = e.Id, BarangSatuan = e, Jumlah = 1, Harga = e.Harga });
            listBarangSatuan.Remove(e);
            Hitung();
            inputBarangSatuan!.Reset();
        }

        protected async Task<IEnumerable<Customer>> CariCustomer(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listCustomer.Where(x => x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected async Task DeleteDetail(PenjualanDetail penjualanDetail)
        {
            if (baru || await PenjualanService.DeletableDetail(penjualanDetail.PenjualanId, penjualanDetail.BarangSatuanId))
            {
                listBarangSatuan.Add(penjualanDetail.BarangSatuan!);
                penjualan.PenjualanDetail!.Remove(penjualanDetail);
                Hitung();
            }
            else
            {
                Snackbar.Add("Beberapa barang sudah diretur", MudBlazor.Severity.Error);
                return;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            List<BarangSatuan> barangSatuan = await PenjualanService.RefreshDetail(penjualan.Id, penjualan.PenjualanDetail!.ConvertAll(x => x.BarangSatuanId));
            if (barangSatuan is null)
            {
                Snackbar.Add("Penjualan telah dihapus", MudBlazor.Severity.Error);
                To.NavigateTo("/penjualan");
                return;
            }
            foreach (var item in barangSatuan) penjualan.PenjualanDetail!.First(x => x.BarangSatuanId == item.Id).BarangSatuan = item;
            Hitung();
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            if (!penjualan.PenjualanDetail!.Any() || penjualan.PenjualanDetail!.Any(x => x.Harga == 0 || x.Jumlah == 0) || penjualan.PenjualanDetail!.GroupBy(x => x.BarangSatuanId).Any(x => x.Count() > 1))
            {
                Snackbar.Add("Daftar barang tidak valid", MudBlazor.Severity.Error);
                return;
            }

            await form!.Validate();
            if (form!.IsValid)
            {
                if (baru)
                {
                    if (transaksiPenjualan.Nominal > 0) penjualan.TransaksiPenjualan = new() { transaksiPenjualan };
                    result = await PenjualanService.Create(penjualan);
                }
                else
                {
                    result = await PenjualanService.Update(penjualan);
                }

                if (result is not null)
                {
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Penjualan", EntitasId = result.Id, Keterangan = baru ? "Create" : "Update" });
                    Snackbar.Add("Penjualan Berhasil", MudBlazor.Severity.Success);
                    To.NavigateTo("/penjualan");
                }
                else
                {
                    pesan = "Terjadi kesalahan saat menyimpan data, harap periksa kembali dan refresh form.";
                }
            }
        }

        public class PenjualanFluentValidator : AbstractValidator<Penjualan>
        {
            public PenjualanFluentValidator()
            {
                RuleFor(x => x.Customer)
                    .NotEmpty().WithMessage("Customer tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Penjualan>.CreateWithOptions((Penjualan)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}