using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._Karyawan
{
    public class KaryawanFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public Karyawan Karyawan { get; set; } = new();

        [Parameter]
        public Dictionary<int, string> Pekerjaan { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected IKaryawanService KaryawanService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected KaryawanFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected Karyawan? result = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Karyawan";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            Karyawan karyawan = new();
            Karyawan.CopyPropertiesTo(karyawan);
            validator = new(karyawan, KaryawanService);

            if (!Baru)
            {
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {Karyawan.Id} - {Karyawan.Nama}";
                warna = Color.Warning;
            }
        }

        protected async Task Refresh()
        {
            result = new();
            if (!Baru)
            {
                Karyawan = await KaryawanService.Find(Karyawan.Id);
                if (Karyawan is null)
                {
                    Snackbar.Add("Karyawan telah dihapus", MudBlazor.Severity.Error);
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
                    result = await KaryawanService.Create(Karyawan);
                else
                    result = await KaryawanService.Update(Karyawan);

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class KaryawanFluentValidator : AbstractValidator<Karyawan>
        {
            private readonly Karyawan _karyawan;
            private readonly IKaryawanService _karyawanService;

            public KaryawanFluentValidator(Karyawan karyawan, IKaryawanService karyawanService)
            {
                _karyawan = karyawan;
                _karyawanService = karyawanService;

                RuleFor(x => x.Nama)
                    .NotEmpty().WithMessage("Nama tidak boleh kosong");

                RuleFor(x => x.Telepon)
                    .MustAsync(UniqueTelepon!).WithMessage("Nomor telepon sudah terdaftar")
                    .When(x => !string.IsNullOrEmpty(x.Telepon));

                RuleFor(x => x.Email)
                    .EmailAddress()
                    .MustAsync(UniqueEmail).WithMessage("Id sudah terdaftar")
                    .When(x => !string.IsNullOrEmpty(x.Email));

                RuleFor(x => x.PekerjaanId)
                    .NotEmpty().WithMessage("Pilih pekerjaan");
            }

            private async Task<bool> UniqueTelepon(string telepon, CancellationToken token)
            {
                return !(await _karyawanService.Get1()).Any(x => x.Telepon == telepon) || telepon == _karyawan.Telepon;
            }

            private async Task<bool> UniqueEmail(string email, CancellationToken token)
            {
                return !(await _karyawanService.Get1()).Any(x => x.Email == email) || email == _karyawan.Email;
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Karyawan>.CreateWithOptions((Karyawan)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}