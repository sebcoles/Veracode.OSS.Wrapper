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
using VeracodeService.Rest;

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
        public void GetLatestBuildForApp()
        {
            var result = _repo.GetLatestcan(testData.AppId);
            Assert.IsNotNull(result);
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
                business_criticality = testData.NewAppCriticality,
                business_owner = "Jammy Jam",
                business_owner_email = "jam@jam.com",
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
        public void Upload_Files_Prescan_And_Scan()
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

            var buildinfo = _repo.StartPrescan($"{app.app_id}");
            var buildFinished = BuildStatusType.PreScanSubmitted;
            while (buildFinished == BuildStatusType.PreScanSubmitted)
            {
                buildFinished = _repo
                    .GetBuildDetail($"{app.app_id}", $"{buildinfo.Build_id}")
                    .build.analysis_unit[0].status;
                Thread.Sleep(30000);
            }

            var modules = _repo.GetModules($"{app.app_id}", $"{buildinfo.Build_id}");
            var scannableModuleIds = modules.Where(x => !x.has_fatal_errors).Select(x => x.id).ToArray();
            var scaninfo = _repo.StartScan($"{app.app_id}", string.Join(",", scannableModuleIds));

            buildFinished = BuildStatusType.ScanInProcess;
            while (buildFinished == BuildStatusType.ScanInProcess)
            {
                buildFinished = _repo
                    .GetBuildDetail($"{app.app_id}", $"{buildinfo.Build_id}")
                    .build.analysis_unit[0].status;
                Thread.Sleep(30000);
            }

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
            var team_name = testData.NewTeamName + _rand.Next(99999);
            var team = new teaminfo
            {
                team_name = team_name
            };

            var newTeam = _repo.CreateTeam(team);

            var user = new LoginAccount
            {
                first_name = testData.NewFirstName,
                last_name = testData.NewLastName,
                email_address = testData.NewUserEmail,
                teams = team_name
            };
            var role = testData.NewUserRoles
                .Select(x => (Roles)x).ToArray();

            var newUser = _repo.CreateUser(user, role);

            var retrievedTeam = _repo.GetTeamInfo(newTeam.team_id, true, false);

            Assert.IsNotNull(retrievedTeam);
            
            team_name = testData.UpdatedTeamName + _rand.Next(99999);
            retrievedTeam.team_name = team_name;

            var teaminfo = new teaminfo
            {
                team_id = newTeam.team_id,
                team_name = retrievedTeam.team_name
            };

            var checkTeam = _repo.UpdateTeam(teaminfo);
            Assert.AreEqual(checkTeam.team_name, team_name);

            _repo.DeleteTeam(checkTeam.team_id);
            _repo.DeleteUser(newUser.email_address);

            var teams = _repo.GetTeams()
                .SingleOrDefault(x => x.team_name.Equals(team_name));

            Assert.IsNull(teams);
        }

        [Test]
        public void GetAppDetail()
        {
            var result = _repo.GetAppDetail(testData.AppId);
            Assert.IsNotNull(result);
        }


        [Test]
        public void XMLParseErrorOnInvalidPayload()
        {
            var ex = Assert.Throws<XmlParseError>(() => _repo.GetAppDetail("JAM!"));
            Assert.AreEqual("no app_id parameter specified", ex.Message);
        }

        [Test]
        public void GetBuildDetail()
        {
            var result = _repo.GetBuildDetail(testData.AppId, testData.BuildId);
            Assert.IsNotNull(result);
            Assert.AreEqual($"{result.build_id}", testData.BuildId);
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
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetSeverity()
        {
            var result = _repo.GetSeverity(testData.BuildId);
            Assert.IsNotNull(result);
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

        [Test]
        public void Create_Update_Delete_Policy()
        {
            var newPolicy = new PolicyVersion
            {
                name = testData.Policyname,
                description = testData.Policydescription,
                sca_blacklist_grace_period = testData.Policysca_blacklist_grace_period,
                score_grace_period = testData.Policyscore_grace_period,
                sev0_grace_period = testData.Policysev0_grace_period,
                sev1_grace_period = testData.Policysev1_grace_period,
                sev2_grace_period = testData.Policysev2_grace_period,
                sev3_grace_period = testData.Policysev3_grace_period,
                sev4_grace_period = testData.Policysev4_grace_period,
                sev5_grace_period = testData.Policysev5_grace_period,
                type = testData.Policytype,
                vendor_policy = testData.Policyvendor_policy,
                scan_frequency_rules = new List<ScanFrequencyRule>(),
                finding_rules = new List<FindingRule>()
            };

            var policy = _repo.CreatePolicy(newPolicy);
            Assert.IsNotNull(policy);

            policy.name = testData.Policynameupdated;
            _ = _repo.UpdatePolicy(policy, policy.guid);

            var retrievedPolicy = _repo.GetPolicies().SingleOrDefault(x => x.name.Contains(testData.Policynameupdated));
            Assert.AreEqual(testData.Policynameupdated, retrievedPolicy.name);

            var deleted = _repo.DeletePolicy(policy.guid).SingleOrDefault(x => x.name.Contains(testData.Policynameupdated));
            Assert.IsNull(deleted);
        }
    }
}
