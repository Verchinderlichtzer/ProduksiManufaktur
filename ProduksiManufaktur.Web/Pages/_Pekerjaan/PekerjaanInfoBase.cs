using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Pekerjaan
{
    [Authorize(Policy = "ProdukRead")]
    public class PekerjaanInfoBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPekerjaanService PekerjaanService { get; set; } = null!;

        protected Pekerjaan pekerjaan = null!;

        protected bool loaded;

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pekerjaan", "/pekerjaan"),
                new BreadcrumbItem("Info", $"/pekerjaan/info/{Id}")
            };
            Layout.Refresh();

            pekerjaan = await PekerjaanService.Find1(Id);

            loaded = true;
        }
    }
}