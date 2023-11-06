using ProduksiManufaktur.Models;

namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Pembelian, R PembelianDetail, CRUD TransaksiPembelian, CRUD ReturPembelian, R ReturPembelianDetail</summary>
    public interface IPembelianRepository
    {
        /// <summary>List Pembelian { Id, Tanggal, Status, GrandTotal, Supplier { Nama } } > PembelianList</summary>
        Task<List<Pembelian>> Get();

        /// <summary>List Pembelian { Id, Tanggal } > ReturPembelianForm (Untuk autocomplete, memilih pembelian yang belum retur untuk diretur)</summary>
        Task<List<Pembelian>> Get1();

        /// <summary>Pembelian { Id, SupplierId, InputTanggal, InputWaktu, JatuhTempo, PPN, Keterangan, Version, HariJatuhTempo, Terbayar, Supplier { Id, Nama }, List PembelianDetail { Id, PembelianId, BahanSatuanId, MinJumlah, Jumlah, Harga, JumlahSebelum, StokAkhir, Total, BahanSatuan { BahanId, Nama, Ukuran, Harga, KonversiStok, Bahan { Nama, SatuanProduksi, Stok, Version } } } } > PembelianForm (Untuk autocomplete, memilih pembelian yang ingin diedit)</summary>
        Task<Pembelian> Find(string id);

        /// <summary>Pembelian { Id, Tanggal, JatuhTempo, HariJatuhTempo, GrandTotal, Terbayar, Sisa, Status, Version, List TransaksiPembelian { Id, PembelianId, Tanggal, Keterangan, Nominal, Version, InputTanggal, InputWaktu, NominalSebelum, Balance } } > TransaksiPembelianForm (Memuat semua transaksi pada pembelian tertentu)</summary>
        Task<Pembelian> Find1(string id);

        /// <summary>Pembelian { Tanggal, Subtotal, PPN, Terbayar, MetodeBayar, Status, JatuhTempo, Keterangan, HariJatuhTempo, GrandTotal, Sisa, Supplier { Nama }, List PembelianDetail { Jumlah, Harga, Total, BahanSatuan { Nama, Ukuran, Bahan { Nama } } } } > PembelianInfo</summary>
        Task<Pembelian> Find2(string id);

        Task<Pembelian> Create(Pembelian pembelian);

        Task<Pembelian> CreatedPembelian(string id);

        Task<Pembelian> Update(Pembelian pembelian);

        Task<bool> Deletable(string id);

        Task Delete(string id);

        /// <summary>List PembelianDetail</summary>
        Task<List<PembelianDetail>> GetDetail();

        /// <summary>List PembelianDetail { BahanSatuanId, Harga, Jumlah, BahanSatuan { Id, Nama, Ukuran, Bahan { Nama, Stok, Version } } } > ReturPembelianForm (Untuk PilihPembelian, PembelianDetail akan dimasukkan ke ReturDetail)</summary>
        Task<List<PembelianDetail>> FindDetail(string pembelianId);

        /// <summary>List BahanSatuan { Id, Nama, Ukuran, Harga, KonversiStok, Bahan { Nama, SatuanProduksi, Version } } > PembelianForm</summary>
        Task<List<BahanSatuan>> RefreshDetail(string id, List<int> bahanSatuanIds);

        Task<bool> DeletableDetail(string pembelianId, int bahanSatuanId);

        /// <summary>List TransaksiPembelian</summary>
        Task<List<TransaksiPembelian>> GetTransaksi();

        /// <summary>TransaksiPembelian</summary>
        Task<TransaksiPembelian> FindTransaksi(int id);

        Task<TransaksiPembelian> CreateTransaksi(TransaksiPembelian transaksiPembelian);

        Task<TransaksiPembelian> CreatedTransaksi(int id);

        Task<TransaksiPembelian> UpdateTransaksi(TransaksiPembelian transaksiPembelian);

        Task DeleteTransaksi(int id);

        /// <summary>List ReturPembelian { Id, Tanggal, Keterangan, GrandTotal, Pembelian { Supplier { Nama } } } > ReturPembelianList</summary>
        Task<List<ReturPembelian>> GetRetur();

        /// <summary>ReturPembelian { Id, PembelianId, Keterangan, Version, InputTanggal, InputWaktu, GrandTotal, Pembelian, List ReturPembelianDetail { BahanSatuanId, Harga, Jumlah, MaxJumlah, Total, BahanSatuan { Id, BahanId, Nama, Ukuran, Bahan { Nama, SatuanProduksi, Stok, Version } } } } > ReturPembelianForm</summary>
        Task<ReturPembelian> FindRetur(string id);

        /// <summary>ReturPembelian { PembelianId, Tanggal, GrandTotal, Keterangan, List ReturPembelianDetail { BahanSatuanId, Jumlah, Harga, Total, BahanSatuan { Nama, Ukuran, Bahan { Nama } } } } > ReturPembelianInfo</summary>
        Task<ReturPembelian> FindRetur1(string id);

        Task<ReturPembelian> CreateRetur(ReturPembelian returPembelian);

        Task<ReturPembelian> CreatedRetur(string id);

        Task<ReturPembelian> UpdateRetur(ReturPembelian returPembelian);

        Task DeleteRetur(string id);

        /// <summary>List ReturPembelianDetail</summary>
        Task<List<ReturPembelianDetail>> GetReturDetail();

        /// <summary>List BahanSatuan { Id, BahanId, Nama, Ukuran, Bahan { Nama, SatuanProduksi, Stok, Version } } > ReturPembelianForm</summary>
        Task<List<BahanSatuan>> RefreshReturDetail(string returId);
    }

    public class PembelianRepository : IPembelianRepository
    {
        private readonly AppDbContext _appDbContext;

        public PembelianRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Pembelian>> Get()
        {
            return await _appDbContext.Pembelian.Include(x => x.Supplier).Select(x => new Pembelian
            {
                Id = x.Id,
                Tanggal = x.Tanggal,
                Status = x.Status,
                GrandTotal = (int)(x.Subtotal * ((x.PPN + 100) / 100m)),
                Supplier = new() { Nama = x.Supplier!.Nama }
            }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<List<Pembelian>> Get1()
        {
            return await _appDbContext.Pembelian.Where(x => x.ReturPembelian == null).Select(x => new Pembelian { Id = x.Id, Tanggal = x.Tanggal }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Pembelian> Find(string id)
        {
            Pembelian pembelian = await _appDbContext.Pembelian.Include(x => x.Supplier!).Include(x => x.PembelianDetail!).ThenInclude(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == id);
            return new Pembelian
            {
                Id = pembelian.Id,
                SupplierId = pembelian.SupplierId,
                InputTanggal = pembelian.Tanggal.Date,
                InputWaktu = pembelian.Tanggal.TimeOfDay,
                JatuhTempo = pembelian.JatuhTempo,
                PPN = pembelian.PPN,
                Keterangan = pembelian.Keterangan,
                Version = pembelian.Version,
                HariJatuhTempo = (pembelian.JatuhTempo! - pembelian.Tanggal.Date)?.Days ?? 1,
                Terbayar = pembelian.Terbayar,
                Supplier = new Supplier { Id = pembelian.Supplier!.Id, Nama = pembelian.Supplier!.Nama },
                PembelianDetail = pembelian.PembelianDetail!.ConvertAll(x => new PembelianDetail
                {
                    Id = x.Id,
                    PembelianId = x.PembelianId,
                    BahanSatuanId = x.BahanSatuanId,
                    MinJumlah = x.MinJumlah,
                    Jumlah = x.Jumlah,
                    Harga = x.Harga,
                    JumlahSebelum = x.Jumlah,
                    StokAkhir = x.BahanSatuan!.Bahan!.Stok,
                    Total = (int)(x.Jumlah * x.Harga),
                    BahanSatuan = new BahanSatuan
                    {
                        BahanId = x.BahanSatuan!.BahanId,
                        Nama = x.BahanSatuan!.Nama,
                        Ukuran = x.BahanSatuan!.Ukuran,
                        Harga = x.BahanSatuan!.Harga,
                        KonversiStok = x.BahanSatuan!.KonversiStok,
                        Bahan = new Bahan
                        {
                            Nama = x.BahanSatuan!.Bahan!.Nama,
                            SatuanProduksi = x.BahanSatuan!.Bahan!.SatuanProduksi,
                            Stok = x.BahanSatuan!.Bahan!.Stok,
                            Version = x.BahanSatuan!.Bahan!.Version
                        }
                    }
                })
            };
        }

        public async Task<Pembelian> Find1(string id)
        {
            Pembelian pembelian = await _appDbContext.Pembelian.Include(x => x.TransaksiPembelian).FirstAsync(x => x.Id == id);
            return new Pembelian
            {
                Id = pembelian.Id,
                Tanggal = pembelian.Tanggal,
                JatuhTempo = pembelian.JatuhTempo,
                HariJatuhTempo = (pembelian.JatuhTempo! - pembelian.Tanggal.Date)?.Days ?? 1,
                GrandTotal = (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)),
                Terbayar = pembelian.Terbayar,
                Sisa = (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)) - pembelian.Terbayar,
                Status = pembelian.Status,
                Version = pembelian.Version,
                TransaksiPembelian = pembelian.TransaksiPembelian!.Select(x => new TransaksiPembelian
                {
                    Id = x.Id,
                    PembelianId = x.PembelianId,
                    Tanggal = x.Tanggal,
                    Keterangan = x.Keterangan,
                    Nominal = x.Nominal,
                    Version = x.Version,
                    InputTanggal = x.Tanggal.Date,
                    InputWaktu = x.Tanggal.TimeOfDay,
                    NominalSebelum = x.Nominal,
                    Balance = (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)) - pembelian.Terbayar
                }).OrderBy(x => x.Tanggal).ToList()
            };
        }

        public async Task<Pembelian> Find2(string id)
        {
            Pembelian pembelian = await _appDbContext.Pembelian.Include(x => x.Supplier!).Include(x => x.PembelianDetail!).ThenInclude(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == id);
            return new Pembelian
            {
                Tanggal = pembelian.Tanggal,
                Subtotal = pembelian.Subtotal,
                PPN = pembelian.PPN,
                Terbayar = pembelian.Terbayar,
                MetodeBayar = pembelian.MetodeBayar,
                Status = pembelian.Status,
                JatuhTempo = pembelian.JatuhTempo,
                Keterangan = pembelian.Keterangan,
                HariJatuhTempo = (pembelian.JatuhTempo! - pembelian.Tanggal.Date)?.Days ?? 1,
                GrandTotal = (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)),
                Sisa = (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)) - pembelian.Terbayar,
                Supplier = new Supplier { Nama = pembelian.Supplier!.Nama },
                PembelianDetail = pembelian.PembelianDetail!.ConvertAll(x => new PembelianDetail
                {
                    Jumlah = x.Jumlah,
                    Harga = x.Harga,
                    Total = (int)(x.Jumlah * x.Harga),
                    BahanSatuan = new BahanSatuan
                    {
                        Nama = x.BahanSatuan!.Nama,
                        Ukuran = x.BahanSatuan!.Ukuran,
                        Bahan = new Bahan { Nama = x.BahanSatuan!.Bahan!.Nama }
                    }
                })
            };
        }

        public async Task<Pembelian> Create(Pembelian pembelian)
        {
            pembelian.Tanggal = (DateTime)(pembelian.InputTanggal + pembelian.InputWaktu)!;
            pembelian.Id = GenerateId("PBLN", pembelian.Tanggal, _appDbContext.Pembelian.Where(x => x.Tanggal.Date == pembelian.Tanggal.Date).Select(x => x.Id));
            if (pembelian.MetodeBayar == "Tunai") pembelian.JatuhTempo = null;
            // Ambil TransaksiPembelian (Jika ada DP)
            var transaksiPembelian = pembelian.TransaksiPembelian?.FirstOrDefault();
            // Membuat PembelianDetail baru dan tambahkan stok bahannya
            var idsDetail = GenerateId(_appDbContext.PembelianDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.PembelianDetail.Select(x => x.Id), pembelian.PembelianDetail!);
            for (int i = 0; i < pembelian.PembelianDetail!.Count; i++)
            {
                pembelian.PembelianDetail[i].Id = idsDetail[i];
                pembelian.PembelianDetail[i].PembelianId = pembelian.Id;
                var bahanSatuan = await _appDbContext.BahanSatuan.Include(x => x.Bahan).FirstAsync(y => y.Id == pembelian.PembelianDetail[i].BahanSatuanId && y.Bahan!.Version.SequenceEqual(pembelian.PembelianDetail[i].BahanSatuan!.Bahan!.Version));
                bahanSatuan.Bahan!.Stok += pembelian.PembelianDetail[i].Jumlah * pembelian.PembelianDetail[i].BahanSatuan!.KonversiStok;
            }

            var pembelianDetail = Nullifies(pembelian.PembelianDetail!);
            Nullify(pembelian);

            var result = await _appDbContext.Pembelian.AddAsync(pembelian);
            await _appDbContext.PembelianDetail.AddRangeAsync(pembelianDetail);
            // Jika ada DP, insert ke database
            if (transaksiPembelian is not null)
            {
                transaksiPembelian.Id = GenerateId(_appDbContext.TransaksiPembelian.Select(x => x.Id));
                transaksiPembelian.PembelianId = pembelian.Id;
                transaksiPembelian.Nominal = transaksiPembelian.Nominal < pembelian.GrandTotal ? transaksiPembelian.Nominal : pembelian.GrandTotal;
                pembelian.Terbayar = transaksiPembelian.Nominal;
                transaksiPembelian.Tanggal = (DateTime)(pembelian.InputTanggal + pembelian.InputWaktu)!;
                await _appDbContext.TransaksiPembelian.AddAsync(transaksiPembelian);
            }
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Pembelian> CreatedPembelian(string id)
        {
            return await _appDbContext.Pembelian.LastAsync(x => x.Id == id);
        }

        public async Task<Pembelian> Update(Pembelian pembelian)
        {
            // Ambil Pembelian yang akan di-update dari database
            Pembelian model = await _appDbContext.Pembelian.Include(x => x.TransaksiPembelian).Include(x => x.PembelianDetail!).ThenInclude(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == pembelian.Id && x.Version.SequenceEqual(pembelian.Version));
            // Jika setelah diupdate, Total harganya lebih kecil daripada yg terbayar. Maka kurangi transaksi pembayarannya mulai dari yg terbaru
            if (pembelian.PembelianDetail!.Any() && model.Terbayar > pembelian.GrandTotal)
            {
                var selisih = model.Terbayar - pembelian.GrandTotal;
                model.Terbayar = pembelian.GrandTotal;
                foreach (var x in model.TransaksiPembelian!.OrderByDescending(x => x.Tanggal).ThenByDescending(x => x.Id))
                {
                    int nominalLama = x.Nominal;
                    x.Nominal -= selisih;
                    Kurangi(ref selisih, nominalLama);
                    if (x.Nominal <= 0) _appDbContext.TransaksiPembelian.Remove(x);
                    if (selisih < 0) break;
                }
            }

            model.Subtotal = pembelian.Subtotal;
            model.PPN = pembelian.PPN;
            model.Keterangan = pembelian.Keterangan;
            model.Status = pembelian.Status;
            model.GrandTotal = pembelian.GrandTotal;
            model.JatuhTempo = pembelian.JatuhTempo;

            var result = await _appDbContext.ReturPembelian.OrderByDescending(x => x.PembelianId == pembelian.Id).FirstOrDefaultAsync(x => x.PembelianId == pembelian.Id);
            if (result is not null && (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)) - pembelian.Terbayar - result.GrandTotal > 0)
                model.Status = "Belum Lunas";
            else
                model.Status = pembelian.Status;

            // Kembalikan (kurangi) stok bahannya, karena PembelianDetail akan dihapus dan di-insert ulang
            foreach (var x in model.PembelianDetail!)
            {
                var bahanSatuan = await _appDbContext.BahanSatuan.Include(y => y.Bahan).FirstAsync(y => y.Id == x.BahanSatuanId && y.Bahan!.Version.SequenceEqual(x.BahanSatuan!.Bahan!.Version));
                bahanSatuan.Bahan!.Stok -= x.Jumlah * x.BahanSatuan!.KonversiStok;
                var returPembelianDetail = await _appDbContext.ReturPembelianDetail.Include(y => y.ReturPembelian).Include(y => y.BahanSatuan).FirstOrDefaultAsync(y => y.ReturPembelian!.PembelianId == x.PembelianId && y.BahanSatuan!.Bahan!.Version.SequenceEqual(x.BahanSatuan!.Bahan!.Version));
                if (returPembelianDetail is not null) returPembelianDetail.MaxJumlah = x.Jumlah;
            }

            _appDbContext.PembelianDetail.RemoveRange(await _appDbContext.PembelianDetail.Where(x => x.PembelianId == pembelian.Id).ToListAsync());
            // Setelah PembalianDetail lama dihapus, PembalianDetail baru di-insert. Stok Bahannya juga ditambah kembali
            var idsDetail = GenerateId(_appDbContext.PembelianDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.PembelianDetail.Select(x => x.Id), pembelian.PembelianDetail!);
            for (int i = 0; i < pembelian.PembelianDetail!.Count; i++)
            {
                pembelian.PembelianDetail[i].Id = idsDetail[i];
                var bahanSatuan = await _appDbContext.BahanSatuan.Include(x => x.Bahan).FirstAsync(y => y.Id == pembelian.PembelianDetail[i].BahanSatuanId);
                bahanSatuan.Bahan!.Stok += pembelian.PembelianDetail[i].Jumlah * pembelian.PembelianDetail[i].BahanSatuan!.KonversiStok;

                if (bahanSatuan.Bahan!.Stok < 0) throw new DbUpdateException();
            }
            // Karena tadi PembelianDetail di-insert ulang, maka dia perlu ditambah lagi kedalam database. Tapi sebelum itu, dinullify dulu
            var pembelianDetail = Nullifies(pembelian.PembelianDetail!);

            await _appDbContext.PembelianDetail.AddRangeAsync(pembelianDetail);
            await _appDbContext.SaveChangesAsync();

            return pembelian;
        }

        public async Task<bool> Deletable(string id)
        {
            return await _appDbContext.Pembelian.AnyAsync(x => x.Id == id && x.ReturPembelian == null);
        }

        public async Task Delete(string id)
        {
            var result = await _appDbContext.Pembelian.Include(x => x.PembelianDetail!).ThenInclude(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == id);
            if (result is not null)
            {
                foreach (var x in result.PembelianDetail!)
                {
                    var bahanSatuan = await _appDbContext.BahanSatuan.Include(x => x.Bahan).FirstAsync(y => y.Id == x.BahanSatuanId && y.Bahan!.Version.SequenceEqual(x.BahanSatuan!.Bahan!.Version));
                    bahanSatuan.Bahan!.Stok -= x.Jumlah * x.BahanSatuan!.KonversiStok;

                    if (bahanSatuan.Bahan!.Stok < 0) throw new DbUpdateException();
                }
                _appDbContext.Pembelian.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<PembelianDetail>> GetDetail()
        {
            return await _appDbContext.PembelianDetail.ToListAsync();
        }

        public async Task<List<PembelianDetail>> FindDetail(string pembelianId)
        {
            return await _appDbContext.PembelianDetail.Include(x => x.BahanSatuan!).Where(x => x.PembelianId == pembelianId).Select(x => new PembelianDetail
            {
                BahanSatuanId = x.BahanSatuanId,
                Harga = x.Harga,
                Jumlah = x.Jumlah,
                BahanSatuan = new BahanSatuan
                {
                    Id = x.BahanSatuan!.Id,
                    Nama = x.BahanSatuan!.Nama,
                    Ukuran = x.BahanSatuan!.Ukuran,
                    KonversiStok = x.BahanSatuan!.KonversiStok,
                    Bahan = new Bahan
                    {
                        Nama = x.BahanSatuan!.Bahan!.Nama,
                        Stok = x.BahanSatuan!.Bahan!.Stok,
                        Version = x.BahanSatuan!.Bahan!.Version
                    }
                }
            }).ToListAsync();
        }

        public async Task<List<BahanSatuan>> RefreshDetail(string id, List<int> bahanSatuanIds)
        {
            if (!string.IsNullOrEmpty(id) && !await _appDbContext.Pembelian.AnyAsync(x => x.Id == id)) return null!;
            return await _appDbContext.BahanSatuan.Include(x => x.Bahan).Where(x => bahanSatuanIds.Contains(x.Id)).Select(x => new BahanSatuan
            {
                Id = x.Id,
                BahanId = x.BahanId,
                Nama = x.Nama,
                Ukuran = x.Ukuran,
                Harga = x.Harga,
                KonversiStok = x.KonversiStok,
                Bahan = new Bahan
                {
                    Nama = x.Bahan!.Nama,
                    SatuanProduksi = x.Bahan!.SatuanProduksi,
                    Stok = x.Bahan!.Stok,
                    Version = x.Bahan!.Version
                }
            }).ToListAsync();
        }

        public async Task<bool> DeletableDetail(string pembelianId, int bahanSatuanId)
        {
            return await _appDbContext.Pembelian.AnyAsync(x => x.ReturPembelian!.ReturPembelianDetail!.Any(y => x.Id == pembelianId && y.Jumlah == 0 && y.BahanSatuanId == bahanSatuanId) || (x.Id == pembelianId && x.ReturPembelian == null));
        }

        public async Task<List<TransaksiPembelian>> GetTransaksi()
        {
            return await _appDbContext.TransaksiPembelian.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<TransaksiPembelian> FindTransaksi(int id)
        {
            return await _appDbContext.TransaksiPembelian.FirstAsync(x => x.Id == id);
        }

        public async Task<TransaksiPembelian> CreateTransaksi(TransaksiPembelian transaksiPembelian)
        {
            Pembelian model = await _appDbContext.Pembelian.Include(x => x.PembelianDetail).Include(x => x.TransaksiPembelian).FirstAsync(x => x.Id == transaksiPembelian.PembelianId && x.Version.SequenceEqual(transaksiPembelian.Pembelian!.Version));
            model.Status = model.Terbayar + transaksiPembelian.Nominal >= (int)(model.Subtotal * ((model.PPN + 100) / 100m)) ? "Lunas" : "Belum Lunas";
            model.Terbayar += transaksiPembelian.Nominal;

            transaksiPembelian.Id = GenerateId(_appDbContext.TransaksiPembelian.Select(x => x.Id));
            transaksiPembelian.Tanggal = (DateTime)(transaksiPembelian.InputTanggal + transaksiPembelian.InputWaktu)!;
            transaksiPembelian.PembelianId = model.Id;

            Nullify(transaksiPembelian);
            var result = await _appDbContext.TransaksiPembelian.AddAsync(transaksiPembelian);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<TransaksiPembelian> CreatedTransaksi(int id)
        {
            return await _appDbContext.TransaksiPembelian.LastAsync(x => x.Id == id);
        }

        public async Task<TransaksiPembelian> UpdateTransaksi(TransaksiPembelian transaksiPembelian)
        {
            Pembelian model = await _appDbContext.Pembelian.Include(x => x.PembelianDetail).Include(x => x.TransaksiPembelian).FirstAsync(x => x.Id == transaksiPembelian.PembelianId && x.Version.SequenceEqual(transaksiPembelian.Pembelian!.Version));
            model.Status = model.TransaksiPembelian!.Where(x => x.Id != transaksiPembelian.Id).Sum(y => y.Nominal) + transaksiPembelian.Nominal >= (int)(model.Subtotal * ((model.PPN + 100) / 100m)) ? "Lunas" : "Belum Lunas";
            model.Terbayar += transaksiPembelian.Nominal - transaksiPembelian.NominalSebelum;

            TransaksiPembelian transaksiPembelianModel = model.TransaksiPembelian!.First(x => x.Id == transaksiPembelian.Id && x.Version.SequenceEqual(transaksiPembelian.Version));
            transaksiPembelianModel.Tanggal = (DateTime)(transaksiPembelian.InputTanggal + transaksiPembelian.InputWaktu)!;
            transaksiPembelianModel.Keterangan = transaksiPembelian.Keterangan;
            transaksiPembelianModel.Nominal = transaksiPembelian.Nominal;

            await _appDbContext.SaveChangesAsync();

            return transaksiPembelian;
        }

        public async Task DeleteTransaksi(int id)
        {
            TransaksiPembelian transaksiPembelian = await _appDbContext.TransaksiPembelian.FirstAsync(x => x.Id == id);

            Pembelian model = await _appDbContext.Pembelian.FirstAsync(x => x.Id == transaksiPembelian.PembelianId);
            model.Status = "Belum Lunas";
            model.Terbayar -= transaksiPembelian.Nominal;

            _appDbContext.TransaksiPembelian.Remove(transaksiPembelian);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<ReturPembelian>> GetRetur()
        {
            return await _appDbContext.ReturPembelian.Include(x => x.Pembelian!).ThenInclude(x => x.Supplier).Select(x => new ReturPembelian
            {
                Id = x.Id,
                Tanggal = x.Tanggal,
                Keterangan = x.Keterangan,
                GrandTotal = x.GrandTotal,
                Pembelian = new Pembelian { Supplier = new Supplier { Nama = x.Pembelian!.Supplier!.Nama } }
            }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<ReturPembelian> FindRetur(string id)
        {
            ReturPembelian returPembelian = (await _appDbContext.ReturPembelian.Include(x => x.ReturPembelianDetail).Include(x => x.Pembelian!).ThenInclude(x => x.PembelianDetail!).ThenInclude(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).FirstOrDefaultAsync(x => x.Id == id))!;
            return returPembelian is null ? null! : new ReturPembelian
            {
                Id = returPembelian.Id,
                PembelianId = returPembelian.PembelianId,
                Keterangan = returPembelian.Keterangan,
                Version = returPembelian.Version,
                InputTanggal = returPembelian.Tanggal.Date,
                InputWaktu = returPembelian.Tanggal.TimeOfDay,
                GrandTotal = returPembelian.GrandTotal,
                Pembelian = returPembelian.Pembelian,
                ReturPembelianDetail = returPembelian.ReturPembelianDetail!.ConvertAll(y => new ReturPembelianDetail
                {
                    BahanSatuanId = y.BahanSatuanId,
                    Harga = returPembelian.Pembelian!.PembelianDetail!.First(x => x.BahanSatuanId == y.BahanSatuanId).Harga,
                    Jumlah = y.Jumlah,
                    MaxJumlah = y.MaxJumlah,
                    Total = (int)(y.Jumlah * y.Harga),
                    BahanSatuan = new BahanSatuan
                    {
                        Id = y.BahanSatuan!.Id,
                        BahanId = y.BahanSatuan!.BahanId,
                        Nama = y.BahanSatuan!.Nama,
                        Ukuran = y.BahanSatuan!.Ukuran,
                        KonversiStok = y.BahanSatuan!.KonversiStok,
                        Bahan = new Bahan
                        {
                            Nama = y.BahanSatuan!.Bahan!.Nama,
                            SatuanProduksi = y.BahanSatuan!.Bahan!.SatuanProduksi,
                            Stok = y.BahanSatuan!.Bahan!.Stok,
                            Version = y.BahanSatuan!.Bahan!.Version
                        }
                    }
                })
            };
        }

        public async Task<ReturPembelian> FindRetur1(string id)
        {
            ReturPembelian returPembelian = await _appDbContext.ReturPembelian.Include(x => x.ReturPembelianDetail!).ThenInclude(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == id);
            return returPembelian is null ? null! : new ReturPembelian
            {
                PembelianId = returPembelian.PembelianId,
                Tanggal = returPembelian.Tanggal,
                GrandTotal = returPembelian.GrandTotal,
                Keterangan = returPembelian.Keterangan,
                ReturPembelianDetail = returPembelian.ReturPembelianDetail!.Where(y => y.Jumlah > 0).Select(y => new ReturPembelianDetail
                {
                    BahanSatuanId = y.BahanSatuanId,
                    Jumlah = y.Jumlah,
                    Harga = y.Harga,
                    Total = (int)(y.Jumlah * y.Harga),
                    BahanSatuan = new BahanSatuan
                    {
                        Nama = y.BahanSatuan!.Nama,
                        Ukuran = y.BahanSatuan!.Ukuran,
                        Bahan = new Bahan { Nama = y.BahanSatuan!.Bahan!.Nama }
                    }
                }).ToList()
            };
        }

        public async Task<ReturPembelian> CreateRetur(ReturPembelian returPembelian)
        {
            returPembelian.Tanggal = (DateTime)(returPembelian.InputTanggal + returPembelian.InputWaktu)!;
            // Membuat ReturPembelianDetail baru dan kurangi stok bahannya
            var idsDetail = GenerateId(_appDbContext.ReturPembelianDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.ReturPembelianDetail.Select(x => x.Id), returPembelian.ReturPembelianDetail!);
            for (int i = 0; i < returPembelian.ReturPembelianDetail!.Count; i++)
            {
                returPembelian.ReturPembelianDetail[i].Id = idsDetail[i];
                returPembelian.ReturPembelianDetail[i].ReturPembelianId = returPembelian.Id;
                var bahanSatuan = await _appDbContext.BahanSatuan.Include(x => x.Bahan).FirstAsync(y => y.Id == returPembelian.ReturPembelianDetail[i].BahanSatuanId && y.Bahan!.Version.SequenceEqual(returPembelian.ReturPembelianDetail[i].BahanSatuan!.Bahan!.Version));
                bahanSatuan.Bahan!.Stok -= returPembelian.ReturPembelianDetail[i].Jumlah * returPembelian.ReturPembelianDetail[i].BahanSatuan!.KonversiStok;

                if (bahanSatuan.Bahan!.Stok < 0) throw new DbUpdateException();
            }

            var pembelian = await _appDbContext.Pembelian.OrderBy(x => x.Id).LastAsync(x => x.Id == returPembelian.PembelianId);
            if (returPembelian.GrandTotal >= (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)) - pembelian.Terbayar)
                pembelian.Status = "Lunas";

            List<PembelianDetail> pembelianDetail = await _appDbContext.PembelianDetail.Include(x => x.BahanSatuan).Where(x => x.PembelianId == returPembelian.PembelianId).ToListAsync();
            foreach (var item in pembelianDetail) item.MinJumlah = returPembelian.ReturPembelianDetail!.First(x => x.BahanSatuanId == item.BahanSatuanId).Jumlah;

            var returPembelianDetail = Nullifies(returPembelian.ReturPembelianDetail!);
            Nullify(returPembelian);

            var result = await _appDbContext.ReturPembelian.AddAsync(returPembelian);
            await _appDbContext.ReturPembelianDetail.AddRangeAsync(returPembelianDetail);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<ReturPembelian> CreatedRetur(string id)
        {
            return await _appDbContext.ReturPembelian.LastAsync(x => x.Id == id);
        }

        public async Task<ReturPembelian> UpdateRetur(ReturPembelian returPembelian)
        {
            // Ambil Pembelian yang akan di-update dari database
            ReturPembelian model = await _appDbContext.ReturPembelian.Include(x => x.ReturPembelianDetail!).ThenInclude(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == returPembelian.Id && x.Version.SequenceEqual(returPembelian.Version));
            model.Tanggal = (DateTime)(returPembelian.InputTanggal + returPembelian.InputWaktu)!;
            model.GrandTotal = returPembelian.GrandTotal;
            model.Keterangan = returPembelian.Keterangan;
            // Kembalikan stok bahannya dengan jumlah lama, kemudian kurangi stok bahannya dengan jumlah baru dan update ReturPembelianDetail pada field Jumlah
            foreach (var x in model.ReturPembelianDetail!)
            {
                var bahanSatuan = await _appDbContext.BahanSatuan.Include(y => y.Bahan).FirstAsync(y => y.Id == x.BahanSatuanId && y.Bahan!.Version.SequenceEqual(x.BahanSatuan!.Bahan!.Version));
                decimal jumlah = returPembelian.ReturPembelianDetail!.First(y => y.BahanSatuanId == bahanSatuan.Id).Jumlah;
                if (jumlah != x.Jumlah)
                {
                    bahanSatuan.Bahan!.Stok -= (jumlah - x.Jumlah) * bahanSatuan.KonversiStok;
                    x.Jumlah = jumlah;
                }

                if (bahanSatuan.Bahan!.Stok < 0) throw new DbUpdateException();
            }

            var pembelian = await _appDbContext.Pembelian.OrderBy(x => x.Id).LastAsync(x => x.Id == returPembelian.PembelianId);
            if (returPembelian.GrandTotal >= (int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)) - pembelian.Terbayar)
                pembelian.Status = "Lunas";

            List<PembelianDetail> pembelianDetail = await _appDbContext.PembelianDetail.Include(x => x.BahanSatuan).Where(x => x.PembelianId == returPembelian.PembelianId).ToListAsync();
            foreach (var item in pembelianDetail) item.MinJumlah = returPembelian.ReturPembelianDetail!.First(x => x.BahanSatuanId == item.BahanSatuanId && x.BahanSatuan!.Bahan!.Version.SequenceEqual(item.BahanSatuan!.Bahan!.Version)).Jumlah;

            await _appDbContext.SaveChangesAsync();

            return returPembelian;
        }

        public async Task DeleteRetur(string id)
        {
            var result = await _appDbContext.ReturPembelian.Include(x => x.ReturPembelianDetail!).ThenInclude(x => x.BahanSatuan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == id);
            if (result is not null)
            {
                foreach (var x in result.ReturPembelianDetail!)
                {
                    var bahanSatuan = await _appDbContext.BahanSatuan.Include(x => x.Bahan).FirstAsync(y => y.Id == x.BahanSatuanId && y.Bahan!.Version.SequenceEqual(x.BahanSatuan!.Bahan!.Version));
                    bahanSatuan.Bahan!.Stok += x.Jumlah * x.BahanSatuan!.KonversiStok;

                    //if (bahanSatuan.Bahan!.Stok < 0) throw new DbUpdateException();
                }

                var pembelian = await _appDbContext.Pembelian.OrderBy(x => x.Id).LastAsync(x => x.Id == result.PembelianId);
                if ((int)(pembelian.Subtotal * ((pembelian.PPN + 100) / 100m)) - pembelian.Terbayar - result.GrandTotal > 0)
                    pembelian.Status = "Belum Lunas";

                List<PembelianDetail> pembelianDetail = await _appDbContext.PembelianDetail.Where(x => x.PembelianId == result.PembelianId).ToListAsync();
                foreach (var item in pembelianDetail) item.MinJumlah = 0;

                _appDbContext.ReturPembelian.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ReturPembelianDetail>> GetReturDetail()
        {
            return await _appDbContext.ReturPembelianDetail.ToListAsync();
        }

        public async Task<List<BahanSatuan>> RefreshReturDetail(string returId)
        {
            List<BahanSatuan> result = await _appDbContext.ReturPembelianDetail.Include(x => x.BahanSatuan!).Where(x => x.ReturPembelianId == returId).Select(x => new BahanSatuan
            {
                Id = x.BahanSatuan!.Id,
                BahanId = x.BahanSatuan!.BahanId,
                Nama = x.BahanSatuan!.Nama,
                Ukuran = x.BahanSatuan!.Ukuran,
                Bahan = new Bahan
                {
                    Nama = x.BahanSatuan!.Bahan!.Nama,
                    SatuanProduksi = x.BahanSatuan!.Bahan!.SatuanProduksi,
                    Stok = x.BahanSatuan!.Bahan!.Stok,
                    Version = x.BahanSatuan!.Bahan!.Version
                }
            }).ToListAsync();
            return result.Any() ? result : null!;
        }
    }
}