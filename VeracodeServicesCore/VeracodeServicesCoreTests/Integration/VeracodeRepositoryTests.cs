using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using VeracodeService;
using VeracodeService.Configuration;
using VeracodeService.Enums;
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
            foreach (var app in apps.Where(x => x.app_name.StartsWith(testData.NewAppName)))
                _repo.DeleteApp(new ApplicationType { app_id = app.app_id });

            var users = _repo.GetUsers();
            foreach (var username in users.Where(x => x.StartsWith(testData.NewUserEmail)))
                _repo.DeleteUser(username);

            var teams = _repo.GetTeams();
            foreach (var team in teams.Where(x => x.team_name.StartsWith(testData.NewTeamName)))
                _repo.DeleteTeam(team.team_id);
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
        public void Upload_File_For_Prescan()
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
            _repo.CreateBuild($"{app.app_id}", build);

            var files = _repo.UploadFileForPrescan($"{app.app_id}", "Assets/testapp1.zip");
            Assert.AreEqual(1, files.Count());

            files = _repo.UploadFileForPrescan($"{app.app_id}", "Assets/testapp2.zip");
            Assert.AreEqual(2, files.Count());

            _repo.DeleteApp(
                new ApplicationType { app_id = app.app_id }
            );
        }

        [Test]
        public void Start_Finish_Prescan_And_Results()
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
            _repo.CreateBuild($"{app.app_id}", build);
            _repo.UploadFileForPrescan($"{app.app_id}", "Assets/testapp1.zip");

            var buildinfo = _repo.StartPrescan($"{app.app_id}");
            var buildFinished = BuildStatusType.PreScanSubmitted;
            while (buildFinished == BuildStatusType.PreScanSubmitted)
            {
                buildFinished = _repo
                    .GetBuildDetail($"{app.app_id}", $"{buildinfo.Build_id}")
                    .build.analysis_unit[0].status;
                Thread.Sleep(30000);
            }

            Assert.AreEqual(BuildStatusType.PreScanSuccess, buildFinished);

            _repo.DeleteApp(
                new ApplicationType { app_id = app.app_id }
            );
        }

        [Test]
        public void Create_Update_Delete_User()
        {
            var user = new LoginAccount
            {
                first_name = testData.NewFirstName,
                last_name = testData.NewLastName,
                email_address = testData.NewUserEmail,
                teams = testData.NewUserTeam
            };
            var role = testData.NewUserRoles
                .Select(x => (Roles)x).ToArray();

            _repo.CreateUser(user, role);            

            var retrievedUserName = _repo.GetUsers()
                .SingleOrDefault(x => x.Equals(testData.NewUserEmail));

            Assert.IsNotNull(retrievedUserName);

            user.username = testData.NewUserEmail;
            user.first_name = testData.UpdatedFirstName;
            user.last_name = testData.UpdatedLastName;
            role = testData.UpdatedUserRoles
                .Select(x => (Roles)x).ToArray();

            user = _repo.UpdateUser(user, role);

            var updatedUser = _repo.GetUser(testData.NewUserEmail);

            Assert.AreEqual(testData.UpdatedFirstName, updatedUser.first_name);
            Assert.AreEqual(testData.UpdatedLastName, updatedUser.last_name);

            _repo.DeleteUser(testData.NewUserEmail);

            retrievedUserName = _repo.GetUsers()
                .SingleOrDefault(x => x.Equals(testData.NewUserEmail));

            Assert.IsNull(retrievedUserName);           
        }

        [Test]
        public void Create_Update_Delete_Team()
        {
            var team = new teaminfo
            {
                team_name = testData.NewTeamName
            };

            _repo.CreateTeam(team);

            var retrievedTeam = _repo.GetTeams()
                .SingleOrDefault(x => x.team_name.Equals(testData.NewTeamName));

            Assert.IsNotNull(retrievedTeam);

            retrievedTeam.team_name = testData.UpdatedTeamName;

            var teaminfo = new teaminfo
            {
                team_id = retrievedTeam.team_id,
                team_name = retrievedTeam.team_name
            };

            var checkTeam = _repo.UpdateTeam(teaminfo);
            Assert.AreEqual(checkTeam.team_name, testData.UpdatedTeamName);

            _repo.DeleteTeam(checkTeam.team_id);

            retrievedTeam = _repo.GetTeams()
                .SingleOrDefault(x => x.team_name.Equals(testData.UpdatedTeamName));

            Assert.IsNull(retrievedTeam);
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
