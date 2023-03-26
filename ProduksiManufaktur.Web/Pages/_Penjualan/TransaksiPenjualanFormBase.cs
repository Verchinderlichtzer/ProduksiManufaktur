using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Penjualan
{
    [Authorize(Policy = "ProdukWrite")]
    public class TransaksiPenjualanFormBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected IPenjualanService PenjualanService { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();
        protected TransaksiPenjualanFluentValidator validator = null!;
        protected MudForm? form = new();

        protected TransaksiPenjualan transaksiPenjualan = new();
        protected Penjualan penjualan = null!;

        protected bool loaded;
        protected bool baru = true;
        protected bool popupTerbuka;
        protected TransaksiPenjualan? result = new();
        protected string pesanHapus = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Transaksi baru";
        protected Color warna = Color.Success;

        protected async Task LoadData()
        {
            penjualan = await PenjualanService.Find1(Id);
            Hitung();
        }

        protected override async Task OnInitializedAsync()
        {
            validator = new();
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Penjualan", "/penjualan"),
                new BreadcrumbItem("Transaksi", $"/penjualan/transaksi/{Id}")
            };
            Layout.Refresh();
            await LoadData();
            transaksiPenjualan.PenjualanId = Id;
            transaksiPenjualan.Balance = penjualan.Sisa;
            transaksiPenjualan.Penjualan = new Penjualan { Version = penjualan.Version };
            loaded = true;
        }

        protected async Task Refresh()
        {
            result = new();
            await LoadData();
            await form!.Validate();
            StateHasChanged();
        }

        protected async Task Reset()
        {
            await LoadData();
            transaksiPenjualan = new() { PenjualanId = Id, Balance = penjualan.Sisa, Penjualan = new Penjualan { Version = penjualan.Version } };
            result = new();
            baru = true;
            icon = Icons.Material.Filled.Add;
            judul = "Transaksi baru";
            warna = Color.Success;
        }

        protected void NominalChanged(int e)
        {
            transaksiPenjualan.Nominal = e;
            Hitung();
        }

        protected void Hitung()
        {
            transaksiPenjualan.Balance = Math.Abs(penjualan.Sisa - (transaksiPenjualan.Nominal - transaksiPenjualan.NominalSebelum));
            transaksiPenjualan.BalanceLabel = penjualan.Sisa - (transaksiPenjualan.Nominal - transaksiPenjualan.NominalSebelum) > 0 ? "Sisa" : "Kembali";
        }

        protected void RowClickEvent(DataGridRowClickEventArgs<TransaksiPenjualan> dataGridRowClickEventArgs)
        {
            dataGridRowClickEventArgs.Item.CopyPropertiesTo(transaksiPenjualan);
            transaksiPenjualan.Penjualan = new Penjualan { Version = penjualan.Version };

            baru = false;
            icon = Icons.Material.Filled.Edit;
            judul = $"Edit transaksi ke - {penjualan.TransaksiPenjualan!.FindIndex(x => x.Id == transaksiPenjualan.Id) + 1}";
            warna = Color.Warning;
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                transaksiPenjualan.Nominal = (transaksiPenjualan.Nominal - transaksiPenjualan.NominalSebelum) >= penjualan.Sisa ? (baru ? penjualan.Sisa : transaksiPenjualan.NominalSebelum + penjualan.Sisa) : transaksiPenjualan.Nominal;
                if (baru)
                    result = await PenjualanService.CreateTransaksi(transaksiPenjualan);
                else
                    result = await PenjualanService.UpdateTransaksi(transaksiPenjualan);
                if (result is not null)
                {
                    Snackbar.Add(baru ? "Transaksi berhasil ditambah" : "Transaksi berhasil diubah", MudBlazor.Severity.Success);
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Transaksi Penjualan", EntitasId = result.Id.ToString(), Keterangan = baru ? "Create" : "Update" });
                    await Reset();
                }
            }
        }

        protected async Task Hapus(TransaksiPenjualan transaksiPenjualan)
        {
            pesanHapus = $"Hapus transaksi ke - {penjualan.TransaksiPenjualan!.FindIndex(x => x.Id == transaksiPenjualan.Id) + 1}?";
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = await PenjualanService.DeleteTransaksi(transaksiPenjualan.Id);
                if (success)
                    Snackbar.Add("Transaksi berhasil dihapus", MudBlazor.Severity.Success);
                else
                    Snackbar.Add("Transaksi gagal dihapus", MudBlazor.Severity.Error);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Transaksi Penjualan", EntitasId = transaksiPenjualan.Id.ToString(), Keterangan = "Delete" });
            }
        }

        public class TransaksiPenjualanFluentValidator : AbstractValidator<TransaksiPenjualan>
        {
            public TransaksiPenjualanFluentValidator()
            {
                RuleFor(x => x.Nominal)
                    .NotEmpty().WithMessage("Nominal tidak boleh nol");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<TransaksiPenjualan>.CreateWithOptions((TransaksiPenjualan)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}