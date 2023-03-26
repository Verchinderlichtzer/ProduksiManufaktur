using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Produksi
{
    [Authorize(Policy = "ProduksiWrite")]
    public class ProduksiFormBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IProduksiService ProduksiService { get; set; } = null!;

        [Inject]
        protected IBarangService BarangService { get; set; } = null!;

        [Inject]
        protected IBahanService BahanService { get; set; } = null!;

        [Inject]
        protected IFormulasiService FormulasiService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected IKaryawanService KaryawanService { get; set; } = null!;

        [Inject]
        protected IOverheadService OverheadService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected ProduksiFluentValidator validator = null!;
        protected MudForm? form = new();
        protected MudAutocomplete<Formulasi>? inputFormulasi = new();
        protected MudAutocomplete<Bahan>? inputBahan = new();
        protected MudAutocomplete<Karyawan>? inputKaryawan = new();
        protected MudAutocomplete<Overhead>? inputOverhead = new();

        protected Produksi produksi = new() { ProduksiDetailBahan = new(), ProduksiDetailJasa = new(), ProduksiDetailOverhead = new() };
        protected List<Formulasi> listFormulasi = null!;
        protected List<Barang> listBarang = null!;
        protected List<Bahan> listBahan = null!;
        protected List<Karyawan> listKaryawan = null!;
        protected List<Overhead> listOverhead = null!;

        protected bool baru;
        protected bool loaded;
        protected bool popupTerbuka;
        protected bool terkunci;
        protected Produksi? result = new();
        protected string pesan = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Produksi";
        protected Color warna = Color.Success;

        protected override async Task OnInitializedAsync()
        {
            baru = string.IsNullOrEmpty(Id);
            if (!baru && !(await Layout.AuthenticationState).User.IsInRole("Admin"))
            {
                To.NavigateTo("/produksi");
                return;
            }
            validator = new();
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Produksi", "/produksi"),
                new BreadcrumbItem("Form", $"/produksi/form/{Id}")
            };
            Layout.Refresh();

            listBarang = await BarangService.Get();
            listBahan = await BahanService.Get1();
            listKaryawan = await KaryawanService.Get2();
            listOverhead = await OverheadService.Get();

            if (!baru)
            {
                produksi = await ProduksiService.Find(Id);
                Hitung();

                foreach (var y in produksi.ProduksiDetailBahan!)
                    listBahan.RemoveAll(x => x.Id == y.Bahan!.Id);
                foreach (var y in produksi.ProduksiDetailJasa!)
                    listKaryawan.RemoveAll(x => x.Id == y.Karyawan!.Id);
                foreach (var y in produksi.ProduksiDetailOverhead!)
                    listOverhead.RemoveAll(x => x.Id == y.Overhead!.Id);

                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {produksi.Id}";
                warna = Color.Warning;
            }
            loaded = true;
        }

        protected void Hitung()
        {
            try
            {
                produksi.BiayaJasa = produksi.ProduksiDetailJasa!.Sum(x => x.Biaya);
                produksi.BiayaOverhead = produksi.ProduksiDetailOverhead!.Sum(x => x.Biaya);
            }
            catch (OverflowException)
            {
                pesan = "Nominal terlalu banyak";
                result = null;
                return;
            }
            result = new();
        }

        protected void Kunci()
        {
            if (terkunci)
            {
                produksi.JumlahTerkunci = produksi.Jumlah;
                foreach (var item in produksi.ProduksiDetailBahan!)
                {
                    item.JumlahTerkunci = item.Jumlah;
                }
            }
            else
            {
                produksi.JumlahTerkunci = 0;
            }
        }

        protected void JumlahBarangChanged()
        {
            if (terkunci)
            {
                foreach (var item in produksi.ProduksiDetailBahan!)
                {
                    item.Jumlah = Math.Round(produksi.Jumlah / produksi.JumlahTerkunci * item.JumlahTerkunci, 2);
                    item.StokAkhir = item.Bahan!.Stok - (item.Jumlah - item.JumlahSebelum);
                }
            }
        }

        protected async Task<IEnumerable<Barang>> CariBarang(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listBarang.Where(x => x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.SatuanProduksi.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected async Task PilihBarang(Barang e)
        {
            if (e is null) return;
            inputFormulasi!.Reset();
            listFormulasi = await FormulasiService.Find1(e.Id);
        }

        protected async Task<IEnumerable<Formulasi>> CariFormulasi(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listFormulasi.Where(x => x.Id.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Id));
        }

        protected async Task PilihFormulasi(Formulasi e)
        {
            Formulasi result = await FormulasiService.Find2(e.Id);
            produksi.ProduksiDetailBahan = result.FormulasiDetail!.ConvertAll(x => new ProduksiDetailBahan
            {
                ProduksiId = produksi.Id,
                BahanId = x.Bahan!.Id,
                Jumlah = x.Jumlah,
                StokAkhir = x.Bahan!.Stok - x.Jumlah,
                Bahan = x.Bahan
            });
            foreach (var item in produksi.ProduksiDetailBahan) listBahan.RemoveAll(x => x.Id == item.BahanId);
            produksi.Jumlah = result.Jumlah;
        }

        protected async Task<IEnumerable<Bahan>> CariBahan(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listBahan.Where(x => (x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.SatuanProduksi.Contains(value, StringComparison.OrdinalIgnoreCase)) && x.Stok > 0).OrderBy(x => x.Nama));
        }

        protected void PilihBahan(Bahan e)
        {
            if (e is null) return;
            produksi.ProduksiDetailBahan!.Add(new() { ProduksiId = produksi.Id, BahanId = e.Id, Bahan = e, Jumlah = 1 });
            listBahan.RemoveAll(x => x.Id == e.Id);
            inputBahan!.Reset();
        }

        protected void DeleteBahan(ProduksiDetailBahan produksiDetailBahan)
        {
            produksi.ProduksiDetailBahan!.Remove(produksiDetailBahan);
            listBahan.Add(produksiDetailBahan.Bahan!);
            Hitung();
        }

        protected async Task<IEnumerable<Karyawan>> CariKaryawan(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listKaryawan.Where(x => x.Id.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Pekerjaan!.Nama.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected void PilihKaryawan(Karyawan e)
        {
            if (e is null) return;
            produksi.ProduksiDetailJasa!.Add(new() { ProduksiId = produksi.Id, KaryawanId = e.Id, Karyawan = e, Biaya = e.Upah });
            listKaryawan.RemoveAll(x => x.Id == e.Id);
            Hitung();
            inputKaryawan!.Reset();
        }

        protected void DeleteKaryawan(ProduksiDetailJasa produksiDetailJasa)
        {
            produksi.ProduksiDetailJasa!.RemoveAll(x => x.KaryawanId == produksiDetailJasa.KaryawanId);
            listKaryawan.Add(produksiDetailJasa.Karyawan!);
            Hitung();
        }

        protected async Task<IEnumerable<Overhead>> CariOverhead(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listOverhead.Where(x => x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected void PilihOverhead(Overhead e)
        {
            if (e is null) return;
            produksi.ProduksiDetailOverhead!.Add(new() { ProduksiId = produksi.Id, OverheadId = e.Id, Overhead = e });
            listOverhead.RemoveAll(x => x.Id == e.Id);
            Hitung();
            inputOverhead!.Reset();
        }

        protected void DeleteOverhead(ProduksiDetailOverhead produksiDetailOverhead)
        {
            produksi.ProduksiDetailOverhead!.RemoveAll(x => x.OverheadId == produksiDetailOverhead.OverheadId);
            listOverhead.Add(produksiDetailOverhead.Overhead!);
            Hitung();
        }

        protected bool Validasi()
        {
            if (produksi.ProduksiDetailBahan!.Any() && result is not null)
            {
                foreach (var item in produksi.ProduksiDetailBahan!) if (item.StokAkhir >= 0) return false;
            }
            return true;
        }

        protected async Task Refresh()
        {
            result = new();
            Produksi produksiDetails = await ProduksiService.RefreshDetail(Id, produksi.ProduksiDetailBahan!.ConvertAll(x => x.BahanId), produksi.ProduksiDetailJasa!.ConvertAll(x => x.KaryawanId), produksi.ProduksiDetailOverhead!.ConvertAll(x => x.OverheadId));
            if (produksiDetails is null)
            {
                Snackbar.Add("Produksi telah dihapus", MudBlazor.Severity.Error);
                To.NavigateTo("/produksi");
                return;
            }
            produksi.Barang = produksiDetails.Barang;
            foreach (var item in produksiDetails.ProduksiDetailBahan!) produksi.ProduksiDetailBahan!.First(x => x.BahanId == item.BahanId).Bahan = item.Bahan;
            foreach (var item in produksiDetails.ProduksiDetailJasa!) produksi.ProduksiDetailJasa!.First(x => x.KaryawanId == item.KaryawanId).Karyawan = item.Karyawan;
            foreach (var item in produksiDetails.ProduksiDetailOverhead!) produksi.ProduksiDetailOverhead!.First(x => x.OverheadId == item.OverheadId).Overhead = item.Overhead;
            Hitung();
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            if (!produksi.ProduksiDetailBahan!.Any() || produksi.ProduksiDetailJasa!.Any(x => x.Biaya == 0) || produksi.ProduksiDetailBahan!.Any(x => x.Jumlah == 0 || x.Bahan!.Stok < x.Jumlah) || produksi.ProduksiDetailOverhead!.Any(x => x.Biaya == 0))
            {
                Snackbar.Add("Daftar detail tidak valid", MudBlazor.Severity.Error);
                return;
            }

            await form!.Validate();
            if (form!.IsValid)
            {
                if (baru)
                {
                    result = await ProduksiService.Create(produksi);
                }
                else
                {
                    result = await ProduksiService.Update(produksi);
                }

                if (result is not null)
                {
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Produksi", EntitasId = result.Id, Keterangan = baru ? "Create" : "Update" });
                    Snackbar.Add("Produksi Berhasil", MudBlazor.Severity.Success);
                    To.NavigateTo("/produksi");
                }
                else
                {
                    pesan = "Terjadi kesalahan saat menyimpan data, harap periksa kembali dan refresh form.";
                }
            }
        }

        public class ProduksiFluentValidator : AbstractValidator<Produksi>
        {
            public ProduksiFluentValidator()
            {
                RuleFor(x => x.Barang)
                    .NotEmpty().WithMessage("Supplier tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Produksi>.CreateWithOptions((Produksi)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}