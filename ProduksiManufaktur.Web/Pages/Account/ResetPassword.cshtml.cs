using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProduksiManufaktur.Web.Pages.Account.Validator;
using System.ComponentModel.DataAnnotations;

namespace ProduksiManufaktur.Web.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        public ResetPasswordModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Id wajib diisi")]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password wajib diisi")]
            [Display(Name = "Password Baru")]
            [RequireDigit(ErrorMessage = "Password harus mengandung angka dan huruf besar")]
            [MinLength(6, ErrorMessage = "Panjang password minimal 6 karakter")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Konfirmasi password")]
            [Compare("Password", ErrorMessage = "Konfirmasi password harus sama dengan password")]
            [RequireDigit(ErrorMessage = "Password harus mengandung angka dan huruf besar")]
            [MinLength(6, ErrorMessage = "Panjang password minimal 6 karakter")]
            public string KonfirmasiPassword { get; set; } = string.Empty;

            public string Token { get; set; } = string.Empty;
        }

        public IActionResult OnGet(string token, string email)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            Input.Token = token;
            Input.Email = email;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, Input.Token, Input.Password);
                    if (result.Succeeded)
                    {
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        await _signInManager.SignOutAsync();
                        Response.Cookies.Delete("api_token");

                        TempData["Title"] = "Password Berhasil Direset";
                        TempData["Pesan"] = $"""Silahkan <a href="{Url.Page("login", Request.Scheme)}">login</a> dengan password baru anda""";
                        return RedirectToPage("pemberitahuan");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return Page();
        }
    }
}