using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Penjualan
{
    public class ReturPenjualanFormBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPenjualanService PenjualanService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected ReturPenjualanFluentValidator validator = null!;
        protected MudForm? form = new();

        protected ReturPenjualan returPenjualan = new();
        protected List<Penjualan> listPenjualan = new();

        protected bool loaded;
        protected bool baru = true;
        protected bool popupTerbuka;
        protected ReturPenjualan? result = new();
        protected string pesan = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Retur Penjualan";
        protected Color warna = Color.Success;

        protected override async Task OnInitializedAsync()
        {
            baru = string.IsNullOrEmpty(Id);
            validator = new();
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Penjualan", "/penjualan"),
                new BreadcrumbItem("Retur", "/penjualan/retur"),
                new BreadcrumbItem("Form", $"/penjualan/retur/form/{Id}")
            };
            Layout.Refresh();

            listPenjualan = await PenjualanService.Get1();
            if (!baru)
            {
                returPenjualan = await PenjualanService.FindRetur(Id);
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {returPenjualan!.Id}";
                warna = Color.Warning;
            }

            loaded = true;
        }

        protected async Task<IEnumerable<Penjualan>> CariPenjualan(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listPenjualan.Where(x => x.Id.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Tanggal.ToString("dd/MM/yyyy").Contains(value, StringComparison.OrdinalIgnoreCase)).OrderByDescending(x => x.Id));
        }

        protected async Task PilihPenjualan(Penjualan e)
        {
            if (e is null) return;
            List<PenjualanDetail> penjualanDetail = await PenjualanService.FindDetail(e.Id);
            returPenjualan = new()
            {
                Id = $"R-{e.Id}",
                PenjualanId = e.Id,
                Penjualan = e,
                ReturPenjualanDetail = penjualanDetail.ConvertAll(x => new ReturPenjualanDetail
                {
                    BarangSatuanId = x.BarangSatuanId,
                    Harga = x.Harga,
                    MaxJumlah = x.Jumlah,
                    BarangSatuan = x.BarangSatuan
                })
            };
        }

        protected void Hitung()
        {
            try
            {
                returPenjualan!.GrandTotal = (int)returPenjualan.ReturPenjualanDetail!.Sum(x => x.Jumlah * x.Harga);
            }
            catch (OverflowException)
            {
                pesan = "Nominal terlalu banyak";
                result = null;
                return;
            }
            result = new();
        }

        protected async Task Refresh()
        {
            result = new();
            if (!baru)
            {
                List<BarangSatuan> barangSatuan = await PenjualanService.RefreshReturDetail(returPenjualan.Id);
                if (barangSatuan is null)
                {
                    Snackbar.Add("Retur telah dihapus", MudBlazor.Severity.Error);
                    To.NavigateTo("/penjualan/retur");
                    return;
                }
                foreach (var item in barangSatuan)
                {
                    ReturPenjualanDetail i = returPenjualan.ReturPenjualanDetail!.First(x => x.BarangSatuanId == item.Id);
                    i.BarangSatuan = item;
                }
            }
            Hitung();
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                if (baru)
                    result = await PenjualanService.CreateRetur(returPenjualan);
                else
                    result = await PenjualanService.UpdateRetur(returPenjualan);

                if (result is not null)
                {
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Retur Penjualan", EntitasId = result.Id, Keterangan = baru ? "Create" : "Update" });
                    Snackbar.Add(baru ? "Retur berhasil ditambah" : "Retur berhasil diubah", MudBlazor.Severity.Success);
                    To.NavigateTo("/penjualan/retur");
                }
                else
                {
                    pesan = "Terjadi kesalahan saat menyimpan data, harap periksa kembali dan refresh form.";
                }
            }
        }

        public class ReturPenjualanFluentValidator : AbstractValidator<ReturPenjualan>
        {
            public ReturPenjualanFluentValidator()
            {
                RuleFor(x => x.Penjualan)
                    .NotEmpty().WithMessage("Penjualan tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<ReturPenjualan>.CreateWithOptions((ReturPenjualan)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}