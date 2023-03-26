namespace ProduksiManufaktur.Web.Services
{
    public interface IAccountService
    {
        Task<string> Login(UserDto userDto);

        Task KirimLinkKonfirmasi(EmailDto emailDto);
    }

    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Login(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/Login", userDto);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task KirimLinkKonfirmasi(EmailDto emailDto)
        {
            await _httpClient.PostAsJsonAsync("api/account/kirim-email", emailDto);
        }
    }
}