using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Formulasi
{
    [Authorize(Policy = "ProdukWrite")]
    public class FormulasiFormBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IFormulasiService FormulasiService { get; set; } = null!;

        [Inject]
        protected IBahanService BahanService { get; set; } = null!;

        [Inject]
        protected IBarangService BarangService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected FormulasiFluentValidator validator = null!;
        protected MudForm? form = new();

        protected Formulasi formulasi = new() { FormulasiDetail = new() };
        protected List<Bahan> listBahan = null!;
        protected List<Barang> listBarang = null!;

        protected MudAutocomplete<Bahan>? inputBahan = new();

        protected bool baru;
        protected bool loaded;
        protected bool popupTerbuka;
        protected Formulasi? result = new();
        protected string pesan = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Formulasi";
        protected Color warna = Color.Success;

        protected override async Task OnInitializedAsync()
        {
            baru = string.IsNullOrEmpty(Id);
            validator = new();
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Formulasi", "/formulasi"),
                new BreadcrumbItem("Form", $"/formulasi/form/{Id}")
            };
            Layout.Refresh();

            listBahan = await BahanService.Get();
            listBarang = await BarangService.Get();

            if (!baru)
            {
                formulasi = await FormulasiService.Find(Id);

                foreach (var y in formulasi.FormulasiDetail!)
                    listBahan.RemoveAll(x => x.Id == y.BahanId);

                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {formulasi.Id}";
                warna = Color.Warning;
            }
            loaded = true;
        }

        protected async Task<IEnumerable<Barang>> CariBarang(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listBarang.Where(x => x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.SatuanProduksi.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected void PilihBarang(Barang e)
        {
            if (e is null) return;
            formulasi.Barang = e;
            formulasi.BarangId = e.Id;
        }

        protected async Task<IEnumerable<Bahan>> CariBahan(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listBahan.Where(x => x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.SatuanProduksi.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected void PilihBahan(Bahan e)
        {
            if (e is null) return;
            formulasi.FormulasiDetail!.Add(new() { FormulasiId = formulasi.Id, BahanId = e.Id, Bahan = e, Jumlah = 1 });
            listBahan.Remove(e);
            inputBahan!.Reset();
        }

        protected void Delete(FormulasiDetail formulasiDetail)
        {
            formulasi.FormulasiDetail!.Remove(formulasiDetail);
            listBahan.Add(formulasiDetail.Bahan!);
        }

        protected async Task Refresh()
        {
            result = new();
            if (!baru)
            {
                formulasi = await FormulasiService.Find(formulasi.Id);
                if (formulasi is null)
                {
                    Snackbar.Add("Formulasi telah dihapus", MudBlazor.Severity.Error);
                    To.NavigateTo("/formulasi");
                    return;
                }
            }
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                if (baru)
                    result = await FormulasiService.Create(formulasi);
                else
                    result = await FormulasiService.Update(formulasi);

                if (result is not null)
                {
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Formulasi", EntitasId = result.Id, Keterangan = baru ? "Create" : "Update" });
                    Snackbar.Add(baru ? "Formulasi berhasil ditambah" : "Formulasi berhasil diubah", MudBlazor.Severity.Success);
                    To.NavigateTo("/formulasi");
                }
                else
                {
                    pesan = "Terjadi kesalahan saat menyimpan data, harap periksa kembali dan refresh form.";
                }
            }
        }

        public class FormulasiFluentValidator : AbstractValidator<Formulasi>
        {
            public FormulasiFluentValidator()
            {
                RuleFor(x => x.Barang)
                    .NotEmpty().WithMessage("Barang tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Formulasi>.CreateWithOptions((Formulasi)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}