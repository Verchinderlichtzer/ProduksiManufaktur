namespace ProduksiManufaktur.Web.Pages._LogTransaksi
{
    public class LogTransaksiListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();
        protected List<LogTransaksi> listLogTransaksi = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;

        protected async Task LoadData()
        {
            listLogTransaksi = await UserService.GetLog();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Log Transaksi", "/log-transaksi")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Hapus()
        {
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = await UserService.DeletesLog();
                if (success)
                    Snackbar.Add("Log berhasil dihapus", Severity.Success);
                else
                    Snackbar.Add("Log gagal dihapus", Severity.Error);
                await LoadData();
            }
        }

        protected Func<LogTransaksi, bool> FilterSearch => x => $"{x.Id} {x.Tanggal} {x.Entitas} {x.EntitasId} {x.Keterangan} {x.User!.Email} {x.User!.PhoneNumber}".Cari(dicari);
    }
}