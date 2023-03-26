using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Pembelian
{
    [Authorize(Policy = "PembelianRead")]
    public class PembelianInfoBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPembelianService PembelianService { get; set; } = null!;

        protected Pembelian pembelian = null!;

        protected bool loaded;
        protected string judul = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pembelian", "/pembelian"),
                new BreadcrumbItem("Info", $"/pembelian/info/{Id}")
            };
            Layout.Refresh();

            pembelian = await PembelianService.Find2(Id);
            judul = $"Info Pembelian {Id}";

            loaded = true;
        }
    }
}