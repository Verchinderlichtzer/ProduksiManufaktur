namespace ProduksiManufaktur.Web.Pages._Lainnya
{
    public class InfoUserBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IUserService UserService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudForm? form = new();

        protected User user = null!;
        protected bool loaded;
        protected bool popupTerbuka;

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Info User", $"/info-user/{Id}")
            };
            Layout.Refresh();
            user = (await UserService.Find(Id))!;
            loaded = true;
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                await UserService.Update(user);
                Snackbar.Add("Profil user berhasil diubah", MudBlazor.Severity.Success);
            }
        }
    }
}