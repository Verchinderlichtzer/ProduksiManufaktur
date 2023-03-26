using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProduksiManufaktur.Web.Pages.Account.Validator;
using System.ComponentModel.DataAnnotations;

namespace ProduksiManufaktur.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountService _accountService;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        public string ReturnUrl { get; set; } = string.Empty;
        public bool HasEmailNotConfirmedError { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; } = null!;

        public LoginModel(UserManager<User> userManager, SignInManager<User> signInManager, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        public class InputModel
        {
            [DataType(DataType.EmailAddress)]
            [Required(ErrorMessage = "Id wajib diisi")]
            public string Email { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Password wajib diisi")]
            [RequireDigit(ErrorMessage = "Password harus mengandung angka dan satu huruf kapital")]
            [MinLength(6, ErrorMessage = "Panjang password minimal 6 karakter")]
            public string Password { get; set; } = string.Empty;

            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null!)
        {
            if (User.Identity!.IsAuthenticated) return LocalRedirect(Url.Content("~/"));
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null!)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email) ?? new();
                var cekPassword = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);
                if (cekPassword.Succeeded)
                {
                    AuthenticationProperties authenticationProperties = new()
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(999),
                        IsPersistent = true,
                        AllowRefresh = false
                    };

                    var claims = await _userManager.GetClaimsAsync(user);
                    await _signInManager.SignInWithClaimsAsync(user, authenticationProperties, claims);
                    Response.Cookies.Append("api_token", await _accountService.Login(new UserDto() { Email = Input.Email, Password = Input.Password }), new CookieOptions()
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(999),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    });
                    return LocalRedirect(returnUrl);
                }
                if (cekPassword.RequiresTwoFactor)
                {
                    return RedirectToPage("/LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                }
                if (cekPassword.IsLockedOut)
                {
                    return RedirectToPage("/Lockout");
                }

                //ModelState.AddModelError(string.Empty, "Your email has not been confirmed yet.");
                if (string.IsNullOrEmpty(user.Email))
                {
                    ViewData["Pesan"] = "Login gagal";
                }
                else if (!string.IsNullOrEmpty(user.Email) && !await _userManager.IsEmailConfirmedAsync(user))
                {
                    ViewData["Pesan"] = "Email belum dikonfirmasi";
                }
                HasEmailNotConfirmedError = true;
                return Page();
            }
            return Page();
        }
    }
}