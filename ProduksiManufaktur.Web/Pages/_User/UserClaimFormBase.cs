using Microsoft.AspNetCore.Identity;

namespace ProduksiManufaktur.Web.Pages._User
{
    public class UserClaimFormBase : ComponentBase
    {
        [Parameter]
        public User User { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected UserManager<User> UserManager { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected Dictionary<string, string> userClaim = new()
        {
            { "Akun", "None" },
            { "Produk", "None" },
            { "Pekerja", "None" },
            { "Pihak", "None" },
            { "Overhead", "None" },
            { "Pembelian", "None" },
            { "Penjualan", "None" },
            { "Produksi", "None" },
            { "TransaksiLain", "None" },
            { "Report", "None" }
        };

        protected bool popupTerbuka;
        protected User? result = new();
        protected string icon = Icons.Material.Filled.Edit;
        protected string judul = "Tambah User";
        protected Color warna = Color.Warning;

        protected override void OnInitialized()
        {
            foreach (var x in User.UserClaim!)
                userClaim[x.ClaimType!] = x.ClaimValue!;
            judul = $"Edit User Claim - {User.Email}";
        }

        protected async Task Refresh()
        {
            result = new();
            User = (await UserManager.FindByIdAsync(User.Id))!;
            if (User is null)
            {
                Snackbar.Add("User telah dihapus", Severity.Error);
                MudDialog.Cancel();
            }
            StateHasChanged();
        }

        protected async Task Save()
        {
            User.UserClaim = new();
            foreach (var x in userClaim.Where(x => x.Value != "None"))
                User.UserClaim.Add(new() { UserId = User.Id, ClaimType = x.Key, ClaimValue = x.Value });
            result = await UserService.UpdatesClaim(User);

            if (result is not null) MudDialog.Close(DialogResult.Ok(result));
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}