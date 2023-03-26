namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PembelianController : ControllerBase
    {
        private readonly IPembelianRepository _pembelianRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public PembelianController(IPembelianRepository pembelianRepository)
        {
            _pembelianRepository = pembelianRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _pembelianRepository.Get(), _options));
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
                return Ok(JsonSerializer.Serialize(await _pembelianRepository.Get1(), _options));
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
                var result = await _pembelianRepository.Find(id);
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
                var result = await _pembelianRepository.Find1(id);
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
                var result = await _pembelianRepository.Find2(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<Pembelian>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Pembelian pembelian = JsonSerializer.Deserialize<Pembelian>(jsonString)!;
                var result = await _pembelianRepository.Create(pembelian);
                return CreatedAtAction(nameof(CreatedPembelian), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("created/{id}"), Authorize]
        public async Task<ActionResult<Pembelian>> CreatedPembelian(string id)
        {
            return await _pembelianRepository.CreatedPembelian(id);
        }

        [HttpPut, Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<Pembelian>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Pembelian pembelian = JsonSerializer.Deserialize<Pembelian>(jsonString)!;
                var result = await _pembelianRepository.Find(pembelian.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _pembelianRepository.Update(pembelian), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("deletable/{id}"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<bool>> Deletable(string id)
        {
            try
            {
                var result = await _pembelianRepository.Deletable(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult> OnDelete(string id)
        {
            try
            {
                await _pembelianRepository.Delete(id);
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
                return Ok(JsonSerializer.Serialize(await _pembelianRepository.GetDetail(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detail/{pembelianId}"), Authorize]
        public async Task<ActionResult<string>> OnFindDetail(string pembelianId)
        {
            try
            {
                var result = await _pembelianRepository.FindDetail(pembelianId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("detail/refresh/{id?}"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<Pembelian>> OnRefreshDetail([FromBody] string jsonString, string? id)
        {
            try
            {
                List<int> bahanSatuanIds = JsonSerializer.Deserialize<List<int>>(jsonString)!;
                var result = await _pembelianRepository.RefreshDetail(id!, bahanSatuanIds);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("detail/deletable/{pembelianId}/{bahanSatuanId}"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<bool>> DeletableDetail(string pembelianId, int bahanSatuanId)
        {
            try
            {
                var result = await _pembelianRepository.DeletableDetail(pembelianId, bahanSatuanId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("transaksi"), Authorize]
        public async Task<ActionResult<string>> OnGetTransaksi()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _pembelianRepository.GetTransaksi(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("transaksi/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFindTransaksi(int id)
        {
            try
            {
                var result = await _pembelianRepository.FindTransaksi(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("transaksi"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<TransaksiPembelian>> OnPostTransaksi([FromBody] string jsonString)
        {
            try
            {
                TransaksiPembelian transaksiPembelian = JsonSerializer.Deserialize<TransaksiPembelian>(jsonString)!;
                var result = await _pembelianRepository.CreateTransaksi(transaksiPembelian);
                return CreatedAtAction(nameof(OnFindTransaksi), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("transaksi/created/{id}"), Authorize]
        public async Task<ActionResult<TransaksiPembelian>> CreatedTransaksi(int id)
        {
            return await _pembelianRepository.CreatedTransaksi(id);
        }

        [HttpPut("transaksi"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<TransaksiPembelian>> OnPutTransaksi([FromBody] string jsonString)
        {
            try
            {
                TransaksiPembelian transaksiPembelian = JsonSerializer.Deserialize<TransaksiPembelian>(jsonString)!;
                var result = await _pembelianRepository.FindTransaksi(transaksiPembelian.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _pembelianRepository.UpdateTransaksi(transaksiPembelian), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("transaksi/{id}"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult> OnDeleteTransaksi(int id)
        {
            try
            {
                await _pembelianRepository.DeleteTransaksi(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("retur"), Authorize]
        public async Task<ActionResult<string>> OnGetRetur()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _pembelianRepository.GetRetur(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("retur/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFindRetur(string id)
        {
            try
            {
                var result = await _pembelianRepository.FindRetur(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("retur/f/1/{id}"), Authorize]
        public async Task<ActionResult<string>> OnFindRetur1(string id)
        {
            try
            {
                var result = await _pembelianRepository.FindRetur1(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("retur"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<ReturPembelian>> OnPostRetur([FromBody] string jsonString)
        {
            try
            {
                ReturPembelian returPembelian = JsonSerializer.Deserialize<ReturPembelian>(jsonString)!;
                var result = await _pembelianRepository.CreateRetur(returPembelian);
                return CreatedAtAction(nameof(OnFindRetur), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("retur/created/{id}"), Authorize]
        public async Task<ActionResult<ReturPembelian>> CreatedRetur(string id)
        {
            return await _pembelianRepository.CreatedRetur(id);
        }

        [HttpPut("retur"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult<ReturPembelian>> OnPutRetur([FromBody] string jsonString)
        {
            try
            {
                ReturPembelian returPembelian = JsonSerializer.Deserialize<ReturPembelian>(jsonString)!;
                var result = await _pembelianRepository.FindRetur(returPembelian.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _pembelianRepository.UpdateRetur(returPembelian), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("retur/{id}"), Authorize(Policy = "PembelianWrite")]
        public async Task<ActionResult> OnDeleteRetur(string id)
        {
            try
            {
                await _pembelianRepository.DeleteRetur(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("retur/detail"), Authorize]
        public async Task<ActionResult<string>> OnGetReturDetail()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _pembelianRepository.GetReturDetail(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("retur/detail/{returId}"), Authorize]
        public async Task<ActionResult<string>> OnRefreshReturDetail(string returId)
        {
            try
            {
                var result = await _pembelianRepository.RefreshReturDetail(returId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}