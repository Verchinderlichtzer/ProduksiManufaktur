using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace ProduksiManufaktur.Web.Pages._Laporan
{
    [Authorize(Policy = "ReportRead")]
    public class LaporanBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected ILaporanService LaporanService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        // LAPORAN KIRI
        //  MASTER
        //      Bahan - master-bahan
        //      Barang - master-barang
        //      Karyawan - master-karyawan
        //      Supplier - master-supplier
        //      Customer - master-customer
        //  TRANSAKSI
        //      Pembelian (Tanggal MetodeBayar Status) - transaksi-pembelian
        //      Penjualan (Tanggal MetodeBayar Status) - transaksi-penjualan
        //      Produksi (Tanggal) - transaksi-produksi
        //      Transaksi Lain (Tanggal) - transaksi-transaksi-lain
        //      Kas (Tanggal) - transaksi-kas
        //  GRAFIK
        //      Diagram Batang (Tahunan) - grafik-batang
        //      Diagram Garis (Tahunan) - grafik-garis

        // LAPORAN TENGAH (Filter)
        //  TANGGAL
        //      Periodik
        //      Bulanan
        //      Tahunan
        //  STATUS
        //      Belum Lunas
        //      Lunas
        //  METODE BAYAR
        //      Tunai
        //      Kredit

        // LAPORAN KANAN
        //  ENTITAS
        //      Faktur Pembelian - entitas-faktur-pembelian
        //      Retur Pembelian - entitas-retur-pembelian
        //      Faktur Penjualan - entitas-faktur-penjualan
        //      Retur Penjualan - entitas-retur-penjualan
        //      Produksi - entitas-produksi
        //      Formulasi - entitas-formulasi
        //      Penggajian (Tanggal) - entitas-penggajian
        //      Stok Bahan (Tanggal) - entitas-stok-bahan
        //      Stok Barang (Tanggal) - entitas-stok-barang
        //      Supplier (Tanggal MetodeBayar Status) - entitas-supplier
        //      Customer (Tanggal MetodeBayar Status) - entitas-customer

        //      FORMAT
        //      Pembelian (Tanggal MetodeBayar Status) - Laporan pembelian {tanggal} {metodeBayar} {status}
        //      Penjualan (Tanggal MetodeBayar Status) - Laporan penjualan {tanggal} {metodeBayar} {status}
        //      Produksi (Tanggal) - Laporan produksi {tanggal}
        //      Transaksi Lain (Tanggal) - Laporan transaksi lain {tanggal}
        //      Kas (Tanggal) - Laporan kas {tanggal}

        //      Diagram Batang (Tahunan) - Laporan grafik {tanggal}
        //      Diagram Garis (Tahunan) - Laporan grafik {tanggal}

        //      Penggajian (Tanggal) - Laporan penggajian kepada {nama_entitas} {tanggal}
        //      Stok Bahan (Tanggal) - Laporan pembelian pada {nama_entitas} {tanggal}
        //      Stok Barang (Tanggal) - Laporan penjualan pada {nama_entitas} {tanggal}
        //      Supplier (Tanggal MetodeBayar Status) - Laporan pembelian bahan yang dibeli dari {nama_entitas} {tanggal} {metodeBayar} {status}
        //      Customer (Tanggal MetodeBayar Status) - Laporan penjualan barang yang dijual kepada {nama_entitas} {tanggal} {metodeBayar} {status}

        //      Possibilities :
        //      {laporan_terpilih}
        //      {laporan_terpilih} {tanggal}
        //      {laporan_terpilih} {tanggal} {metodeBayar}
        //      {laporan_terpilih} {tanggal} {status}
        //      {laporan_terpilih} {tanggal} {metodeBayar} {status}
        //      {laporan_terpilih} {nama_entitas}
        //      {laporan_terpilih} {nama_entitas} {tanggal}
        //      {laporan_terpilih} {nama_entitas} {tanggal} {metodeBayar}
        //      {laporan_terpilih} {nama_entitas} {tanggal} {status}
        //      {laporan_terpilih} {nama_entitas} {tanggal} {metodeBayar} {status}

        protected Dictionary<string, string> daftarLaporanKiri = new() // Key = laporan terpilih yang akan menentukan laporan yang ditampilkan. Value = text yang ditampilkan pada isi laporan (text filter)
        {
            { "master-bahan", "bahan" },
            { "master-barang", "barang" },
            { "master-karyawan", "karyawan" },
            { "master-supplier", "supplier" },
            { "master-customer", "customer" },
            { "transaksi-pembelian", "pembelian" },
            { "transaksi-penjualan", "penjualan" },
            { "transaksi-produksi", "produksi" },
            { "transaksi-transaksi-lain", "transaksi lain" },
            { "transaksi-kas", "kas" },
            { "grafik-batang", "grafik" },
            { "grafik-garis", "grafik" }
        };

        protected Dictionary<string, string> daftarLaporanKanan = new() // Key = laporan terpilih yang akan menentukan laporan yang ditampilkan. Value = text yang ditampilkan pada isi laporan (text filter)
        {
            { "-", "" },
            { "entitas-faktur-pembelian", "Faktur Pembelian" },
            { "entitas-retur-pembelian", "Retur Pembelian" },
            { "entitas-faktur-penjualan", "Faktur Penjualan" },
            { "entitas-retur-penjualan", "Retur Penjualan" },
            { "entitas-produksi", "Produksi" },
            { "entitas-formulasi", "Formulasi" },
            { "entitas-penggajian", "Karyawan" },
            { "entitas-stok-bahan", "Bahan" },
            { "entitas-stok-barang", "Barang" },
            { "entitas-supplier", "Supplier" },
            { "entitas-customer", "Customer" }
        };

        protected List<EntitasDto> listModel = new();
        protected MudMessageBox? deleteDialog = new();
        protected EntitasDto entitasTerpilih = null!; // filterTextNamaEntitas

        protected string dicari = string.Empty;
        protected string pesanHapus = string.Empty;
        protected DateTime? tahun = DateTime.Now;
        protected bool? loaded;

        protected string laporan = string.Empty; // Unique

        protected List<string> filterTerpilih = new(); // Support Multiple Filtering (Tanggal, Metode Bayar, Status)
        protected string filterText = "-"; // Text Filter Gabungan antara keempat Text Filter dibawah ini. Contoh :

        // Laporan penjualan barang yang dijual kepada CV Mitra Setia Abadi antara tanggal 30/01/2022 sampai 31/12/2022 yang belum lunas
        protected string filterTextLaporan = string.Empty; // penjualan barang yang dijual kepada

        // Untuk nama entitas terpilih, didapat dari variabel entitasTerpilih diatas // CV Mitra Setia Abadi
        protected string filterTextTanggal = string.Empty; // antara tanggal 30/01/2022 sampai 31/12/2022

        protected string filterTextMetodeBayar = string.Empty; // yang belum lunas
        protected string filterTextStatus = string.Empty; // yang belum lunas

        protected KeyValuePair<string, string> jenisEntitas = new("a", "b"); // Input Select Entitas
        protected string entitas = string.Empty; // Khusus Entitas (Laporan Kanan)
        protected string tanggal = string.Empty;
        protected string metodeBayar = string.Empty;
        protected string status = string.Empty;
        // Format Tanggal :
        // Periodik : {e.Start?.Date:dd.MM.yyyy}-{e.End?.Date:dd.MM.yyyy}
        // Bulanan : {e?.Date:MMMM-yyyy}
        // Tahunan : {e?.Year}

        protected List<string> izinTanggal = new() { "transaksi-pembelian", "transaksi-penjualan", "transaksi-produksi", "transaksi-transaksi-lain", "transaksi-kas", "entitas-penggajian", "entitas-stok-bahan", "entitas-stok-barang", "entitas-supplier", "entitas-customer" };
        protected List<string> izinTahunan = new() { "transaksi-pembelian", "transaksi-penjualan", "transaksi-produksi", "transaksi-transaksi-lain", "transaksi-kas", "entitas-penggajian", "entitas-stok-bahan", "entitas-stok-barang", "entitas-supplier", "entitas-customer", "grafik-batang", "grafik-garis" };
        protected List<string> izinMetodeBayarDanStatus = new() { "transaksi-pembelian", "transaksi-penjualan", "entitas-supplier", "entitas-customer" };
        protected List<string> noFilter = new() { "master-bahan", "master-barang", "master-karyawan", "master-supplier", "master-customer" };
        protected List<string> requireEntitas = new() { "entitas-faktur-pembelian", "entitas-retur-pembelian", "entitas-faktur-penjualan", "entitas-retur-penjualan", "entitas-produksi", "entitas-formulasi", "entitas-penggajian", "entitas-stok-bahan", "entitas-stok-barang", "entitas-supplier", "entitas-customer" };
        protected List<string> entitasOnly = new() { "entitas-faktur-pembelian", "entitas-retur-pembelian", "entitas-faktur-penjualan", "entitas-retur-penjualan", "entitas-produksi", "entitas-formulasi" };

        // URL Parameters Possibilities
        // https://localhost:7017/api/laporan/{laporanTerpilih}/filter?filterText={filterText}&filterTerpilih={filterTerpilih}&tanggalTerpilih={tanggalTerpilih}&idTerpilih={idTerpilih}
        // https://localhost:7017/api/laporan/pembelian/filter?filterText=Transaksi%20Blablabla&filterTerpilih=tanggal&tanggalTerpilih=1-1-2022.31-12-2023
        // https://localhost:7017/api/laporan/produksi/filter?filterText=Transaksi%20Blablabla&filterTerpilih=id&idTerpilih=PDKS-22122502

        protected override void OnInitialized()
        {
            jenisEntitas = daftarLaporanKanan.GetKvp("-");
            Layout.BreadcrumbItems = new()
            {
                new BreadcrumbItem("Laporan", "/laporan")
            };
            Layout.Refresh();
        }

        protected void UpdateTextFilter()
        {
            StringBuilder sb = new("Laporan ");
            sb.Append(filterTextLaporan);

            if (!izinMetodeBayarDanStatus.Contains(laporan))
            {
                filterTerpilih.RemoveAll(x => x == "tunai" || x == "kredit");
                filterTextMetodeBayar = string.Empty;
                metodeBayar = string.Empty;
                filterTextStatus = string.Empty;
                status = string.Empty;
            }
            if (!izinTahunan.Contains(laporan))
            {
                filterTerpilih.RemoveAll(x => x == "periodik" || x == "bulanan" || x == "tahunan");
                filterTextTanggal = string.Empty;
                tanggal = string.Empty;
            }
            if (!requireEntitas.Contains(laporan))
            {
                entitasTerpilih = null!;
                entitas = string.Empty;
            }
            if (noFilter.Contains(laporan))
            {
                filterTerpilih.Clear();
                filterText = sb.ToString();
                return;
            }
            if (laporan != "grafik-batang" && laporan != "grafik-garis") filterTerpilih.RemoveAll(x => x == "grafik-batang" || x == "grafik-garis");

            if (entitasTerpilih is not null && !entitasOnly.Contains(laporan))
                sb.Append(entitasTerpilih.Nama);
            if (!string.IsNullOrEmpty(tanggal))
                sb.Append(filterTextTanggal);
            if (!string.IsNullOrEmpty(metodeBayar) || !string.IsNullOrEmpty(status))
            {
                sb.Append(" yang");
                if (!string.IsNullOrEmpty(metodeBayar))
                    sb.Append(filterTextMetodeBayar);
                if (!string.IsNullOrEmpty(status))
                {
                    if (!string.IsNullOrEmpty(metodeBayar)) sb.Append(" dan");
                    sb.Append(filterTextStatus);
                }
            }
            filterText = sb.ToString();
        }

        protected async Task TampilLaporan()
        {
            var result = await LaporanService.Get(laporan, filterTerpilih, filterText, entitas, tanggal);
            if (result)
                Snackbar.Add("Laporan berhasil didownload", Severity.Success);
            else
                Snackbar.Add("Laporan gagal didownload", Severity.Error);
        }

        protected async Task PilihEntitas(KeyValuePair<string, string> e)
        {
            if (string.IsNullOrEmpty(e.Key) || e.Key == "-")
            {
                laporan = string.Empty;
                return;
            }
            loaded = false;
            if (e.Key == "entitas-faktur-pembelian")
            {
                listModel = await LaporanService.GetPembelian();
            }
            else if (e.Key == "entitas-retur-pembelian")
            {
                listModel = await LaporanService.GetReturPembelian();
            }
            else if (e.Key == "entitas-faktur-penjualan")
            {
                listModel = await LaporanService.GetPenjualan();
            }
            else if (e.Key == "entitas-retur-penjualan")
            {
                listModel = await LaporanService.GetReturPenjualan();
            }
            else if (e.Key == "entitas-produksi")
            {
                listModel = await LaporanService.GetProduksi();
            }
            else if (e.Key == "entitas-formulasi")
            {
                listModel = await LaporanService.GetFormulasi();
            }
            else if (e.Key == "entitas-penggajian")
            {
                listModel = await LaporanService.GetKaryawan();
            }
            else if (e.Key == "entitas-stok-bahan")
            {
                listModel = await LaporanService.GetBahan();
            }
            else if (e.Key == "entitas-stok-barang")
            {
                listModel = await LaporanService.GetBarang();
            }
            else if (e.Key == "entitas-supplier")
            {
                listModel = await LaporanService.GetSupplier();
            }
            else if (e.Key == "entitas-customer")
            {
                listModel = await LaporanService.GetCustomer();
            }
            loaded = true;
        }

        protected void PilihItem(EntitasDto e)
        {
            laporan = jenisEntitas.Key;
            entitasTerpilih = e;
            entitas = e.Id;
            if (jenisEntitas.Key == "entitas-faktur-pembelian")
            {
                filterTextLaporan = $"faktur pembelian dengan nomor {e.Id}";
            }
            else if (jenisEntitas.Key == "entitas-retur-pembelian")
            {
                filterTextLaporan = $"retur pembelian dengan nomor {e.Id}";
            }
            else if (jenisEntitas.Key == "entitas-faktur-penjualan")
            {
                filterTextLaporan = $"faktur penjualan dengan nomor {e.Id}";
            }
            else if (jenisEntitas.Key == "entitas-retur-penjualan")
            {
                filterTextLaporan = $"retur penjualan dengan nomor {e.Id}";
            }
            else if (jenisEntitas.Key == "entitas-produksi")
            {
                filterTextLaporan = $"produksi dengan nomor {e.Id}";
            }
            else if (jenisEntitas.Key == "entitas-formulasi")
            {
                filterTextLaporan = $"formulasi dengan nomor {e.Id}";
            }
            else if (jenisEntitas.Key == "entitas-penggajian")
            {
                filterTextLaporan = "penggajian kepada ";
            }
            else if (jenisEntitas.Key == "entitas-stok-bahan")
            {
                filterTextLaporan = "bahan masuk dan keluar pada ";
            }
            else if (jenisEntitas.Key == "entitas-stok-barang")
            {
                filterTextLaporan = "barang masuk dan keluar pada ";
            }
            else if (jenisEntitas.Key == "entitas-supplier")
            {
                filterTextLaporan = "pembelian bahan yang dibeli dari ";
            }
            else if (jenisEntitas.Key == "entitas-customer")
            {
                filterTextLaporan = "penjualan barang yang dijual kepada ";
            }
            UpdateTextFilter();
        }

        protected void PilihLaporanKiri(KeyValuePair<string, string> e)
        {
            laporan = e.Key;
            if (laporan == "grafik-batang" || laporan == "grafik-garis")
            {
                filterTerpilih.Clear();
                filterTerpilih.Add(laporan);
                filterTextTanggal = $" pada tahun {DateTime.Now.Year}";
                tanggal = $"{DateTime.Now.Year}";
                tahun = DateTime.Now;
            }
            filterTextLaporan = e.Value;
            entitasTerpilih = null!;
            UpdateTextFilter();
        }

        protected void PilihPeriodik(DateRange e)
        {
            if (string.IsNullOrEmpty(tanggal)) return;
            filterTerpilih.RemoveAll(x => x == "periodik" || x == "bulanan" || x == "tahunan");
            filterTerpilih.Add("periodik");
            filterTextTanggal = e.Start?.Date == e.End?.Date ? $" pada tanggal {e.Start?.Date:dd/MM/yyyy}" : $" antara tanggal {e.Start?.Date:dd/MM/yyyy} sampai {e.End?.Date:dd/MM/yyyy}";
            UpdateTextFilter();
        }

        protected void PilihBulanan(DateTime? e)
        {
            if (string.IsNullOrEmpty(tanggal)) return;
            filterTerpilih.RemoveAll(x => x == "periodik" || x == "bulanan" || x == "tahunan");
            filterTerpilih.Add("bulanan");
            filterTextTanggal = $" pada bulan {e?.Date:MMMM} {e?.Year}";
            UpdateTextFilter();
        }

        protected void PilihTahunan(DateTime? e)
        {
            if (string.IsNullOrEmpty(tanggal)) return;
            filterTerpilih.RemoveAll(x => x == "periodik" || x == "bulanan" || x == "tahunan");
            filterTerpilih.Add("tahunan");
            filterTextTanggal = $" pada tahun {e?.Year}";
            UpdateTextFilter();
        }

        protected void PilihMetodeBayar(string e)
        {
            if (string.IsNullOrEmpty(metodeBayar)) return;
            filterTerpilih.RemoveAll(x => x == "tunai" || x == "kredit");
            filterTerpilih.Add(e);
            filterTextMetodeBayar = e == "tunai" ? " dibayar secara tunai" : " dibayar secara kredit";
            if (e == "tunai") { status = string.Empty; filterTextStatus = string.Empty; }
            UpdateTextFilter();
        }

        protected void PilihStatus(string e)
        {
            if (string.IsNullOrEmpty(status)) return;
            filterTerpilih.RemoveAll(x => x == "lunas" || x == "belumLunas");
            filterTerpilih.Add(e);
            filterTextStatus = e == "lunas" ? " sudah lunas" : " belum lunas";
            UpdateTextFilter();
        }

        protected void BersihkanFilter()
        {
            if (laporan == "grafik-batang" && laporan == "grafik-garis") return;
            filterTerpilih.Clear();
            filterTextTanggal = string.Empty;
            filterTextMetodeBayar = string.Empty;
            filterTextStatus = string.Empty;
            tanggal = string.Empty;
            metodeBayar = string.Empty;
            status = string.Empty;
            entitas = string.Empty;
            UpdateTextFilter();
        }

        protected Func<EntitasDto, bool> FilterSearch => x => $"{x.Id} {x.Nama}".Cari(dicari);
    }
}