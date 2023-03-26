using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ProduksiManufaktur.Web.Pages._User
{
    public class UserFormBase : ComponentBase
    {
        [Parameter]
        public User User { get; set; } = new();

        [Parameter]
        public List<Role> ListRole { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected IRoleService RoleService { get; set; } = null!;

        [Inject]
        protected UserManager<User> UserManager { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudForm? form = new();
        protected MudAutocomplete<Role>? inputRole = new();

        protected bool popupTerbuka;
        protected User? result = new();
        protected string icon = Icons.Material.Filled.Edit;
        protected string judul = string.Empty;
        protected Color warna = Color.Warning;

        protected async Task Closed(MudChip chip)
        {
            User.UserRole!.Remove(User.UserRole!.First(x => x.RoleName == chip.Text));
            ListRole.Add(await RoleService.Find(chip.Text));
        }

        protected async Task<IEnumerable<Role>> CariRole(string value)
        {
            value ??= string.Empty;
            return await Task.FromResult(ListRole.Where(x => x.Name?.Contains(value, StringComparison.OrdinalIgnoreCase) == true).OrderBy(x => x.Name));
        }

        protected void PilihRole(Role e)
        {
            if (e is null) return;
            User.UserRole!.Add(new() { Role = e, RoleName = e.Name!, UserId = User.Id });
            ListRole.RemoveAll(x => x.Id == e.Id);
            inputRole!.Reset();
        }

        protected override void OnInitialized()
        {
            User.UserRole!.ForEach(x => x.RoleName = x.Role!.Name!);
            foreach (var x in User.UserRole!)
                ListRole.RemoveAll(y => y.Id == x.Role!.Id);
            judul = $"Edit {User.Email}";
        }

        protected async Task Refresh()
        {
            result = new();
            User = (await UserManager.FindByIdAsync(User.Id))!;
            if (User is null)
            {
                Snackbar.Add("User telah dihapus", MudBlazor.Severity.Error);
                MudDialog.Cancel();
            }
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                result = await UserService.Update(User);
                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}