using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Overhead
{
    public class OverheadFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public Overhead Overhead { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IOverheadService OverheadService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected OverheadFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected Overhead? result = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Overhead";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            Overhead overhead = new();
            Overhead.CopyPropertiesTo(overhead);
            validator = new(overhead, OverheadService);

            if (!Baru)
            {
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {Overhead.Id} - {Overhead.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                Overhead = await OverheadService.Find(Overhead.Id);
                if (Overhead is null)
                {
                    Snackbar.Add("Overhead telah dihapus", MudBlazor.Severity.Error);
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
                    result = await OverheadService.Create(Overhead);
                else
                    result = await OverheadService.Update(Overhead);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class OverheadFluentValidator : AbstractValidator<Overhead>
        {
            private readonly Overhead _overhead;
            private readonly IOverheadService _overheadService;

            public OverheadFluentValidator(Overhead overhead, IOverheadService overheadService)
            {
                _overhead = overhead;
                _overheadService = overheadService;

                RuleFor(x => x.Nama)
                    .NotEmpty().WithMessage("Nama tidak boleh kosong")
                    .MustAsync(UniqueNama).WithMessage("Nama sudah terdaftar");
            }

            private async Task<bool> UniqueNama(string nama, CancellationToken token)
            {
                return !(await _overheadService.Get()).Any(x => x.Nama == nama) || nama == _overhead.Nama;
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Overhead>.CreateWithOptions((Overhead)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}