using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Pekerjaan
{
    [Authorize(Policy = "PekerjaRead")]
    public class PekerjaanListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPekerjaanService PekerjaanService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        protected NavigationManager To { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Pekerjaan> listPekerjaan = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listPekerjaan = await PekerjaanService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pekerjaan", "/pekerjaan")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(Pekerjaan pekerjaan = null!)
        {
            baru = pekerjaan is null;
            Pekerjaan model = new();

            if (!baru) pekerjaan.CopyPropertiesTo(model);

            var parameters = new DialogParameters { ["Baru"] = baru, ["Pekerjaan"] = model };
            var form = await DialogService.Show<PekerjaanForm>("Form Pekerjaan", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add(baru ? "Pekerjaan berhasil ditambah" : "Pekerjaan berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Pekerjaan", EntitasId = ((Pekerjaan)form.Data).Id.ToString(), Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(Pekerjaan pekerjaan)
        {
            if (await PekerjaanService.Deletable(pekerjaan.Id))
            {
                pesanHapus = $"Hapus {pekerjaan.Nama}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await PekerjaanService.Delete(pekerjaan.Id);
                    if (success)
                        Snackbar.Add("Pekerjaan berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Pekerjaan gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Pekerjaan", EntitasId = pekerjaan.Id.ToString(), Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add($"Pekerjaan dilakukan oleh {pekerjaan.JumlahKaryawan} karyawan", Severity.Error);
                return;
            }
        }

        protected Func<Pekerjaan, bool> FilterSearch => x => $"{x.Id} {x.Nama}".Cari(dicari);
    }
}