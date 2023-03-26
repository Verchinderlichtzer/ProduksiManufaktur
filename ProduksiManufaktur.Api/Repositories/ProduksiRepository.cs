using ProduksiManufaktur.Models;

namespace ProduksiManufaktur.Api.Repositories
{
    /// <summary>CRUD Produksi, R ProduksiDetail</summary>
    public interface IProduksiRepository
    {
        /// <summary>List Produksi { Id, Tanggal, Jumlah, Total, Barang { Nama, SatuanProduksi } } > ProduksiList</summary>
        Task<List<Produksi>> Get();

        /// <summary>Produksi { Id, BarangId, InputTanggal, InputWaktu, Jumlah, Keterangan, BiayaJasa, BiayaOverhead, Version, Total, Barang { Id, Nama, SatuanProduksi, Stok, Version }, List ProduksiDetailBahan { Id, ProduksiId, BahanId, Jumlah, JumlahSebelum, Bahan { Id, Nama, SatuanProduksi, Stok, Version } }, List ProduksiDetailJasa { Id, ProduksiId, KaryawanId, Biaya, Karyawan { Id, Nama, PekerjaanId, Pekerjaan { Id, Nama } } }, List ProduksiDetailOverhead { Id, ProduksiId, OverheadId, Biaya, Overhead { Id, Nama } } } > ProduksiForm</summary>
        Task<Produksi> Find(string id);

        /// <summary>Produksi { Tanggal, Jumlah, Keterangan, BiayaJasa, BiayaOverhead, Barang { Nama, SatuanProduksi }, ProduksiDetailBahan { Jumlah, Bahan { Nama, SatuanProduksi } }, ProduksiDetailJasa { Biaya, Karyawan { Nama, Pekerjaan { Nama } } }, ProduksiDetailOverhead { Biaya, Overhead { Nama } } } > ProduksiInfo</summary>
        Task<Produksi> Find1(string id);

        Task<Produksi> Create(Produksi produksi);

        Task<Produksi> Update(Produksi produksi);

        Task Delete(string id);

        /// <summary>List ProduksiDetailBahan</summary>
        Task<List<ProduksiDetailBahan>> GetDetailBahan();

        /// <summary>List ProduksiDetailBahan { Id, ProduksiId, BahanId, Jumlah, JumlahSebelum, Bahan { Id, Nama, SatuanProduksi, Stok, Version } }</summary>
        Task<List<ProduksiDetailBahan>> FindDetailBahan(string produksiId);

        /// <summary>List ProduksiDetailJasa</summary>
        Task<List<ProduksiDetailJasa>> GetDetailJasa();

        /// <summary>List ProduksiDetailJasa { Id, ProduksiId, KaryawanId, Biaya, Karyawan { Id, Nama, PekerjaanId, Pekerjaan { Id, Nama } } }</summary>
        Task<List<ProduksiDetailJasa>> FindDetailJasa(string produksiId);

        /// <summary>List ProduksiDetailOverhead</summary>
        Task<List<ProduksiDetailOverhead>> GetDetailOverhead();

        /// <summary>List ProduksiDetailOverhead { Id, ProduksiId, OverheadId, Biaya, Overhead { Id, Nama } }</summary>
        Task<List<ProduksiDetailOverhead>> FindDetailOverhead(string produksiId);

        /// <summary>Produksi { Barang { Id, Nama, SatuanProduksi, Stok, Version }, List ProduksiDetailBahan { Id, ProduksiId, BahanId, Jumlah, JumlahSebelum, Bahan { Id, Nama, SatuanProduksi, Stok, Version } }, List ProduksiDetailJasa { Id, ProduksiId, KaryawanId, Biaya, Karyawan { Id, Nama, PekerjaanId, Pekerjaan { Id, Nama } } }, List ProduksiDetailOverhead { Id, ProduksiId, OverheadId, Biaya, Overhead { Id, Nama } } } > ProduksiForm</summary>
        Task<Produksi> RefreshDetail(string produksiId, List<string> bahanIds, List<string> karyawanIds, List<int> OverheadIds);
    }

