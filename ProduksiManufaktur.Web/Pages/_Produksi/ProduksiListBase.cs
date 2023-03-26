using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Produksi
{
    [Authorize(Policy = "ProduksiRead")]
    public class ProduksiListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IProduksiService ProduksiService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Produksi> listProduksi = null!;

        protected bool loaded;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listProduksi = await ProduksiService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Produksi", "/produksi")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Hapus(Produksi produksi)
        {
            pesanHapus = $"Hapus {produksi.Id}?";
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = await ProduksiService.Delete(produksi.Id);
                if (success)
                    Snackbar.Add("Produksi berhasil dihapus", Severity.Success);
                else
                    Snackbar.Add("Produksi gagal dihapus", Severity.Error);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Produksi", EntitasId = produksi.Id, Keterangan = "Delete" });
            }
        }

        protected Func<Produksi, bool> FilterSearch => x => $"{x.Id} {x.Tanggal} {x.Barang!.Nama}".Cari(dicari);
    }
}