using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProduksiManufaktur.Web.Pages.Account.Validator;
using System.ComponentModel.DataAnnotations;

namespace ProduksiManufaktur.Web.Pages.Account
{
    public class RegistrasiModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public RegistrasiModel(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
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

            [DataType(DataType.Password)]
            [Display(Name = "Konfirmasi password")]
            [Compare("Password", ErrorMessage = "Konfirmasi password harus sama dengan password")]
            public string KonfirmasiPassword { get; set; } = string.Empty;

            public string Alamat { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;

            [Display(Name = "Tempat Lahir")]
            public string TempatLahir { get; set; } = string.Empty;

            [DataType(DataType.Date)]
            [Display(Name = "Tanggal Lahir")]
            public DateTime TanggalLahir { get; set; } = DateTime.Now.Date;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //model = Input;
                var user = new User
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Alamat = Input.Alamat,
                    PhoneNumber = Input.PhoneNumber,
                    TempatLahir = Input.TempatLahir,
                    TanggalLahir = Input.TanggalLahir
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    string confirmationLink = Url.Page("konfirmasiemail", null, new { userId = user.Id, token }, Request.Scheme) ?? string.Empty;
                    string body = $"""
                        <div style="height: 200px">
                            <p style="font-family: 'Segoe UI'; font-size: 24px; color: #333">Klik tombol berikut untuk mengonfirmasi email anda.</p>
                            <a style="font-family: 'Trebuchet MS'; background-color: #00695C; color: white; text-decoration: none; padding: 10px; border-radius: 5px;" href="{confirmationLink}">Konfirmasi</a>
                        </div>
                        """;
                    await _accountService.KirimLinkKonfirmasi(new() { To = Input.Email, Body = body });

                    TempData["Title"] = "Registrasi Berhasil";
                    TempData["Pesan"] = "Sebelum login, konfirmasikan email anda terlebih dahulu dengan mengklik link konfirmasi yang telah kami kirim ke email anda.";
                    return RedirectToPage("pemberitahuan");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            return Page();
        }
    }
}