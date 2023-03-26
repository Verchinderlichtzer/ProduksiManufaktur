using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Bahan
{
    public class BahanFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public Bahan Bahan { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IBahanService BahanService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected BahanFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected Bahan? result = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Bahan";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            validator = new();
            if (!Baru)
            {
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {Bahan.Id} - {Bahan.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                Bahan = await BahanService.Find(Bahan.Id);
                if (Bahan is null)
                {
                    Snackbar.Add("Bahan telah dihapus", MudBlazor.Severity.Error);
                    MudDialog.Cancel();
                }
            }
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task DeleteBahanSatuan(BahanSatuan bahanSatuan)
        {
            if (!Baru && Bahan.BahanSatuan!.Count == 1)
            {
                Snackbar.Add("Bahan harus memiliki setidaknya 1 satuan pembelian", MudBlazor.Severity.Error);
            }
            else if (Baru || await BahanService.DeletableBahanSatuan(bahanSatuan.Id))
            {
                Bahan.BahanSatuan!.Remove(bahanSatuan);
            }
            else
            {
                Snackbar.Add("Satuan ini terpakai dalam transaksi", MudBlazor.Severity.Error);
            }
        }

        protected async Task Save()
        {
            if (!Bahan.BahanSatuan!.Any() || Bahan.BahanSatuan!.Any(x => string.IsNullOrEmpty(x.Nama) || x.KonversiStok == 0))
            {
                Snackbar.Add("Satuan pembelian tidak valid", MudBlazor.Severity.Error);
                return;
            }
            await form!.Validate();
            if (form!.IsValid)
            {
                if (Baru)
                    result = await BahanService.Create(Bahan);
                else
                    result = await BahanService.Update(Bahan);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class BahanFluentValidator : AbstractValidator<Bahan>
        {
            public BahanFluentValidator()
            {
                RuleFor(x => x.Nama)
                    .NotEmpty().WithMessage("Nama tidak boleh kosong");

                RuleFor(x => x.SatuanProduksi)
                    .NotEmpty().WithMessage("Satuan tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Bahan>.CreateWithOptions((Bahan)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}