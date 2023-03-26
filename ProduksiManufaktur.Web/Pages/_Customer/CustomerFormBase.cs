using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Customer
{
    public class CustomerFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public Customer Customer { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected ICustomerService CustomerService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected CustomerFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected Customer? result = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Customer";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            Customer customer = new();
            Customer.CopyPropertiesTo(customer);
            validator = new(customer, CustomerService);

            if (!Baru)
            {
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {Customer.Id} - {Customer.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                Customer = await CustomerService.Find(Customer.Id);
                if (Customer is null)
                {
                    Snackbar.Add("Customer telah dihapus", MudBlazor.Severity.Error);
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
                    result = await CustomerService.Create(Customer);
                else
                    result = await CustomerService.Update(Customer);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class CustomerFluentValidator : AbstractValidator<Customer>
        {
            private readonly ICustomerService _customerService;
            private readonly Customer _customer;

            public CustomerFluentValidator(Customer customer, ICustomerService customerService)
            {
                _customerService = customerService;
                _customer = customer;

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
                return !(await _customerService.Get1()).Any(x => x.Telepon == telepon) || telepon == _customer.Telepon;
            }

            private async Task<bool> UniqueFax(string fax, CancellationToken token)
            {
                return !(await _customerService.Get1()).Any(x => x.Fax == fax) || fax == _customer.Fax;
            }

            private async Task<bool> UniqueEmail(string email, CancellationToken token)
            {
                return !(await _customerService.Get1()).Any(x => x.Email == email) || email == _customer.Email;
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Customer>.CreateWithOptions((Customer)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}