namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProfilController : ControllerBase
    {
        private readonly IProfilRepository _profilRepository;

        public ProfilController(IProfilRepository profilRepository)
        {
            _profilRepository = profilRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _profilRepository.Get(), new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Profil>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Profil profil = JsonSerializer.Deserialize<Profil>(jsonString)!;
                var result = await _profilRepository.Get();
                return result is null ? NotFound() : Ok(await _profilRepository.Update(profil));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}