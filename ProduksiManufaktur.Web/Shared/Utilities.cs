using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace ProduksiManufaktur.Web.Shared
{
    public static class Utilities
    {
        public static bool Cari(this string text, string searchFor)
        {
            return text.Contains(searchFor, StringComparison.OrdinalIgnoreCase);
        }

        public static IEnumerable<string> Cari(this IEnumerable<string> text, string searchFor)
        {
            return text.Where(x => x.Contains(searchFor, StringComparison.InvariantCultureIgnoreCase));
        }

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

        public static KeyValuePair<TKey, TValue> GetKvp<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return new KeyValuePair<TKey, TValue>(key, dictionary[key]);
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

        public static DateTime GetDate()
        {
            var client = new TcpClient("time.nist.gov", 13);
            using var streamReader = new StreamReader(client.GetStream());
            var response = streamReader.ReadToEnd();
            var utcDateTimeString = response.Substring(7, 17);
            return DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        private static async Task<DateTime?> GetNetworkTime(string ntpServer = "pool.ntp.org")
        {
            // https://stackoverflow.com/questions/1193955/how-to-query-an-ntp-server-using-c

            if (ntpServer == null)
            {
                throw new ArgumentNullException(nameof(ntpServer));
            }

            try
            {
                const int daysTo1900 = (1900 * 365) + 95; // 95 = offset for leap-years etc.
                const long ticksPerSecond = 10000000L;
                const long ticksPerDay = 24 * 60 * 60 * ticksPerSecond;
                const long ticksTo1900 = daysTo1900 * ticksPerDay;

                var ntpData = new byte[48];
                ntpData[0] = 0x1B; // LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                // ReSharper disable once RedundantAssignment

                var pingDuration = Stopwatch.GetTimestamp(); // temp access (JIT-Compiler need some time at first call)

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    await socket.ConnectAsync(ipEndPoint);
                    socket.ReceiveTimeout = 5000;
                    socket.Send(ntpData);
                    pingDuration = Stopwatch.GetTimestamp(); // after Send-Method to reduce WinSocket API-Call time

                    socket.Receive(ntpData);
                    pingDuration = Stopwatch.GetTimestamp() - pingDuration;
                }

                var pingTicks = pingDuration * ticksPerSecond / Stopwatch.Frequency;

                // optional: display response-time
                // Console.WriteLine("{0:N2} ms", new TimeSpan(pingTicks).TotalMilliseconds);

                var intPart = (long)ntpData[40] << 24 | (long)ntpData[41] << 16 | (long)ntpData[42] << 8 | ntpData[43];
                var fractPart = (long)ntpData[44] << 24 | (long)ntpData[45] << 16 | (long)ntpData[46] << 8 | ntpData[47];
                var netTicks = (intPart * ticksPerSecond) + (fractPart * ticksPerSecond >> 32);

                var networkDateTime = new DateTime(ticksTo1900 + netTicks + (pingTicks / 2));

                return networkDateTime.ToLocalTime(); // without ToLocalTime() = faster
            }
            catch
            {
                // fail
                return null;
            }
        }

        public static string KonversiHak(this char x) => x switch
        {
            '0' => "<span style='color:#f44336'>0</span>",
            '1' => "<span style='color:#ff9800'>1</span>",
            '2' => "<span style='color:#00c853'>2</span>",
            _ => string.Empty
        };

        public static void CopyPropertiesTo<T>(this T source, T target)
        {
            var type = typeof(T);
            foreach (var sourceProperty in type.GetProperties())
            {
                var targetProperty = type.GetProperty(sourceProperty.Name);
                targetProperty!.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
            foreach (var sourceField in type.GetFields())
            {
                var targetField = type.GetField(sourceField.Name);
                targetField!.SetValue(target, sourceField.GetValue(source));
            }
        }

        public static bool HaveNullProperty(object myObject)
        {
            return myObject.GetType().GetProperties()
                    .Where(x => x.PropertyType == typeof(string) || x.PropertyType == typeof(int))
                    .Select(y => y.GetValue(myObject)?.ToString())
                    .Any(z => string.IsNullOrEmpty(z) || z == "0");
        }

        public static string Capitalize(string input)
        {
            return string.IsNullOrEmpty(input) ? string.Empty : string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
        }

        public static bool ReadOnlyAccess(AuthorizationHandlerContext context, string entitas)
        {
            return !context.User.HasClaim(entitas, "S0") && (context.User.HasClaim(entitas, "W1") || context.User.HasClaim(entitas, "W2") || context.User.HasClaim(entitas, "S1") || context.User.HasClaim(entitas, "S2"));
        }

        public static bool ReadWriteAccess(AuthorizationHandlerContext context, string entitas)
        {
            return !context.User.HasClaim(entitas, "S0") && !context.User.HasClaim(entitas, "S1") && (context.User.HasClaim(entitas, "W2") || context.User.HasClaim(entitas, "S2"));
        }
    }
}