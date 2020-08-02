using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VeracodeService;
using VeracodeService.Configuration;
using VeracodeService.Models;

namespace VeracodeServicesCoreTests.Integration
{
    [TestFixture]
    [Category("Integration")]
    public class VeracodeRepositoryTests
    {
        private VeracodeConfiguration veracodeConfig = new VeracodeConfiguration();
        private TestData testData = new TestData();
        private IVeracodeRepository _repo;
        private Random _rand = new Random();

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
            _repo = new VeracodeRepository(options);
        }

        [TearDown]
        public void TearDown()
        {
            var apps = _repo.GetAllApps();
            var appsToRemove = new List<AppType>();
            foreach (var app in apps.Where(x => x.app_name.StartsWith(testData.NewAppName)))
                _repo.DeleteApp(new ApplicationType { app_id = app.app_id });
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
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_Update_Delete_App()
        {
            var newApp = new ApplicationType
            {
                app_name = testData.NewAppName + _rand.Next(99999),
                business_criticality = testData.NewAppCriticality
            };
            var result = _repo.CreateApp(newApp);
            Assert.IsNotNull(result);

            var created = _repo.GetAllApps()
                .Single(x => x.app_name == newApp.app_name);

            Assert.IsNotNull(created);

            result.app_name = testData.UpdatedAppName;
            result.business_criticality = testData.UpdatedAppCriticality;

            var updatedApp = _repo.UpdateApp(result);

            Assert.AreEqual(testData.UpdatedAppName, updatedApp.app_name);
            Assert.AreEqual(testData.UpdatedAppCriticality, updatedApp.business_criticality);

            _repo.DeleteApp(
                new ApplicationType { app_id = created.app_id }
            );

            created = _repo.GetAllApps()
                .SingleOrDefault(x => x.app_name == newApp.app_name);

            Assert.IsNull(created);
        }

        [Test]
        public void Create_Update_Delete_Build()
        {
            var app = new ApplicationType
            {
                app_name = testData.NewAppName + _rand.Next(99999),
                business_criticality = testData.NewAppCriticality
            };
            app = _repo.CreateApp(app);

            var build = new BuildInfoBuildType
            {
                version = testData.NewBuildVersion
            };
            var newBuild = _repo.CreateBuild($"{app.app_id}", build);
            var retrievedBuild = _repo.GetBuildDetail($"{app.app_id}", $"{newBuild.build_id}");

            Assert.IsNotNull(retrievedBuild);

            var updatedBuild = new BuildInfoBuildType
            {
                version = testData.UpdatedBuildVersion,
                build_id = newBuild.build_id
            };

            var updatedRetrievedBuild = _repo.UpdateBuild($"{app.app_id}", updatedBuild);
            retrievedBuild = _repo.GetBuildDetail($"{app.app_id}", $"{newBuild.build_id}");

            Assert.AreEqual(testData.UpdatedBuildVersion, retrievedBuild.build.version);

            _repo.DeleteBuild($"{app.app_id}");

            var buildList = _repo.GetAllBuildsForApp($"{app.app_id}");

            Assert.IsNull(buildList.SingleOrDefault(x => x.build_id == retrievedBuild.build.build_id));

            _repo.DeleteApp(
                new ApplicationType { app_id = app.app_id }
            );
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
