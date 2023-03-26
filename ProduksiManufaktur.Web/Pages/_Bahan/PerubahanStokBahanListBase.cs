using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Bahan
{
    [Authorize(Policy = "ProdukRead")]
    public class PerubahanStokBahanListBase : ComponentBase
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

        protected MudMessageBox? deleteDialog = new();

        protected List<PerubahanStokBahan> listPerubahanStokBahan = null!;

        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;
        protected bool baru;

        protected async Task LoadData()
        {
            listPerubahanStokBahan = await BahanService.GetPerubahanStok();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Bahan", "/bahan"),
                new BreadcrumbItem("Perubahan Stok Bahan", "/bahan/perubahan-stok-bahan")
            };
            Layout.Refresh();

            await LoadData();
        }

        protected async Task Form(PerubahanStokBahan perubahanStokBahan = null!)
        {
            baru = perubahanStokBahan is null;
            PerubahanStokBahan model = new();

            if (!baru) perubahanStokBahan.CopyPropertiesTo(model);

            List<Bahan> listBahan = await BahanService.Get();

            var parameters = new DialogParameters { ["Baru"] = baru, ["PerubahanStokBahan"] = model, ["ListBahan"] = listBahan };
            var form = await DialogService.Show<PerubahanStokBahanForm>("Form Perubahan Stok Bahan", parameters).Result;

            if (!form.Canceled)
            {
                var result = (PerubahanStokBahan)form.Data;

                Snackbar.Add(baru ? "Data berhasil ditambah" : "Data berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Perubahan Stok Bahan", EntitasId = result.Id.ToString(), Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(PerubahanStokBahan perubahanStokBahan)
        {
            if (await BahanService.DeletablePerubahanStok(perubahanStokBahan.Id))
            {
                pesanHapus = $"Hapus data dengan Id {perubahanStokBahan.Id}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await BahanService.DeletePerubahanStok(perubahanStokBahan.Id);
                    if (success)
                        Snackbar.Add("Data berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Data gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Perubahan Stok Bahan", EntitasId = perubahanStokBahan.Id.ToString(), Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Jumlah penambahan melebihi stok", Severity.Error);
                return;
            }
        }

        protected Func<PerubahanStokBahan, bool> FilterSearch => x => $"{x.Id} {x.Bahan!.Nama} {x.Tanggal} {x.Jenis} {x.Jumlah} {x.Keterangan}".Cari(dicari);
    }
}