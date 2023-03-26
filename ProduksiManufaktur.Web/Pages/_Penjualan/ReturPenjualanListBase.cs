using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Penjualan
{
    [Authorize(Policy = "PenjualanRead")]
    public class ReturPenjualanListBase : ComponentBase
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

        protected List<ReturPenjualan> listReturPenjualan = null!;

        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;
        protected bool baru;

        protected async Task LoadData()
        {
            listReturPenjualan = await PenjualanService.GetRetur();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Penjualan", "/penjualan"),
                new BreadcrumbItem("Retur", "/penjualan/retur")
            };
            Layout.Refresh();

            await LoadData();
        }

        protected async Task Hapus(ReturPenjualan returPenjualan)
        {
            pesanHapus = $"Hapus data dengan Id {returPenjualan.Id}?";
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = await PenjualanService.DeleteRetur(returPenjualan.Id);
                if (success)
                    Snackbar.Add("Transaksi berhasil dihapus", Severity.Success);
                else
                    Snackbar.Add("Transaksi gagal dihapus", Severity.Error);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Retur Penjualan", EntitasId = returPenjualan.Id, Keterangan = "Delete" });
            }
        }

        protected Func<ReturPenjualan, bool> FilterSearch => x => $"{x.Id} {x.PenjualanId!} {x.Tanggal} {x.Keterangan} {x.GrandTotal}".Cari(dicari);
    }
}