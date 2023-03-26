namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TransaksiLainController : ControllerBase
    {
        private readonly ITransaksiLainRepository _transaksiLainRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public TransaksiLainController(ITransaksiLainRepository transaksiLainRepository)
        {
            _transaksiLainRepository = transaksiLainRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _transaksiLainRepository.Get(), _options));
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
                var result = await _transaksiLainRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "TransaksiLainWrite")]
        public async Task<ActionResult<List<TransaksiLain>>> OnPosts([FromBody] string jsonString)
        {
            try
            {
                List<TransaksiLain> transaksiLain = JsonSerializer.Deserialize<List<TransaksiLain>>(jsonString)!;
                var result = await _transaksiLainRepository.Creates(transaksiLain);
                return CreatedAtAction(nameof(OnGet), JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "TransaksiLainWrite")]
        public async Task<ActionResult<TransaksiLain>> OnPut([FromBody] string jsonString)
        {
            try
            {
                TransaksiLain transaksiLain = JsonSerializer.Deserialize<TransaksiLain>(jsonString)!;
                var result = await _transaksiLainRepository.Find(transaksiLain.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _transaksiLainRepository.Update(transaksiLain), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "TransaksiLainWrite")]
        public async Task<ActionResult> OnDelete(int id)
        {
            try
            {
                await _transaksiLainRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}