namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PekerjaanController : ControllerBase
    {
        private readonly IPekerjaanRepository _pekerjaanRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public PekerjaanController(IPekerjaanRepository pekerjaanRepository)
        {
            _pekerjaanRepository = pekerjaanRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _pekerjaanRepository.Get(), _options));
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
                return Ok(JsonSerializer.Serialize(await _pekerjaanRepository.Get1(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<string>> OnFind(int id)
        {
            try
            {
                var result = await _pekerjaanRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/q/1"), Authorize]
        public async Task<ActionResult<string>> OnFind1(int id)
        {
            try
            {
                var result = await _pekerjaanRepository.Find1(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "PekerjaWrite")]
        public async Task<ActionResult<Pekerjaan>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Pekerjaan pekerjaan = JsonSerializer.Deserialize<Pekerjaan>(jsonString)!;
                var result = await _pekerjaanRepository.Create(pekerjaan);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "PekerjaWrite")]
        public async Task<ActionResult<Pekerjaan>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Pekerjaan pekerjaan = JsonSerializer.Deserialize<Pekerjaan>(jsonString)!;
                var result = await _pekerjaanRepository.Find(pekerjaan.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _pekerjaanRepository.Update(pekerjaan), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}/deletable"), Authorize(Policy = "PekerjaWrite")]
        public async Task<ActionResult<bool>> Deletable(int id)
        {
            try
            {
                var result = await _pekerjaanRepository.Deletable(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "PekerjaWrite")]
        public async Task<ActionResult> OnDelete(int id)
        {
            try
            {
                await _pekerjaanRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}