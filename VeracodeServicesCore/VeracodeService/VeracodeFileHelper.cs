using System;
using VeracodeService.Configuration;

namespace VeracodeService
{
    public static class VeracodeFileHelper
    {
        public static VeracodeConfiguration GetConfiguration(string filelocation)
        {
            string apikey = "", apiId = "";
            var filePath = Environment.ExpandEnvironmentVariables(filelocation);
            using (var file = new System.IO.StreamReader(filePath))
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

            return new VeracodeConfiguration
            {
                ApiId = apiId,
                ApiKey = apikey
            };
        }
    }
}
