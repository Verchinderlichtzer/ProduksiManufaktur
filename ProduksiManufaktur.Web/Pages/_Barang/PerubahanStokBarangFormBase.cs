using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Barang
{
    public class PerubahanStokBarangFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public PerubahanStokBarang PerubahanStokBarang { get; set; } = new();

        [Parameter]
        public List<Barang> ListBarang { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IBarangService BarangService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected PerubahanStokBarangFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected PerubahanStokBarang? result = new();
        protected decimal jumlahMax;
        protected string adornmentJumlah = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Perubahan Stok Barang";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            validator = new();
            if (!Baru)
            {
                UpdateState();
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {PerubahanStokBarang.Id} - {PerubahanStokBarang.Barang!.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task<IEnumerable<Barang>> CariBarang(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(ListBarang.Where(x => x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.SatuanProduksi.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected void PilihBarang(Barang e)
        {
            if (e is null) return;
            PerubahanStokBarang.Barang = e;
            PerubahanStokBarang.BarangId = e.Id;
            UpdateState();
        }

        protected void PilihJenis(string e)
        {
            if (string.IsNullOrEmpty(e)) return;
            PerubahanStokBarang.Jenis = e;
            UpdateState();
        }

        protected void UpdateState()
        {
            if (string.IsNullOrEmpty(PerubahanStokBarang.BarangId) || string.IsNullOrEmpty(PerubahanStokBarang.Jenis)) return;
            adornmentJumlah = Baru ? (PerubahanStokBarang.Jenis == "Pengurangan" ? $"/ {PerubahanStokBarang.Barang!.Stok:g0} {PerubahanStokBarang.Barang!.SatuanProduksi}" : string.Empty) : (PerubahanStokBarang.Jenis == "Pengurangan" ? (PerubahanStokBarang.JenisSebelum == "Pengurangan" ? $"/ {PerubahanStokBarang.Barang!.Stok + PerubahanStokBarang.JumlahSebelum:g0} {PerubahanStokBarang.Barang!.SatuanProduksi}" : $"/ {PerubahanStokBarang.Barang!.Stok - PerubahanStokBarang.JumlahSebelum:g0} {PerubahanStokBarang.Barang!.SatuanProduksi}") : string.Empty);

            if (Baru)
            {
                if (PerubahanStokBarang.Jenis == "Pengurangan")
                {
                    adornmentJumlah = $"/ {PerubahanStokBarang.Barang!.Stok:g0} {PerubahanStokBarang.Barang!.SatuanProduksi}";
                    jumlahMax = PerubahanStokBarang.Barang!.Stok;
                }
                else if (PerubahanStokBarang.Jenis == "Penambahan")
                {
                    adornmentJumlah = PerubahanStokBarang.Barang!.SatuanProduksi;
                }
            }
            else
            {
                if (PerubahanStokBarang.Jenis == "Pengurangan")
                {
                    if (PerubahanStokBarang.JenisSebelum == "Pengurangan")
                    {
                        adornmentJumlah = $"/ {PerubahanStokBarang.Barang!.Stok + PerubahanStokBarang.JumlahSebelum:g0} {PerubahanStokBarang.Barang!.SatuanProduksi}";
                        jumlahMax = PerubahanStokBarang.Barang!.Stok + PerubahanStokBarang.JumlahSebelum;
                    }
                    else if (PerubahanStokBarang.JenisSebelum == "Penambahan")
                    {
                        adornmentJumlah = $"/ {PerubahanStokBarang.Barang!.Stok - PerubahanStokBarang.JumlahSebelum:g0} {PerubahanStokBarang.Barang!.SatuanProduksi}";
                        jumlahMax = PerubahanStokBarang.Barang!.Stok - PerubahanStokBarang.JumlahSebelum;
                    }
                }
                else if (PerubahanStokBarang.Jenis == "Penambahan")
                {
                    adornmentJumlah = PerubahanStokBarang.Barang!.SatuanProduksi;
                }
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                PerubahanStokBarang = await BarangService.FindPerubahanStok(PerubahanStokBarang.Id);
                if (PerubahanStokBarang is null)
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
                    result = await BarangService.CreatePerubahanStok(PerubahanStokBarang);
                else
                    result = await BarangService.UpdatePerubahanStok(PerubahanStokBarang);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class PerubahanStokBarangFluentValidator : AbstractValidator<PerubahanStokBarang>
        {
            public PerubahanStokBarangFluentValidator()
            {
                RuleFor(x => x.Jenis)
                    .NotEmpty().WithMessage("Jenis tidak boleh kosong");

                RuleFor(x => x.Barang)
                    .NotEmpty().WithMessage("Barang tidak boleh kosong");

                RuleFor(x => x.Jumlah)
                    .NotEmpty().WithMessage("Jumlah tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<PerubahanStokBarang>.CreateWithOptions((PerubahanStokBarang)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}