using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Bahan
{
    public class PerubahanStokBahanFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public PerubahanStokBahan PerubahanStokBahan { get; set; } = new();

        [Parameter]
        public List<Bahan> ListBahan { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IBahanService BahanService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected PerubahanStokBahanFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected PerubahanStokBahan? result = new();
        protected decimal jumlahMax;
        protected string adornmentJumlah = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Perubahan Stok Bahan";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            validator = new();
            if (!Baru)
            {
                UpdateState();
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {PerubahanStokBahan.Id} - {PerubahanStokBahan.Bahan!.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task<IEnumerable<Bahan>> CariBahan(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(ListBahan.Where(x => x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.SatuanProduksi.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected void PilihBahan(Bahan e)
        {
            if (e is null) return;
            PerubahanStokBahan.Bahan = e;
            PerubahanStokBahan.BahanId = e.Id;
            UpdateState();
        }

        protected void PilihJenis(string e)
        {
            if (string.IsNullOrEmpty(e)) return;
            PerubahanStokBahan.Jenis = e;
            UpdateState();
        }

        protected void UpdateState()
        {
            if (string.IsNullOrEmpty(PerubahanStokBahan.BahanId) || string.IsNullOrEmpty(PerubahanStokBahan.Jenis)) return;
            adornmentJumlah = Baru ? (PerubahanStokBahan.Jenis == "Pengurangan" ? $"/ {PerubahanStokBahan.Bahan!.Stok:g0} {PerubahanStokBahan.Bahan!.SatuanProduksi}" : string.Empty) : (PerubahanStokBahan.Jenis == "Pengurangan" ? (PerubahanStokBahan.JenisSebelum == "Pengurangan" ? $"/ {PerubahanStokBahan.Bahan!.Stok + PerubahanStokBahan.JumlahSebelum:g0} {PerubahanStokBahan.Bahan!.SatuanProduksi}" : $"/ {PerubahanStokBahan.Bahan!.Stok - PerubahanStokBahan.JumlahSebelum:g0} {PerubahanStokBahan.Bahan!.SatuanProduksi}") : string.Empty);

            if (Baru)
            {
                if (PerubahanStokBahan.Jenis == "Pengurangan")
                {
                    adornmentJumlah = $"/ {PerubahanStokBahan.Bahan!.Stok:g0} {PerubahanStokBahan.Bahan!.SatuanProduksi}";
                    jumlahMax = PerubahanStokBahan.Bahan!.Stok;
                }
                else if (PerubahanStokBahan.Jenis == "Penambahan")
                {
                    adornmentJumlah = PerubahanStokBahan.Bahan!.SatuanProduksi;
                }
            }
            else
            {
                if (PerubahanStokBahan.Jenis == "Pengurangan")
                {
                    if (PerubahanStokBahan.JenisSebelum == "Pengurangan")
                    {
                        adornmentJumlah = $"/ {PerubahanStokBahan.Bahan!.Stok + PerubahanStokBahan.JumlahSebelum:g0} {PerubahanStokBahan.Bahan!.SatuanProduksi}";
                        jumlahMax = PerubahanStokBahan.Bahan!.Stok + PerubahanStokBahan.JumlahSebelum;
                    }
                    else if (PerubahanStokBahan.JenisSebelum == "Penambahan")
                    {
                        adornmentJumlah = $"/ {PerubahanStokBahan.Bahan!.Stok - PerubahanStokBahan.JumlahSebelum:g0} {PerubahanStokBahan.Bahan!.SatuanProduksi}";
                        jumlahMax = PerubahanStokBahan.Bahan!.Stok - PerubahanStokBahan.JumlahSebelum;
                    }
                }
                else if (PerubahanStokBahan.Jenis == "Penambahan")
                {
                    adornmentJumlah = PerubahanStokBahan.Bahan!.SatuanProduksi;
                }
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                PerubahanStokBahan = await BahanService.FindPerubahanStok(PerubahanStokBahan.Id);
                if (PerubahanStokBahan is null)
                {
                    Snackbar.Add("Item telah dihapus", MudBlazor.Severity.Error);
                    MudDialog.Cancel();
                }
            }
            UpdateState();
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                if (Baru)
                    result = await BahanService.CreatePerubahanStok(PerubahanStokBahan);
                else
                    result = await BahanService.UpdatePerubahanStok(PerubahanStokBahan);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class PerubahanStokBahanFluentValidator : AbstractValidator<PerubahanStokBahan>
        {
            public PerubahanStokBahanFluentValidator()
            {
                RuleFor(x => x.Jenis)
                    .NotEmpty().WithMessage("Jenis tidak boleh kosong");

                RuleFor(x => x.Bahan)
                    .NotEmpty().WithMessage("Bahan tidak boleh kosong");

                RuleFor(x => x.Jumlah)
                    .NotEmpty().WithMessage("Jumlah tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<PerubahanStokBahan>.CreateWithOptions((PerubahanStokBahan)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}