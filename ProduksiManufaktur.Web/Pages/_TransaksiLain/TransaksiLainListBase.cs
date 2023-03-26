using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._TransaksiLain
{
    [Authorize(Policy = "ProdukRead")]
    public class TransaksiLainListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected ITransaksiLainService TransaksiLainService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<TransaksiLain> listTransaksiLain = null!;

        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;
        protected bool baru;

        protected async Task LoadData()
        {
            listTransaksiLain = await TransaksiLainService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Transaksi Lain", "/transaksi-lain")
            };
            Layout.Refresh();

            await LoadData();
        }

        protected async Task Form(TransaksiLain transaksiLain = null!)
        {
            baru = transaksiLain is null;

            List<TransaksiLain> listTransaksiLain = new();
            if (!baru)
                listTransaksiLain.Add(transaksiLain!);
            else
                listTransaksiLain.Add(new());

            var parameters = new DialogParameters { ["Baru"] = baru, ["ListTransaksiLain"] = listTransaksiLain };
            var form = await DialogService.Show<TransaksiLainForm>("Form Transaksi Lain", parameters).Result;

            if (!form.Canceled)
            {
                List<TransaksiLain> result = (List<TransaksiLain>)form.Data;

                Snackbar.Add(baru ? "Data berhasil ditambah" : "Data berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Transaksi Lain", EntitasId = baru ? string.Join(", ", result.Select(x => x.Id)) : result.Single().Id.ToString(), Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(TransaksiLain transaksiLain)
        {
            pesanHapus = $"Hapus data dengan Id {transaksiLain.Id}?";
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = await TransaksiLainService.Delete(transaksiLain.Id);
                if (success)
                    Snackbar.Add("Data berhasil dihapus", Severity.Success);
                else
                    Snackbar.Add("Data gagal dihapus", Severity.Error);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Transaksi Lain", EntitasId = transaksiLain.Id.ToString(), Keterangan = "Delete" });
            }
        }

        protected Func<TransaksiLain, bool> FilterSearch => x => $"{x.Tanggal} {x.Jenis} {x.Kategori} {x.Keterangan}".Cari(dicari);
    }
}