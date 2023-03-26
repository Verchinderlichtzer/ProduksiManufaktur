using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Customer
{
    [Authorize(Policy = "PihakRead")]
    public class CustomerListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected ICustomerService CustomerService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Customer> listCustomer = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listCustomer = await CustomerService.Get();
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Customer", "/customer")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(Customer customer = null!)
        {
            baru = customer is null;
            Customer model = new();

            if (!baru) customer.CopyPropertiesTo(model);

            var parameters = new DialogParameters { ["Baru"] = baru, ["Customer"] = model };
            var form = await DialogService.Show<CustomerForm>("Form Customer", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add(baru ? "Customer berhasil ditambah" : "Customer berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Customer", EntitasId = ((Customer)form.Data).Id, Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(Customer customer)
        {
            if (await CustomerService.Deletable(customer.Id))
            {
                pesanHapus = $"Hapus {customer.Nama}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = await CustomerService.Delete(customer.Id);
                    if (success)
                        Snackbar.Add("Customer berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("Customer gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Customer", EntitasId = customer.Id, Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("Customer pernah melakukan transaksi", Severity.Error);
                return;
            }
        }

        protected Func<Customer, bool> FilterSearch => x => $"{x.Id} {x.Nama} {x.Alamat} {x.Telepon} {x.Fax} {x.Email}".Cari(dicari);
    }
}