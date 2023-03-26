using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Barang
{
    [Authorize(Policy = "ProdukRead")]
    public class BarangListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IBarangService BarangService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Barang> listBarang = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listBarang = await BarangService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Barang", "/barang")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(Barang barang = null!)
        {
            baru = barang is null;
            Barang model = new() { BarangSatuan = new() };

            if (!baru)
            {
                barang!.BarangSatuan = await BarangService.FindBarangSatuan(barang.Id);
                barang.CopyPropertiesTo(model);
            }

            var parameters = new DialogParameters { ["Baru"] = baru, ["Barang"] = model };
            var form = await DialogService.Show<BarangForm>("Form Barang", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add(baru ? "Barang berhasil ditambah" : "Barang berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Barang", EntitasId = ((Barang)form.Data).Id, Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(Barang barang)
        {
            if (await BarangService.Deletable(barang.Id))
            {
                pesanHapus = $"Hapus {barang.Nama}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await BarangService.Delete(barang.Id);
                    if (success)
                        Snackbar.Add("Barang berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Barang gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Barang", EntitasId = barang.Id, Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Barang telah digunakan dalam transaksi", Severity.Error);
                return;
            }
        }

        protected Func<Barang, bool> FilterSearch => x => $"{x.Id} {x.Nama} {x.SatuanProduksi}".Cari(dicari);
    }
}