using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Overhead
{
    [Authorize(Policy = "OverheadRead")]
    public class OverheadListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IOverheadService OverheadService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Overhead> listOverhead = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listOverhead = await OverheadService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Overhead", "/overhead")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(Overhead overhead = null!)
        {
            baru = overhead is null;
            Overhead model = new();

            if (!baru) overhead.CopyPropertiesTo(model);

            var parameters = new DialogParameters { ["Baru"] = baru, ["Overhead"] = model };
            var form = await DialogService.Show<OverheadForm>("Form Overhead", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add(baru ? "Overhead berhasil ditambah" : "Overhead berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Overhead", EntitasId = ((Overhead)form.Data).Id.ToString(), Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(Overhead overhead)
        {
            if (await OverheadService.Deletable(overhead.Id))
            {
                pesanHapus = $"Hapus {overhead.Nama}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await OverheadService.Delete(overhead.Id);
                    if (success)
                        Snackbar.Add("Overhead berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Overhead gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Overhead", EntitasId = overhead.Id.ToString(), Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Overhead telah digunakan dalam transaksi", Severity.Error);
                return;
            }
        }

        protected Func<Overhead, bool> FilterSearch => x => $"{x.Id} {x.Nama}".Cari(dicari);
    }
}