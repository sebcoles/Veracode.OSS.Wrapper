using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using VeracodeService.Configuration;
using VeracodeService.Security;

namespace VeracodeService.Http
{
    public interface IHttpService
    {
        string Get(string path, NameValueCollection queryParams);
    }
    public class HttpService : IHttpService
    {
        private const string _authHeader = "Authorization";
        private const string _base = "analysiscenter.veracode.com";

        private readonly ICryptoService _cryptoService;
        private string _apiId;
        private string _apiKey;

        public HttpService(IOptions<VeracodeConfiguration> config,
            ICryptoService cryptoService)
        {
            _apiId = config.Value.ApiId;
            _apiKey = config.Value.ApiKey;
            _cryptoService = cryptoService;
        }

        public string Get(string path, NameValueCollection queryParams)
        {
            var webClient = new WebClient { BaseAddress = $"https://{_base}" };
            var queryString = ToQueryString(queryParams);
            var hmacRequest = new HmacRequest
            {
                ApiId = _apiId,
                ApiKey = _apiKey,
                HostName = _base,
                HttpMethod = "GET",
                UrlQueryParams = queryString,
                UriString = path
            };

            var authorization = _cryptoService.GetHmacHeader(hmacRequest);
            webClient.Headers.Add(_authHeader, authorization);
            var response = webClient.DownloadString($"{path}{queryString}");
            return XmlParseHelper.GetDecodedXmlResponse(response, true);
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (
                from key in nvc.AllKeys
                from value in nvc.GetValues(key)
                select string.Format(
            "{0}={1}",
            key,
            HttpUtility.UrlEncode(value, Encoding.GetEncoding("ISO-8859-1")))
                ).ToArray();
            return "?" + string.Join("&", array);
        }
    }
}
