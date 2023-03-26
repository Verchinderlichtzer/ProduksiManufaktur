using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ProduksiManufaktur.Web.Pages._Role
{
    public class RoleFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public Role Role { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IRoleService RoleService { get; set; } = null!;

        [Inject]
        protected RoleManager<Role> RoleManager { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected RoleFluentValidator validator = null!;
        protected MudForm? form = new();

        protected Dictionary<string, string> roleClaim = new()
        {
            { "Akun", "W0" },
            { "Produk", "W0" },
            { "Pekerja", "W0" },
            { "Pihak", "W0" },
            { "Overhead", "W0" },
            { "Pembelian", "W0" },
            { "Penjualan", "W0" },
            { "Produksi", "W0" },
            { "TransaksiLain", "W0" },
            { "Report", "W0" }
        };

        protected bool popupTerbuka;
        protected Role? result = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Role";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            validator = new(Role, RoleManager);

            if (!Baru)
            {
                foreach (var x in Role.RoleClaim!)
                    roleClaim[x.ClaimType!] = x.ClaimValue!;
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {Role.Name}";
                warna = Color.Warning;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                Role = await RoleService.Find(Role.Id);
                if (Role is null)
                {
                    Snackbar.Add("Role telah dihapus", MudBlazor.Severity.Error);
                    MudDialog.Cancel();
                }
            }
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                if (Baru)
                {
                    await RoleManager.CreateAsync(Role);
                    result = await RoleManager.FindByNameAsync(Role.Name!);
                    foreach (var x in roleClaim) await RoleManager.AddClaimAsync(Role, new(x.Key, x.Value));
                }
                else
                {
                    foreach (var x in Role.RoleClaim!) x.ClaimValue = roleClaim[x.ClaimType!];
                    result = await RoleService.Update(Role);
                }

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class RoleFluentValidator : AbstractValidator<Role>
        {
            private readonly Role _role;
            private readonly RoleManager<Role> _roleManager;

            public RoleFluentValidator(Role role, RoleManager<Role> roleManager)
            {
                _role = role;
                _roleManager = roleManager;

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Nama tidak boleh kosong")
                    .MustAsync(UniqueName!).WithMessage("Nama sudah terpakai");
            }

            private async Task<bool> UniqueName(string name, CancellationToken token)
            {
                return await Task.FromResult(_roleManager.Roles.Any(x => x.Name == name) || name == _role.Name);
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Role>.CreateWithOptions((Role)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}