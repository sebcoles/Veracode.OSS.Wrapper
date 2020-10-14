using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VeracodeService.Configuration;
using VeracodeService.Enums;
using VeracodeService.Http;
using VeracodeService.Models;
using VeracodeService.Rest;
using VeracodeService.Security;

namespace VeracodeService
{
    public interface IVeracodeRepository
    {
        IEnumerable<AppType> GetAllApps();
        ApplicationType CreateApp(ApplicationType app);
        ApplicationType UpdateApp(ApplicationType app);
        deleteapp DeleteApp(ApplicationType app);
        IEnumerable<BuildType> GetAllBuildsForApp(string appId);
        appinfo GetAppDetail(string appId);
        IEnumerable<SandboxType> GetSandboxesForApp(string appId);
        buildinfo GetBuildDetail(string appId, string buildId);
        buildinfo GetLatestcan(string appId);
        IEnumerable<FileListFileType> GetFilesForBuild(string appId, string buildId);
        IEnumerable<ModuleType> GetModules(string appId, string buildId);
        detailedreport GetDetailedReport(string buildId);
        MitigationInfoIssueType[] GetAllMitigationsForBuild(string buildIds);
        FlawType[] GetFlaws(string buildId);
        SeverityType[] GetSeverity(string buildId);
        DetailedReportModuleType[] GetEntryPoints(string buildId);
        Callstacks GetCallStacks(string buildId, string flawId);
        BuildInfoBuildType CreateBuild(string app_id, BuildInfoBuildType build);
        BuildInfoBuildType UpdateBuild(string app_id, BuildInfoBuildType build);
        deletebuildresult DeleteBuild(string app_id, string sandbox_id = "");
        string[] GetUsers();
        teamlistTeam[] GetTeams();
        teaminfo GetTeamInfo(string team_id, bool include_users = false, bool include_applications = false);
        deleteuserresult DeleteUser(string username);
        deleteteamresult DeleteTeam(string team_id);
        LoginAccount CreateUser(LoginAccount user, Roles[] roles);
        teaminfo CreateTeam(teaminfo team);
        LoginAccount UpdateUser(LoginAccount user, Roles[] roles);
        teaminfo UpdateTeam(teaminfo team);
        LoginAccount GetUser(string username);
        FileListFileType[] UploadFileForPrescan(string app_id, string filepath);
        buildinfo StartPrescan(string app_id);
        buildinfo StartScan(string app_id, string modules);
        PolicyVersion[] GetPolicies();
        PolicyVersion[] DeletePolicy(string policyGuid);
        PolicyVersion UpdatePolicy(PolicyVersion policy, string policyGuid);
        PolicyVersion CreatePolicy(PolicyVersion policy);

    }
    public class VeracodeRepository : IVeracodeRepository
    {
        private const int FLAW_BATCH_LIMIT = 500;
        private readonly IVeracodeWrapper _wrapper;
        private readonly IPolicyClient _policyClient;
        public VeracodeRepository(IOptions<VeracodeConfiguration> config)
        {
            _wrapper = new VeracodeWrapper(new HttpService(config, new CryptoService()));
            _policyClient = new PolicyClient(new HttpService(config, new CryptoService()));
        }
        public VeracodeRepository(IVeracodeWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public IEnumerable<AppType> GetAllApps()
        {
            var xml = _wrapper.GetAppList();

            if (string.IsNullOrWhiteSpace(xml))
                return new AppType[0];

            var list = XmlParseHelper.Parse<applist>(xml);
            return list.app;
        }

        public IEnumerable<BuildType> GetAllBuildsForApp(string appId)
        {
            var builds = new List<BuildType>();

            var xml = _wrapper.GetBuildList(appId);

            if (!string.IsNullOrWhiteSpace(xml))
                if(XmlParseHelper.Parse<buildlist>(xml).build != null)
                    builds.AddRange(XmlParseHelper.Parse<buildlist>(xml).build);

            var sandboxXml = _wrapper.GetSandboxes(appId);
            if (!string.IsNullOrWhiteSpace(sandboxXml))
            {
                var sandboxes = XmlParseHelper.Parse<sandboxlist>(sandboxXml);
                if(sandboxes.sandbox != null)
                {
                    foreach (var sandbox in sandboxes.sandbox)
                    {
                        var sandboxBuildXml = _wrapper.GetBuildListForSandbox(appId, $"{sandbox.sandbox_id}");
                        if (!string.IsNullOrWhiteSpace(sandboxBuildXml))
                            builds.AddRange(XmlParseHelper.Parse<buildlist>(sandboxBuildXml).build);
                    }
                }               
            }

            return builds.GroupBy(p => p.build_id)
                .Select(g => g.First())
                .ToList();
        }
        public MitigationInfoIssueType[] GetAllMitigationsForBuild(string buildId)
        {
            var mitgations = new List<MitigationInfoIssueType>();
            var flawIds = GetFlaws(buildId).Select(x => x.issueid).ToArray();

            for (var i = 0; i < flawIds.Length; i += FLAW_BATCH_LIMIT)
            {
                var batch = flawIds.Skip(i).Take(FLAW_BATCH_LIMIT);
                var flaw_string = string.Join(",", batch);
                var xml = _wrapper.GetMitigationInfo(buildId, flaw_string);

                if (string.IsNullOrWhiteSpace(xml))
                    continue;

                var issueType = XmlParseHelper.Parse<mitigationinfo>(xml);
                var issuesWithActions = issueType.issue.Where(x => x.mitigation_action != null && !x.mitigation_action.Any());
                mitgations.AddRange(issuesWithActions);
            }

            return mitgations.ToArray();
        }

        public detailedreport GetDetailedReport(string buildId)
        {
            var xml = _wrapper.GetDetailedResults(buildId);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<detailedreport>(xml);
        }

        public FlawType[] GetFlaws(string buildId)
        {
            var xml = _wrapper.GetDetailedResults(buildId);

            if (string.IsNullOrWhiteSpace(xml))
                return new FlawType[0];

            var report = XmlParseHelper.Parse<detailedreport>(xml);

            if (report.severity == null || !report.severity.Any())
                return new FlawType[0];

            return report.severity.Where(x => x.category != null && x.category.Any())
                .SelectMany(sev => sev.category.Where(x => x.cwe != null && x.cwe.Any())
                .SelectMany(cat => cat.cwe.Where(x => x.staticflaws != null && x.staticflaws.Any())
                .SelectMany(cwe => cwe.staticflaws)))
                .ToArray();
        }

        public DetailedReportModuleType[] GetEntryPoints(string buildId)
        {
            var xml = _wrapper.GetDetailedResults(buildId);

            if (string.IsNullOrWhiteSpace(xml))
                return new DetailedReportModuleType[0];

            var report = XmlParseHelper.Parse<detailedreport>(xml);

            if (report.staticanalysis == null || report.staticanalysis.modules == null
                || !report.staticanalysis.modules.Any())
                return new DetailedReportModuleType[0];

            return report.staticanalysis.modules.ToArray();
        }

        public SeverityType[] GetSeverity(string buildId)
        {
            var xml = _wrapper.GetDetailedResults(buildId);

            if (string.IsNullOrWhiteSpace(xml))
                return new SeverityType[0];

            var report = XmlParseHelper.Parse<detailedreport>(xml);
            return report.severity.ToArray();
        }

        public IEnumerable<SandboxType> GetSandboxesForApp(string appId)
        {
            var xml = _wrapper.GetSandboxes(appId);

            if (string.IsNullOrWhiteSpace(xml))
                return new SandboxType[0];

            return XmlParseHelper.Parse<sandboxlist>(xml).sandbox ?? new SandboxType[0];
        }

        public IEnumerable<FileListFileType> GetFilesForBuild(string appId, string buildId)
        {
            var xml = _wrapper.GetFiles(appId, buildId);

            if (string.IsNullOrWhiteSpace(xml))
                return new FileListFileType[0];

            return XmlParseHelper.Parse<filelist>(xml).file ?? new FileListFileType[0]; ;
        }

        public IEnumerable<ModuleType> GetModules(string appId, string buildId)
        {
            var xml = _wrapper.GetPreScanResults(appId, buildId);

            if (string.IsNullOrWhiteSpace(xml))
                return new ModuleType[0];

            return XmlParseHelper.Parse<prescanresults>(xml).module;
        }

        public appinfo GetAppDetail(string appId)
        {
            var xml = _wrapper.GetAppInfo(appId);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<appinfo>(xml);
        }

        public buildinfo GetBuildDetail(string appId, string buildId)
        {
            var xml = _wrapper.GetBuildInfo(appId, buildId);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<buildinfo>(xml);
        }

        public Callstacks GetCallStacks(string buildId, string flawId)
        {
            var xml = _wrapper.GetCallStack(buildId, flawId);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<Callstacks>(xml);
        }

        public ApplicationType CreateApp(ApplicationType newApp)
        {
            var xml = _wrapper.NewApp(
                newApp.app_name, 
                newApp.business_criticality,
                newApp.policy,
                newApp.business_owner,
                newApp.business_owner_email);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            var appinfo = XmlParseHelper.Parse<appinfo>(xml);
            if (appinfo.application.Count() == 0)
                return null;

            return appinfo.application[0];
        }

        public ApplicationType UpdateApp(ApplicationType app)
        {
            var xml = _wrapper.UpdateApp(
                app.app_id,
                app.app_name,
                app.business_criticality,
                app.policy,
                app.business_owner,
                app.business_owner_email);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            var appinfo = XmlParseHelper.Parse<appinfo>(xml);
            if (appinfo.application.Count() == 0)
                return null;

            return XmlParseHelper.Parse<appinfo>(xml).application[0];
        }

        public deleteapp DeleteApp(ApplicationType app)
        {
            var xml = _wrapper.DeleteApp(app.app_id);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<deleteapp>(xml);
        }

        public BuildInfoBuildType CreateBuild(string app_id, BuildInfoBuildType build)
        {
            var xml = _wrapper.NewBuild(app_id, build.version);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            var appinfo = XmlParseHelper.Parse<buildinfo>(xml);
            if (appinfo.build == null)
                return null;

            return XmlParseHelper.Parse<buildinfo>(xml).build;
        }

        public BuildInfoBuildType UpdateBuild(string app_id, BuildInfoBuildType build)
        {
            var xml = _wrapper.UpdateBuild(
                app_id,
                build.build_id,
                build.version);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            var appinfo = XmlParseHelper.Parse<buildinfo>(xml);
            if (appinfo.build == null)
                return null;

            return XmlParseHelper.Parse<buildinfo>(xml).build;
        }

        public deletebuildresult DeleteBuild(string app_id, string sandbox_id)
        {
            var xml = _wrapper.DeleteBuild(app_id, sandbox_id);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<deletebuildresult>(xml);
        }

        public string[] GetUsers()
        {
            var xml = _wrapper.GetUserList();

            if (string.IsNullOrWhiteSpace(xml))
                return new string[0];

            var userlist = XmlParseHelper.Parse<userlist>(xml);
            if (userlist.users == null || string.IsNullOrWhiteSpace(userlist.users.usernames))
                return new string[0];

            return userlist.users.usernames.Split(",");
        }

        public teamlistTeam[] GetTeams()
        {
            var xml = _wrapper.GetTeamList();

            if (string.IsNullOrWhiteSpace(xml))
                return new teamlistTeam[0];

            return XmlParseHelper.Parse<teamlist>(xml).team;
        }

        public teaminfo GetTeamInfo(string team_id, bool include_users = false, bool include_applications = false)
        {
            var xml = _wrapper.GetTeamInfo(team_id, include_users, include_applications);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<teaminfo>(xml);
        }

        public deleteuserresult DeleteUser(string username)
        {
            var xml = _wrapper.DeleteUser(username);
            return XmlParseHelper.Parse<deleteuserresult>(xml);
        }

        public deleteteamresult DeleteTeam(string team_id)
        {
            var xml = _wrapper.DeleteTeam(team_id);
            return XmlParseHelper.Parse<deleteteamresult>(xml);
        }

        public LoginAccount CreateUser(LoginAccount user, Roles[] roles)
        {
            var roles_parsed = string.Join(",", roles.Select(VeracodeEnumConverter.Convert).ToArray());
            var xml = _wrapper.CreateUser(user.first_name, user.last_name, 
                user.email_address, roles_parsed, user.teams);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<userinfo>(xml).login_account;
        }

        public teaminfo CreateTeam(teaminfo team)
        {
            var xml = _wrapper.CreateTeam(team.team_name);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<teaminfo>(xml);
        }

        public LoginAccount UpdateUser(LoginAccount user, Roles[] roles)
        {
            var roles_parsed = string.Join(",", roles.Select(VeracodeEnumConverter.Convert).ToArray());
            var xml = _wrapper.UpdateUser(user.username, user.first_name,
                user.last_name, user.email_address, roles_parsed, user.teams);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<userinfo>(xml).login_account;
        }

        public teaminfo UpdateTeam(teaminfo team)
        {
            var xml = _wrapper.UpdateTeam(team.team_id, team.team_name);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<teaminfo>(xml);
        }

        public LoginAccount GetUser(string username)
        {
            var xml = _wrapper.GetUserDetail(username);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<userinfo>(xml).login_account;
        }

        public FileListFileType[] UploadFileForPrescan(string app_id, string filepath)
        {
            var fileInfo = new FileInfo(filepath);            
            var xml = _wrapper.UploadFileForPrescan(app_id, filepath, fileInfo.Name);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<filelist>(xml).file;
        }

        public buildinfo StartPrescan(string app_id)
        {
            var xml = _wrapper.StartPrescan(app_id);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<buildinfo>(xml);
        }

        public buildinfo StartScan(string app_id, string modules)
        {
            var xml = _wrapper.StartScan(app_id, modules);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<buildinfo>(xml);
        }

        public PolicyVersion[] GetPolicies()
        {
            return _policyClient.Get(null, null)
                .Result._embedded.policy_versions
                .ToArray();
        }

        public PolicyVersion[] DeletePolicy(string policyGuid)
        {
            var result = _policyClient.Delete(policyGuid, null, null).Result;
            if (result)
                return GetPolicies();
            else
                return null;
        }

        public PolicyVersion UpdatePolicy(PolicyVersion policy, string policyGuid)
        {
            return _policyClient.Update(policy, policyGuid).Result;
        }

        public PolicyVersion CreatePolicy(PolicyVersion policy)
        {
            var sendPolicy = new SendPolicyVersion
            {
                name = policy.name,
                description = policy.description,
                sca_blacklist_grace_period = policy.sca_blacklist_grace_period,
                score_grace_period = policy.score_grace_period,
                sev0_grace_period = policy.sev0_grace_period,
                sev1_grace_period = policy.sev1_grace_period,
                sev2_grace_period = policy.sev2_grace_period,
                sev3_grace_period = policy.sev3_grace_period,
                sev4_grace_period = policy.sev4_grace_period,
                sev5_grace_period = policy.sev5_grace_period,
                type = policy.type,
                vendor_policy = policy.vendor_policy,
                scan_frequency_rules = policy.scan_frequency_rules,
                finding_rules = policy.finding_rules
            };
            return _policyClient.Create(sendPolicy).Result;
        }

        public buildinfo GetLatestcan(string appId)
        {
            var xml = _wrapper.GetBuildInfo(appId, null);

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            return XmlParseHelper.Parse<buildinfo>(xml);
        }
    }
}

