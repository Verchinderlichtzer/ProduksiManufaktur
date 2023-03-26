namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class FormulasiController : ControllerBase
    {
        private readonly IFormulasiRepository _formulasiRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public FormulasiController(IFormulasiRepository formulasiRepository)
        {
            _formulasiRepository = formulasiRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _formulasiRepository.Get(), _options));
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
                var result = await _formulasiRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("f/1/{barangId}"), Authorize]
        public async Task<ActionResult<string>> OnFind1(string barangId)
        {
            try
            {
                var result = await _formulasiRepository.Find1(barangId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("f/2/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFind2(string id)
        {
            try
            {
                var result = await _formulasiRepository.Find2(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<Formulasi>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Formulasi formulasi = JsonSerializer.Deserialize<Formulasi>(jsonString)!;
                var result = await _formulasiRepository.Create(formulasi);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<Formulasi>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Formulasi formulasi = JsonSerializer.Deserialize<Formulasi>(jsonString)!;
                var result = await _formulasiRepository.Find(formulasi.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _formulasiRepository.Update(formulasi), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult> OnDelete(string id)
        {
            try
            {
                await _formulasiRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detail"), Authorize]
        public async Task<ActionResult<string>> OnGetDetail()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _formulasiRepository.GetDetail(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}