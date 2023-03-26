using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace ProduksiManufaktur.Web.Pages._Lainnya
{
    [Authorize(Roles = "Admin")]
    public class ProfilFormBase : ComponentBase
    {
        [Inject]
        protected IProfilService ProfilService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudForm? form = new();
        protected Profil profil = null!;
        protected bool popupTerbuka;

        protected override async Task OnInitializedAsync()
        {
            profil = await ProfilService.Get();
        }

        protected async Task UploadFiles(IBrowserFile file)
        {
            if (file.ContentType != "image/png" && file.ContentType != "image/jpg" && file.ContentType != "image/jpeg")
            {
                Snackbar.Add("Ekstensi file tidak diperbolehkan", Severity.Error);
                return;
            }
            else if (file.Size > 524288)
            {
                Snackbar.Add("Ukuran file terlalu besar", Severity.Error);
                return;
            }
            await using MemoryStream fs = new();
            await file.OpenReadStream(524288).CopyToAsync(fs);
            profil.Logo = fs.ToArray();
        }

        protected async Task Save()
        {
            await form!.Validate();
            if (form!.IsValid)
            {
                var result = await ProfilService.Update(profil);
                if (result is null)
                    Snackbar.Add("Profil gagal diubah", Severity.Error);
                else
                    Snackbar.Add("Profil berhasil diubah", Severity.Success);
            }
        }
    }
}