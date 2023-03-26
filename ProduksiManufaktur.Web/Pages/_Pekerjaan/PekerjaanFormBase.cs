using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Pekerjaan
{
    public class PekerjaanFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public Pekerjaan Pekerjaan { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IPekerjaanService PekerjaanService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected PekerjaanFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected Pekerjaan? result = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Pekerjaan";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            Pekerjaan pekerjaan = new();
            Pekerjaan.CopyPropertiesTo(pekerjaan);
            validator = new(pekerjaan, PekerjaanService);

            if (!Baru)
            {
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {Pekerjaan.Id} - {Pekerjaan.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                Pekerjaan = await PekerjaanService.Find(Pekerjaan.Id);
                if (Pekerjaan is null)
                {
                    Snackbar.Add("Pekerjaan telah dihapus", MudBlazor.Severity.Error);
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
                    result = await PekerjaanService.Create(Pekerjaan);
                else
                    result = await PekerjaanService.Update(Pekerjaan);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class PekerjaanFluentValidator : AbstractValidator<Pekerjaan>
        {
            private readonly Pekerjaan _pekerjaan;
            private readonly IPekerjaanService _pekerjaanService;

            public PekerjaanFluentValidator(Pekerjaan pekerjaan, IPekerjaanService pekerjaanService)
            {
                _pekerjaan = pekerjaan;
                _pekerjaanService = pekerjaanService;

                RuleFor(x => x.Nama)
                    .NotEmpty().WithMessage("Nama tidak boleh kosong")
                    .MustAsync(UniqueNama).WithMessage("Nama sudah terdaftar");
            }

            private async Task<bool> UniqueNama(string nama, CancellationToken token)
            {
                return !(await _pekerjaanService.Get1()).Any(x => x.Nama == nama) || nama == _pekerjaan.Nama;
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Pekerjaan>.CreateWithOptions((Pekerjaan)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}