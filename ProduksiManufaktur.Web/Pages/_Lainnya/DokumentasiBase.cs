namespace ProduksiManufaktur.Web.Pages._Lainnya
{
    public class DokumentasiBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IJSRuntime JSRuntime { get; set; } = null!;

        protected object? sectionId;

        protected override void OnInitialized()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Dokumentasi", "/dokumentasi")
            };
            Layout.Refresh();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("updateUrl");
            }
        }

        protected async Task NavigateToSection()
        {
            await JSRuntime.InvokeAsync<object>("scrollToSection", sectionId);
        }
    }
}