namespace ProduksiManufaktur.Web.Pages._Lainnya
{
    public class DokumentasiBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IJSRuntime JSRuntime { get; set; } = null!;

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
                //await JSRuntime.InvokeVoidAsync("window.addEventListener", "scroll", DotNetObjectReference.Create(this).Value.InvokeVoidAsync("updateUrl"));
                await JSRuntime.InvokeVoidAsync("updateUrl");
            }
        }

        protected async Task NavigateToSection(string sectionId)
        {
            // Construct the URL of the current page with the section ID as the fragment
            //var uri = To.ToAbsoluteUri(To.Uri);
            //uri = new UriBuilder(uri) { Fragment = sectionId }.Uri;

            // Use JSRuntime to execute JavaScript code that scrolls the page to the specified section
            await JSRuntime.InvokeAsync<object>("scrollToSection", sectionId);

            // Navigate to the updated URL
            //To.NavigateTo(uri.ToString());
        }
    }
}