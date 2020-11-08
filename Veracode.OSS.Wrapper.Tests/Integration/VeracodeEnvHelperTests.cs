using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Veracode.OSS.Wrapper.Tests.Integration
{
    [TestFixture]
    public class VeracodeEnvHelperTests
    {
        private string apiIdLabel = "VERACODE_API_KEY_ID";
        private string apiKeyLabel = "VERACODE_API_KEY_SECRET";
        private string test_id = "jam_id";
        private string test_key = "jam_key";
        [SetUp]
        public void SetEnvVariables()
        {
            Environment.SetEnvironmentVariable(apiIdLabel, test_id);
            Environment.SetEnvironmentVariable(apiKeyLabel, test_key);
        }

        [Test]
        public void VeracodeConfiguration_GetCredentials()
        {
            var configuration = VeracodeEnvHelper.GetConfiguration();
            Assert.AreEqual(test_id, configuration.ApiId);
            Assert.AreEqual(test_key, configuration.ApiKey);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable(apiIdLabel, null);
            Environment.SetEnvironmentVariable(apiKeyLabel, null);
        }
    }
}
