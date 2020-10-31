using System;
using System.IO;
using Veracode.OSS.Wrapper.Configuration;

namespace Veracode.OSS.Wrapper
{
    public static class VeracodeFileHelper
    {
        public static VeracodeConfiguration GetConfiguration(string filelocation)
        {
            string apikey = "", apiId = "";
            var filePath = Environment.ExpandEnvironmentVariables(filelocation);

            if (!File.Exists(filePath))
                throw new ArgumentException("The veracode credential file provided is invalid.");

            using (var file = new StreamReader(filePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("veracode_api_key_id"))
                        apiId = line.Replace(" ", "").Substring(20);

                    if (line.Contains("veracode_api_key_secret "))
                        apikey = line.Replace(" ", "").Substring(24);
                }
            }

            if (string.IsNullOrWhiteSpace(apiId) || string.IsNullOrWhiteSpace(apikey))
                throw new ArgumentException("The veracode credential file provided is invalid.");

            return new VeracodeConfiguration
            {
                ApiId = apiId,
                ApiKey = apikey
            };
        }
    }
}
