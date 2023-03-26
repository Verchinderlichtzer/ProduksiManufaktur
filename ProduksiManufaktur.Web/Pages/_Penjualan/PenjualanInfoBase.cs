using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Penjualan
{
    [Authorize(Policy = "PenjualanRead")]
    public class PenjualanInfoBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IPenjualanService PenjualanService { get; set; } = null!;

        protected Penjualan penjualan = null!;

        protected bool loaded;
        protected string judul = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Penjualan", "/penjualan"),
                new BreadcrumbItem("Info", $"/penjualan/info/{Id}")
            };
            Layout.Refresh();

            penjualan = await PenjualanService.Find2(Id);
            judul = $"Info Penjualan {Id}";

            loaded = true;
        }
    }
}