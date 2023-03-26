using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Barang
{
    [Authorize(Policy = "ProdukRead")]
    public class PerubahanStokBarangListBase : ComponentBase
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

        protected MudMessageBox? deleteDialog = new();

        protected List<PerubahanStokBarang> listPerubahanStokBarang = null!;

        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;
        protected bool baru;

        protected async Task LoadData()
        {
            listPerubahanStokBarang = await BarangService.GetPerubahanStok();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Barang", "/barang"),
                new BreadcrumbItem("Perubahan Stok Barang", "/barang/perubahan-stok-barang")
            };
            Layout.Refresh();

            await LoadData();
        }

        protected async Task Form(PerubahanStokBarang perubahanStokBarang = null!)
        {
            baru = perubahanStokBarang is null;
            PerubahanStokBarang model = new();

            if (!baru) perubahanStokBarang.CopyPropertiesTo(model);

            List<Barang> listBarang = await BarangService.Get();

            var parameters = new DialogParameters { ["Baru"] = baru, ["PerubahanStokBarang"] = model, ["ListBarang"] = listBarang };
            var form = await DialogService.Show<PerubahanStokBarangForm>("Form Perubahan Stok Barang", parameters).Result;

            if (!form.Canceled)
            {
                var result = (PerubahanStokBarang)form.Data;

                Snackbar.Add(baru ? "Data berhasil ditambah" : "Data berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Perubahan Stok Barang", EntitasId = result.Id.ToString(), Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(PerubahanStokBarang perubahanStokBarang)
        {
            if (await BarangService.DeletablePerubahanStok(perubahanStokBarang.Id))
            {
                pesanHapus = $"Hapus data dengan Id {perubahanStokBarang.Id}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await BarangService.DeletePerubahanStok(perubahanStokBarang.Id);
                    if (success)
                        Snackbar.Add("Data berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Data gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Perubahan Stok Barang", EntitasId = perubahanStokBarang.Id.ToString(), Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Jumlah penambarang melebihi stok", Severity.Error);
                return;
            }
        }

        protected Func<PerubahanStokBarang, bool> FilterSearch => x => $"{x.Id} {x.Barang!.Nama} {x.Tanggal} {x.Jenis} {x.Jumlah} {x.Keterangan}".Cari(dicari);
    }
}