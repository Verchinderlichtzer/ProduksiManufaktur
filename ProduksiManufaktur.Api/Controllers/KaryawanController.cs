namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class KaryawanController : ControllerBase
    {
        private readonly IKaryawanRepository _karyawanRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public KaryawanController(IKaryawanRepository karyawanRepository)
        {
            _karyawanRepository = karyawanRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _karyawanRepository.Get(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("g/1"), Authorize]
        public async Task<ActionResult<string>> OnGet1()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _karyawanRepository.Get1(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("g/2"), Authorize]
        public async Task<ActionResult<string>> OnGet2()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _karyawanRepository.Get2(), _options));
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
                var result = await _karyawanRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "PekerjaWrite")]
        public async Task<ActionResult<Karyawan>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Karyawan karyawan = JsonSerializer.Deserialize<Karyawan>(jsonString)!;
                var result = await _karyawanRepository.Create(karyawan);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "PekerjaWrite")]
        public async Task<ActionResult<Karyawan>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Karyawan karyawan = JsonSerializer.Deserialize<Karyawan>(jsonString)!;
                var result = await _karyawanRepository.Find(karyawan.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _karyawanRepository.Update(karyawan), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("deletable/{id}"), Authorize(Policy = "PekerjaWrite")]
        public async Task<ActionResult<bool>> Deletable(string id)
        {
            try
            {
                var result = await _karyawanRepository.Deletable(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "PekerjaWrite")]
        public async Task<ActionResult> OnDelete(string id)
        {
            try
            {
                await _karyawanRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}