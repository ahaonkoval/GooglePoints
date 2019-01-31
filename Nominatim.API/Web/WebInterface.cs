using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Nominatim.API.Contracts;
using System.Text;
using System;
using System.Linq;
using System.Collections;
using System.Web;

namespace Nominatim.API.Web {
    /// <summary>
    ///     Provides a means of sending HTTP requests to a Nominatim server
    /// </summary>
    public static class WebInterface {
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        ///     Send a request to the Nominatim server
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize response onto</typeparam>
        /// <param name="url">URL of Nominatim server method</param>
        /// <param name="parameters">Query string parameters</param>
        /// <returns>Deserialized instance of T</returns>
        public static async Task<T> GetRequest<T>(string url, Dictionary<string, string> parameters) {

            //QueryHelpers.AddQueryString(url, parameters);
            //var req = string.Empty;// QueryHelpers.AddQueryString(url, parameters);
            var req = StringFormatExtensions.GetFullUrl(url, parameters);

            _httpClient.DefaultRequestHeaders.UserAgent.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("f1ana.Nominatim.API", Assembly.GetExecutingAssembly().GetName().Version.ToString()));

            var result = await _httpClient.GetStringAsync(req).ConfigureAwait(false);
            var settings = new JsonSerializerSettings {ContractResolver = new PrivateContractResolver()};

            return JsonConvert.DeserializeObject<T>(result, settings);
        }
    }

    public static class StringFormatExtensions
    {
        public static String GetFullUrl(this string value, Dictionary<string, string> source)
        {
            if (String.IsNullOrEmpty(value))
                return value;
            //var url = HttpUtility.UrlEncode(
            //    value + string.Format("?{0}",
            //        string.Join("&",
            //            source.Select(kvp =>
            //                string.Format("{0}={1}", kvp.Key, kvp.Value)))));

            var url = value + string.Format("?{0}",
                    string.Join("&",
                        source.Select(kvp =>
                            string.Format("{0}={1}", kvp.Key, kvp.Value))));

            return url;
        }
    }
}