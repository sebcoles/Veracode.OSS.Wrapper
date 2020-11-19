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
            return GetConfiguration(filelocation, "default");
        }
        public static VeracodeConfiguration GetConfiguration(string filelocation, string profileName)
        {
            if (String.IsNullOrEmpty(profileName))
                profileName = "default";

            var filePath = Environment.ExpandEnvironmentVariables(filelocation);

            if (!File.Exists(filePath))
                throw new ArgumentException("The veracode credential file provided is invalid.");

            var configFileFromPath = new ConfigParser(filePath);

            string apiId = configFileFromPath.GetValue(profileName, "veracode_api_key_id").Trim();
            string apikey = configFileFromPath.GetValue(profileName, "veracode_api_key_secret").Trim();

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
