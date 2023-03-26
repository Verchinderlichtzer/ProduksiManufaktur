using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Bahan
{
    [Authorize(Policy = "ProdukRead")]
    public class BahanListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IBahanService BahanService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Bahan> listBahan = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listBahan = await BahanService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Bahan", "/bahan")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(Bahan bahan = null!)
        {
            baru = bahan is null;
            Bahan model = new() { BahanSatuan = new() };

            if (!baru)
            {
                bahan!.BahanSatuan = await BahanService.FindBahanSatuan(bahan.Id);
                bahan.CopyPropertiesTo(model);
            }

            var parameters = new DialogParameters { ["Baru"] = baru, ["Bahan"] = model };
            var form = await DialogService.Show<BahanForm>("Form Bahan", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add(baru ? "Bahan berhasil ditambah" : "Bahan berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Bahan", EntitasId = ((Bahan)form.Data).Id, Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(Bahan bahan)
        {
            if (await BahanService.Deletable(bahan.Id))
            {
                pesanHapus = $"Hapus {bahan.Nama}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await BahanService.Delete(bahan.Id);
                    if (success)
                        Snackbar.Add("Bahan berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Bahan gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Bahan", EntitasId = bahan.Id, Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Bahan telah digunakan dalam transaksi", Severity.Error);
                return;
            }
        }

        protected Func<Bahan, bool> FilterSearch => x => $"{x.Id} {x.Nama} {x.SatuanProduksi}".Cari(dicari);
    }
}