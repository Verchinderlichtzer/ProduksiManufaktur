using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Pembelian
{
    [Authorize(Policy = "PembelianWrite")]
    public class PembelianFormBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPembelianService PembelianService { get; set; } = null!;

        [Inject]
        protected ISupplierService SupplierService { get; set; } = null!;

        [Inject]
        protected IBahanService BahanService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected PembelianFluentValidator validator = null!;
        protected MudForm? form = new();
        protected MudAutocomplete<BahanSatuan>? inputBahanSatuan = new();

        protected Pembelian pembelian = new() { PembelianDetail = new() };
        protected TransaksiPembelian transaksiPembelian = new();
        protected List<BahanSatuan> listBahanSatuan = null!;
        protected List<Supplier> listSupplier = null!;

        protected bool baru;
        protected bool loaded;
        protected bool popupTerbuka;
        protected Pembelian? result = new();
        protected string pesan = string.Empty;
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Pembelian";
        protected Color warna = Color.Success;

        protected override async Task OnInitializedAsync()
        {
            baru = string.IsNullOrEmpty(Id);
            if (!baru && !(await Layout.AuthenticationState).User.IsInRole("Admin"))
            {
                To.NavigateTo("/pembelian");
                return;
            }
            validator = new();
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pembelian", "/pembelian"),
                new BreadcrumbItem("Form", $"/pembelian/form/{Id}")
            };
            Layout.Refresh();

            listBahanSatuan = await BahanService.GetBahanSatuan();
            listSupplier = await SupplierService.Get();

            if (!baru)
            {
                pembelian = await PembelianService.Find(Id);

                foreach (var item in pembelian.PembelianDetail!)
                    listBahanSatuan.RemoveAll(x => x.Id == item.BahanSatuanId);

                Hitung();

                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {pembelian.Id}";
                warna = Color.Warning;
            }
            loaded = true;
        }

        protected void Hitung()
        {
            try
            {
                pembelian.Subtotal = (int)pembelian.PembelianDetail!.Sum(x => x.Harga * x.Jumlah);
                pembelian.GrandTotal = (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m));

                transaksiPembelian.Balance = Math.Abs(transaksiPembelian.Nominal + pembelian.Terbayar - pembelian.GrandTotal);
                transaksiPembelian.BalanceLabel = transaksiPembelian.Nominal + pembelian.Terbayar < pembelian.GrandTotal ? "Sisa" : "Kembali";

                foreach (var item in pembelian.PembelianDetail!)
                {
                    item.StokAkhir = item.BahanSatuan!.Bahan!.Stok + pembelian.PembelianDetail!.Where(x => x.BahanSatuan!.BahanId == item.BahanSatuan!.BahanId).Sum(x => (x.Jumlah - x.JumlahSebelum) * x.BahanSatuan!.KonversiStok);
                    item.Total = (int)(item.Jumlah * item.Harga);
                }

                if (baru && pembelian.Subtotal > 0)
                    pembelian.MetodeBayar = transaksiPembelian.Nominal < pembelian.GrandTotal ? "Kredit" : "Tunai";
                else if (baru)
                    pembelian.MetodeBayar = string.Empty;
                if (pembelian.Subtotal > 0)
                    pembelian.Status = transaksiPembelian.Nominal + pembelian.Terbayar < pembelian.GrandTotal ? "Belum Lunas" : "Lunas";
                else
                    pembelian.Status = string.Empty;
            }
            catch (OverflowException)
            {
                pesan = "Nominal terlalu banyak";
                result = null;
                return;
            }
            result = new();
        }

        protected void HitungJatuhTempo(DateTime? e)
        {
            if (e <= pembelian.InputTanggal) e = pembelian.InputTanggal?.AddDays(1);
            pembelian.JatuhTempo = e;
            pembelian.HariJatuhTempo = (e! - pembelian.InputTanggal?.Date)?.Days ?? 1;
        }

        protected async Task<IEnumerable<BahanSatuan>> CariBahanSatuan(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listBahanSatuan.Where(x => x.Bahan!.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Ukuran.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Bahan!.Nama).ThenBy(x => x.Id));
        }

        protected void PilihBahanSatuan(BahanSatuan e)
        {
            if (e is null) return;
            pembelian.PembelianDetail!.Add(new() { PembelianId = pembelian.Id, BahanSatuanId = e.Id, BahanSatuan = e, Jumlah = 1, Harga = e.Harga });
            listBahanSatuan.Remove(e);
            Hitung();
            inputBahanSatuan!.Reset();
        }

        protected async Task<IEnumerable<Supplier>> CariSupplier(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(listSupplier.Where(x => x.Nama.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Nama));
        }

        protected async Task DeleteDetail(PembelianDetail pembelianDetail)
        {
            if (baru || await PembelianService.DeletableDetail(pembelianDetail.PembelianId, pembelianDetail.BahanSatuanId))
            {
                listBahanSatuan.Add(pembelianDetail.BahanSatuan!);
                pembelian.PembelianDetail!.Remove(pembelianDetail);
                Hitung();
            }
            else
            {
                Snackbar.Add("Beberapa bahan sudah diretur", MudBlazor.Severity.Error);
                return;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            List<BahanSatuan> bahanSatuan = await PembelianService.RefreshDetail(pembelian.Id, pembelian.PembelianDetail!.ConvertAll(x => x.BahanSatuanId));
            if (bahanSatuan is null)
            {
                Snackbar.Add("Pembelian telah dihapus", MudBlazor.Severity.Error);
                To.NavigateTo("/pembelian");
                return;
            }
            foreach (var item in bahanSatuan) pembelian.PembelianDetail!.First(x => x.BahanSatuanId == item.Id).BahanSatuan = item;
            Hitung();
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            if (!pembelian.PembelianDetail!.Any() || pembelian.PembelianDetail!.Any(x => x.Harga == 0 || x.Jumlah == 0) || pembelian.PembelianDetail!.GroupBy(x => x.BahanSatuanId).Any(x => x.Count() > 1))
            {
                Snackbar.Add("Daftar bahan tidak valid", MudBlazor.Severity.Error);
                return;
            }

            await form!.Validate();
            if (form!.IsValid)
            {
                if (baru)
                {
                    if (transaksiPembelian.Nominal > 0) pembelian.TransaksiPembelian = new() { transaksiPembelian };
                    result = await PembelianService.Create(pembelian);
                }
                else
                {
                    result = await PembelianService.Update(pembelian);
                }

                if (result is not null)
                {
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Pembelian", EntitasId = result.Id, Keterangan = baru ? "Create" : "Update" });
                    Snackbar.Add("Pembelian Berhasil", MudBlazor.Severity.Success);
                    To.NavigateTo("/pembelian");
                }
                else
                {
                    pesan = "Terjadi kesalahan saat menyimpan data, harap periksa kembali dan refresh form.";
                }
            }
        }

        public class PembelianFluentValidator : AbstractValidator<Pembelian>
        {
            public PembelianFluentValidator()
            {
                RuleFor(x => x.Supplier)
                    .NotEmpty().WithMessage("Supplier tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Pembelian>.CreateWithOptions((Pembelian)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}