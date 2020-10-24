using Microsoft.Extensions.Options;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VeracodeService.Configuration;
using VeracodeService.Security;

namespace VeracodeService.Http
{
    public interface IHttpService
    {
        string Get(string path, NameValueCollection queryParams);
        string PostFile(string path, NameValueCollection queryParams, string filepath);
        Task<HttpResponseMessage> RestPost(string path, NameValueCollection queryParams, StringContent content);
        Task<HttpResponseMessage> RestGet(string path, NameValueCollection queryParams);
        Task<HttpResponseMessage> RestPut(string path, NameValueCollection queryParams, StringContent content);
        Task<HttpResponseMessage> RestDelete(string path, NameValueCollection queryParams);
    }
    public class HttpService : IHttpService
    {
        private const string _authHeader = "Authorization";
        private const string _base = "analysiscenter.veracode.com";
        private const string _restBase = "api.veracode.com";

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

        public Task<HttpResponseMessage> RestPost(string path, NameValueCollection queryParams, StringContent content)
        {
            return Request("POST", path, queryParams, content);
        }

        public Task<HttpResponseMessage> RestDelete(string path, NameValueCollection queryParams)
        {
            return Request("DELETE", path, queryParams, null);
        }

        public Task<HttpResponseMessage> RestPut(string path, NameValueCollection queryParams, StringContent content)
        {
            return Request("PUT", path, queryParams, content);
        }

        public Task<HttpResponseMessage> RestGet(string path, NameValueCollection queryParams)
        {
            return Request("GET", path, queryParams, null);
        }

        private Task<HttpResponseMessage> Request(string method, string path, NameValueCollection queryParams, StringContent content)
        {
            var queryString = "";
            if (queryParams.Count > 0)
                queryString = ToQueryString(queryParams);

            var hmacRequest = new HmacRequest
            {
                ApiId = _apiId,
                ApiKey = _apiKey,
                HostName = _restBase,
                HttpMethod = method,
                Url = path
            };
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri($"https://{_restBase}{path}{queryString}", UriKind.Absolute)
            };
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            if (content != null)
                request.Content = content;

            request.Headers.Add(_authHeader, _cryptoService.GetHmacHeader(hmacRequest));
            return new HttpClient().SendAsync(request);
        }

        public string Get(string path, NameValueCollection queryParams)
        {
            var webClient = new WebClient { BaseAddress = $"https://{_base}" };

            var queryString = "";
            if (queryParams.Count > 0)
                queryString = ToQueryString(queryParams);

            var hmacRequest = new HmacRequest
            {
                ApiId = _apiId,
                ApiKey = _apiKey,
                HostName = _base,
                HttpMethod = "GET",
                Url = path + queryString
            };

            var authorization = _cryptoService.GetHmacHeader(hmacRequest);
            webClient.Headers.Add(_authHeader, authorization);
            var response = webClient.DownloadString($"{path}{queryString}");
            return XmlParseHelper.GetDecodedXmlResponse(response, true);
        }

        public string PostFile(string path, NameValueCollection queryParams, string filepath)
        {
            var webClient = new WebClient { BaseAddress = $"https://{_base}" };

            var queryString = "";
            if (queryParams.Count > 0)
                queryString = ToQueryString(queryParams);

            var hmacRequest = new HmacRequest
            {
                ApiId = _apiId,
                ApiKey = _apiKey,
                HostName = _base,
                HttpMethod = "POST",
                Url = path + queryString
            };

            var authorization = _cryptoService.GetHmacHeader(hmacRequest);
            webClient.Headers.Add(_authHeader, authorization);
            webClient.Headers[HttpRequestHeader.ContentType] = "binary/octet-stream";
            byte[] bytes = File.ReadAllBytes(filepath);
            var responseBytes = webClient.UploadData($"{path}{queryString}", "POST", bytes);
            var responseString = Encoding.ASCII.GetString(responseBytes);
            return XmlParseHelper.GetDecodedXmlResponse(responseString, true);
        }

        private string ToQueryString(NameValueCollection nvc) =>
            $"?" + string.Join("&", nvc.AllKeys.Select(key => $"{key}={HttpUtility.UrlEncode(nvc.GetValues(key).First())}").ToArray());
    }
}