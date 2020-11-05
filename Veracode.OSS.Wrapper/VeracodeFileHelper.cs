using System;
using System.IO;
using Veracode.OSS.Wrapper.Configuration;
using Salaros.Configuration;

namespace Veracode.OSS.Wrapper
{
    public static class VeracodeFileHelper
    {
        public static VeracodeConfiguration GetConfiguration(string filelocation)
        {
            return VeracodeFileHelper.GetConfiguration(filelocation, "default");
        }
        public static VeracodeConfiguration GetConfiguration(string filelocation, string profileName)
        {
            string apikey = "", apiId = "";
            var filePath = Environment.ExpandEnvironmentVariables(filelocation);

            if (!File.Exists(filePath))
                throw new ArgumentException("The veracode credential file provided is invalid.");

            var configFileFromPath = new ConfigParser(filePath);

            apiId = configFileFromPath.GetValue(profileName, "veracode_api_key_id");
            apikey = configFileFromPath.GetValue(profileName, "veracode_api_key_secret");

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
