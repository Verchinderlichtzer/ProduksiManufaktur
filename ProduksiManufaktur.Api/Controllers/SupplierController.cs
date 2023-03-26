namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _supplierRepository.Get(), _options));
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
                return Ok(JsonSerializer.Serialize(await _supplierRepository.Get1(), _options));
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
                return Ok(JsonSerializer.Serialize(await _supplierRepository.Get2(), _options));
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
                var result = await _supplierRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "PihakWrite")]
        public async Task<ActionResult<Supplier>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Supplier supplier = JsonSerializer.Deserialize<Supplier>(jsonString)!;
                var result = await _supplierRepository.Create(supplier);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "PihakWrite")]
        public async Task<ActionResult<Supplier>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Supplier supplier = JsonSerializer.Deserialize<Supplier>(jsonString)!;
                var result = await _supplierRepository.Find(supplier.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _supplierRepository.Update(supplier), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("deletable/{id}"), Authorize(Policy = "PihakWrite")]
        public async Task<ActionResult<bool>> Deletable(string id)
        {
            try
            {
                var result = await _supplierRepository.Deletable(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "PihakWrite")]
        public async Task<ActionResult> OnDelete(string id)
        {
            try
            {
                await _supplierRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}