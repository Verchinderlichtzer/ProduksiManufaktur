using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Formulasi
{
    [Authorize(Policy = "ProdukRead")]
    public class FormulasiListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IFormulasiService FormulasiService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Formulasi> listFormulasi = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listFormulasi = await FormulasiService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Formulasi", "/formulasi")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Hapus(Formulasi formulasi)
        {
            pesanHapus = $"Hapus {formulasi.Id}?";
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = await FormulasiService.Delete(formulasi.Id);
                if (success)
                    Snackbar.Add("Formulasi berhasil dihapus", Severity.Success);
                else
                    Snackbar.Add("Formulasi gagal dihapus", Severity.Error);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Formulasi", EntitasId = formulasi.Id, Keterangan = "Delete" });
            }
        }

        protected Func<Formulasi, bool> FilterSearch => x => $"{x.Id} {x.Barang!.Nama} {x.Barang!.SatuanProduksi}".Cari(dicari);
    }
}