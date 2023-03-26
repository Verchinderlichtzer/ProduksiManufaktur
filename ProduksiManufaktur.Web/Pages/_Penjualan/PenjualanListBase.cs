using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Penjualan
{
    [Authorize(Policy = "PenjualanRead")]
    public class PenjualanListBase : ComponentBase
    {
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

        protected MudMessageBox? deleteDialog = new();

        protected List<Penjualan> listPenjualan = null!;

        protected bool loaded;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listPenjualan = await PenjualanService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Penjualan", "/penjualan")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Hapus(Penjualan penjualan)
        {
            if (await PenjualanService.Deletable(penjualan.Id))
            {
                pesanHapus = $"Hapus {penjualan.Id}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await PenjualanService.Delete(penjualan.Id);
                    if (success)
                        Snackbar.Add("Penjualan berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Penjualan gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Penjualan", EntitasId = penjualan.Id, Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Beberapa barang pada transaksi ini sudah diretur", Severity.Error);
                return;
            }
        }

        protected Func<Penjualan, bool> FilterSearch => x => $"{x.Id} {x.Customer!.Nama} {x.Tanggal} {x.MetodeBayar} {x.PPN} {x.Keterangan} {x.GrandTotal}".Cari(dicari);
    }
}