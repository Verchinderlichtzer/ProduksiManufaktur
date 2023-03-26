using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ProduksiManufaktur.Web.Pages.Account
{
    public class LupaPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IAccountService _accountService;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public LupaPasswordModel(UserManager<User> userManager, IAccountService accountService)
        {
            _userManager = userManager;
            _accountService = accountService;
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Id wajib diisi")]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user is not null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    string passwordResetLink = Url.Page("resetpassword", null, new { email = Input.Email, token }, Request.Scheme) ?? string.Empty;
                    string body = $"""
                        <div style="height: 200px">
                            <p style="font-family: 'Segoe UI'; font-size: 24px; color: #333">Klik tombol berikut untuk mengganti password anda.</p>
                            <a style="font-family: 'Trebuchet MS'; background-color: #850054; color: white; text-decoration: none; padding: 10px; border-radius: 5px;" href="{passwordResetLink}">Reset Password</a>
                        </div>
                        """;
                    await _accountService.KirimLinkKonfirmasi(new() { To = Input.Email, Body = body });

                    TempData["Title"] = "Lupa Password";
                    TempData["Pesan"] = "Klik link yang telah kami berikan melalui email anda";
                    return RedirectToPage("pemberitahuan");
                }

                return RedirectToPage("pemberitahuan");
            }

            return Page();
        }
    }
}