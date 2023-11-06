namespace ProduksiManufaktur.Api
{
    public static class Utilities
    {
        public static string Left(this string text, int characterCount)
        {
            text += string.Empty;
            if (characterCount > text.Length) characterCount = text.Length;
            return text[..characterCount];
        }

        public static string Right(this string text, int characterCount)
        {
            text += string.Empty;
            if (characterCount > text.Length) characterCount = text.Length;
            return text[^characterCount..];
        }

        public static string Mid(this string text, int index)
        {
            text += string.Empty;
            return text[index..];
        }

        public static string Mid(this string text, int index, int characterCount)
        {
            text += string.Empty;
            if (characterCount > text.Length) characterCount = text.Length - index;
            return text.Substring(index, characterCount);
        }

        public static int IVal(object obj)
        {
            _ = int.TryParse(obj.ToString(), out int result);
            return result;
        }

        public static decimal DVal(object obj)
        {
            _ = decimal.TryParse(obj.ToString(), out decimal result);
            return result;
        }

        public static bool BVal(object obj)
        {
            _ = bool.TryParse(obj.ToString(), out bool result);
            return result;
        }

        public static DateTime DtVal(object obj)
        {
            _ = DateTime.TryParse(obj.ToString(), out DateTime result);
            return result;
        }

        public static decimal Tambahi(ref decimal x, decimal y)
        {
            return x += y;
        }

        public static decimal Kurangi(ref decimal x, decimal y)
        {
            return x -= y;
        }

        public static int Tambahi(ref int x, int y)
        {
            return x += y;
        }

        public static int Kurangi(ref int x, int y)
        {
            return x -= y;
        }

        public static T Nullify<T>(T obj)
        {
            var type = typeof(T);
            foreach (var x in type.GetProperties())
            {
                if ((x.PropertyType.IsClass && x.PropertyType != typeof(string) && x.PropertyType != typeof(byte[])) || (x.PropertyType.IsGenericType && x.PropertyType != typeof(DateTime?) && x.PropertyType != typeof(TimeSpan?)) || (x.PropertyType.IsArray && x.PropertyType != typeof(byte[]))) x!.SetValue(obj, null, null);
            }
            return obj;
        }

        public static List<T> Nullifies<T>(IEnumerable<T> obj)
        {
            foreach (var o in obj)
            {
                var type = typeof(T);
                foreach (var x in type.GetProperties())
                {
                    if ((x.PropertyType.IsClass && x.PropertyType != typeof(string) && x.PropertyType != typeof(byte[])) || (x.PropertyType.IsGenericType && x.PropertyType != typeof(DateTime?) && x.PropertyType != typeof(TimeSpan?)) || (x.PropertyType.IsArray && x.PropertyType != typeof(byte[]))) x!.SetValue(o, null, null);
                }
            }
            return obj.ToList();
        }

        public static bool HaveNullProperty(object myObject)
        {
            return myObject.GetType().GetProperties()
                    .Where(x => x.PropertyType == typeof(string) || x.PropertyType == typeof(int))
                    .Select(y => y.GetValue(myObject)?.ToString())
                    .Any(z => string.IsNullOrEmpty(z) || z == "0");
        }

        //public static string Capitalize(string input)
        //{
        //    return string.IsNullOrEmpty(input) ? string.Empty : string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
        //}

        public static bool ReadOnlyAccess(AuthorizationHandlerContext context, string entitas)
        {
            return !context.User.HasClaim(entitas, "S0") && (context.User.HasClaim(entitas, "W1") || context.User.HasClaim(entitas, "W2") || context.User.HasClaim(entitas, "S1") || context.User.HasClaim(entitas, "S2"));
        }

        public static bool ReadWriteAccess(AuthorizationHandlerContext context, string entitas)
        {
            return !context.User.HasClaim(entitas, "S0") && !context.User.HasClaim(entitas, "S1") && (context.User.HasClaim(entitas, "W2") || context.User.HasClaim(entitas, "S2"));
        }

        /// <summary>
        /// <para>Membuat Id baru untuk string. Secara otomatis mengisi kekosongan Id</para>
        /// <para>barang.Id = GenerateId(_appDbContext.Barang.Select(x => x.Id), 4, "BRG");</para>
        /// </summary>
        public static string GenerateId(IEnumerable<string> ids, int digit, string prefix)
        {
            int count = 1;
            foreach (var x in ids.Order())
            {
                if (IVal(x.Right(digit)) != count) break;
                count++;
            }
            return prefix + count.ToString(new string('0', digit));
        }

        /// <summary>
        /// <para>Membuat satu Id int baru (mengisi kekosongan)</para>
        /// <para>perubahanStokBarang.Id = GenerateId(_appDbContext.PerubahanStokBarang.Select(x => x.Id));</para>
        /// </summary>
        public static int GenerateId(IEnumerable<int> ids)
        {
            int count = 1;
            foreach (var x in ids.Order())
            {
                if (x != count) break;
                count++;
            }
            return count;
        }

        /// <summary>
        /// <para>Membuat Id baru untuk transaksi berdasarkan tanggalnya</para>
        /// <para>pembelian.Id = GenerateId("PBLN", pembelian.Tanggal, _appDbContext.Pembelian.Where(x => x.Tanggal.Date == pembelian.Tanggal.Date).Select(x => x.Id));</para>
        /// </summary>
        public static string GenerateId(string prefix, DateTime tanggalTerpilih, IEnumerable<string> ids)
        {
            int nomorAkhir = IVal(ids.Order().LastOrDefault("00").Right(2)) + 1;
            return $"{prefix}-{tanggalTerpilih:yyMMdd}{nomorAkhir:00}";
        }

        /// <summary>
        /// <para>Membuat kumpulan Id int baru untuk Detail (mengisi kekosongan)</para>
        /// <para>var idsDetail = GenerateId(_appDbContext.FormulasiDetail.Select(x => x.Id).DefaultIfEmpty().Max(), _appDbContext.FormulasiDetail.Select(x => x.Id), formulasiDetail);</para>
        /// </summary>
        public static int[] GenerateId<T>(int terbesar, IEnumerable<int> ids, IEnumerable<T> dataBaru)
        {
            var list1 = ids;
            var list2 = Enumerable.Range(1, terbesar + dataBaru.Count());
            return list2.Except(list1).ToArray();
        }
    }
}