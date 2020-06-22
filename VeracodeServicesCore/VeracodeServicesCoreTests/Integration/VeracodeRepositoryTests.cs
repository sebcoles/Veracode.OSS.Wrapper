using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.IO;
using VeracodeService;
using VeracodeService.Http;
using VeracodeService.Repositories;
using VeracodeService.Security;
using VeracodeWebhooks.Configuration;

namespace VeracodeServicesCoreTests.Integration
{
    [TestFixture]
    [Category("Integration")]
    public class VeracodeRepositoryTests
    {
        private VeracodeConfiguration veracodeConfig = new VeracodeConfiguration();
        private TestData testData = new TestData();
        private IVeracodeRepository _repo;

        [OneTimeSetUp]
        public void Setup()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.Development.json", false)
                .Build();

            Configuration.Bind("Veracode", veracodeConfig);
            Configuration.Bind("TestData", testData);

            var options = Options.Create(veracodeConfig);
            _repo = new VeracodeRepository(
                new VeracodeWrapper(new HttpService(options, new CryptoService())));
        }

        [Test]
        public void GetAllApps()
        {
            var result = _repo.GetAllApps();
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetAllBuildsForApp()
        {
            var result = _repo.GetAllBuildsForApp(testData.AppId);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetAllMitigationsForBuild()
        {
            var result = _repo.GetAllMitigationsForBuild(testData.BuildId);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetAppDetail()
        {
            var result = _repo.GetAppDetail(testData.AppId);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetBuildDetail()
        {
            var result = _repo.GetBuildDetail(testData.AppId, testData.BuildId);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetDetailedReport()
        {
            var result = _repo.GetDetailedReport(testData.BuildId);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetFilesForBuild()
        {
            var result = _repo.GetFilesForBuild(testData.AppId, testData.BuildId);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetFlaws()
        {
            var result = _repo.GetFlaws(testData.BuildId);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetModules()
        {
            var result = _repo.GetModules(testData.AppId, testData.BuildId);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetSandboxesForApp()
        {
            var result = _repo.GetSandboxesForApp(testData.AppId);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetSeverity()
        {
            var result = _repo.GetSeverity(testData.BuildId);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetEntryPoints()
        {
            var result = _repo.GetEntryPoints(testData.BuildId);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetCallStacks()
        {
            var result = _repo.GetCallStacks(testData.BuildId, testData.FlawId);
            Assert.IsNotNull(result);
        }
    }
}
