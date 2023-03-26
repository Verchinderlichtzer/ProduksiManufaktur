using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Karyawan
{
    [Authorize(Policy = "PekerjaRead")]
    public class KaryawanListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IKaryawanService KaryawanService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected IPekerjaanService PekerjaanService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Karyawan> listKaryawan = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listKaryawan = await KaryawanService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Karyawan", "/karyawan")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(Karyawan karyawan = null!)
        {
            baru = karyawan is null;
            Karyawan model = new();

            if (!baru) karyawan.CopyPropertiesTo(model);

            Dictionary<int, string> pekerjaan = (await PekerjaanService.Get1()).ToDictionary(x => x.Id, y => y.Nama);

            var parameters = new DialogParameters { ["Baru"] = baru, ["Karyawan"] = model, ["Pekerjaan"] = pekerjaan };
            var form = await DialogService.Show<KaryawanForm>("Form Karyawan", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add(baru ? "Karyawan berhasil ditambah" : "Karyawan berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Karyawan", EntitasId = ((Karyawan)form.Data).Id, Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(Karyawan karyawan)
        {
            if (await KaryawanService.Deletable(karyawan.Id))
            {
                pesanHapus = $"Hapus {karyawan.Nama}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await KaryawanService.Delete(karyawan.Id);
                    if (success)
                        Snackbar.Add("Karyawan berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Karyawan gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Karyawan", EntitasId = karyawan.Id, Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Karyawan pernah bekerja", Severity.Error);
                return;
            }
        }

        protected Func<Karyawan, bool> FilterSearch => x => $"{x.Id} {x.Nama} {x.TempatLahir} {x.TanggalLahir} {x.Alamat} {x.Telepon} {x.Email} {x.Upah} {x.Pekerjaan!.Nama}".Cari(dicari);
    }
}