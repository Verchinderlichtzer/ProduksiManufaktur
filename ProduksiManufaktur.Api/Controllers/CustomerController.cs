namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _customerRepository.Get(), _options));
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
                return Ok(JsonSerializer.Serialize(await _customerRepository.Get1(), _options));
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
                return Ok(JsonSerializer.Serialize(await _customerRepository.Get2(), _options));
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
                var result = await _customerRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "PihakWrite")]
        public async Task<ActionResult<Customer>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Customer customer = JsonSerializer.Deserialize<Customer>(jsonString)!;
                var result = await _customerRepository.Create(customer);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "PihakWrite")]
        public async Task<ActionResult<Customer>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Customer customer = JsonSerializer.Deserialize<Customer>(jsonString)!;
                var result = await _customerRepository.Find(customer.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _customerRepository.Update(customer), _options));
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
                var result = await _customerRepository.Deletable(id);
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
                await _customerRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}