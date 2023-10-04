using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace FishingCactus
{
    public static class NetworkStatusChecker
    {
        // -- FIELDS

        private const string DefaultUrl = "http://www.google.com";
        private const string ChinaUrl = "http://www.baidu.com";
        private const string IranUrl = "http://www.aparat.com";

        private static string CurrentTestUrl = string.Empty;

        // -- METHODS

        static NetworkStatusChecker()
        {
            CurrentTestUrl = DetermineUrlToTest();
        }

        private static string DetermineUrlToTest()
        {
            string culture_name = CultureInfo.InstalledUICulture.Name;

            if( culture_name.StartsWith( "fa" ) )
            {
                return IranUrl;
            }

            if( culture_name.StartsWith( "zh" ) )
            {
                return ChinaUrl;
            }

            return DefaultUrl;
        }

        public static async Task<bool> IsInternetAvailableAsync(
            int timeout_in_ms = 5000
            )
        {
            try
            {
                using( var httpClient = new HttpClient() )
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds( timeout_in_ms );
                    var response = await httpClient.GetAsync( CurrentTestUrl );
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
