namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class OverheadController : ControllerBase
    {
        private readonly IOverheadRepository _overheadRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public OverheadController(IOverheadRepository overheadRepository)
        {
            _overheadRepository = overheadRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _overheadRepository.Get(), _options));
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
                var result = await _overheadRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound(new());
            }
        }

        [HttpPost, Authorize(Policy = "OverheadWrite")]
        public async Task<ActionResult<Overhead>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Overhead overhead = JsonSerializer.Deserialize<Overhead>(jsonString)!;
                var result = await _overheadRepository.Create(overhead);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "OverheadWrite")]
        public async Task<ActionResult<Overhead>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Overhead overhead = JsonSerializer.Deserialize<Overhead>(jsonString)!;
                var result = await _overheadRepository.Find(overhead.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _overheadRepository.Update(overhead), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}/deletable"), Authorize(Policy = "OverheadWrite")]
        public async Task<ActionResult<bool>> Deletable(int id)
        {
            try
            {
                var result = await _overheadRepository.Deletable(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "OverheadWrite")]
        public async Task<ActionResult> OnDelete(int id)
        {
            try
            {
                await _overheadRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}