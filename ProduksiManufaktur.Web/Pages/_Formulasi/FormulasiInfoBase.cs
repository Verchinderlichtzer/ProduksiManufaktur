using Microsoft.AspNetCore.Authorization;

namespace ProduksiManufaktur.Web.Pages._Formulasi
{
    [Authorize(Policy = "ProdukRead")]
    public class FormulasiInfoBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IFormulasiService FormulasiService { get; set; } = null!;

        protected Formulasi formulasi = null!;

        protected bool loaded;
        protected string judul = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Formulasi", "/formulasi"),
                new BreadcrumbItem("Info", $"/formulasi/info/{Id}")
            };
            Layout.Refresh();

            formulasi = await FormulasiService.Find(Id);
            judul = $"Info Formulasi {Id}";

            loaded = true;
        }
    }
}