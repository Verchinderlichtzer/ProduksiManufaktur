namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class BahanController : ControllerBase
    {
        private readonly IBahanRepository _bahanRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public BahanController(IBahanRepository bahanRepository)
        {
            _bahanRepository = bahanRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _bahanRepository.Get(), _options));
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
                return Ok(JsonSerializer.Serialize(await _bahanRepository.Get1(), _options));
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
                var result = await _bahanRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<Bahan>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Bahan bahan = JsonSerializer.Deserialize<Bahan>(jsonString)!;
                var result = await _bahanRepository.Create(bahan);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<Bahan>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Bahan bahan = JsonSerializer.Deserialize<Bahan>(jsonString)!;
                var result = await _bahanRepository.Find(bahan.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _bahanRepository.Update(bahan), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("deletable/{id}"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<bool>> Deletable(string id)
        {
            try
            {
                var result = await _bahanRepository.Deletable(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult> OnDelete(string id)
        {
            try
            {
                await _bahanRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("bahansatuan"), Authorize]
        public async Task<ActionResult<string>> OnGetBahanSatuan()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _bahanRepository.GetBahanSatuan(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("bahansatuan/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFindBahanSatuan(string id)
        {
            try
            {
                var result = await _bahanRepository.FindBahanSatuan(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("bahansatuan/deletable/{id}"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<bool>> DeletableBahanSatuan(int id)
        {
            try
            {
                var result = await _bahanRepository.DeletableBahanSatuan(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("cekstok"), Authorize(Policy = "ProdukRead")]
        public async Task<ActionResult<bool>> CekStokBahan()
        {
            try
            {
                var result = await _bahanRepository.CekStokBahan();
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("perubahanstok"), Authorize]
        public async Task<ActionResult<string>> OnGetPerubahanStok()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _bahanRepository.GetPerubahanStok(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("perubahanstok/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFindPerubahanStok(int id)
        {
            try
            {
                var result = await _bahanRepository.FindPerubahanStok(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("perubahanstok"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<PerubahanStokBahan>> OnPostPerubahanStok([FromBody] string jsonString)
        {
            try
            {
                PerubahanStokBahan perubahanStokBahan = JsonSerializer.Deserialize<PerubahanStokBahan>(jsonString)!;
                var result = await _bahanRepository.CreatePerubahanStok(perubahanStokBahan);
                return CreatedAtAction(nameof(OnFindPerubahanStok), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("perubahanstok"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<PerubahanStokBahan>> OnPutPerubahanStok([FromBody] string jsonString)
        {
            try
            {
                PerubahanStokBahan perubahanStokBahan = JsonSerializer.Deserialize<PerubahanStokBahan>(jsonString)!;
                var result = await _bahanRepository.FindPerubahanStok(perubahanStokBahan.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _bahanRepository.UpdatePerubahanStok(perubahanStokBahan), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("perubahanstok/deletable/{id}"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<bool>> DeletablePerubahanStok(int id)
        {
            try
            {
                var result = await _bahanRepository.DeletablePerubahanStok(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("perubahanstok/{id}"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult> OnDeletePerubahanStok(int id)
        {
            try
            {
                await _bahanRepository.DeletePerubahanStok(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}