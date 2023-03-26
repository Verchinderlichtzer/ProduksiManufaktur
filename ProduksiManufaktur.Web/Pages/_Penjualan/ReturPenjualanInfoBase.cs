using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Penjualan
{
    [Authorize(Policy = "PenjualanRead")]
    public class ReturPenjualanInfoBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPenjualanService PenjualanService { get; set; } = null!;

        protected ReturPenjualan returPenjualan = null!;

        protected bool loaded;
        protected string judul = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Penjualan", "/penjualan"),
                new BreadcrumbItem("Retur", "/penjualan/retur"),
                new BreadcrumbItem("Info", $"/penjualan/retur/info/{Id}")
            };
            Layout.Refresh();

            returPenjualan = await PenjualanService.FindRetur1(Id);
            judul = $"Info Retur Penjualan {Id}";

            loaded = true;
        }
    }
}