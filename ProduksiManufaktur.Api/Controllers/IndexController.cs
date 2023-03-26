namespace ProduksiManufaktur.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class IndexController : ControllerBase
    {
        private readonly IIndexRepository _indexRepository;
        private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        public IndexController(IIndexRepository indexRepository)
        {
            _indexRepository = indexRepository;
        }

        [HttpGet("jumlahpakai"), Authorize]
        public async Task<ActionResult<string>> OnGetJumlahPakai()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetJumlahPakai(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("jumlahbeli"), Authorize]
        public async Task<ActionResult<string>> OnGetJumlahBeli()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetJumlahBeli(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("jumlahproduksi"), Authorize]
        public async Task<ActionResult<string>> OnGetJumlahProduksi()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetJumlahProduksi(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("jumlahjual"), Authorize]
        public async Task<ActionResult<string>> OnGetJumlahJual()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetJumlahJual(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("pengeluaran"), Authorize]
        public async Task<ActionResult<string>> OnGetPengeluaran()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetPengeluaran(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("pendapatan"), Authorize]
        public async Task<ActionResult<string>> OnGetPendapatan()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetPendapatan(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("stokbahanminim"), Authorize]
        public async Task<ActionResult<string>> OnGetStokBahanMinim()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetStokBahanMinim(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("stokbarangminim"), Authorize]
        public async Task<ActionResult<string>> OnGetStokBarangMinim()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetStokBarangMinim(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("utang"), Authorize]
        public async Task<ActionResult<string>> OnGetUtang()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetUtang(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("piutang"), Authorize]
        public async Task<ActionResult<string>> OnGetPiutang()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetPiutang(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("barangpopuler"), Authorize]
        public async Task<ActionResult<string>> OnGetBarangPopuler()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(await _indexRepository.GetBarangPopuler(), _options));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}