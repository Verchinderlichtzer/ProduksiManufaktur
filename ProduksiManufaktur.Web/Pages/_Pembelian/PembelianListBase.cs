using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Pembelian
{
    [Authorize(Policy = "PembelianRead")]
    public class PembelianListBase : ComponentBase
    {
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

        protected MudMessageBox? deleteDialog = new();

        protected List<Pembelian> listPembelian = null!;

        protected bool loaded;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listPembelian = await PembelianService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pembelian", "/pembelian")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Hapus(Pembelian pembelian)
        {
            if (await PembelianService.Deletable(pembelian.Id))
            {
                pesanHapus = $"Hapus {pembelian.Id}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await PembelianService.Delete(pembelian.Id);
                    if (success)
                        Snackbar.Add("Pembelian berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Pembelian gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Pembelian", EntitasId = pembelian.Id, Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Beberapa bahan pada transaksi ini sudah diretur", Severity.Error);
                return;
            }
        }

        protected Func<Pembelian, bool> FilterSearch => x => $"{x.Id} {x.Supplier!.Nama} {x.Tanggal} {x.MetodeBayar} {x.PPN} {x.Keterangan} {x.GrandTotal}".Cari(dicari);
    }
}