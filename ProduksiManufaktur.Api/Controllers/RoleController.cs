namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _roleRepository.Get(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("q/1"), Authorize]
        public async Task<ActionResult<string>> OnGet1()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _roleRepository.Get1(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<string>> OnFind(string id)
        {
            try
            {
                var result = await _roleRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut, Authorize(Policy = "AkunWrite")]
        public async Task<ActionResult<Role>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Role role = JsonSerializer.Deserialize<Role>(jsonString)!;
                var result = await _roleRepository.Find(role.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _roleRepository.Update(role), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("claim"), Authorize]
        public async Task<ActionResult<string>> OnGetClaim()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _roleRepository.GetClaim(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}