using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProduksiManufaktur.Web.Pages.Account.Validator;
using System.ComponentModel.DataAnnotations;

namespace ProduksiManufaktur.Web.Pages.Account
{
    public class GantiPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        public GantiPasswordModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public class InputModel
        {
            //public string Id { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Password wajib diisi")]
            [Display(Name = "Password Baru")]
            [RequireDigit(ErrorMessage = "Password harus mengandung angka dan huruf besar")]
            [MinLength(6, ErrorMessage = "Panjang password minimal 6 karakter")]
            public string PasswordLama { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Password baru wajib diisi")]
            [Display(Name = "Password baru")]
            [RequireDigit(ErrorMessage = "Password harus mengandung angka dan huruf besar")]
            [MinLength(6, ErrorMessage = "Panjang password minimal 6 karakter")]
            public string PasswordBaru { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("PasswordBaru", ErrorMessage = "Konfirmasi password harus sama dengan password")]
            [Display(Name = "Konfirmasi password baru")]
            public string KonfirmasiPasswordBaru { get; set; } = string.Empty;
        }

        public IActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            //Input.Id = id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, Input.PasswordLama, Input.PasswordBaru);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user);
                TempData["Title"] = "Password Berhasil Diubah";
                TempData["Pesan"] = $"""Klik <a href="{Url.Content("~/")}">disini</a> untuk kembali ke home""";
                return RedirectToPage("pemberitahuan");
            }
            return Page();
        }
    }
}