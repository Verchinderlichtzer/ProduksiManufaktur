namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PenjualanController : ControllerBase
    {
        private readonly IPenjualanRepository _penjualanRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public PenjualanController(IPenjualanRepository penjualanRepository)
        {
            _penjualanRepository = penjualanRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<string>> OnGet()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _penjualanRepository.Get(), _options));
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
                return Ok(JsonSerializer.Serialize(await _penjualanRepository.Get1(), _options));
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
                var result = await _penjualanRepository.Find(id);
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
                var result = await _penjualanRepository.Find1(id);
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
                var result = await _penjualanRepository.Find2(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<Penjualan>> OnPost([FromBody] string jsonString)
        {
            try
            {
                Penjualan penjualan = JsonSerializer.Deserialize<Penjualan>(jsonString)!;
                var result = await _penjualanRepository.Create(penjualan);
                return CreatedAtAction(nameof(CreatedPenjualan), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("created/{id}"), Authorize]
        public async Task<ActionResult<Penjualan>> CreatedPenjualan(string id)
        {
            return await _penjualanRepository.CreatedPenjualan(id);
        }

        [HttpPut, Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<Penjualan>> OnPut([FromBody] string jsonString)
        {
            try
            {
                Penjualan penjualan = JsonSerializer.Deserialize<Penjualan>(jsonString)!;
                var result = await _penjualanRepository.Find(penjualan.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _penjualanRepository.Update(penjualan), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("deletable/{id}"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<bool>> Deletable(string id)
        {
            try
            {
                var result = await _penjualanRepository.Deletable(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult> OnDelete(string id)
        {
            try
            {
                await _penjualanRepository.Delete(id);
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
                return Ok(JsonSerializer.Serialize(await _penjualanRepository.GetDetail(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("detail/{penjualanId}"), Authorize]
        public async Task<ActionResult<string>> OnFindDetail(string penjualanId)
        {
            try
            {
                var result = await _penjualanRepository.FindDetail(penjualanId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("detail/refresh/{id?}"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<Penjualan>> OnRefreshDetail([FromBody] string jsonString, string? id)
        {
            try
            {
                List<int> barangSatuanIds = JsonSerializer.Deserialize<List<int>>(jsonString)!;
                var result = await _penjualanRepository.RefreshDetail(id!, barangSatuanIds);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("detail/deletable/{penjualanId}/{barangSatuanId}"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<bool>> DeletableDetail(string penjualanId, int barangSatuanId)
        {
            try
            {
                var result = await _penjualanRepository.DeletableDetail(penjualanId, barangSatuanId);
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
                return Ok(JsonSerializer.Serialize(await _penjualanRepository.GetTransaksi(), _options));
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
                var result = await _penjualanRepository.FindTransaksi(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("transaksi"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<TransaksiPenjualan>> OnPostTransaksi([FromBody] string jsonString)
        {
            try
            {
                TransaksiPenjualan transaksiPenjualan = JsonSerializer.Deserialize<TransaksiPenjualan>(jsonString)!;
                var result = await _penjualanRepository.CreateTransaksi(transaksiPenjualan);
                return CreatedAtAction(nameof(OnFindTransaksi), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("transaksi/created/{id}"), Authorize]
        public async Task<ActionResult<TransaksiPenjualan>> CreatedTransaksi(int id)
        {
            return await _penjualanRepository.CreatedTransaksi(id);
        }

        [HttpPut("transaksi"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<TransaksiPenjualan>> OnPutTransaksi([FromBody] string jsonString)
        {
            try
            {
                TransaksiPenjualan transaksiPenjualan = JsonSerializer.Deserialize<TransaksiPenjualan>(jsonString)!;
                var result = await _penjualanRepository.FindTransaksi(transaksiPenjualan.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _penjualanRepository.UpdateTransaksi(transaksiPenjualan), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("transaksi/{id}"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult> OnDeleteTransaksi(int id)
        {
            try
            {
                await _penjualanRepository.DeleteTransaksi(id);
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
                return Ok(JsonSerializer.Serialize(await _penjualanRepository.GetRetur(), _options));
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
                var result = await _penjualanRepository.FindRetur(id);
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
                var result = await _penjualanRepository.FindRetur1(id);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("retur"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<ReturPenjualan>> OnPostRetur([FromBody] string jsonString)
        {
            try
            {
                ReturPenjualan returPenjualan = JsonSerializer.Deserialize<ReturPenjualan>(jsonString)!;
                var result = await _penjualanRepository.CreateRetur(returPenjualan);
                return CreatedAtAction(nameof(OnFindRetur), new { id = result.Id }, JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("retur/created/{id}"), Authorize]
        public async Task<ActionResult<ReturPenjualan>> CreatedRetur(string id)
        {
            return await _penjualanRepository.CreatedRetur(id);
        }

        [HttpPut("retur"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult<ReturPenjualan>> OnPutRetur([FromBody] string jsonString)
        {
            try
            {
                ReturPenjualan returPenjualan = JsonSerializer.Deserialize<ReturPenjualan>(jsonString)!;
                var result = await _penjualanRepository.FindRetur(returPenjualan.Id);
                return result is null ? NotFound() : Ok(JsonSerializer.Serialize(await _penjualanRepository.UpdateRetur(returPenjualan), _options));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("retur/{id}"), Authorize(Policy = "PenjualanWrite")]
        public async Task<ActionResult> OnDeleteRetur(string id)
        {
            try
            {
                await _penjualanRepository.DeleteRetur(id);
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
                return Ok(JsonSerializer.Serialize(await _penjualanRepository.GetReturDetail(), _options));
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
                var result = await _penjualanRepository.RefreshReturDetail(returId);
                return Ok(JsonSerializer.Serialize(result, _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}