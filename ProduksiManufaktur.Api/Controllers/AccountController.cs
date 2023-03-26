using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountController(IAccountRepository accountRepository, IConfiguration config, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _config = config;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] UserDto userDto)
        {
            var user = await Authenticate(userDto);

            if (user is not null)
            {
                var token = await GenerateToken(user);
                return Ok(token);
            }
            return NotFound("User tidak ditemukan");
        }

        private async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var userRole = (await _roleRepository.Get()).SelectMany(x => x.UserRole!).Where(x => x.UserId == user.Id).Select(x => x.Role);
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            foreach (var x in userRole) claims.Add(new Claim(ClaimTypes.Role, x!.Name!));
            foreach (var x in (await _roleRepository.GetClaim1(user.UserRole!.ConvertAll(x => x.RoleId))))
                claims.Add(new Claim(x.ClaimType!, x.ClaimValue!));
            foreach (var x in (await _userRepository.GetClaim()).Where(x => x.UserId == user.Id))
                claims.Add(new Claim(x.ClaimType!, x.ClaimValue!));

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTimeOffset.UtcNow.AddDays(999).DateTime, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> Authenticate(UserDto userDto)
        {
            User user = await _userRepository.Find1(userDto.Email);

            var passwordHash = new PasswordHasher<User>();

            if (user is not null && passwordHash.VerifyHashedPassword(user, user.PasswordHash!, userDto.Password) != PasswordVerificationResult.Failed)
            {
                return user;
            }
            return null!;
        }

        [HttpPost("kirim-email"), AllowAnonymous]
        public async Task<ActionResult> KirimLinkKonfirmasi(EmailDto emailDto)
        {
            await _accountRepository.KirimEmail(emailDto);

            return Ok();
        }
    }
}