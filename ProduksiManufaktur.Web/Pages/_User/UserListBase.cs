using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ProduksiManufaktur.Web.Pages._User
{
    [Authorize(Policy = "AkunRead")]
    public class UserListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected IRoleService RoleService { get; set; } = null!;

        [Inject]
        public UserManager<User> UserManager { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<User> listUser = new();

        protected bool loaded;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listUser = await UserService.Get();
            listUser.RemoveAll(x => x.Email == Layout.currentUser.Email);
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("User", "/user")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(User user = null!)
        {
            User model = new();

            user = await UserService.Find(user!.Id);
            user.CopyPropertiesTo(model);

            List<Role> listRole = await RoleService.Get();

            var parameters = new DialogParameters { ["User"] = model, ["ListRole"] = listRole };
            var form = await DialogService.Show<UserForm>("Form User", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add("User berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "User", EntitasId = ((User)form.Data).Email!, Keterangan = "Update" });
            }
        }

        protected async Task FormUserClaim(User user = null!)
        {
            User model = new();

            user.CopyPropertiesTo(model);

            var parameters = new DialogParameters { ["User"] = model };
            var form = await DialogService.Show<UserClaimForm>("Form User", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add("Claim user berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "User", EntitasId = ((User)form.Data).Email!, Keterangan = "Update" });
            }
        }

        protected async Task Hapus(User user)
        {
            if (await UserService.Deletable(user.Id))
            {
                pesanHapus = $"Hapus {user.Email}?";
                bool? result = await deleteDialog!.Show();
                if (result == false)
                {
                    bool success = (await UserManager.DeleteAsync(user)).Succeeded;
                    if (success)
                        Snackbar.Add("User berhasil dihapus", Severity.Success);
                    else
                        Snackbar.Add("User gagal dihapus", Severity.Error);
                    await LoadData();
                    await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "User", EntitasId = user.Email!, Keterangan = "Delete" });
                }
            }
            else
            {
                Snackbar.Add("User pernah menginput data", Severity.Error);
                return;
            }
        }

        protected Func<User, bool> FilterSearch => x => $"{x.Id} {x.Email} {x.PhoneNumber} {x.Alamat} {x.TempatLahir} {x.TanggalLahir} {x.Roles}".Cari(dicari);
    }
}