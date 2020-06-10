using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VeracodeService.Security
{
    public interface ICryptoService
    {
        string GetHmacHeader(HmacRequest request);
    }

    public class CryptoService : ICryptoService
    {
        private const string _encoding = "UTF-8";
        private const string _scheme = "VERACODE-HMAC-SHA-256";
        private const string _hmac = "HmacSHA256";
        private const string _request_version = "vcode_request_version_1";
        private readonly RNGCryptoServiceProvider _rng;

        public CryptoService()
        {
            _rng = new RNGCryptoServiceProvider();
        }

        private byte[] ComputeHash(byte[] data, byte[] key)
        {
            HMAC hmac = HMAC.Create(_hmac);
            hmac.Key = key;
            return hmac.ComputeHash(data);
        }

        private byte[] CalculateDataSignature(byte[] apiKeyBytes,
                    byte[] nonceBytes, string dateStamp, string data)
        {
            var encoding = Encoding.GetEncoding(_encoding);
            var versionBytes = encoding.GetBytes(_request_version);
            var dataBytes = encoding.GetBytes(data);

            var hash1 = ComputeHash(nonceBytes, apiKeyBytes);
            var hash2 = ComputeHash(encoding.GetBytes(dateStamp), hash1);
            var hash3 = ComputeHash(versionBytes, hash2);
            return ComputeHash(dataBytes, hash3);
        }
        private string ToHexBinary(byte[] bytes) => BitConverter.ToString(bytes).Replace("-", "");

        private byte[] GetNonce(int size)
        {
            byte[] data = new byte[size];
            _rng.GetBytes(data);
            return data;
        }

        private byte[] FromHexBinary(string hex) => Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();

        public string GetHmacHeader(HmacRequest request)
        {
            if (request.UrlQueryParams != null)
                request.UriString += request.UrlQueryParams;

            var data = $"id={request.ApiId}&host={request.HostName}&url={request.UriString}&method={request.HttpMethod}";
            var dateStamp = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            var nonce = GetNonce(16);
            byte[] dataSignature = CalculateDataSignature(FromHexBinary(request.ApiKey), nonce, dateStamp, data);
            return $"{_scheme} id={request.ApiId},ts={dateStamp},nonce={ToHexBinary(nonce)},sig={ToHexBinary(dataSignature)}";
        }
    }
}
