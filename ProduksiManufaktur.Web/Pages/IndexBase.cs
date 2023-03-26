using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace ProduksiManufaktur.Web.Pages
{
    [Authorize]
    public class IndexBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = null!;

        [Inject]
        protected IIndexService IndexService { get; set; } = null!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!;

        protected MudSelect<int>? inputInterval;
        protected List<ChartSeries> series = new();
        protected List<BarangPopulerDto> listBarangPopuler = new();

        protected List<GrafikDto> seriesPendapatan = null!;
        protected List<GrafikDto> seriesPengeluaran = null!;

        protected List<JumlahItemDto> listBahanBeli = null!;
        protected List<JumlahItemDto> listBahanPakai = null!;
        protected List<Bahan> listStokBahanMinim = null!;

        protected List<JumlahItemDto> listBarangJual = null!;
        protected List<JumlahItemDto> listBarangProduksi = null!;
        protected List<Barang> listStokBarangMinim = null!;

        protected List<Pembelian> listUtang = null!;
        protected List<Penjualan> listPiutang = null!;

        protected bool loaded;
        protected int grafik;
        protected int intervalY = 10000;
        protected int index = -1;
        protected string[] xAxisLabels = null!;
        protected string yAxisFormat = string.Empty;

        protected List<string> labelBarangPopuler = new();
        protected List<double> dataBarangPopuler = new();

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new()
            {
                new("Home", "/")
            };
            Layout.Refresh();

            await PilihPeriodeGrafik(0);
            listBarangPopuler = await IndexService.GetBarangPopuler();
            foreach (var item in listBarangPopuler)
            {
                labelBarangPopuler.Add(item.Label);
                dataBarangPopuler.Add(item.Data);
            }

            listBahanPakai = await IndexService.GetJumlahPakai();
            listBarangJual = await IndexService.GetJumlahJual();
            listBahanBeli = await IndexService.GetJumlahBeli();
            listBarangProduksi = await IndexService.GetJumlahProduksi();
            listStokBahanMinim = await IndexService.GetStokBahanMinim();
            listStokBarangMinim = await IndexService.GetStokBarangMinim();
            listUtang = await IndexService.GetUtang();
            listPiutang = await IndexService.GetPiutang();
            loaded = true;
        }

        //protected void PilihIndex(int e)
        //{
        //}

        protected async Task PilihPeriodeGrafik(int e)
        {
            if (e == 0)
                intervalY = 10000;
            else if (e == 1)
                intervalY = 100000;
            else if (e == 2)
                intervalY = 1000000;
            else if (e == 3)
                intervalY = 10000000;
            await TerapkanGrafik();
        }

        protected async Task TerapkanGrafik()
        {
            yAxisFormat = intervalY < 1000000 ? "#,##0,K" : "#,##0,,M";
            seriesPendapatan = await IndexService.GetPendapatan();
            seriesPengeluaran = await IndexService.GetPengeluaran();
            List<string> axis = new();
            List<GrafikDto> tanggal = new();
            if (grafik == 0) // Harian
            {
                for (int i = 9; i >= 0; i--)
                {
                    axis.Add(DateTime.Now.AddDays(-i).ToString("dd/MM"));
                    tanggal.Add(new() { Tanggal = DateTime.Now.AddDays(-i).Date });
                }
                seriesPendapatan.AddRange(tanggal);
                seriesPengeluaran.AddRange(tanggal);
                seriesPendapatan = seriesPendapatan.Where(x => x.Tanggal.Date >= DateTime.Now.AddDays(-9).Date && x.Tanggal <= DateTime.Now).GroupBy(x => x.Tanggal.Date).Select(x => new GrafikDto { Tanggal = x.Key, Nominal = x.Sum(y => y.Nominal) }).ToList();
                seriesPengeluaran = seriesPengeluaran.Where(x => x.Tanggal.Date >= DateTime.Now.AddDays(-9).Date && x.Tanggal <= DateTime.Now).GroupBy(x => x.Tanggal.Date).Select(x => new GrafikDto { Tanggal = x.Key, Nominal = x.Sum(y => y.Nominal) }).ToList();
            }
            else if (grafik == 1) // Mingguan
            {
                for (int i = 9; i >= 0; i--)
                {
                    axis.Add($"M-{i}");
                    tanggal.Add(new() { Tanggal = DateTime.Now.AddDays(-i * 7).Date });
                }
                seriesPendapatan.AddRange(tanggal);
                seriesPengeluaran.AddRange(tanggal);
                seriesPendapatan = seriesPendapatan.Where(x => x.Tanggal.Date >= DateTime.Now.AddDays(-69).Date && x.Tanggal <= DateTime.Now).GroupBy(x => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.Tanggal, CalendarWeekRule.FirstDay, DayOfWeek.Monday)).Select(x => new GrafikDto { No = x.Key, Tanggal = x.Max(y => y.Tanggal), Nominal = x.Sum(y => y.Nominal) }).OrderBy(x => x.Tanggal).TakeLast(10).ToList();
                seriesPengeluaran = seriesPengeluaran.Where(x => x.Tanggal.Date >= DateTime.Now.AddDays(-69).Date && x.Tanggal <= DateTime.Now).GroupBy(x => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.Tanggal, CalendarWeekRule.FirstDay, DayOfWeek.Monday)).Select(x => new GrafikDto { No = x.Key, Tanggal = x.Max(y => y.Tanggal), Nominal = x.Sum(y => y.Nominal) }).OrderBy(x => x.Tanggal).TakeLast(10).ToList();
            }
            else if (grafik == 2) // Bulanan
            {
                for (int i = 1; i <= 12; i++)
                {
                    axis.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i));
                    tanggal.Add(new() { Tanggal = new DateTime(DateTime.Now.Year, i, 1) });
                }
                seriesPendapatan.AddRange(tanggal);
                seriesPengeluaran.AddRange(tanggal);
                seriesPendapatan = seriesPendapatan.Where(x => x.Tanggal.Year == DateTime.Now.Year).GroupBy(x => x.Tanggal.Month).Select(x => new GrafikDto { No = x.Key, Tanggal = x.Max(y => y.Tanggal), Nominal = x.Sum(y => y.Nominal) }).ToList();
                seriesPengeluaran = seriesPengeluaran.Where(x => x.Tanggal.Year == DateTime.Now.Year).GroupBy(x => x.Tanggal.Month).Select(x => new GrafikDto { No = x.Key, Tanggal = x.Max(y => y.Tanggal), Nominal = x.Sum(y => y.Nominal) }).ToList();
            }
            else if (grafik == 3) // Tahunan
            {
                for (int i = 9; i >= 0; i--)
                {
                    axis.Add((DateTime.Now.Year - i).ToString());
                    tanggal.Add(new() { Tanggal = new DateTime(DateTime.Now.Year - i, 1, 1) });
                }
                seriesPendapatan.AddRange(tanggal);
                seriesPengeluaran.AddRange(tanggal);
                seriesPendapatan = seriesPendapatan.Where(x => x.Tanggal.Year >= DateTime.Now.AddYears(-9).Year).GroupBy(x => x.Tanggal.Year).Select(x => new GrafikDto { No = x.Key, Tanggal = x.Max(y => y.Tanggal), Nominal = x.Sum(y => y.Nominal) }).ToList();
                seriesPengeluaran = seriesPengeluaran.Where(x => x.Tanggal.Year >= DateTime.Now.AddYears(-9).Year).GroupBy(x => x.Tanggal.Year).Select(x => new GrafikDto { No = x.Key, Tanggal = x.Max(y => y.Tanggal), Nominal = x.Sum(y => y.Nominal) }).ToList();
            }
            series.Clear();
            series.Add(new() { Name = "Pendapatan", Data = seriesPendapatan.OrderBy(x => x.Tanggal).Select(x => x.Nominal).ToArray() });
            series.Add(new() { Name = "Pengeluaran", Data = seriesPengeluaran.OrderBy(x => x.Tanggal).Select(x => x.Nominal).ToArray() });
            xAxisLabels = axis.ToArray();
        }
    }
}