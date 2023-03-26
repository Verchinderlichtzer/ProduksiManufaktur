using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Pembelian
{
    public class ReturPembelianFormBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPembelianService PembelianService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected ReturPembelianFluentValidator validator = null!;
        protected MudForm? form = new();

        protected ReturPembelian returPembelian = new();
        protected List<Pembelian> listPembelian = new();

        protected bool loaded;
        protected bool baru = true;
        protected bool popupTerbuka;
        protected ReturPembelian? result = new();
        protected string pesan = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Retur Pembelian";
        protected Color warna = Color.Success;

        protected override async Task OnInitializedAsync()
        {
            baru = string.IsNullOrEmpty(Id);
            validator = new();
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pembelian", "/pembelian"),
                new BreadcrumbItem("Retur", "/pembelian/retur"),
                new BreadcrumbItem("Form", $"/pembelian/retur/form/{Id}")
            };
            Layout.Refresh();

            listPembelian = await PembelianService.Get1();
            if (!baru)
            {
                returPembelian = await PembelianService.FindRetur(Id);
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {returPembelian!.Id}";
                warna = Color.Warning;
            }

            loaded = true;
        }

        protected async Task<IEnumerable<Pembelian>> CariPembelian(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listPembelian.Where(x => x.Id.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Tanggal.ToString("dd/MM/yyyy").Contains(value, StringComparison.OrdinalIgnoreCase)).OrderByDescending(x => x.Id));
        }

        protected async Task PilihPembelian(Pembelian e)
        {
            if (e is null) return;
            List<PembelianDetail> pembelianDetail = await PembelianService.FindDetail(e.Id);
            returPembelian = new()
            {
                Id = $"R-{e.Id}",
                PembelianId = e.Id,
                Pembelian = e,
                ReturPembelianDetail = pembelianDetail.ConvertAll(x => new ReturPembelianDetail
                {
                    BahanSatuanId = x.BahanSatuanId,
                    Harga = x.Harga,
                    MaxJumlah = x.Jumlah,
                    BahanSatuan = x.BahanSatuan
                })
            };
        }

        protected void Hitung()
        {
            try
            {
                returPembelian!.GrandTotal = (int)returPembelian.ReturPembelianDetail!.Sum(x => x.Jumlah * x.Harga);
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
                List<BahanSatuan> bahanSatuan = await PembelianService.RefreshReturDetail(returPembelian.Id);
                if (bahanSatuan is null)
                {
                    Snackbar.Add("Retur telah dihapus", MudBlazor.Severity.Error);
                    To.NavigateTo("/pembelian/retur");
                    return;
                }
                foreach (var item in bahanSatuan)
                {
                    ReturPembelianDetail i = returPembelian.ReturPembelianDetail!.First(x => x.BahanSatuanId == item.Id);
                    i.BahanSatuan = item;
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
                    result = await PembelianService.CreateRetur(returPembelian);
                else
                    result = await PembelianService.UpdateRetur(returPembelian);

                if (result is not null)
                {
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Retur Pembelian", EntitasId = result.Id, Keterangan = baru ? "Create" : "Update" });
                    Snackbar.Add(baru ? "Retur berhasil ditambah" : "Retur berhasil diubah", MudBlazor.Severity.Success);
                    To.NavigateTo("/pembelian/retur");
                }
                else
                {
                    pesan = "Terjadi kesalahan saat menyimpan data, harap periksa kembali dan refresh form.";
                }
            }
        }

        public class ReturPembelianFluentValidator : AbstractValidator<ReturPembelian>
        {
            public ReturPembelianFluentValidator()
            {
                RuleFor(x => x.Pembelian)
                    .NotEmpty().WithMessage("Pembelian tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<ReturPembelian>.CreateWithOptions((ReturPembelian)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}