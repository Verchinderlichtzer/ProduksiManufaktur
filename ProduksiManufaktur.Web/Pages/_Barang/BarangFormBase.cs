using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Barang
{
    public class BarangFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public Barang Barang { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IBarangService BarangService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected BarangFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected Barang? result = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Barang";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            validator = new();
            if (!Baru)
            {
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {Barang.Id} - {Barang.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                Barang = await BarangService.Find(Barang.Id);
                if (Barang is null)
                {
                    Snackbar.Add("Barang telah dihapus", MudBlazor.Severity.Error);
                    MudDialog.Cancel();
                }
            }
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task DeleteBarangSatuan(BarangSatuan barangSatuan)
        {
            if (!Baru && Barang.BarangSatuan!.Count == 1)
            {
                Snackbar.Add("Barang harus memiliki setidaknya 1 satuan penjualan", MudBlazor.Severity.Error);
            }
            else if (Baru || await BarangService.DeletableBarangSatuan(barangSatuan.Id))
            {
                Barang.BarangSatuan!.Remove(barangSatuan);
            }
            else
            {
                Snackbar.Add("Satuan ini terpakai dalam transaksi", MudBlazor.Severity.Error);
            }
        }

        protected async Task Save()
        {
            if (!Barang.BarangSatuan!.Any() || Barang.BarangSatuan!.Any(x => string.IsNullOrEmpty(x.Nama) || x.KonversiStok == 0))
            {
                Snackbar.Add("Satuan penjualan tidak valid", MudBlazor.Severity.Error);
                return;
            }
            await form!.Validate();
            if (form!.IsValid)
            {
                if (Baru)
                    result = await BarangService.Create(Barang);
                else
                    result = await BarangService.Update(Barang);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class BarangFluentValidator : AbstractValidator<Barang>
        {
            public BarangFluentValidator()
            {
                RuleFor(x => x.Nama)
                    .NotEmpty().WithMessage("Nama tidak boleh kosong");

                RuleFor(x => x.SatuanProduksi)
                    .NotEmpty().WithMessage("Satuan tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Barang>.CreateWithOptions((Barang)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}