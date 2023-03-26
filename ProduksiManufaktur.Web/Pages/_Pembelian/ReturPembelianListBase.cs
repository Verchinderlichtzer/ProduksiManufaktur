using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Pembelian
{
    [Authorize(Policy = "PembelianRead")]
    public class ReturPembelianListBase : ComponentBase
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

        protected List<ReturPembelian> listReturPembelian = null!;

        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;
        protected bool baru;

        protected async Task LoadData()
        {
            listReturPembelian = await PembelianService.GetRetur();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pembelian", "/pembelian"),
                new BreadcrumbItem("Retur", "/pembelian/retur")
            };
            Layout.Refresh();

            await LoadData();
        }

        protected async Task Hapus(ReturPembelian returPembelian)
        {
            pesanHapus = $"Hapus data dengan Id {returPembelian.Id}?";
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = await PembelianService.DeleteRetur(returPembelian.Id);
                if (success)
                    Snackbar.Add("Transaksi berhasil dihapus", Severity.Success);
                else
                    Snackbar.Add("Transaksi gagal dihapus", Severity.Error);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Retur Pembelian", EntitasId = returPembelian.Id, Keterangan = "Delete" });
            }
        }

        protected Func<ReturPembelian, bool> FilterSearch => x => $"{x.Id} {x.PembelianId!} {x.Tanggal} {x.Keterangan} {x.GrandTotal}".Cari(dicari);
    }
}