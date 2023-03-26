using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Supplier
{
    public class SupplierFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public Supplier Supplier { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected ISupplierService SupplierService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected SupplierFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected Supplier? result = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Supplier";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            Supplier supplier = new();
            Supplier.CopyPropertiesTo(supplier);
            validator = new(supplier, SupplierService);

            if (!Baru)
            {
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {Supplier.Id} - {Supplier.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                Supplier = await SupplierService.Find(Supplier.Id);
                if (Supplier is null)
                {
                    Snackbar.Add("Supplier telah dihapus", MudBlazor.Severity.Error);
                    MudDialog.Cancel();
                }
            }
            form!.ResetValidation();
            StateHasChanged();
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                if (Baru)
                    result = await SupplierService.Create(Supplier);
                else
                    result = await SupplierService.Update(Supplier);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class SupplierFluentValidator : AbstractValidator<Supplier>
        {
            private readonly ISupplierService _supplierService;
            private readonly Supplier _supplier;

            public SupplierFluentValidator(Supplier supplier, ISupplierService supplierService)
            {
                _supplierService = supplierService;
                _supplier = supplier;

                RuleFor(x => x.Nama)
                    .NotEmpty().WithMessage("Nama tidak boleh kosong");

                RuleFor(x => x.Telepon)
                    .MustAsync(UniqueTelepon!).WithMessage("Nomor telepon sudah terdaftar")
                    .When(x => !string.IsNullOrEmpty(x.Telepon));

                RuleFor(x => x.Fax)
                    .MustAsync(UniqueFax!).WithMessage("Fax sudah terdaftar")
                    .When(x => !string.IsNullOrEmpty(x.Fax));

                RuleFor(x => x.Email)
                    .EmailAddress()
                    .MustAsync(UniqueEmail).WithMessage("Id sudah terdaftar")
                    .When(x => !string.IsNullOrEmpty(x.Email));
            }

            private async Task<bool> UniqueTelepon(string telepon, CancellationToken token)
            {
                return !(await _supplierService.Get1()).Any(x => x.Telepon == telepon) || telepon == _supplier.Telepon;
            }

            private async Task<bool> UniqueFax(string fax, CancellationToken token)
            {
                return !(await _supplierService.Get1()).Any(x => x.Fax == fax) || fax == _supplier.Fax;
            }

            private async Task<bool> UniqueEmail(string email, CancellationToken token)
            {
                return !(await _supplierService.Get1()).Any(x => x.Email == email) || email == _supplier.Email;
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Supplier>.CreateWithOptions((Supplier)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}