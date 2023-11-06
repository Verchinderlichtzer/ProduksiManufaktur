using Microsoft.Reporting.NETCore;
using System.Text;

namespace ProduksiManufaktur.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class LaporanController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IProfilRepository _profilRepository;
    private readonly ILaporanRepository _laporanRepository;
    private ReportParameter? _paramNama, _paramAlamat, _paramTelepon, _paramFax, _paramEmail, _paramWebsite, _paramPengurus, _paramJabatan, _paramLogo;
    private readonly List<ReportParameter> _parameter = new();
    private DateTime _tanggal1 = DateTime.Now.Date;
    private DateTime _tanggal2 = DateTime.Now.Date;
    private readonly JsonSerializerOptions _options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

    private void ParameterInfo(Profil profil)
    {
        _paramNama = new ReportParameter("Nama", profil.Nama);
        _paramAlamat = new ReportParameter("Alamat", profil.Alamat);
        _paramTelepon = new ReportParameter("Telepon", profil.Telepon);
        _paramFax = new ReportParameter("Fax", profil.Fax);
        _paramWebsite = new ReportParameter("Website", profil.Website);
        _paramEmail = new ReportParameter("Email", profil.Email);
        _paramPengurus = new ReportParameter("Pengurus", profil.Pengurus);
        _paramJabatan = new ReportParameter("Jabatan", profil.Jabatan);
        _paramLogo = new ReportParameter("Logo", Convert.ToBase64String(profil.Logo!));

        _parameter.AddRange(new[] { _paramNama, _paramAlamat, _paramTelepon, _paramFax, _paramWebsite, _paramEmail, _paramPengurus, _paramJabatan, _paramLogo }!);
    }

    public LaporanController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, IProfilRepository profilRepository, ILaporanRepository laporanRepository)
    {
        _appDbContext = appDbContext;
        _webHostEnvironment = webHostEnvironment;
        _profilRepository = profilRepository;
        _laporanRepository = laporanRepository;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [HttpGet("get/bahan"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetBahan()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetBahan(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/barang"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetBarang()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetBarang(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/karyawan"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetKaryawan()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetKaryawan(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/supplier"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetSupplier()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetSupplier(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/customer"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetCustomer()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetCustomer(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/pembelian"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetPembelian()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetPembelian(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/retur-pembelian"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetReturPembelian()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetReturPembelian(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/penjualan"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetPenjualan()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetPenjualan(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/retur-penjualan"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetReturPenjualan()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetReturPenjualan(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/produksi"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetProduksi()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetProduksi(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("get/formulasi"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult<string>> OnGetFormulasi()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(await _laporanRepository.GetFormulasi(), _options));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    public async Task<FileContentResult> Lapor<T>(string judul, IEnumerable<T> data, string filterText = "", bool grafik = false)
    {
        using Stream reportDefinition = new FileStream($"{_webHostEnvironment.WebRootPath}\\Laporan{judul}.rdlc", FileMode.Open);
        LocalReport report = new();
        report.LoadReportDefinition(reportDefinition);

        if (!grafik)
        {
            ParameterInfo(await _profilRepository.Get());
            foreach (var x in _parameter) report.SetParameters(x);
            report.SetParameters(new ReportParameter("Filter", filterText));
            report.DataSources.Add(new ReportDataSource($"DataSet{judul}", data));
        }
        else
        {
            report.SetParameters(new ReportParameter("Tahun", filterText));
            report.DataSources.Add(new ReportDataSource("DataSetGrafik", data));
        }

        byte[] pdf = report.Render("PDF");
        return File(pdf, "application/pdf", $"Laporan{judul}{DateTime.Now.Date:ddMMyy}.pdf");
    }

    [HttpGet("master-bahan/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanMasterBahan()
    {
        var data = await _appDbContext.Bahan.Select(x => new { x.Id, x.Nama, x.SatuanProduksi, x.StokMinimal }).ToListAsync();
        return await Lapor("MasterBahan", data);
    }

    [HttpGet("master-barang/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanMasterBarang()
    {
        var data = await _appDbContext.Barang.Select(x => new { x.Id, x.Nama, x.SatuanProduksi, x.StokMinimal }).ToListAsync();
        return await Lapor("MasterBarang", data);
    }

    [HttpGet("master-karyawan/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanMasterKaryawan()
    {
        var data = await _appDbContext.Karyawan.Include(x => x.Pekerjaan).Select(x => new { x.Id, NamaKaryawan = x.Nama, x.TempatLahir, x.TanggalLahir, x.Alamat, x.Telepon, x.Email, NamaPekerjaan = x.Pekerjaan!.Nama }).ToListAsync();
        return await Lapor("MasterKaryawan", data);
    }

    [HttpGet("master-supplier/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanMasterSupplier()
    {
        var data = await _appDbContext.Supplier.Select(x => new { x.Id, x.Nama, x.Alamat, x.Telepon, x.Fax, x.Email }).ToListAsync();
        return await Lapor("MasterSupplier", data);
    }

    [HttpGet("master-customer/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanMasterCustomer()
    {
        var data = await _appDbContext.Customer.Select(x => new { x.Id, x.Nama, x.Alamat, x.Telepon, x.Fax, x.Email }).ToListAsync();
        return await Lapor("MasterCustomer", data);
    }

    [HttpGet("transaksi-pembelian/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanTransaksiPembelian(string? filterTerpilih, string filterText, string? tanggal)
    {
        var data = await (from pembelian in _appDbContext.Pembelian
                          join supplier in _appDbContext.Supplier on pembelian.SupplierId equals supplier.Id
                          join pembelianDetail in _appDbContext.PembelianDetail on pembelian.Id equals pembelianDetail.PembelianId
                          join bahanSatuan in _appDbContext.BahanSatuan on pembelianDetail.BahanSatuanId equals bahanSatuan.Id
                          join bahan in _appDbContext.Bahan on bahanSatuan.BahanId equals bahan.Id
                          select new
                          {
                              pembelian.Id,
                              pembelian.Tanggal,
                              NamaSupplier = supplier.Nama,
                              pembelian.Subtotal,
                              pembelian.PPN,
                              pembelian.MetodeBayar,
                              pembelian.Status,
                              NamaBahan = bahan.Nama,
                              pembelianDetail.Jumlah,
                              SatuanBahan = bahanSatuan.Nama,
                              bahanSatuan.Ukuran,
                              pembelianDetail.Harga
                          }).ToListAsync();
        if (!string.IsNullOrEmpty(filterTerpilih))
        {
            if (!string.IsNullOrEmpty(tanggal))
            {
                if (filterTerpilih!.Contains("periodik"))
                {
                    string[] tgl = tanggal.Split('.');
                    _tanggal1 = DateTime.Parse(tgl[0]);
                    _tanggal2 = DateTime.Parse(tgl[1]);
                    data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date).ToList();
                }
                else if (filterTerpilih!.Contains("bulanan"))
                {
                    _tanggal1 = DateTime.Parse(tanggal);
                    data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year).ToList();
                }
                else if (filterTerpilih!.Contains("tahunan"))
                {
                    data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal)).ToList();
                }
            }
            if (filterTerpilih!.Contains("tunai"))
            {
                data = data.Where(x => x.MetodeBayar == "Tunai").ToList();
            }
            else if (filterTerpilih!.Contains("kredit"))
            {
                data = data.Where(x => x.MetodeBayar == "Kredit").ToList();
            }
            if (filterTerpilih!.Contains("lunas"))
            {
                data = data.Where(x => x.Status == "Lunas").ToList();
            }
            else if (filterTerpilih!.Contains("belumLunas"))
            {
                data = data.Where(x => x.Status == "Belum Lunas").ToList();
            }
        }
        return await Lapor("TransaksiPembelian", data, filterText);
    }

    [HttpGet("transaksi-penjualan/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanTransaksiPenjualan(string? filterTerpilih, string filterText, string? tanggal)
    {
        var data = await (from penjualan in _appDbContext.Penjualan
                          join customer in _appDbContext.Customer on penjualan.CustomerId equals customer.Id
                          join penjualanDetail in _appDbContext.PenjualanDetail on penjualan.Id equals penjualanDetail.PenjualanId
                          join barangSatuan in _appDbContext.BarangSatuan on penjualanDetail.BarangSatuanId equals barangSatuan.Id
                          join barang in _appDbContext.Barang on barangSatuan.BarangId equals barang.Id
                          select new
                          {
                              penjualan.Id,
                              penjualan.Tanggal,
                              NamaCustomer = customer.Nama,
                              penjualan.Subtotal,
                              penjualan.PPN,
                              penjualan.MetodeBayar,
                              penjualan.Status,
                              NamaBarang = barang.Nama,
                              penjualanDetail.Jumlah,
                              SatuanBarang = barangSatuan.Nama,
                              barangSatuan.Ukuran,
                              penjualanDetail.Harga
                          }).ToListAsync();
        if (!string.IsNullOrEmpty(filterTerpilih))
        {
            if (!string.IsNullOrEmpty(tanggal))
            {
                if (filterTerpilih!.Contains("periodik"))
                {
                    string[] tgl = tanggal.Split('.');
                    _tanggal1 = DateTime.Parse(tgl[0]);
                    _tanggal2 = DateTime.Parse(tgl[1]);
                    data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date).ToList();
                }
                else if (filterTerpilih!.Contains("bulanan"))
                {
                    _tanggal1 = DateTime.Parse(tanggal);
                    data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year).ToList();
                }
                else if (filterTerpilih!.Contains("tahunan"))
                {
                    data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal)).ToList();
                }
            }
            if (filterTerpilih!.Contains("tunai"))
            {
                data = data.Where(x => x.MetodeBayar == "Tunai").ToList();
            }
            else if (filterTerpilih!.Contains("kredit"))
            {
                data = data.Where(x => x.MetodeBayar == "Kredit").ToList();
            }
            if (filterTerpilih!.Contains("lunas"))
            {
                data = data.Where(x => x.Status == "Lunas").ToList();
            }
            else if (filterTerpilih!.Contains("belumLunas"))
            {
                data = data.Where(x => x.Status == "Belum Lunas").ToList();
            }
        }
        return await Lapor("TransaksiPenjualan", data, filterText);
    }

    [HttpGet("transaksi-produksi/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanTransaksiProduksi(string? filterTerpilih, string filterText, string? tanggal)
    {
        var data = await (from produksi in _appDbContext.Produksi
                          join barang in _appDbContext.Barang on produksi.BarangId equals barang.Id
                          join produksiDetailBahan in _appDbContext.ProduksiDetailBahan on produksi.Id equals produksiDetailBahan.ProduksiId
                          join bahan in _appDbContext.Bahan on produksiDetailBahan.BahanId equals bahan.Id
                          select new
                          {
                              produksi.Id,
                              produksi.Tanggal,
                              NamaBarang = barang.Nama,
                              JumlahBarang = produksi.Jumlah,
                              SatuanBarang = barang.SatuanProduksi,
                              produksi.BiayaJasa,
                              produksi.BiayaOverhead,
                              NamaBahan = bahan.Nama,
                              JumlahBahan = produksiDetailBahan.Jumlah,
                              SatuanBahan = bahan.SatuanProduksi
                          }).ToListAsync();
        if (!string.IsNullOrEmpty(tanggal))
        {
            if (filterTerpilih!.Contains("periodik"))
            {
                string[] tgl = tanggal.Split('.');
                _tanggal1 = DateTime.Parse(tgl[0]);
                _tanggal2 = DateTime.Parse(tgl[1]);
                data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date).ToList();
            }
            else if (filterTerpilih!.Contains("bulanan"))
            {
                _tanggal1 = DateTime.Parse(tanggal);
                data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year).ToList();
            }
            else if (filterTerpilih!.Contains("tahunan"))
            {
                data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal)).ToList();
            }
        }
        return await Lapor("TransaksiProduksi", data, filterText);
    }

    [HttpGet("transaksi-transaksi-lain/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanTransaksiTransaksiLain(string? filterTerpilih, string filterText, string? tanggal)
    {
        var data = await _appDbContext.TransaksiLain.ToListAsync();
        if (!string.IsNullOrEmpty(tanggal))
        {
            if (filterTerpilih!.Contains("periodik"))
            {
                string[] tgl = tanggal.Split('.');
                _tanggal1 = DateTime.Parse(tgl[0]);
                _tanggal2 = DateTime.Parse(tgl[1]);
                data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date).ToList();
            }
            else if (filterTerpilih!.Contains("bulanan"))
            {
                _tanggal1 = DateTime.Parse(tanggal);
                data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year).ToList();
            }
            else if (filterTerpilih!.Contains("tahunan"))
            {
                data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal)).ToList();
            }
        }
        return await Lapor("TransaksiTransaksiLain", data, filterText);
    }

    [HttpGet("grafik-batang/{filter}"), Authorize(Policy = "ReportRead")]
    [HttpGet("grafik-garis/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanGrafik(string filterTerpilih, string tanggal)
    {
        string jenis = filterTerpilih.Contains("grafik-batang") ? "Batang" : "Garis";
        var dataPembelian = await _appDbContext.TransaksiPembelian.Where(x => x.Tanggal.Year == int.Parse(tanggal)).Select(x => new { Id = x.PembelianId, x.Tanggal, x.Nominal }).ToListAsync();
        var dataPenjualan = await _appDbContext.TransaksiPenjualan.Where(x => x.Tanggal.Year == int.Parse(tanggal)).Select(x => new { Id = x.PenjualanId, x.Tanggal, x.Nominal }).ToListAsync();
        var data = dataPembelian.Concat(dataPenjualan).OrderBy(x => x.Tanggal);
        return await Lapor($"Grafik{jenis}", data, tanggal, grafik: true);
    }

    [HttpGet("transaksi-kas/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanTransaksiKas(string? filterTerpilih, string filterText, string? tanggal)
    {
        var dataPembelian = await (from pembelian in _appDbContext.Pembelian
                                   join transaksiPembelian in _appDbContext.TransaksiPembelian on pembelian.Id equals transaksiPembelian.PembelianId
                                   join supplier in _appDbContext.Supplier on pembelian.SupplierId equals supplier.Id
                                   select new
                                   {
                                       pembelian.Id,
                                       pembelian.Tanggal,
                                       Jenis = "Pembelian",
                                       pembelian.Keterangan,
                                       transaksiPembelian.Nominal,
                                       NamaPihak = supplier.Nama
                                   }).ToListAsync();
        var dataReturPembelian = await (from returPembelian in _appDbContext.ReturPembelian
                                        join pembelian in _appDbContext.Pembelian on returPembelian.PembelianId equals pembelian.Id
                                        join supplier in _appDbContext.Supplier on pembelian.SupplierId equals supplier.Id
                                        select new
                                        {
                                            returPembelian.Id,
                                            returPembelian.Tanggal,
                                            Jenis = "Retur Pembelian",
                                            returPembelian.Keterangan,
                                            Nominal = returPembelian.GrandTotal,
                                            NamaPihak = supplier.Nama
                                        }).ToListAsync();
        var dataPenjualan = await (from penjualan in _appDbContext.Penjualan
                                   join transaksiPenjualan in _appDbContext.TransaksiPenjualan on penjualan.Id equals transaksiPenjualan.PenjualanId
                                   join customer in _appDbContext.Customer on penjualan.CustomerId equals customer.Id
                                   select new
                                   {
                                       penjualan.Id,
                                       penjualan.Tanggal,
                                       Jenis = "Penjualan",
                                       penjualan.Keterangan,
                                       transaksiPenjualan.Nominal,
                                       NamaPihak = customer.Nama
                                   }).ToListAsync();
        var dataReturPenjualan = await (from returPenjualan in _appDbContext.ReturPenjualan
                                        join penjualan in _appDbContext.Penjualan on returPenjualan.PenjualanId equals penjualan.Id
                                        join customer in _appDbContext.Customer on penjualan.CustomerId equals customer.Id
                                        select new
                                        {
                                            returPenjualan.Id,
                                            returPenjualan.Tanggal,
                                            Jenis = "Retur Penjualan",
                                            returPenjualan.Keterangan,
                                            Nominal = returPenjualan.GrandTotal,
                                            NamaPihak = customer.Nama
                                        }).ToListAsync();
        var data = dataPembelian.Concat(dataReturPembelian).Concat(dataPenjualan).Concat(dataReturPenjualan);
        if (!string.IsNullOrEmpty(tanggal))
        {
            if (filterTerpilih!.Contains("periodik"))
            {
                string[] tgl = tanggal.Split('.');
                _tanggal1 = DateTime.Parse(tgl[0]);
                _tanggal2 = DateTime.Parse(tgl[1]);
                data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date);
            }
            else if (filterTerpilih!.Contains("bulanan"))
            {
                _tanggal1 = DateTime.Parse(tanggal);
                data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year);
            }
            else if (filterTerpilih!.Contains("tahunan"))
            {
                data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal));
            }
        }
        data = data.OrderBy(x => x.Tanggal);
        return await Lapor("TransaksiKas", data, filterText);
    }

    [HttpGet("entitas-faktur-pembelian/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasFakturPembelian(string entitas)
    {
        var data = await (from pembelian in _appDbContext.Pembelian
                          join supplier in _appDbContext.Supplier on pembelian.SupplierId equals supplier.Id
                          join pembelianDetail in _appDbContext.PembelianDetail on pembelian.Id equals pembelianDetail.PembelianId
                          join bahanSatuan in _appDbContext.BahanSatuan on pembelianDetail.BahanSatuanId equals bahanSatuan.Id
                          join bahan in _appDbContext.Bahan on bahanSatuan.BahanId equals bahan.Id
                          where pembelian.Id == entitas
                          select new
                          {
                              pembelian.Id,
                              pembelian.Tanggal,
                              NamaSupplier = supplier.Nama,
                              supplier.Alamat,
                              supplier.Telepon,
                              supplier.Fax,
                              supplier.Email,
                              pembelian.Subtotal,
                              pembelian.PPN,
                              pembelian.MetodeBayar,
                              pembelian.Status,
                              pembelian.JatuhTempo,
                              pembelian.Keterangan,
                              NamaBahan = bahan.Nama,
                              pembelianDetail.Jumlah,
                              SatuanBahan = bahanSatuan.Nama,
                              bahanSatuan.Ukuran,
                              pembelianDetail.Harga
                          }).ToListAsync();
        return await Lapor("EntitasFakturPembelian", data);
    }

    [HttpGet("entitas-faktur-penjualan/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasFakturPenjualan(string entitas)
    {
        var data = await (from penjualan in _appDbContext.Penjualan
                          join customer in _appDbContext.Customer on penjualan.CustomerId equals customer.Id
                          join penjualanDetail in _appDbContext.PenjualanDetail on penjualan.Id equals penjualanDetail.PenjualanId
                          join barangSatuan in _appDbContext.BarangSatuan on penjualanDetail.BarangSatuanId equals barangSatuan.Id
                          join barang in _appDbContext.Barang on barangSatuan.BarangId equals barang.Id
                          where penjualan.Id == entitas
                          select new
                          {
                              penjualan.Id,
                              penjualan.Tanggal,
                              NamaCustomer = customer.Nama,
                              customer.Alamat,
                              customer.Telepon,
                              customer.Fax,
                              customer.Email,
                              penjualan.Subtotal,
                              penjualan.PPN,
                              penjualan.MetodeBayar,
                              penjualan.Status,
                              penjualan.JatuhTempo,
                              penjualan.Keterangan,
                              NamaBarang = barang.Nama,
                              penjualanDetail.Jumlah,
                              SatuanBarang = barangSatuan.Nama,
                              barangSatuan.Ukuran,
                              penjualanDetail.Harga
                          }).ToListAsync();
        return await Lapor("EntitasFakturPenjualan", data);
    }

    [HttpGet("entitas-stok-bahan/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasStokBahan(string? filterTerpilih, string filterText, string? tanggal, string entitas)
    {
        var dataPembelian = await (from pembelian in _appDbContext.Pembelian
                                   join pembelianDetail in _appDbContext.PembelianDetail on pembelian.Id equals pembelianDetail.PembelianId
                                   join bahanSatuan in _appDbContext.BahanSatuan on pembelianDetail.BahanSatuanId equals bahanSatuan.Id
                                   join bahan in _appDbContext.Bahan on bahanSatuan.BahanId equals bahan.Id
                                   where bahan.Id == entitas
                                   select new
                                   {
                                       pembelian.Id,
                                       pembelian.Tanggal,
                                       bahanSatuan.Nama,
                                       bahanSatuan.Ukuran,
                                       Jenis = "Pembelian",
                                       bahan.StokAwal,
                                       pembelianDetail.Jumlah,
                                       bahanSatuan.KonversiStok
                                   }).ToListAsync();
        var dataReturPembelian = await (from returPembelian in _appDbContext.ReturPembelian
                                        join returPembelianDetail in _appDbContext.ReturPembelianDetail on returPembelian.Id equals returPembelianDetail.ReturPembelianId
                                        join bahanSatuan in _appDbContext.BahanSatuan on returPembelianDetail.BahanSatuanId equals bahanSatuan.Id
                                        join bahan in _appDbContext.Bahan on bahanSatuan.BahanId equals bahan.Id
                                        where bahan.Id == entitas && returPembelianDetail.Jumlah > 0
                                        select new
                                        {
                                            returPembelian.Id,
                                            returPembelian.Tanggal,
                                            bahanSatuan.Nama,
                                            bahanSatuan.Ukuran,
                                            Jenis = "Retur",
                                            bahan.StokAwal,
                                            returPembelianDetail.Jumlah,
                                            bahanSatuan.KonversiStok
                                        }).ToListAsync();
        var dataProduksiDetailBahan = await (from produksi in _appDbContext.Produksi
                                             join produksiDetailBahan in _appDbContext.ProduksiDetailBahan on produksi.Id equals produksiDetailBahan.ProduksiId
                                             join bahan in _appDbContext.Bahan on produksiDetailBahan.BahanId equals bahan.Id
                                             where bahan.Id == entitas
                                             select new
                                             {
                                                 produksi.Id,
                                                 produksi.Tanggal,
                                                 Nama = bahan.SatuanProduksi,
                                                 Ukuran = string.Empty,
                                                 Jenis = "Produksi",
                                                 bahan.StokAwal,
                                                 produksiDetailBahan.Jumlah,
                                                 KonversiStok = 1m
                                             }).ToListAsync();
        var dataPerubahanStokBahan = await (from perubahanStokBahan in _appDbContext.PerubahanStokBahan
                                            join bahan in _appDbContext.Bahan on perubahanStokBahan.BahanId equals bahan.Id
                                            where bahan.Id == entitas
                                            select new
                                            {
                                                Id = perubahanStokBahan.Id.ToString(),
                                                perubahanStokBahan.Tanggal,
                                                Nama = bahan.SatuanProduksi,
                                                Ukuran = string.Empty,
                                                perubahanStokBahan.Jenis,
                                                bahan.StokAwal,
                                                perubahanStokBahan.Jumlah,
                                                KonversiStok = 1m
                                            }).ToListAsync();
        decimal arusStok = dataPembelian.Select(x => x.StokAwal).FirstOrDefault();
        var data = dataPembelian.Concat(dataReturPembelian).Concat(dataProduksiDetailBahan).Concat(dataPerubahanStokBahan).OrderBy(x => x.Tanggal).Select(x => new
        {
            x.Id,
            x.Tanggal,
            x.Nama,
            x.Ukuran,
            x.Jenis,
            StokAwal = arusStok,
            Jumlah = x.Jenis == "Pembelian" || x.Jenis == "Retur" ? x.Jumlah : x.Jumlah * x.KonversiStok,
            StokAkhir = x.Jenis == "Pembelian" || x.Jenis == "Penambahan" ? Tambahi(ref arusStok, x.Jenis == "Pembelian" ? x.Jumlah * x.KonversiStok : x.Jumlah) : Kurangi(ref arusStok, x.Jenis == "Retur" ? x.Jumlah * x.KonversiStok : x.Jumlah),
            x.KonversiStok
        });

        if (!string.IsNullOrEmpty(tanggal))
        {
            if (filterTerpilih!.Contains("periodik"))
            {
                string[] tgl = tanggal.Split('.');
                _tanggal1 = DateTime.Parse(tgl[0]);
                _tanggal2 = DateTime.Parse(tgl[1]);
                data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date);
            }
            else if (filterTerpilih!.Contains("bulanan"))
            {
                _tanggal1 = DateTime.Parse(tanggal);
                data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year);
            }
            else if (filterTerpilih!.Contains("tahunan"))
            {
                data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal));
            }
        }
        return await Lapor("EntitasStokBahan", data, filterText);
    }

    [HttpGet("entitas-stok-barang/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasStokBarang(string? filterTerpilih, string filterText, string? tanggal, string entitas)
    {
        var dataPenjualan = await (from penjualan in _appDbContext.Penjualan
                                   join penjualanDetail in _appDbContext.PenjualanDetail on penjualan.Id equals penjualanDetail.PenjualanId
                                   join barangSatuan in _appDbContext.BarangSatuan on penjualanDetail.BarangSatuanId equals barangSatuan.Id
                                   join barang in _appDbContext.Barang on barangSatuan.BarangId equals barang.Id
                                   where barang.Id == entitas
                                   select new
                                   {
                                       penjualan.Id,
                                       penjualan.Tanggal,
                                       barangSatuan.Nama,
                                       barangSatuan.Ukuran,
                                       Jenis = "Penjualan",
                                       barang.StokAwal,
                                       penjualanDetail.Jumlah,
                                       barangSatuan.KonversiStok
                                   }).ToListAsync();
        var dataReturPenjualan = await (from returPenjualan in _appDbContext.ReturPenjualan
                                        join returPenjualanDetail in _appDbContext.ReturPenjualanDetail on returPenjualan.Id equals returPenjualanDetail.ReturPenjualanId
                                        join barangSatuan in _appDbContext.BarangSatuan on returPenjualanDetail.BarangSatuanId equals barangSatuan.Id
                                        join barang in _appDbContext.Barang on barangSatuan.BarangId equals barang.Id
                                        where barang.Id == entitas && returPenjualanDetail.Jumlah > 0
                                        select new
                                        {
                                            returPenjualan.Id,
                                            returPenjualan.Tanggal,
                                            barangSatuan.Nama,
                                            barangSatuan.Ukuran,
                                            Jenis = "Retur",
                                            barang.StokAwal,
                                            returPenjualanDetail.Jumlah,
                                            barangSatuan.KonversiStok
                                        }).ToListAsync();
        var dataProduksi = await (from produksi in _appDbContext.Produksi
                                  join barang in _appDbContext.Barang on produksi.BarangId equals barang.Id
                                  where barang.Id == entitas
                                  select new
                                  {
                                      produksi.Id,
                                      produksi.Tanggal,
                                      Nama = barang.SatuanProduksi,
                                      Ukuran = string.Empty,
                                      Jenis = "Produksi",
                                      barang.StokAwal,
                                      produksi.Jumlah,
                                      KonversiStok = 1m
                                  }).ToListAsync();
        var dataPerubahanStokBarang = await (from perubahanStokBarang in _appDbContext.PerubahanStokBarang
                                             join barang in _appDbContext.Barang on perubahanStokBarang.BarangId equals barang.Id
                                             where barang.Id == entitas
                                             select new
                                             {
                                                 Id = perubahanStokBarang.Id.ToString(),
                                                 perubahanStokBarang.Tanggal,
                                                 Nama = barang.SatuanProduksi,
                                                 Ukuran = string.Empty,
                                                 perubahanStokBarang.Jenis,
                                                 barang.StokAwal,
                                                 perubahanStokBarang.Jumlah,
                                                 KonversiStok = 1m
                                             }).ToListAsync();
        decimal arusStok = dataPenjualan.Select(x => x.StokAwal).FirstOrDefault();
        var data = dataPenjualan.Concat(dataReturPenjualan).Concat(dataProduksi).Concat(dataPerubahanStokBarang).OrderBy(x => x.Tanggal).Select(x => new
        {
            x.Id,
            x.Tanggal,
            x.Nama,
            x.Ukuran,
            x.Jenis,
            StokAwal = arusStok,
            Jumlah = x.Jenis == "Penjualan" || x.Jenis == "Retur" ? x.Jumlah : x.Jumlah * x.KonversiStok,
            StokAkhir = x.Jenis == "Penjualan" || x.Jenis == "Pengurangan" ? Kurangi(ref arusStok, x.Jenis == "Penjualan" ? x.Jumlah * x.KonversiStok : x.Jumlah) : Tambahi(ref arusStok, x.Jenis == "Retur" ? x.Jumlah * x.KonversiStok : x.Jumlah),
            x.KonversiStok
        });

        if (!string.IsNullOrEmpty(tanggal))
        {
            if (filterTerpilih!.Contains("periodik"))
            {
                string[] tgl = tanggal.Split('.');
                _tanggal1 = DateTime.Parse(tgl[0]);
                _tanggal2 = DateTime.Parse(tgl[1]);
                data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date);
            }
            else if (filterTerpilih!.Contains("bulanan"))
            {
                _tanggal1 = DateTime.Parse(tanggal);
                data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year);
            }
            else if (filterTerpilih!.Contains("tahunan"))
            {
                data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal));
            }
        }
        return await Lapor("EntitasStokBarang", data, filterText);
    }

    [HttpGet("entitas-formulasi/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasFormulasi(string entitas)
    {
        var data = await (from formulasi in _appDbContext.Formulasi
                          join barang in _appDbContext.Barang on formulasi.BarangId equals barang.Id
                          join formulasiDetail in _appDbContext.FormulasiDetail on formulasi.Id equals formulasiDetail.FormulasiId
                          join bahan in _appDbContext.Bahan on formulasiDetail.BahanId equals bahan.Id
                          where formulasi.Id == entitas
                          select new
                          {
                              formulasi.Id,
                              NamaBarang = barang.Nama,
                              JumlahBarang = formulasi.Jumlah,
                              SatuanBarang = barang.SatuanProduksi,
                              NamaBahan = bahan.Nama,
                              JumlahBahan = formulasiDetail.Jumlah,
                              SatuanBahan = bahan.SatuanProduksi
                          }).ToListAsync();
        return await Lapor("EntitasFormulasi", data);
    }

    [HttpGet("entitas-produksi/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasProduksi(string entitas)
    {
        var dataProduksi = await (from produksi in _appDbContext.Produksi
                                  join barang in _appDbContext.Barang on produksi.BarangId equals barang.Id
                                  where produksi.Id == entitas
                                  select new
                                  {
                                      produksi.Id,
                                      produksi.Tanggal,
                                      NamaBarang = barang.Nama,
                                      JumlahBarang = produksi.Jumlah,
                                      SatuanBarang = barang.SatuanProduksi,
                                      produksi.Keterangan
                                  }).ToListAsync();
        var dataProduksiDetailBahan = await (from produksiDetailBahan in _appDbContext.ProduksiDetailBahan
                                             join bahan in _appDbContext.Bahan on produksiDetailBahan.BahanId equals bahan.Id
                                             where produksiDetailBahan.ProduksiId == entitas
                                             select new
                                             {
                                                 NamaBahan = bahan.Nama,
                                                 JumlahBahan = produksiDetailBahan.Jumlah,
                                                 SatuanBahan = bahan.SatuanProduksi
                                             }).ToListAsync();
        var dataProduksiDetailJasa = await (from produksiDetailJasa in _appDbContext.ProduksiDetailJasa
                                            join karyawan in _appDbContext.Karyawan on produksiDetailJasa.KaryawanId equals karyawan.Id
                                            join pekerjaan in _appDbContext.Pekerjaan on karyawan.PekerjaanId equals pekerjaan.Id
                                            where produksiDetailJasa.ProduksiId == entitas
                                            select new
                                            {
                                                NamaKaryawan = karyawan.Nama,
                                                NamaPekerjaan = pekerjaan.Nama,
                                                BiayaJasa = produksiDetailJasa.Biaya
                                            }).ToListAsync();
        var dataProduksiDetailOverhead = await (from produksiDetailOverhead in _appDbContext.ProduksiDetailOverhead
                                                join overhead in _appDbContext.Overhead on produksiDetailOverhead.OverheadId equals overhead.Id
                                                where produksiDetailOverhead.ProduksiId == entitas
                                                select new
                                                {
                                                    NamaOverhead = overhead.Nama,
                                                    BiayaOverhead = produksiDetailOverhead.Biaya
                                                }).ToListAsync();
        using Stream reportDefinition = new FileStream($"{_webHostEnvironment.WebRootPath}\\LaporanEntitasProduksi.rdlc", FileMode.Open);
        LocalReport report = new();
        report.LoadReportDefinition(reportDefinition);
        report.DataSources.Add(new ReportDataSource("DataSetEntitasProduksi", dataProduksi));
        report.DataSources.Add(new ReportDataSource("DataSetEntitasProduksiDetailBahan", dataProduksiDetailBahan));
        report.DataSources.Add(new ReportDataSource("DataSetEntitasProduksiDetailJasa", dataProduksiDetailJasa));
        report.DataSources.Add(new ReportDataSource("DataSetEntitasProduksiDetailOverhead", dataProduksiDetailOverhead));

        ParameterInfo(await _profilRepository.Get());
        foreach (var x in _parameter) report.SetParameters(x);

        byte[] pdf = report.Render("PDF");
        return File(pdf, "application/pdf", $"LaporanEntitasProduksi{DateTime.Now.Date:ddMMyy}.pdf");
    }

    [HttpGet("entitas-penggajian/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasPenggajian(string? filterTerpilih, string filterText, string? tanggal, string entitas)
    {
        var data = await (from karyawan in _appDbContext.Karyawan
                          join pekerjaan in _appDbContext.Pekerjaan on karyawan.PekerjaanId equals pekerjaan.Id
                          join produksiDetailJasa in _appDbContext.ProduksiDetailJasa on karyawan.Id equals produksiDetailJasa.KaryawanId
                          join produksi in _appDbContext.Produksi on produksiDetailJasa.ProduksiId equals produksi.Id
                          where karyawan.Id == entitas
                          select new
                          {
                              karyawan.Id,
                              NamaKaryawan = karyawan.Nama,
                              NamaPekerjaan = pekerjaan.Nama,
                              karyawan.TempatLahir,
                              karyawan.TanggalLahir,
                              karyawan.Alamat,
                              karyawan.Telepon,
                              karyawan.Email,
                              IdProduksi = produksi.Id,
                              produksi.Tanggal,
                              produksiDetailJasa.Biaya,
                              produksi.Keterangan
                          }).ToListAsync();
        if (!string.IsNullOrEmpty(tanggal))
        {
            if (filterTerpilih!.Contains("periodik"))
            {
                string[] tgl = tanggal.Split('.');
                _tanggal1 = DateTime.Parse(tgl[0]);
                _tanggal2 = DateTime.Parse(tgl[1]);
                data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date).ToList();
            }
            else if (filterTerpilih!.Contains("bulanan"))
            {
                _tanggal1 = DateTime.Parse(tanggal);
                data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year).ToList();
            }
            else if (filterTerpilih!.Contains("tahunan"))
            {
                data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal)).ToList();
            }
        }
        return await Lapor("EntitasPenggajian", data, filterText);
    }

    [HttpGet("entitas-supplier/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasSupplier(string? filterTerpilih, string filterText, string? tanggal, string entitas)
    {
        var data = await (from supplier in _appDbContext.Supplier
                          join pembelian in _appDbContext.Pembelian on supplier.Id equals pembelian.SupplierId
                          join pembelianDetail in _appDbContext.PembelianDetail on pembelian.Id equals pembelianDetail.PembelianId
                          join bahanSatuan in _appDbContext.BahanSatuan on pembelianDetail.BahanSatuanId equals bahanSatuan.Id
                          join bahan in _appDbContext.Bahan on bahanSatuan.BahanId equals bahan.Id
                          where supplier.Id == entitas
                          select new
                          {
                              IdSupplier = supplier.Id,
                              NamaSupplier = supplier.Nama,
                              supplier.Alamat,
                              supplier.Telepon,
                              supplier.Fax,
                              supplier.Email,
                              IdPembelian = pembelian.Id,
                              pembelian.Tanggal,
                              pembelian.Subtotal,
                              pembelian.PPN,
                              pembelian.MetodeBayar,
                              pembelian.Status,
                              NamaBahan = bahan.Nama,
                              pembelianDetail.Harga,
                              pembelianDetail.Jumlah,
                              SatuanBahan = bahanSatuan.Nama,
                              bahanSatuan.Ukuran
                          }).ToListAsync();
        if (!string.IsNullOrEmpty(filterTerpilih))
        {
            if (!string.IsNullOrEmpty(tanggal))
            {
                if (filterTerpilih!.Contains("periodik"))
                {
                    string[] tgl = tanggal.Split('.');
                    _tanggal1 = DateTime.Parse(tgl[0]);
                    _tanggal2 = DateTime.Parse(tgl[1]);
                    data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date).ToList();
                }
                else if (filterTerpilih!.Contains("bulanan"))
                {
                    _tanggal1 = DateTime.Parse(tanggal);
                    data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year).ToList();
                }
                else if (filterTerpilih!.Contains("tahunan"))
                {
                    data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal)).ToList();
                }
            }
            if (filterTerpilih!.Contains("tunai"))
            {
                data = data.Where(x => x.MetodeBayar == "Tunai").ToList();
            }
            else if (filterTerpilih!.Contains("kredit"))
            {
                data = data.Where(x => x.MetodeBayar == "Kredit").ToList();
            }
            if (filterTerpilih!.Contains("lunas"))
            {
                data = data.Where(x => x.Status == "Lunas").ToList();
            }
            else if (filterTerpilih!.Contains("belumLunas"))
            {
                data = data.Where(x => x.Status == "Belum Lunas").ToList();
            }
        }
        return await Lapor("EntitasSupplier", data, filterText);
    }

    [HttpGet("entitas-customer/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasCustomer(string? filterTerpilih, string filterText, string? tanggal, string entitas)
    {
        var data = await (from customer in _appDbContext.Customer
                          join penjualan in _appDbContext.Penjualan on customer.Id equals penjualan.CustomerId
                          join penjualanDetail in _appDbContext.PenjualanDetail on penjualan.Id equals penjualanDetail.PenjualanId
                          join barangSatuan in _appDbContext.BarangSatuan on penjualanDetail.BarangSatuanId equals barangSatuan.Id
                          join barang in _appDbContext.Barang on barangSatuan.BarangId equals barang.Id
                          where customer.Id == entitas
                          select new
                          {
                              IdCustomer = customer.Id,
                              NamaCustomer = customer.Nama,
                              customer.Alamat,
                              customer.Telepon,
                              customer.Fax,
                              customer.Email,
                              IdPenjualan = penjualan.Id,
                              penjualan.Tanggal,
                              penjualan.Subtotal,
                              penjualan.PPN,
                              penjualan.MetodeBayar,
                              penjualan.Status,
                              NamaBarang = barang.Nama,
                              penjualanDetail.Harga,
                              penjualanDetail.Jumlah,
                              SatuanBarang = barangSatuan.Nama,
                              barangSatuan.Ukuran
                          }).ToListAsync();
        if (!string.IsNullOrEmpty(filterTerpilih))
        {
            if (!string.IsNullOrEmpty(tanggal))
            {
                if (filterTerpilih!.Contains("periodik"))
                {
                    string[] tgl = tanggal.Split('.');
                    _tanggal1 = DateTime.Parse(tgl[0]);
                    _tanggal2 = DateTime.Parse(tgl[1]);
                    data = data.Where(x => x.Tanggal.Date >= _tanggal1.Date && x.Tanggal.Date <= _tanggal2.Date).ToList();
                }
                else if (filterTerpilih!.Contains("bulanan"))
                {
                    _tanggal1 = DateTime.Parse(tanggal);
                    data = data.Where(x => x.Tanggal.Month == _tanggal1.Month && x.Tanggal.Year == _tanggal1.Year).ToList();
                }
                else if (filterTerpilih!.Contains("tahunan"))
                {
                    data = data.Where(x => x.Tanggal.Year == int.Parse(tanggal)).ToList();
                }
            }
            if (filterTerpilih!.Contains("tunai"))
            {
                data = data.Where(x => x.MetodeBayar == "Tunai").ToList();
            }
            else if (filterTerpilih!.Contains("kredit"))
            {
                data = data.Where(x => x.MetodeBayar == "Kredit").ToList();
            }
            if (filterTerpilih!.Contains("lunas"))
            {
                data = data.Where(x => x.Status == "Lunas").ToList();
            }
            else if (filterTerpilih!.Contains("belumLunas"))
            {
                data = data.Where(x => x.Status == "Belum Lunas").ToList();
            }
        }
        return await Lapor("EntitasCustomer", data, filterText);
    }

    [HttpGet("entitas-retur-pembelian/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasReturPembelian(string filterText, string entitas)
    {
        var data = await (from retur in _appDbContext.ReturPembelian
                          join pembelian in _appDbContext.Pembelian on retur.PembelianId equals pembelian.Id
                          join supplier in _appDbContext.Supplier on pembelian.SupplierId equals supplier.Id
                          join detail in _appDbContext.ReturPembelianDetail on retur.Id equals detail.ReturPembelianId
                          join satuan in _appDbContext.BahanSatuan on detail.BahanSatuanId equals satuan.Id
                          join bahan in _appDbContext.Bahan on satuan.BahanId equals bahan.Id
                          where retur.Id == entitas && detail.Jumlah > 0
                          select new
                          {
                              retur.Id,
                              retur.PembelianId,
                              retur.Tanggal,
                              NamaSupplier = supplier.Nama,
                              retur.GrandTotal,
                              retur.Keterangan,
                              NamaBahan = bahan.Nama,
                              detail.Harga,
                              detail.Jumlah,
                              SatuanBahan = satuan.Nama,
                              satuan.Ukuran
                          }).ToListAsync();
        return await Lapor("EntitasReturPembelian", data, filterText);
    }

    [HttpGet("entitas-retur-penjualan/{filter}"), Authorize(Policy = "ReportRead")]
    public async Task<ActionResult> LaporanEntitasReturPenjualan(string filterText, string entitas)
    {
        var data = await (from retur in _appDbContext.ReturPenjualan
                          join penjualan in _appDbContext.Penjualan on retur.PenjualanId equals penjualan.Id
                          join customer in _appDbContext.Customer on penjualan.CustomerId equals customer.Id
                          join detail in _appDbContext.ReturPenjualanDetail on retur.Id equals detail.ReturPenjualanId
                          join satuan in _appDbContext.BarangSatuan on detail.BarangSatuanId equals satuan.Id
                          join barang in _appDbContext.Barang on satuan.BarangId equals barang.Id
                          where retur.Id == entitas && detail.Jumlah > 0
                          select new
                          {
                              retur.Id,
                              retur.PenjualanId,
                              retur.Tanggal,
                              NamaCustomer = customer.Nama,
                              retur.GrandTotal,
                              retur.Keterangan,
                              NamaBarang = barang.Nama,
                              detail.Harga,
                              detail.Jumlah,
                              SatuanBarang = satuan.Nama,
                              satuan.Ukuran
                          }).ToListAsync();
        return await Lapor("EntitasReturPenjualan", data, filterText);
    }
}