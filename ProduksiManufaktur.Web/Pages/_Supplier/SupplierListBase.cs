using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Supplier
{
    [Authorize(Policy = "PihakRead")]
    public class SupplierListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected ISupplierService SupplierService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Supplier> listSupplier = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listSupplier = await SupplierService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Supplier", "/supplier")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(Supplier supplier = null!)
        {
            baru = supplier is null;
            Supplier model = new();

            if (!baru) supplier.CopyPropertiesTo(model);

            var parameters = new DialogParameters { ["Baru"] = baru, ["Supplier"] = model };
            var form = await DialogService.Show<SupplierForm>("Form Supplier", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add(baru ? "Supplier berhasil ditambah" : "Supplier berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Supplier", EntitasId = ((Supplier)form.Data).Id, Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(Supplier supplier)
        {
            if (await SupplierService.Deletable(supplier.Id))
            {
                pesanHapus = $"Hapus {supplier.Nama}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await SupplierService.Delete(supplier.Id);
                    if (success)
                        Snackbar.Add("Supplier berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Supplier gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Supplier", EntitasId = supplier.Id, Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Supplier pernah melakukan transaksi", Severity.Error);
                return;
            }
        }

        protected Func<Supplier, bool> FilterSearch => x => $"{x.Id} {x.Nama} {x.Alamat} {x.Telepon} {x.Fax} {x.Email}".Cari(dicari);
    }
}