using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Produksi
{
    [Authorize(Policy = "ProduksiRead")]
    public class ProduksiInfoBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IProduksiService ProduksiService { get; set; } = null!;

        protected Produksi produksi = null!;

        protected bool loaded;
        protected string judul = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Produksi", "/produksi"),
                new BreadcrumbItem("Info", $"/produksi/info/{Id}")
            };
            Layout.Refresh();

            produksi = await ProduksiService.Find1(Id);
            judul = $"Info Produksi {Id}";

            loaded = true;
        }
    }
}