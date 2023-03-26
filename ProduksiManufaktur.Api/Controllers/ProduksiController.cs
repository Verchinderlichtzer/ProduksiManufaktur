namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProduksiController : ControllerBase
    {
        private readonly IProduksiRepository _produksiRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public ProduksiController(IProduksiRepository produksiRepository)
        {
            _produksiRepository = produksiRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _produksiRepository.Get(), _options));
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
                var result = await _produksiRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("f/1/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFind1(string id)
        {
            try
            {
                var result = await _produksiRepository.Find1(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "ProduksiWrite")]
        public async Task<ActionResult<Produksi>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Produksi produksi = JsonSerializer.Deserialize<Produksi>(jsonString)!;
                var result = await _produksiRepository.Create(produksi);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "ProduksiWrite")]
        public async Task<ActionResult<Produksi>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Produksi produksi = JsonSerializer.Deserialize<Produksi>(jsonString)!;
                var result = await _produksiRepository.Find(produksi.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _produksiRepository.Update(produksi), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "ProduksiWrite")]
        public async Task<ActionResult> OnDelete(string id)
        {
            try
            {
                await _produksiRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detailbahan"), Authorize]
        public async Task<ActionResult<string>> OnGetDetailBahan()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _produksiRepository.GetDetailBahan(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detailbahan/{produksiId}"), Authorize]
        public async Task<ActionResult<string>> OnFindDetailBahan(string produksiId)
        {
            try
            {
                var result = await _produksiRepository.FindDetailBahan(produksiId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detailjasa"), Authorize]
        public async Task<ActionResult<string>> OnGetDetailJasa()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _produksiRepository.GetDetailJasa(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detailjasa/{produksiId}"), Authorize]
        public async Task<ActionResult<string>> OnFindDetailJasa(string produksiId)
        {
            try
            {
                var result = await _produksiRepository.FindDetailJasa(produksiId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detailoverhead"), Authorize]
        public async Task<ActionResult<string>> OnGetDetailOverhead()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _produksiRepository.GetDetailOverhead(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detailoverhead/{produksiId}"), Authorize]
        public async Task<ActionResult<string>> OnFindDetailOverhead(string produksiId)
        {
            try
            {
                var result = await _produksiRepository.FindDetailOverhead(produksiId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detail/refresh/{produksiId?}"), Authorize]
        public async Task<ActionResult<string>> OnRefreshDetail([FromBody] string jsonString, string? produksiId)
        {
            try
            {
                object[] objek = JsonSerializer.Deserialize<object[]>(jsonString)!;
                var result = await _produksiRepository.RefreshDetail(produksiId!, (List<string>)objek[0], (List<string>)objek[1], (List<int>)objek[2]);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}