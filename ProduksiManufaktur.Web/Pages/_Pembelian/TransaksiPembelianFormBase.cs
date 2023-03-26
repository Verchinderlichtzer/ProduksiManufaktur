using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Pembelian
{
    [Authorize(Policy = "ProdukWrite")]
    public class TransaksiPembelianFormBase : ComponentBase
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
        protected IPembelianService PembelianService { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();
        protected TransaksiPembelianFluentValidator validator = null!;
        protected MudForm? form = new();

        protected TransaksiPembelian transaksiPembelian = new();
        protected Pembelian pembelian = null!;

        protected bool loaded;
        protected bool baru = true;
        protected bool popupTerbuka;
        protected TransaksiPembelian? result = new();
        protected string pesanHapus = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Transaksi baru";
        protected Color warna = Color.Success;

        protected async Task LoadData()
        {
            pembelian = await PembelianService.Find1(Id);
            Hitung();
        }

        protected override async Task OnInitializedAsync()
        {
            validator = new();
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pembelian", "/pembelian"),
                new BreadcrumbItem("Transaksi", $"/pembelian/transaksi/{Id}")
            };
            Layout.Refresh();
            await LoadData();
            transaksiPembelian.PembelianId = Id;
            transaksiPembelian.Balance = pembelian.Sisa;
            transaksiPembelian.Pembelian = new Pembelian { Version = pembelian.Version };
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
            transaksiPembelian = new() { PembelianId = Id, Balance = pembelian.Sisa, Pembelian = new Pembelian { Version = pembelian.Version } };
            result = new();
            baru = true;
            icon = Icons.Material.Filled.Add;
            judul = "Transaksi baru";
            warna = Color.Success;
        }

        protected void NominalChanged(int e)
        {
            transaksiPembelian.Nominal = e;
            Hitung();
        }

        protected void Hitung()
        {
            transaksiPembelian.Balance = Math.Abs(pembelian.Sisa - (transaksiPembelian.Nominal - transaksiPembelian.NominalSebelum));
            transaksiPembelian.BalanceLabel = pembelian.Sisa - (transaksiPembelian.Nominal - transaksiPembelian.NominalSebelum) > 0 ? "Sisa" : "Kembali";
        }

        protected void RowClickEvent(DataGridRowClickEventArgs<TransaksiPembelian> dataGridRowClickEventArgs)
        {
            dataGridRowClickEventArgs.Item.CopyPropertiesTo(transaksiPembelian);
            transaksiPembelian.Pembelian = new Pembelian { Version = pembelian.Version };

            baru = false;
            icon = Icons.Material.Filled.Edit;
            judul = $"Edit transaksi ke - {pembelian.TransaksiPembelian!.FindIndex(x => x.Id == transaksiPembelian.Id) + 1}";
            warna = Color.Warning;
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                transaksiPembelian.Nominal = (transaksiPembelian.Nominal - transaksiPembelian.NominalSebelum) >= pembelian.Sisa ? (baru ? pembelian.Sisa : transaksiPembelian.NominalSebelum + pembelian.Sisa) : transaksiPembelian.Nominal;
                if (baru)
                    result = await PembelianService.CreateTransaksi(transaksiPembelian);
                else
                    result = await PembelianService.UpdateTransaksi(transaksiPembelian);
                if (result is not null)
                {
                    Snackbar.Add(baru ? "Transaksi berhasil ditambah" : "Transaksi berhasil diubah", MudBlazor.Severity.Success);
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Transaksi Pembelian", EntitasId = result.Id.ToString(), Keterangan = baru ? "Create" : "Update" });
                    await Reset();
                }
            }
        }

        protected async Task Hapus(TransaksiPembelian transaksiPembelian)
        {
            pesanHapus = $"Hapus transaksi ke - {pembelian.TransaksiPembelian!.FindIndex(x => x.Id == transaksiPembelian.Id) + 1}?";
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = await PembelianService.DeleteTransaksi(transaksiPembelian.Id);
                if (success)
                    Snackbar.Add("Transaksi berhasil dihapus", MudBlazor.Severity.Success);
                else
                    Snackbar.Add("Transaksi gagal dihapus", MudBlazor.Severity.Error);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Transaksi Pembelian", EntitasId = transaksiPembelian.Id.ToString(), Keterangan = "Delete" });
            }
        }

        public class TransaksiPembelianFluentValidator : AbstractValidator<TransaksiPembelian>
        {
            public TransaksiPembelianFluentValidator()
            {
                RuleFor(x => x.Nominal)
                    .NotEmpty().WithMessage("Nominal tidak boleh nol");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<TransaksiPembelian>.CreateWithOptions((TransaksiPembelian)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}