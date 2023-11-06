namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class BarangController : ControllerBase
    {
        private readonly IBarangRepository _barangRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public BarangController(IBarangRepository barangRepository)
        {
            _barangRepository = barangRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _barangRepository.Get(), _options));
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
                var result = await _barangRepository.Find(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<Barang>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Barang barang = JsonSerializer.Deserialize<Barang>(jsonString)!;
                var result = await _barangRepository.Create(barang);
                return CreatedAtAction(nameof(OnFind), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut, Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<Barang>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Barang barang = JsonSerializer.Deserialize<Barang>(jsonString)!;
                var result = await _barangRepository.Find(barang.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _barangRepository.Update(barang), _options));
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
                var result = await _barangRepository.Deletable(id);
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
                await _barangRepository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("barangsatuan"), Authorize]
        public async Task<ActionResult<string>> OnGetBarangSatuan()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _barangRepository.GetBarangSatuan(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("barangsatuan/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFindBarangSatuan(string id)
        {
            try
            {
                var result = await _barangRepository.FindBarangSatuan(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("barangsatuan/deletable/{id}"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<bool>> DeletableBarangSatuan(int id)
        {
            try
            {
                var result = await _barangRepository.DeletableBarangSatuan(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("cekstok"), Authorize(Policy = "ProdukRead")]
        public async Task<ActionResult<bool>> CekStokBarang()
        {
            try
            {
                var result = await _barangRepository.CekStokBarang();
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
                return Ok(JsonSerializer.Serialize(await _barangRepository.GetPerubahanStok(), _options));
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
                var result = await _barangRepository.FindPerubahanStok(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("perubahanstok"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<PerubahanStokBarang>> OnPostPerubahanStok([FromBody] string jsonString)
        {
            try
            {
                PerubahanStokBarang perubahanStokBarang = JsonSerializer.Deserialize<PerubahanStokBarang>(jsonString)!;
                var result = await _barangRepository.CreatePerubahanStok(perubahanStokBarang);
                return CreatedAtAction(nameof(OnFindPerubahanStok), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("perubahanstok"), Authorize(Policy = "ProdukWrite")]
        public async Task<ActionResult<PerubahanStokBarang>> OnPutPerubahanStok([FromBody] string jsonString)
        {
            try
            {
                PerubahanStokBarang perubahanStokBarang = JsonSerializer.Deserialize<PerubahanStokBarang>(jsonString)!;
                var result = await _barangRepository.FindPerubahanStok(perubahanStokBarang.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _barangRepository.UpdatePerubahanStok(perubahanStokBarang), _options));
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
                var result = await _barangRepository.DeletablePerubahanStok(id);
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
                await _barangRepository.DeletePerubahanStok(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}