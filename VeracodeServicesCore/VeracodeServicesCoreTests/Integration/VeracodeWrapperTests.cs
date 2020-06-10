using Microsoft.Extensions.Options;
using NUnit.Framework;
using VeracodeService;
using VeracodeService.Http;
using VeracodeService.Security;
using VeracodeWebhooks.Configuration;

namespace VeracodeServicesCoreTests.Integration
{
    [TestFixture]
    [Category("Integration")]
    public class VeracodeWrapperTests
    {
        private VeracodeConfiguration veracodeConfig = new VeracodeConfiguration
        {
            ApiId = "__YOUR_API_ID__",
            ApiKey = "__YOUR_API_KEY__"
        };

        [Test]
        public void GetAppList()
        {
            var options = Options.Create(veracodeConfig);
            var wrapper = new VeracodeWrapper(new HttpService(options, new CryptoService()));
            var xml = wrapper.GetAppList();
            Assert.IsNotEmpty(xml);
        }

        [Test]
        public void GetBuildList()
        {
            var options = Options.Create(veracodeConfig);
            var wrapper = new VeracodeWrapper(new HttpService(options, new CryptoService()));
            var xml = wrapper.GetBuildList("758217");
            Assert.IsNotEmpty(xml);
        }
    }
}
