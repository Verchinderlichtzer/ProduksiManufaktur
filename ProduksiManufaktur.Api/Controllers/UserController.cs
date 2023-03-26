namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _userRepository.Get(), _options));
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
                var result = await _userRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut, Authorize(Policy = "AkunWrite")]
        public async Task<ActionResult<User>> OnPut([FromBody] string jsonString)
        {
            try
            {
                User user = JsonSerializer.Deserialize<User>(jsonString)!;
                var result = await _userRepository.Find(user.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _userRepository.Update(user), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("deletable/{id}"), Authorize(Policy = "AkunWrite")]
        public async Task<ActionResult<bool>> Deletable(string id)
        {
            try
            {
                var result = await _userRepository.Deletable(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("claim"), Authorize]
        public async Task<ActionResult<string>> OnGetClaim()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _userRepository.GetClaim(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut("claim/{userId}"), Authorize(Policy = "AkunWrite")]
        public async Task<ActionResult<User>> OnPutsClaim(string userId, [FromBody] string jsonString)
        {
            try
            {
                List<UserClaim> userClaim = JsonSerializer.Deserialize<List<UserClaim>>(jsonString)!;
                var result = await _userRepository.Find(userId);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _userRepository.UpdatesClaim(userId, userClaim), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("log"), Authorize]
        public async Task<ActionResult<string>> OnGetLog()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _userRepository.GetLog(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("log/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFindLog(int id)
        {
            try
            {
                var result = await _userRepository.FindLog(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("log"), Authorize]
        public async Task<ActionResult<LogTransaksi>> OnPostLog([FromBody] string jsonString)
        {
            try
            {
                LogTransaksi logTransaksi = JsonSerializer.Deserialize<LogTransaksi>(jsonString)!;
                var result = await _userRepository.CreateLog(logTransaksi);
                return CreatedAtAction(nameof(OnFindLog), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("log"), Authorize(Policy = "AkunWrite")]
        public async Task<ActionResult> OnDeletesLog()
        {
            try
            {
                await _userRepository.DeletesLog();
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}