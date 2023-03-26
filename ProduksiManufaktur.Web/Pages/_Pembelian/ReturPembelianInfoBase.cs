using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Pembelian
{
    [Authorize(Policy = "PembelianRead")]
    public class ReturPembelianInfoBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPembelianService PembelianService { get; set; } = null!;

        protected ReturPembelian returPembelian = null!;

        protected bool loaded;
        protected string judul = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Pembelian", "/pembelian"),
                new BreadcrumbItem("Retur", "/pembelian/retur"),
                new BreadcrumbItem("Info", $"/pembelian/retur/info/{Id}")
            };
            Layout.Refresh();

            returPembelian = await PembelianService.FindRetur1(Id);
            judul = $"Info Retur Pembelian {Id}";

            loaded = true;
        }
    }
}