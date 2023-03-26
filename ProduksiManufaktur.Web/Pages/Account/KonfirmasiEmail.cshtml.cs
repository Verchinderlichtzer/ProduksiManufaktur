using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProduksiManufaktur.Web.Pages.Account
{
    public class KonfirmasiEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public KonfirmasiEmailModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (userId is null || token is null)
            {
                TempData["Title"] = "Konfirmasi Gagal";
                TempData["Pesan"] = "Id gagal dikonfirmasi.";
                return RedirectToPage("pemberitahuan");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                TempData["Pesan"] = $"User dengan ID {userId} tidak ditemukan";
                return RedirectToPage("pemberitahuan");
            }

            var konfirmasi = await _userManager.ConfirmEmailAsync(user, token);

            if (konfirmasi.Succeeded)
            {
                TempData["Pesan"] = $"""User {user.Email} berhasil dikonfirmasi. klik <a href="{Url.Page("login", Request.Scheme)}">disini</a> untuk login. """;
                return Page();
            }
            else
            {
                TempData["Pesan"] = "Id tidak bisa dikonfirmasi";
                return RedirectToPage("pemberitahuan");
            }
        }
    }
}