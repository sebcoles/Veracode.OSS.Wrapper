using System;
using System.IO;
using Veracode.OSS.Wrapper.Configuration;
using Salaros.Configuration;

namespace Veracode.OSS.Wrapper
{
    public static class VeracodeEnvHelper
    {
        public static VeracodeConfiguration GetConfiguration(string api_id_env_name = "VERACODE_API_KEY_ID", string api_key_env_name = "VERACODE_API_KEY_SECRET")
        {
            string apikey = Environment.GetEnvironmentVariable(api_key_env_name);
            string apiId = Environment.GetEnvironmentVariable(api_id_env_name);

            if (string.IsNullOrWhiteSpace(apiId))
                throw new ArgumentException("Value retrieved for veracode_api_key_id from ENV variables is empty!");

            if (string.IsNullOrWhiteSpace(apikey))
                throw new ArgumentException("Value retrieved for veracode_api_key_secret from ENV variables is empty!");

            return new VeracodeConfiguration
            {
                ApiId = apiId,
                ApiKey = apikey
            };
        }
    }
}
