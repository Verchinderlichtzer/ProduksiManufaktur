using ProduksiManufaktur.Models;

namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Penjualan, R PenjualanDetail, CRUD TransaksiPenjualan, CRUD ReturPenjualan, R ReturPenjualanDetail</summary>
    public interface IPenjualanRepository
    {
        /// <summary>List Penjualan { Id, Tanggal, Status, GrandTotal, Customer { Nama } } > PenjualanList</summary>
        Task<List<Penjualan>> Get();

        /// <summary>List Penjualan { Id, Tanggal } > ReturPenjualanForm (Untuk autocomplete, memilih penjualan yang belum retur untuk diretur)</summary>
        Task<List<Penjualan>> Get1();

        /// <summary>Penjualan { Id, CustomerId, InputTanggal, InputWaktu, JatuhTempo, PPN, Keterangan, Version, HariJatuhTempo, Terbayar, Customer { Id, Nama }, List PenjualanDetail { Id, PenjualanId, BarangSatuanId, MinJumlah, Jumlah, Harga, JumlahSebelum, StokAkhir, Total, BarangSatuan { BarangId, Nama, Ukuran, Harga, KonversiStok, Barang { Nama, SatuanProduksi, Stok, Version } } } } > PenjualanForm (Untuk autocomplete, memilih penjualan yang ingin diedit)</summary>
        Task<Penjualan> Find(string id);

        /// <summary>Penjualan { Id, Tanggal, JatuhTempo, HariJatuhTempo, GrandTotal, Terbayar, Sisa, Status, Version, List TransaksiPenjualan { Id, PenjualanId, Tanggal, Keterangan, Nominal, Version, InputTanggal, InputWaktu, NominalSebelum, Balance } } > TransaksiPenjualanForm (Memuat semua transaksi pada penjualan tertentu)</summary>
        Task<Penjualan> Find1(string id);

        /// <summary>Penjualan { Tanggal, Subtotal, PPN, Terbayar, MetodeBayar, Status, JatuhTempo, Keterangan, HariJatuhTempo, GrandTotal, Sisa, Customer { Nama }, List PenjualanDetail { Jumlah, Harga, Total, BarangSatuan { Nama, Ukuran, Barang { Nama } } } } > PenjualanInfo</summary>
        Task<Penjualan> Find2(string id);

        Task<Penjualan> Create(Penjualan penjualan);

        Task<Penjualan> CreatedPenjualan(string id);

        Task<Penjualan> Update(Penjualan penjualan);

        Task<bool> Deletable(string id);

        Task Delete(string id);

        /// <summary>List PenjualanDetail</summary>
        Task<List<PenjualanDetail>> GetDetail();

        /// <summary>List PenjualanDetail { BarangSatuanId, Harga, Jumlah, BarangSatuan { Id, Nama, Ukuran, Barang { Nama, Stok, Version } } } > ReturPenjualanForm (Untuk PilihPenjualan, PenjualanDetail akan dimasukkan ke ReturDetail)</summary>
        Task<List<PenjualanDetail>> FindDetail(string penjualanId);

        /// <summary>List BarangSatuan { Id, Nama, Ukuran, Harga, KonversiStok, Barang { Nama, SatuanProduksi, Version } } > PenjualanForm</summary>
        Task<List<BarangSatuan>> RefreshDetail(string id, List<int> barangSatuanIds);

        Task<bool> DeletableDetail(string penjualanId, int barangSatuanId);

        /// <summary>List TransaksiPenjualan</summary>
        Task<List<TransaksiPenjualan>> GetTransaksi();

        /// <summary>TransaksiPenjualan</summary>
        Task<TransaksiPenjualan> FindTransaksi(int id);

        Task<TransaksiPenjualan> CreateTransaksi(TransaksiPenjualan transaksiPenjualan);

        Task<TransaksiPenjualan> CreatedTransaksi(int id);

        Task<TransaksiPenjualan> UpdateTransaksi(TransaksiPenjualan transaksiPenjualan);

        Task DeleteTransaksi(int id);

        /// <summary>List ReturPenjualan { Id, Tanggal, Keterangan, GrandTotal, Penjualan { Customer { Nama } } } > ReturPenjualanList</summary>
        Task<List<ReturPenjualan>> GetRetur();

        /// <summary>ReturPenjualan { Id, PenjualanId, Keterangan, Version, InputTanggal, InputWaktu, GrandTotal, Penjualan, List ReturPenjualanDetail { BarangSatuanId, Harga, Jumlah, MaxJumlah, Total, BarangSatuan { Id, BarangId, Nama, Ukuran, Barang { Nama, SatuanProduksi, Stok, Version } } } } > ReturPenjualanForm</summary>
        Task<ReturPenjualan> FindRetur(string id);

        /// <summary>ReturPenjualan { PenjualanId, Tanggal, GrandTotal, Keterangan, List ReturPenjualanDetail { BarangSatuanId, Jumlah, Harga, Total, BarangSatuan { Nama, Ukuran, Barang { Nama } } } } > ReturPenjualanInfo</summary>
        Task<ReturPenjualan> FindRetur1(string id);

        Task<ReturPenjualan> CreateRetur(ReturPenjualan returPenjualan);

        Task<ReturPenjualan> CreatedRetur(string id);

        Task<ReturPenjualan> UpdateRetur(ReturPenjualan returPenjualan);

        Task DeleteRetur(string id);

        /// <summary>List ReturPenjualanDetail</summary>
        Task<List<ReturPenjualanDetail>> GetReturDetail();

        /// <summary>List BarangSatuan { Id, BarangId, Nama, Ukuran, Barang { Nama, SatuanProduksi, Stok, Version } } > ReturPenjualanForm</summary>
        Task<List<BarangSatuan>> RefreshReturDetail(string returId);
    }

    public class PenjualanRepository : IPenjualanRepository
    {
        private readonly AppDbContext _appDbContext;

        public PenjualanRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Penjualan>> Get()
        {
            return await _appDbContext.Penjualan.Include(x => x.Customer).Select(x => new Penjualan
            {
                Id = x.Id,
                Tanggal = x.Tanggal,
                Status = x.Status,
                GrandTotal = (int)(x.Subtotal * ((x.PPN + 100) / 100m)),
                Customer = new() { Nama = x.Customer!.Nama }
            }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<List<Penjualan>> Get1()
        {
            return await _appDbContext.Penjualan.Where(x => x.ReturPenjualan == null).Select(x => new Penjualan { Id = x.Id, Tanggal = x.Tanggal }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Penjualan> Find(string id)
        {
            Penjualan penjualan = await _appDbContext.Penjualan.Include(x => x.Customer!).Include(x => x.PenjualanDetail!).ThenInclude(x => x.BarangSatuan!).ThenInclude(x => x.Barang).FirstAsync(x => x.Id == id);
            return new Penjualan
            {
                Id = penjualan.Id,
                CustomerId = penjualan.CustomerId,
                InputTanggal = penjualan.Tanggal.Date,
                InputWaktu = penjualan.Tanggal.TimeOfDay,
                JatuhTempo = penjualan.JatuhTempo,
                PPN = penjualan.PPN,
                Keterangan = penjualan.Keterangan,
                Version = penjualan.Version,
                HariJatuhTempo = (penjualan.JatuhTempo! - penjualan.Tanggal.Date)?.Days ?? 1,
                Terbayar = penjualan.Terbayar,
                Customer = new Customer { Id = penjualan.Customer!.Id, Nama = penjualan.Customer!.Nama },
                PenjualanDetail = penjualan.PenjualanDetail!.ConvertAll(x => new PenjualanDetail
                {
                    Id = x.Id,
                    PenjualanId = x.PenjualanId,
                    BarangSatuanId = x.BarangSatuanId,
                    MinJumlah = x.MinJumlah,
                    Jumlah = x.Jumlah,
                    Harga = x.Harga,
                    JumlahSebelum = x.Jumlah,
                    StokAkhir = x.BarangSatuan!.Barang!.Stok,
                    Total = (int)(x.Jumlah * x.Harga),
                    BarangSatuan = new BarangSatuan
                    {
                        BarangId = x.BarangSatuan!.BarangId,
                        Nama = x.BarangSatuan!.Nama,
                        Ukuran = x.BarangSatuan!.Ukuran,
                        Harga = x.BarangSatuan!.Harga,
                        KonversiStok = x.BarangSatuan!.KonversiStok,
                        Barang = new Barang
                        {
                            Nama = x.BarangSatuan!.Barang!.Nama,
                            SatuanProduksi = x.BarangSatuan!.Barang!.SatuanProduksi,
                            Stok = x.BarangSatuan!.Barang!.Stok,
                            Version = x.BarangSatuan!.Barang!.Version
                        }
                    }
                })
            };
        }

        public async Task<Penjualan> Find1(string id)
        {
            Penjualan penjualan = await _appDbContext.Penjualan.Include(x => x.TransaksiPenjualan).FirstAsync(x => x.Id == id);
            return new Penjualan
            {
                Id = penjualan.Id,
                Tanggal = penjualan.Tanggal,
                JatuhTempo = penjualan.JatuhTempo,
                HariJatuhTempo = (penjualan.JatuhTempo! - penjualan.Tanggal.Date)?.Days ?? 1,
                GrandTotal = (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)),
                Terbayar = penjualan.Terbayar,
                Sisa = (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)) - penjualan.Terbayar,
                Status = penjualan.Status,
                Version = penjualan.Version,
                TransaksiPenjualan = penjualan.TransaksiPenjualan!.Select(x => new TransaksiPenjualan
                {
                    Id = x.Id,
                    PenjualanId = x.PenjualanId,
                    Tanggal = x.Tanggal,
                    Keterangan = x.Keterangan,
                    Nominal = x.Nominal,
                    Version = x.Version,
                    InputTanggal = x.Tanggal.Date,
                    InputWaktu = x.Tanggal.TimeOfDay,
                    NominalSebelum = x.Nominal,
                    Balance = (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)) - penjualan.Terbayar
                }).OrderBy(x => x.Tanggal).ToList()
            };
        }

        public async Task<Penjualan> Find2(string id)
        {
            Penjualan penjualan = await _appDbContext.Penjualan.Include(x => x.Customer!).Include(x => x.PenjualanDetail!).ThenInclude(x => x.BarangSatuan!).ThenInclude(x => x.Barang).FirstAsync(x => x.Id == id);
            return new Penjualan
            {
                Tanggal = penjualan.Tanggal,
                Subtotal = penjualan.Subtotal,
                PPN = penjualan.PPN,
                Terbayar = penjualan.Terbayar,
                MetodeBayar = penjualan.MetodeBayar,
                Status = penjualan.Status,
                JatuhTempo = penjualan.JatuhTempo,
                Keterangan = penjualan.Keterangan,
                HariJatuhTempo = (penjualan.JatuhTempo! - penjualan.Tanggal.Date)?.Days ?? 1,
                GrandTotal = (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)),
                Sisa = (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)) - penjualan.Terbayar,
                Customer = new Customer { Nama = penjualan.Customer!.Nama },
                PenjualanDetail = penjualan.PenjualanDetail!.ConvertAll(x => new PenjualanDetail
                {
                    Jumlah = x.Jumlah,
                    Harga = x.Harga,
                    Total = (int)(x.Jumlah * x.Harga),
                    BarangSatuan = new BarangSatuan
                    {
                        Nama = x.BarangSatuan!.Nama,
                        Ukuran = x.BarangSatuan!.Ukuran,
                        Barang = new Barang { Nama = x.BarangSatuan!.Barang!.Nama }
                    }
                })
            };
        }

        public async Task<Penjualan> Create(Penjualan penjualan)
        {
            penjualan.Tanggal = (DateTime)(penjualan.InputTanggal + penjualan.InputWaktu)!;
            penjualan.Id = GenerateId("PJLN", penjualan.Tanggal, _appDbContext.Penjualan.Where(x => x.Tanggal.Date == penjualan.Tanggal.Date).Select(x => x.Id));
            if (penjualan.MetodeBayar == "Tunai") penjualan.JatuhTempo = null;
            // Ambil TransaksiPenjualan (Jika ada DP)
            var transaksiPenjualan = penjualan.TransaksiPenjualan?.FirstOrDefault();
            // Membuat PenjualanDetail baru dan tambahkan stok barangnya
            var idsDetail = GenerateId(_appDbContext.PenjualanDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.PenjualanDetail.Select(x => x.Id), penjualan.PenjualanDetail!);
            for (int i = 0; i < penjualan.PenjualanDetail!.Count; i++)
            {
                penjualan.PenjualanDetail[i].Id = idsDetail[i];
                penjualan.PenjualanDetail[i].PenjualanId = penjualan.Id;
                var barangSatuan = await _appDbContext.BarangSatuan.Include(x => x.Barang).FirstAsync(y => y.Id == penjualan.PenjualanDetail[i].BarangSatuanId && y.Barang!.Version.SequenceEqual(penjualan.PenjualanDetail[i].BarangSatuan!.Barang!.Version));
                barangSatuan.Barang!.Stok -= penjualan.PenjualanDetail[i].Jumlah * penjualan.PenjualanDetail[i].BarangSatuan!.KonversiStok;

                if (barangSatuan.Barang!.Stok < 0) throw new DbUpdateException();
            }

            var penjualanDetail = Nullifies(penjualan.PenjualanDetail!);
            Nullify(penjualan);

            var result = await _appDbContext.Penjualan.AddAsync(penjualan);
            await _appDbContext.PenjualanDetail.AddRangeAsync(penjualanDetail);
            // Jika ada DP, insert ke database
            if (transaksiPenjualan is not null)
            {
                transaksiPenjualan.Id = GenerateId(_appDbContext.TransaksiPenjualan.Select(x => x.Id));
                transaksiPenjualan.PenjualanId = penjualan.Id;
                transaksiPenjualan.Nominal = transaksiPenjualan.Nominal < penjualan.GrandTotal ? transaksiPenjualan.Nominal : penjualan.GrandTotal;
                penjualan.Terbayar = transaksiPenjualan.Nominal;
                transaksiPenjualan.Tanggal = (DateTime)(penjualan.InputTanggal + penjualan.InputWaktu)!;
                await _appDbContext.TransaksiPenjualan.AddAsync(transaksiPenjualan);
            }
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Penjualan> CreatedPenjualan(string id)
        {
            return await _appDbContext.Penjualan.LastAsync(x => x.Id == id);
        }

        public async Task<Penjualan> Update(Penjualan penjualan)
        {
            // Ambil Penjualan yang akan di-update dari database
            Penjualan model = await _appDbContext.Penjualan.Include(x => x.TransaksiPenjualan).Include(x => x.PenjualanDetail!).ThenInclude(x => x.BarangSatuan!).ThenInclude(x => x.Barang).FirstAsync(x => x.Id == penjualan.Id && x.Version.SequenceEqual(penjualan.Version));
            // Jika setelah diupdate, Total harganya lebih kecil daripada yg terbayar. Maka kurangi transaksi pembayarannya mulai dari yg terbaru
            if (penjualan.PenjualanDetail!.Any() && model.Terbayar > penjualan.GrandTotal)
            {
                var selisih = model.Terbayar - penjualan.GrandTotal;
                model.Terbayar = penjualan.GrandTotal;
                foreach (var x in model.TransaksiPenjualan!.OrderByDescending(x => x.Tanggal).ThenByDescending(x => x.Id))
                {
                    int nominalLama = x.Nominal;
                    x.Nominal -= selisih;
                    Kurangi(ref selisih, nominalLama);
                    if (x.Nominal <= 0) _appDbContext.TransaksiPenjualan.Remove(x);
                    if (selisih < 0) break;
                }
            }

            model.Subtotal = penjualan.Subtotal;
            model.PPN = penjualan.PPN;
            model.Keterangan = penjualan.Keterangan;
            model.GrandTotal = penjualan.GrandTotal;
            model.JatuhTempo = penjualan.JatuhTempo;

            var result = await _appDbContext.ReturPenjualan.LastOrDefaultAsync(x => x.PenjualanId == penjualan.Id);
            if (result is not null && (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)) - penjualan.Terbayar - result.GrandTotal > 0)
                model.Status = "Belum Lunas";
            else
                model.Status = penjualan.Status;

            // Kembalikan (tambah) stok barangnya, karena PenjualanDetail akan dihapus dan di-insert ulang
            foreach (var x in model.PenjualanDetail!)
            {
                var barangSatuan = await _appDbContext.BarangSatuan.Include(y => y.Barang).FirstAsync(y => y.Id == x.BarangSatuanId && y.Barang!.Version.SequenceEqual(x.BarangSatuan!.Barang!.Version));
                barangSatuan.Barang!.Stok += x.Jumlah * x.BarangSatuan!.KonversiStok;
                var returPenjualanDetail = await _appDbContext.ReturPenjualanDetail.Include(y => y.ReturPenjualan).Include(y => y.BarangSatuan).FirstOrDefaultAsync(y => y.ReturPenjualan!.PenjualanId == x.PenjualanId && y.BarangSatuan!.Barang!.Version.SequenceEqual(x.BarangSatuan!.Barang!.Version));
                if (returPenjualanDetail is not null) returPenjualanDetail.MaxJumlah = x.Jumlah;
            }

            _appDbContext.PenjualanDetail.RemoveRange(await _appDbContext.PenjualanDetail.Where(x => x.PenjualanId == penjualan.Id).ToListAsync());
            // Setelah PembalianDetail lama dihapus, PembalianDetail baru di-insert. Stok Barangnya juga dikurangi kembali
            var idsDetail = GenerateId(_appDbContext.PenjualanDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.PenjualanDetail.Select(x => x.Id), penjualan.PenjualanDetail!);
            for (int i = 0; i < penjualan.PenjualanDetail!.Count; i++)
            {
                penjualan.PenjualanDetail[i].Id = idsDetail[i];
                var barangSatuan = await _appDbContext.BarangSatuan.Include(x => x.Barang).FirstAsync(y => y.Id == penjualan.PenjualanDetail[i].BarangSatuanId);
                barangSatuan.Barang!.Stok -= penjualan.PenjualanDetail[i].Jumlah * penjualan.PenjualanDetail[i].BarangSatuan!.KonversiStok;

                if (barangSatuan.Barang!.Stok < 0) throw new DbUpdateException();
            }
            // Karena tadi PenjualanDetail di-insert ulang, maka dia perlu ditambah lagi kedalam database. Tapi sebelum itu, dinullify dulu
            var penjualanDetail = Nullifies(penjualan.PenjualanDetail!);

            await _appDbContext.PenjualanDetail.AddRangeAsync(penjualanDetail);
            await _appDbContext.SaveChangesAsync();

            return penjualan;
        }

        public async Task<bool> Deletable(string id)
        {
            return await _appDbContext.Penjualan.AnyAsync(x => x.Id == id && x.ReturPenjualan == null);
        }

        public async Task Delete(string id)
        {
            var result = await _appDbContext.Penjualan.Include(x => x.PenjualanDetail!).ThenInclude(x => x.BarangSatuan!).ThenInclude(x => x.Barang).FirstAsync(x => x.Id == id);
            if (result is not null)
            {
                foreach (var x in result.PenjualanDetail!)
                {
                    var barangSatuan = await _appDbContext.BarangSatuan.Include(x => x.Barang).FirstAsync(y => y.Id == x.BarangSatuanId && y.Barang!.Version.SequenceEqual(x.BarangSatuan!.Barang!.Version));
                    barangSatuan.Barang!.Stok += x.Jumlah * x.BarangSatuan!.KonversiStok;

                    if (barangSatuan.Barang!.Stok < 0) throw new DbUpdateException();
                }
                _appDbContext.Penjualan.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<PenjualanDetail>> GetDetail()
        {
            return await _appDbContext.PenjualanDetail.ToListAsync();
        }

        public async Task<List<PenjualanDetail>> FindDetail(string penjualanId)
        {
            return await _appDbContext.PenjualanDetail.Include(x => x.BarangSatuan!).Where(x => x.PenjualanId == penjualanId).Select(x => new PenjualanDetail
            {
                BarangSatuanId = x.BarangSatuanId,
                Harga = x.Harga,
                Jumlah = x.Jumlah,
                BarangSatuan = new BarangSatuan
                {
                    Id = x.BarangSatuan!.Id,
                    Nama = x.BarangSatuan!.Nama,
                    Ukuran = x.BarangSatuan!.Ukuran,
                    KonversiStok = x.BarangSatuan!.KonversiStok,
                    Barang = new Barang
                    {
                        Nama = x.BarangSatuan!.Barang!.Nama,
                        Stok = x.BarangSatuan!.Barang!.Stok,
                        Version = x.BarangSatuan!.Barang!.Version
                    }
                }
            }).ToListAsync();
        }

        public async Task<List<BarangSatuan>> RefreshDetail(string id, List<int> barangSatuanIds)
        {
            if (!string.IsNullOrEmpty(id) && !await _appDbContext.Penjualan.AnyAsync(x => x.Id == id)) return null!;
            return await _appDbContext.BarangSatuan.Include(x => x.Barang).Where(x => barangSatuanIds.Contains(x.Id)).Select(x => new BarangSatuan
            {
                Id = x.Id,
                BarangId = x.BarangId,
                Nama = x.Nama,
                Ukuran = x.Ukuran,
                Harga = x.Harga,
                KonversiStok = x.KonversiStok,
                Barang = new Barang
                {
                    Nama = x.Barang!.Nama,
                    SatuanProduksi = x.Barang!.SatuanProduksi,
                    Stok = x.Barang!.Stok,
                    Version = x.Barang!.Version
                }
            }).ToListAsync();
        }

        public async Task<bool> DeletableDetail(string penjualanId, int barangSatuanId)
        {
            return await _appDbContext.Penjualan.AnyAsync(x => x.ReturPenjualan!.ReturPenjualanDetail!.Any(y => x.Id == penjualanId && y.Jumlah == 0 && y.BarangSatuanId == barangSatuanId) || (x.Id == penjualanId && x.ReturPenjualan == null));
        }

        public async Task<List<TransaksiPenjualan>> GetTransaksi()
        {
            return await _appDbContext.TransaksiPenjualan.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<TransaksiPenjualan> FindTransaksi(int id)
        {
            return await _appDbContext.TransaksiPenjualan.FirstAsync(x => x.Id == id);
        }

        public async Task<TransaksiPenjualan> CreateTransaksi(TransaksiPenjualan transaksiPenjualan)
        {
            Penjualan model = await _appDbContext.Penjualan.Include(x => x.PenjualanDetail).Include(x => x.TransaksiPenjualan).FirstAsync(x => x.Id == transaksiPenjualan.PenjualanId && x.Version.SequenceEqual(transaksiPenjualan.Penjualan!.Version));
            model.Status = model.Terbayar + transaksiPenjualan.Nominal >= (int)(model.Subtotal * ((model.PPN + 100) / 100m)) ? "Lunas" : "Belum Lunas";
            model.Terbayar += transaksiPenjualan.Nominal;

            transaksiPenjualan.Id = GenerateId(_appDbContext.TransaksiPenjualan.Select(x => x.Id));
            transaksiPenjualan.Tanggal = (DateTime)(transaksiPenjualan.InputTanggal + transaksiPenjualan.InputWaktu)!;
            transaksiPenjualan.PenjualanId = model.Id;

            Nullify(transaksiPenjualan);
            var result = await _appDbContext.TransaksiPenjualan.AddAsync(transaksiPenjualan);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<TransaksiPenjualan> CreatedTransaksi(int id)
        {
            return await _appDbContext.TransaksiPenjualan.LastAsync(x => x.Id == id);
        }

        public async Task<TransaksiPenjualan> UpdateTransaksi(TransaksiPenjualan transaksiPenjualan)
        {
            Penjualan model = await _appDbContext.Penjualan.Include(x => x.PenjualanDetail).Include(x => x.TransaksiPenjualan).FirstAsync(x => x.Id == transaksiPenjualan.PenjualanId && x.Version.SequenceEqual(transaksiPenjualan.Penjualan!.Version));
            model.Status = model.TransaksiPenjualan!.Where(x => x.Id != transaksiPenjualan.Id).Sum(y => y.Nominal) + transaksiPenjualan.Nominal >= (int)(model.Subtotal * ((model.PPN + 100) / 100m)) ? "Lunas" : "Belum Lunas";
            model.Terbayar += transaksiPenjualan.Nominal - transaksiPenjualan.NominalSebelum;

            TransaksiPenjualan transaksiPenjualanModel = model.TransaksiPenjualan!.First(x => x.Id == transaksiPenjualan.Id && x.Version.SequenceEqual(transaksiPenjualan.Version));
            transaksiPenjualanModel.Tanggal = (DateTime)(transaksiPenjualan.InputTanggal + transaksiPenjualan.InputWaktu)!;
            transaksiPenjualanModel.Keterangan = transaksiPenjualan.Keterangan;
            transaksiPenjualanModel.Nominal = transaksiPenjualan.Nominal;

            await _appDbContext.SaveChangesAsync();

            return transaksiPenjualan;
        }

        public async Task DeleteTransaksi(int id)
        {
            TransaksiPenjualan transaksiPenjualan = await _appDbContext.TransaksiPenjualan.FirstAsync(x => x.Id == id);

            Penjualan model = await _appDbContext.Penjualan.FirstAsync(x => x.Id == transaksiPenjualan.PenjualanId);
            model.Status = "Belum Lunas";
            model.Terbayar -= transaksiPenjualan.Nominal;

            _appDbContext.TransaksiPenjualan.Remove(transaksiPenjualan);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<ReturPenjualan>> GetRetur()
        {
            return await _appDbContext.ReturPenjualan.Include(x => x.Penjualan!).ThenInclude(x => x.Customer).Select(x => new ReturPenjualan
            {
                Id = x.Id,
                Tanggal = x.Tanggal,
                Keterangan = x.Keterangan,
                GrandTotal = x.GrandTotal,
                Penjualan = new Penjualan { Customer = new Customer { Nama = x.Penjualan!.Customer!.Nama } }
            }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<ReturPenjualan> FindRetur(string id)
        {
            ReturPenjualan returPenjualan = (await _appDbContext.ReturPenjualan.Include(x => x.ReturPenjualanDetail).Include(x => x.Penjualan!).ThenInclude(x => x.PenjualanDetail!).ThenInclude(x => x.BarangSatuan!).ThenInclude(x => x.Barang).FirstOrDefaultAsync(x => x.Id == id))!;
            return returPenjualan is null ? null! : new ReturPenjualan
            {
                Id = returPenjualan.Id,
                PenjualanId = returPenjualan.PenjualanId,
                Keterangan = returPenjualan.Keterangan,
                Version = returPenjualan.Version,
                InputTanggal = returPenjualan.Tanggal.Date,
                InputWaktu = returPenjualan.Tanggal.TimeOfDay,
                GrandTotal = returPenjualan.GrandTotal,
                Penjualan = returPenjualan.Penjualan,
                ReturPenjualanDetail = returPenjualan.ReturPenjualanDetail!.ConvertAll(y => new ReturPenjualanDetail
                {
                    BarangSatuanId = y.BarangSatuanId,
                    Harga = returPenjualan.Penjualan!.PenjualanDetail!.First(x => x.BarangSatuanId == y.BarangSatuanId).Harga,
                    Jumlah = y.Jumlah,
                    MaxJumlah = y.MaxJumlah,
                    Total = (int)(y.Jumlah * y.Harga),
                    BarangSatuan = new BarangSatuan
                    {
                        Id = y.BarangSatuan!.Id,
                        BarangId = y.BarangSatuan!.BarangId,
                        Nama = y.BarangSatuan!.Nama,
                        Ukuran = y.BarangSatuan!.Ukuran,
                        KonversiStok = y.BarangSatuan!.KonversiStok,
                        Barang = new Barang
                        {
                            Nama = y.BarangSatuan!.Barang!.Nama,
                            SatuanProduksi = y.BarangSatuan!.Barang!.SatuanProduksi,
                            Stok = y.BarangSatuan!.Barang!.Stok,
                            Version = y.BarangSatuan!.Barang!.Version
                        }
                    }
                })
            };
        }

        public async Task<ReturPenjualan> FindRetur1(string id)
        {
            ReturPenjualan returPenjualan = await _appDbContext.ReturPenjualan.Include(x => x.ReturPenjualanDetail!).ThenInclude(x => x.BarangSatuan!).ThenInclude(x => x.Barang).FirstAsync(x => x.Id == id);
            return returPenjualan is null ? null! : new ReturPenjualan
            {
                PenjualanId = returPenjualan.PenjualanId,
                Tanggal = returPenjualan.Tanggal,
                GrandTotal = returPenjualan.GrandTotal,
                Keterangan = returPenjualan.Keterangan,
                ReturPenjualanDetail = returPenjualan.ReturPenjualanDetail!.Where(y => y.Jumlah > 0).Select(y => new ReturPenjualanDetail
                {
                    BarangSatuanId = y.BarangSatuanId,
                    Jumlah = y.Jumlah,
                    Harga = y.Harga,
                    Total = (int)(y.Jumlah * y.Harga),
                    BarangSatuan = new BarangSatuan
                    {
                        Nama = y.BarangSatuan!.Nama,
                        Ukuran = y.BarangSatuan!.Ukuran,
                        Barang = new Barang { Nama = y.BarangSatuan!.Barang!.Nama }
                    }
                }).ToList()
            };
        }

        public async Task<ReturPenjualan> CreateRetur(ReturPenjualan returPenjualan)
        {
            returPenjualan.Tanggal = (DateTime)(returPenjualan.InputTanggal + returPenjualan.InputWaktu)!;
            // Membuat ReturPenjualanDetail baru dan kurangi stok barangnya
            var idsDetail = GenerateId(_appDbContext.ReturPenjualanDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.ReturPenjualanDetail.Select(x => x.Id), returPenjualan.ReturPenjualanDetail!);
            for (int i = 0; i < returPenjualan.ReturPenjualanDetail!.Count; i++)
            {
                returPenjualan.ReturPenjualanDetail[i].Id = idsDetail[i];
                returPenjualan.ReturPenjualanDetail[i].ReturPenjualanId = returPenjualan.Id;
                var barangSatuan = await _appDbContext.BarangSatuan.Include(x => x.Barang).FirstAsync(y => y.Id == returPenjualan.ReturPenjualanDetail[i].BarangSatuanId && y.Barang!.Version.SequenceEqual(returPenjualan.ReturPenjualanDetail[i].BarangSatuan!.Barang!.Version));
                barangSatuan.Barang!.Stok += returPenjualan.ReturPenjualanDetail[i].Jumlah * returPenjualan.ReturPenjualanDetail[i].BarangSatuan!.KonversiStok;

                //if (barangSatuan.Barang!.Stok < 0) throw new DbUpdateException();
            }

            var penjualan = await _appDbContext.Penjualan.OrderBy(x => x.Id).LastAsync(x => x.Id == returPenjualan.PenjualanId);
            if (returPenjualan.GrandTotal >= (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)) - penjualan.Terbayar)
                penjualan.Status = "Lunas";

            List<PenjualanDetail> penjualanDetail = await _appDbContext.PenjualanDetail.Include(x => x.BarangSatuan).Where(x => x.PenjualanId == returPenjualan.PenjualanId).ToListAsync();
            foreach (var item in penjualanDetail) item.MinJumlah = returPenjualan.ReturPenjualanDetail!.First(x => x.BarangSatuanId == item.BarangSatuanId).Jumlah;

            var returPenjualanDetail = Nullifies(returPenjualan.ReturPenjualanDetail!);
            Nullify(returPenjualan);

            var result = await _appDbContext.ReturPenjualan.AddAsync(returPenjualan);
            await _appDbContext.ReturPenjualanDetail.AddRangeAsync(returPenjualanDetail);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<ReturPenjualan> CreatedRetur(string id)
        {
            return await _appDbContext.ReturPenjualan.LastAsync(x => x.Id == id);
        }

        public async Task<ReturPenjualan> UpdateRetur(ReturPenjualan returPenjualan)
        {
            // Ambil Penjualan yang akan di-update dari database
            ReturPenjualan model = await _appDbContext.ReturPenjualan.Include(x => x.ReturPenjualanDetail!).ThenInclude(x => x.BarangSatuan!).ThenInclude(x => x.Barang).FirstAsync(x => x.Id == returPenjualan.Id && x.Version.SequenceEqual(returPenjualan.Version));
            model.Tanggal = (DateTime)(returPenjualan.InputTanggal + returPenjualan.InputWaktu)!;
            model.GrandTotal = returPenjualan.GrandTotal;
            model.Keterangan = returPenjualan.Keterangan;
            // Kembalikan stok barangnya dengan jumlah lama, kemudian kurangi stok barangnya dengan jumlah baru dan update ReturPenjualanDetail pada field Jumlah
            foreach (var x in model.ReturPenjualanDetail!)
            {
                var barangSatuan = await _appDbContext.BarangSatuan.Include(y => y.Barang).FirstAsync(y => y.Id == x.BarangSatuanId && y.Barang!.Version.SequenceEqual(x.BarangSatuan!.Barang!.Version));
                decimal jumlah = returPenjualan.ReturPenjualanDetail!.First(y => y.BarangSatuanId == barangSatuan.Id).Jumlah;
                if (jumlah != x.Jumlah)
                {
                    barangSatuan.Barang!.Stok += (jumlah - x.Jumlah) * barangSatuan.KonversiStok;
                    x.Jumlah = jumlah;
                }

                if (barangSatuan.Barang!.Stok < 0) throw new DbUpdateException();
            }

            var penjualan = await _appDbContext.Penjualan.OrderBy(x => x.Id).LastAsync(x => x.Id == returPenjualan.PenjualanId);
            if (returPenjualan.GrandTotal >= (int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)) - penjualan.Terbayar)
                penjualan.Status = "Lunas";

            List<PenjualanDetail> penjualanDetail = await _appDbContext.PenjualanDetail.Include(x => x.BarangSatuan).Where(x => x.PenjualanId == returPenjualan.PenjualanId).ToListAsync();
            foreach (var item in penjualanDetail) item.MinJumlah = returPenjualan.ReturPenjualanDetail!.First(x => x.BarangSatuanId == item.BarangSatuanId && x.BarangSatuan!.Barang!.Version.SequenceEqual(item.BarangSatuan!.Barang!.Version)).Jumlah;

            await _appDbContext.SaveChangesAsync();

            return returPenjualan;
        }

        public async Task DeleteRetur(string id)
        {
            var result = await _appDbContext.ReturPenjualan.Include(x => x.ReturPenjualanDetail!).ThenInclude(x => x.BarangSatuan!).ThenInclude(x => x.Barang).FirstAsync(x => x.Id == id);
            if (result is not null)
            {
                foreach (var x in result.ReturPenjualanDetail!)
                {
                    var barangSatuan = await _appDbContext.BarangSatuan.Include(x => x.Barang).FirstAsync(y => y.Id == x.BarangSatuanId && y.Barang!.Version.SequenceEqual(x.BarangSatuan!.Barang!.Version));
                    barangSatuan.Barang!.Stok -= x.Jumlah * x.BarangSatuan!.KonversiStok;

                    if (barangSatuan.Barang!.Stok < 0) throw new DbUpdateException();
                }

                var penjualan = await _appDbContext.Penjualan.OrderBy(x => x.Id).LastAsync(x => x.Id == result.PenjualanId);
                if ((int)(penjualan.Subtotal * ((penjualan.PPN + 100) / 100m)) - penjualan.Terbayar - result.GrandTotal > 0)
                    penjualan.Status = "Belum Lunas";

                List<PenjualanDetail> penjualanDetail = await _appDbContext.PenjualanDetail.Where(x => x.PenjualanId == result.PenjualanId).ToListAsync();
                foreach (var item in penjualanDetail) item.MinJumlah = 0;

                _appDbContext.ReturPenjualan.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ReturPenjualanDetail>> GetReturDetail()
        {
            return await _appDbContext.ReturPenjualanDetail.ToListAsync();
        }

        public async Task<List<BarangSatuan>> RefreshReturDetail(string returId)
        {
            List<BarangSatuan> result = await _appDbContext.ReturPenjualanDetail.Include(x => x.BarangSatuan!).Where(x => x.ReturPenjualanId == returId).Select(x => new BarangSatuan
            {
                Id = x.BarangSatuan!.Id,
                BarangId = x.BarangSatuan!.BarangId,
                Nama = x.BarangSatuan!.Nama,
                Ukuran = x.BarangSatuan!.Ukuran,
                Barang = new Barang
                {
                    Nama = x.BarangSatuan!.Barang!.Nama,
                    SatuanProduksi = x.BarangSatuan!.Barang!.SatuanProduksi,
                    Stok = x.BarangSatuan!.Barang!.Stok,
                    Version = x.BarangSatuan!.Barang!.Version
                }
            }).ToListAsync();
            return result.Any() ? result : null!;
        }
    }
}