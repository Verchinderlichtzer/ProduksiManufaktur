using FluentValidation;

namespace ProduksiManufaktur.Web.Pages._TransaksiLain
{
    public class TransaksiLainFormBase : ComponentBase
    {
        [Parameter]
        public bool Baru { get; set; }

        [Parameter]
        public List<TransaksiLain> ListTransaksiLain { get; set; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        protected ITransaksiLainService TransaksiLainService { get; set; } = null!;

        protected TransaksiLainFluentValidator validator = null!;
        protected MudForm? form = new();

        protected bool popupTerbuka;
        protected List<TransaksiLain>? result = new();
        protected TransaksiLain transaksiLainEdit = new();
        protected string icon = Icons.Material.Filled.Add;
        protected string judul = "Tambah Transaksi";
        protected string classCreate = "border-dashed border-2 mud-border-inherit pa-3";
        protected Color warna = Color.Success;

        protected override void OnInitialized()
        {
            validator = new();
            if (!Baru)
            {
                transaksiLainEdit = ListTransaksiLain.Single();
                icon = Icons.Material.Filled.Edit;
                judul = $"Edit {transaksiLainEdit.Id} - {transaksiLainEdit.Tanggal:dd/MM/yyyy}";
                warna = Color.Warning;
            }
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                if (Baru)
                    result = await TransaksiLainService.Creates(ListTransaksiLain);
                else
                    result = new List<TransaksiLain>() { await TransaksiLainService.Update(transaksiLainEdit) };

                if (result is not null) MudDialog.Close(DialogResult.Ok(result));
            }
        }

        public class TransaksiLainFluentValidator : AbstractValidator<TransaksiLain>
        {
            public TransaksiLainFluentValidator()
            {
                RuleFor(x => x.Jenis)
                    .NotEmpty().WithMessage("Jenis tidak boleh kosong");

                RuleFor(x => x.Kategori)
                    .NotEmpty().WithMessage("Kategori tidak boleh kosong");

                RuleFor(x => x.Nominal)
                    .NotEmpty().WithMessage("Nominal tidak boleh kosong");

                RuleFor(x => x.Keterangan)
                    .NotEmpty().WithMessage("Keterangan tidak boleh kosong");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<TransaksiLain>.CreateWithOptions((TransaksiLain)model, x => x.IncludeProperties(propertyName)));
                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}