    public class ProduksiRepository : IProduksiRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProduksiRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Produksi>> Get()
        {
            return await _appDbContext.Produksi.Include(x => x.Barang).Select(x => new Produksi
            {
                Id = x.Id,
                Tanggal = x.Tanggal,
                Jumlah = x.Jumlah,
                Total = x.BiayaJasa + x.BiayaOverhead,
                Barang = new() { Nama = x.Barang!.Nama, SatuanProduksi = x.Barang!.SatuanProduksi }
            }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Produksi> Find(string id)
        {
            Produksi produksi = await _appDbContext.Produksi
                .Include(x => x.Barang)
                .Include(x => x.ProduksiDetailBahan!).ThenInclude(x => x.Bahan)
                .Include(x => x.ProduksiDetailJasa!).ThenInclude(x => x.Karyawan!).ThenInclude(x => x.Pekerjaan)
                .Include(x => x.ProduksiDetailOverhead!).ThenInclude(x => x.Overhead)
                .FirstAsync(x => x.Id == id);
            return new Produksi
            {
                Id = produksi.Id,
                BarangId = produksi.BarangId,
                InputTanggal = produksi.Tanggal.Date,
                InputWaktu = produksi.Tanggal.TimeOfDay,
                Jumlah = produksi.Jumlah,
                Keterangan = produksi.Keterangan,
                BiayaJasa = produksi.BiayaJasa,
                BiayaOverhead = produksi.BiayaOverhead,
                Version = produksi.Version,
                Total = produksi.BiayaJasa + produksi.BiayaOverhead,
                Barang = new Barang
                {
                    Id = produksi.Barang!.Id,
                    Nama = produksi.Barang!.Nama,
                    SatuanProduksi = produksi.Barang!.SatuanProduksi,
                    Stok = produksi.Barang!.Stok,
                    Version = produksi.Barang!.Version
                },
                ProduksiDetailBahan = produksi.ProduksiDetailBahan!.ConvertAll(x => new ProduksiDetailBahan
                {
                    Id = x.Id,
                    ProduksiId = x.ProduksiId,
                    BahanId = x.BahanId,
                    Jumlah = x.Jumlah,
                    JumlahSebelum = x.Jumlah,
                    Bahan = new Bahan
                    {
                        Id = x.Bahan!.Id,
                        Nama = x.Bahan!.Nama,
                        SatuanProduksi = x.Bahan!.SatuanProduksi,
                        Stok = x.Bahan!.Stok,
                        Version = x.Bahan!.Version
                    }
                }),
                ProduksiDetailJasa = produksi.ProduksiDetailJasa!.ConvertAll(x => new ProduksiDetailJasa
                {
                    Id = x.Id,
                    ProduksiId = x.ProduksiId,
                    KaryawanId = x.KaryawanId,
                    Biaya = x.Biaya,
                    Karyawan = new Karyawan
                    {
                        Id = x.Karyawan!.Id,
                        Nama = x.Karyawan!.Nama,
                        PekerjaanId = x.Karyawan!.PekerjaanId,
                        Pekerjaan = new Pekerjaan { Id = x.Karyawan!.Pekerjaan!.Id, Nama = x.Karyawan!.Pekerjaan!.Nama }
                    }
                }),
                ProduksiDetailOverhead = produksi.ProduksiDetailOverhead!.ConvertAll(x => new ProduksiDetailOverhead
                {
                    Id = x.Id,
                    ProduksiId = x.ProduksiId,
                    OverheadId = x.OverheadId,
                    Biaya = x.Biaya,
                    Overhead = new Overhead
                    {
                        Id = x.Overhead!.Id,
                        Nama = x.Overhead!.Nama
                    }
                }),
            };
        }

        public async Task<Produksi> Find1(string id)
        {
            Produksi produksi = await _appDbContext.Produksi
                .Include(x => x.Barang)
                .Include(x => x.ProduksiDetailBahan!).ThenInclude(x => x.Bahan)
                .Include(x => x.ProduksiDetailJasa!).ThenInclude(x => x.Karyawan!).ThenInclude(x => x.Pekerjaan)
                .Include(x => x.ProduksiDetailOverhead!).ThenInclude(x => x.Overhead)
                .FirstAsync(x => x.Id == id);
            return new Produksi
            {
                Tanggal = produksi.Tanggal,
                Jumlah = produksi.Jumlah,
                Keterangan = produksi.Keterangan,
                BiayaJasa = produksi.BiayaJasa,
                BiayaOverhead = produksi.BiayaOverhead,
                Barang = new Barang
                {
                    Nama = produksi.Barang!.Nama,
                    SatuanProduksi = produksi.Barang!.SatuanProduksi,
                },
                ProduksiDetailBahan = produksi.ProduksiDetailBahan!.ConvertAll(x => new ProduksiDetailBahan
                {
                    Jumlah = x.Jumlah,
                    Bahan = new Bahan
                    {
                        Nama = x.Bahan!.Nama,
                        SatuanProduksi = x.Bahan!.SatuanProduksi,
                    }
                }),
                ProduksiDetailJasa = produksi.ProduksiDetailJasa!.ConvertAll(x => new ProduksiDetailJasa
                {
                    Biaya = x.Biaya,
                    Karyawan = new Karyawan
                    {
                        Nama = x.Karyawan!.Nama,
                        Pekerjaan = new Pekerjaan { Nama = x.Karyawan!.Pekerjaan!.Nama }
                    }
                }),
                ProduksiDetailOverhead = produksi.ProduksiDetailOverhead!.ConvertAll(x => new ProduksiDetailOverhead
                {
                    Biaya = x.Biaya,
                    Overhead = new Overhead { Nama = x.Overhead!.Nama }
                }),
            };
        }

        public async Task<Produksi> Create(Produksi produksi)
        {
            produksi.Tanggal = (DateTime)(produksi.InputTanggal + produksi.InputWaktu)!;
            produksi.Id = GenerateId("PDKS", produksi.Tanggal, _appDbContext.Produksi.Where(x => x.Tanggal.Date == produksi.Tanggal.Date).Select(x => x.Id));

            var idsDetailBahan = GenerateId(_appDbContext.ProduksiDetailBahan.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.ProduksiDetailBahan.Select(x => x.Id), produksi.ProduksiDetailBahan!);
            var idsDetailJasa = GenerateId(_appDbContext.ProduksiDetailJasa.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.ProduksiDetailJasa.Select(x => x.Id), produksi.ProduksiDetailJasa!);
            var idsDetailOverhead = GenerateId(_appDbContext.ProduksiDetailOverhead.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.ProduksiDetailOverhead.Select(x => x.Id), produksi.ProduksiDetailOverhead!);

            for (int i = 0; i < produksi.ProduksiDetailBahan!.Count; i++)
            {
                produksi.ProduksiDetailBahan[i].Id = idsDetailBahan[i];
                produksi.ProduksiDetailBahan[i].ProduksiId = produksi.Id;
                var bahan = await _appDbContext.Bahan.FirstAsync(y => y.Id == produksi.ProduksiDetailBahan[i].BahanId && y.Version.SequenceEqual(produksi.ProduksiDetailBahan[i].Bahan!.Version));
                bahan.Stok -= produksi.ProduksiDetailBahan[i].Jumlah;

                if (bahan.Stok < 0) throw new DbUpdateException();
            }
            for (int i = 0; i < produksi.ProduksiDetailJasa!.Count; i++)
            {
                produksi.ProduksiDetailJasa[i].Id = idsDetailJasa[i];
                produksi.ProduksiDetailJasa[i].ProduksiId = produksi.Id;
            }
            for (int i = 0; i < produksi.ProduksiDetailOverhead!.Count; i++)
            {
                produksi.ProduksiDetailOverhead[i].Id = idsDetailOverhead[i];
                produksi.ProduksiDetailOverhead[i].ProduksiId = produksi.Id;
            }

            var barang = await _appDbContext.Barang.FirstAsync(y => y.Id == produksi.BarangId && y.Version.SequenceEqual(produksi.Barang!.Version));
            barang.Stok += produksi.Jumlah;

            var produksiDetailBahan = Nullifies(produksi.ProduksiDetailBahan!);
            var produksiDetailJasa = Nullifies(produksi.ProduksiDetailJasa!);
            var produksiDetailOverhead = Nullifies(produksi.ProduksiDetailOverhead!);
            Nullify(produksi);

            var result = await _appDbContext.Produksi.AddAsync(produksi);
            await _appDbContext.ProduksiDetailBahan.AddRangeAsync(produksiDetailBahan);
            await _appDbContext.ProduksiDetailJasa.AddRangeAsync(produksiDetailJasa);
            await _appDbContext.ProduksiDetailOverhead.AddRangeAsync(produksiDetailOverhead);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Produksi> Update(Produksi produksi)
        {
            // Ambil Produksi yang akan di-update dari database
            Produksi model = await _appDbContext.Produksi.Include(x => x.Barang).Include(x => x.ProduksiDetailJasa).Include(x => x.ProduksiDetailOverhead).Include(x => x.ProduksiDetailBahan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == produksi.Id && x.Version.SequenceEqual(produksi.Version));

            model.Keterangan = produksi.Keterangan;
            model.BiayaJasa = produksi.BiayaJasa;
            model.BiayaOverhead = produksi.BiayaOverhead;

            // Edit Barang
            if (model.BarangId != produksi.BarangId)
            {
                var barangLama = await _appDbContext.Barang.FirstAsync(y => y.Id == model.BarangId && y.Version.SequenceEqual(model.Barang!.Version));
                barangLama.Stok -= model.Jumlah;
                model.BarangId = produksi.BarangId;
            }

            var barangBaru = await _appDbContext.Barang.FirstAsync(y => y.Id == produksi.BarangId && y.Version.SequenceEqual(produksi.Barang!.Version));
            if (model.BarangId != produksi.BarangId)
            {
                barangBaru.Stok += produksi.Jumlah;
            }
            else
            {
                barangBaru.Stok = barangBaru.Stok - model.Jumlah + produksi.Jumlah;
            }
            model.Jumlah = produksi.Jumlah;

            // Kembalikan (tambahi) stok bahannya, karena ProduksiDetailBahan akan dihapus dan di-insert ulang
            foreach (var x in model.ProduksiDetailBahan!)
            {
                var bahan = await _appDbContext.Bahan.FirstAsync(y => y.Id == x.BahanId && y.Version.SequenceEqual(x.Bahan!.Version));
                bahan.Stok += x.Jumlah;
            }

            _appDbContext.ProduksiDetailBahan.RemoveRange(await _appDbContext.ProduksiDetailBahan.Where(x => x.ProduksiId == produksi.Id).ToListAsync());
            _appDbContext.ProduksiDetailJasa.RemoveRange(await _appDbContext.ProduksiDetailJasa.Where(x => x.ProduksiId == produksi.Id).ToListAsync());
            _appDbContext.ProduksiDetailOverhead.RemoveRange(await _appDbContext.ProduksiDetailOverhead.Where(x => x.ProduksiId == produksi.Id).ToListAsync());

            var idsDetailBahan = GenerateId(_appDbContext.ProduksiDetailBahan.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.ProduksiDetailBahan.Select(x => x.Id), produksi.ProduksiDetailBahan!);
            var idsDetailJasa = GenerateId(_appDbContext.ProduksiDetailJasa.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.ProduksiDetailJasa.Select(x => x.Id), produksi.ProduksiDetailJasa!);
            var idsDetailOverhead = GenerateId(_appDbContext.ProduksiDetailOverhead.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.ProduksiDetailOverhead.Select(x => x.Id), produksi.ProduksiDetailOverhead!);

            for (int i = 0; i < produksi.ProduksiDetailBahan!.Count; i++)
            {
                produksi.ProduksiDetailBahan[i].Id = idsDetailBahan[i];
                produksi.ProduksiDetailBahan[i].ProduksiId = produksi.Id;
                var bahan = await _appDbContext.Bahan.FirstAsync(y => y.Id == produksi.ProduksiDetailBahan[i].BahanId && y.Version.SequenceEqual(produksi.ProduksiDetailBahan[i].Bahan!.Version));
                bahan.Stok -= produksi.ProduksiDetailBahan[i].Jumlah;

                if (bahan.Stok < 0) throw new DbUpdateException();
            }
            for (int i = 0; i < produksi.ProduksiDetailJasa!.Count; i++)
            {
                produksi.ProduksiDetailJasa[i].Id = idsDetailJasa[i];
                produksi.ProduksiDetailJasa[i].ProduksiId = produksi.Id;
            }
            for (int i = 0; i < produksi.ProduksiDetailOverhead!.Count; i++)
            {
                produksi.ProduksiDetailOverhead[i].Id = idsDetailOverhead[i];
                produksi.ProduksiDetailOverhead[i].ProduksiId = produksi.Id;
            }

            var produksiDetailBahan = Nullifies(produksi.ProduksiDetailBahan!);
            var produksiDetailJasa = Nullifies(produksi.ProduksiDetailJasa!);
            var produksiDetailOverhead = Nullifies(produksi.ProduksiDetailOverhead!);

            await _appDbContext.ProduksiDetailBahan.AddRangeAsync(produksiDetailBahan);
            await _appDbContext.ProduksiDetailJasa.AddRangeAsync(produksiDetailJasa);
            await _appDbContext.ProduksiDetailOverhead.AddRangeAsync(produksiDetailOverhead);
            await _appDbContext.SaveChangesAsync();

            return produksi;
        }

        public async Task Delete(string id)
        {
            var result = await _appDbContext.Produksi.Include(x => x.Barang).Include(x => x.ProduksiDetailBahan!).ThenInclude(x => x.Bahan).FirstAsync(x => x.Id == id);
            if (result is not null)
            {
                var barang = await _appDbContext.Barang.FirstAsync(y => y.Id == result.BarangId && y.Version.SequenceEqual(result.Barang!.Version));
                barang.Stok -= result.Jumlah;
                if (barang.Stok < 0) throw new DbUpdateException();

                foreach (var x in result.ProduksiDetailBahan!)
                {
                    var bahan = await _appDbContext.Bahan.FirstAsync(y => y.Id == x.BahanId && y.Version.SequenceEqual(x.Bahan!.Version));
                    bahan.Stok += x.Jumlah;
                }

                _appDbContext.Produksi.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ProduksiDetailBahan>> GetDetailBahan()
        {
            return await _appDbContext.ProduksiDetailBahan.ToListAsync();
        }

        public async Task<List<ProduksiDetailBahan>> FindDetailBahan(string produksiId)
        {
            return await _appDbContext.ProduksiDetailBahan.Include(x => x.Bahan).Where(x => x.ProduksiId == produksiId).Select(x => new ProduksiDetailBahan
            {
                Id = x.Id,
                ProduksiId = x.ProduksiId,
                BahanId = x.BahanId,
                Jumlah = x.Jumlah,
                JumlahSebelum = x.Jumlah,
                Bahan = new Bahan
                {
                    Id = x.Bahan!.Id,
                    Nama = x.Bahan!.Nama,
                    SatuanProduksi = x.Bahan!.SatuanProduksi,
                    Stok = x.Bahan!.Stok,
                    Version = x.Bahan!.Version
                }
            }).ToListAsync();
        }

        public async Task<List<ProduksiDetailJasa>> GetDetailJasa()
        {
            return await _appDbContext.ProduksiDetailJasa.ToListAsync();
        }

        public async Task<List<ProduksiDetailJasa>> FindDetailJasa(string produksiId)
        {
            return await _appDbContext.ProduksiDetailJasa.Include(x => x.Karyawan!).ThenInclude(x => x.Pekerjaan).Where(x => x.ProduksiId == produksiId).Select(x => new ProduksiDetailJasa
            {
                Id = x.Id,
                ProduksiId = x.ProduksiId,
                KaryawanId = x.KaryawanId,
                Biaya = x.Biaya,
                Karyawan = new Karyawan
                {
                    Id = x.Karyawan!.Id,
                    Nama = x.Karyawan!.Nama,
                    PekerjaanId = x.Karyawan!.PekerjaanId,
                    Pekerjaan = new Pekerjaan { Id = x.Karyawan!.Pekerjaan!.Id, Nama = x.Karyawan!.Pekerjaan!.Nama }
                }
            }).ToListAsync();
        }

        public async Task<List<ProduksiDetailOverhead>> GetDetailOverhead()
        {
            return await _appDbContext.ProduksiDetailOverhead.ToListAsync();
        }

        public async Task<List<ProduksiDetailOverhead>> FindDetailOverhead(string produksiId)
        {
            return await _appDbContext.ProduksiDetailOverhead.Include(x => x.Overhead).Where(x => x.ProduksiId == produksiId).Select(x => new ProduksiDetailOverhead
            {
                Id = x.Id,
                ProduksiId = x.ProduksiId,
                OverheadId = x.OverheadId,
                Biaya = x.Biaya,
                Overhead = new Overhead
                {
                    Id = x.Overhead!.Id,
                    Nama = x.Overhead!.Nama
                }
            }).ToListAsync();
        }

        public async Task<Produksi> RefreshDetail(string produksiId, List<string> bahanIds, List<string> karyawanIds, List<int> OverheadIds)
        {
            if (!string.IsNullOrEmpty(produksiId) && !await _appDbContext.Produksi.AnyAsync(x => x.Id == produksiId)) return null!;
            Barang barang = (await _appDbContext.Produksi.Include(x => x.Barang).FirstAsync(x => x.Id == produksiId)).Barang!;
            List<ProduksiDetailBahan> pdb = await _appDbContext.ProduksiDetailBahan.Include(x => x.Bahan).Where(x => bahanIds.Contains(x.BahanId)).Select(x => new ProduksiDetailBahan
            {
                Id = x.Id,
                ProduksiId = x.ProduksiId,
                BahanId = x.BahanId,
                Jumlah = x.Jumlah,
                JumlahSebelum = x.Jumlah,
                Bahan = new Bahan
                {
                    Id = x.Bahan!.Id,
                    Nama = x.Bahan!.Nama,
                    SatuanProduksi = x.Bahan!.SatuanProduksi,
                    Stok = x.Bahan!.Stok,
                    Version = x.Bahan!.Version
                }
            }).ToListAsync();
            List<ProduksiDetailJasa> pdj = await _appDbContext.ProduksiDetailJasa.Include(x => x.Karyawan!).ThenInclude(x => x.Pekerjaan).Where(x => karyawanIds.Contains(x.KaryawanId)).Select(x => new ProduksiDetailJasa
            {
                Id = x.Id,
                ProduksiId = x.ProduksiId,
                KaryawanId = x.KaryawanId,
                Biaya = x.Biaya,
                Karyawan = new Karyawan
                {
                    Id = x.Karyawan!.Id,
                    Nama = x.Karyawan!.Nama,
                    PekerjaanId = x.Karyawan!.PekerjaanId,
                    Pekerjaan = new Pekerjaan { Id = x.Karyawan!.Pekerjaan!.Id, Nama = x.Karyawan!.Pekerjaan!.Nama }
                }
            }).ToListAsync();
            List<ProduksiDetailOverhead> pdo = await _appDbContext.ProduksiDetailOverhead.Include(x => x.Overhead!).Where(x => OverheadIds.Contains(x.OverheadId)).Select(x => new ProduksiDetailOverhead
            {
                Id = x.Id,
                ProduksiId = x.ProduksiId,
                OverheadId = x.OverheadId,
                Biaya = x.Biaya,
                Overhead = new Overhead
                {
                    Id = x.Overhead!.Id,
                    Nama = x.Overhead!.Nama
                }
            }).ToListAsync();
            return new Produksi { Barang = barang, ProduksiDetailBahan = pdb, ProduksiDetailJasa = pdj, ProduksiDetailOverhead = pdo };
        }
    }
}