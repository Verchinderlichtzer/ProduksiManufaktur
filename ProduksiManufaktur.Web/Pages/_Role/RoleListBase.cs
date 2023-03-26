using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ProduksiManufaktur.Web.Pages._Role
{
    [Authorize(Policy = "AkunRead")]
    public class RoleListBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IRoleService RoleService { get; set; } = null!;

        [Inject]
        protected IDialogService DialogService { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected RoleManager<Role> RoleManager { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudMessageBox? deleteDialog = new();

        protected List<Role> listRole = null!;

        protected bool loaded;
        protected bool baru;
        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;

        protected async Task LoadData()
        {
            listRole = (await RoleService.Get1()).ConvertAll(x => new Role
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                ConcurrencyStamp = x.ConcurrencyStamp,
                ClaimNoAccess = x.RoleClaim!.Count(y => y.ClaimValue == "W0"),
                ClaimRead = x.RoleClaim!.Count(y => y.ClaimValue == "W1"),
                ClaimWrite = x.RoleClaim!.Count(y => y.ClaimValue == "W2"),
                JumlahUser = x.UserRole!.Count,
                UserRole = x.UserRole,
                RoleClaim = x.RoleClaim
            });
        }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Role", "/role")
            };
            Layout.Refresh();

            await LoadData();
            loaded = true;
        }

        protected async Task Form(Role role = null!)
        {
            baru = role is null;
            Role model = new();

            if (!baru) role.CopyPropertiesTo(model);

            var parameters = new DialogParameters { ["Baru"] = baru, ["Role"] = model };
            var form = await DialogService.Show<RoleForm>("Form Role", parameters).Result;

            if (!form.Canceled)
            {
                Snackbar.Add(baru ? "Role berhasil ditambah" : "Role berhasil diubah", Severity.Success);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Role", EntitasId = ((Role)form.Data).Id, Keterangan = baru ? "Create" : "Update" });
            }
        }

        protected async Task Hapus(Role role)
        {
            if (role.UserRole!.Any())
            {
                Snackbar.Add("Role digunakan oleh user", Severity.Error);
                return;
            }
            pesanHapus = $"Hapus {role.Name}?";
            bool? result = await deleteDialog!.Show();
            if (result == false)
            {
                bool success = (await RoleManager.DeleteAsync(role)).Succeeded;
                if (success)
                    Snackbar.Add("Role berhasil dihapus", Severity.Success);
                else
                    Snackbar.Add("Role gagal dihapus", Severity.Error);
                await LoadData();
                await UserService.CreateLog(new() { UserId = Layout.currentUser.Id, Entitas = "Role", EntitasId = role.Id, Keterangan = "Delete" });
            }
        }

        protected Func<Role, bool> FilterSearch => x => $"{x.Id} {x.Name}".Cari(dicari);
    }
}