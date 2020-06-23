using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using VeracodeService.Http;
using VeracodeService.Models;
using VeracodeService.Security;
using VeracodeWebhooks.Configuration;

namespace VeracodeService.Repositories
{
    public interface IVeracodeRepository
    {
        IEnumerable<AppType> GetAllApps();
        IEnumerable<BuildType> GetAllBuildsForApp(string appId);
        appinfo GetAppDetail(string appId);
        IEnumerable<SandboxType> GetSandboxesForApp(string appId);
        buildinfo GetBuildDetail(string appId, string buildId);
        IEnumerable<FileListFileType> GetFilesForBuild(string appId, string buildId);
        IEnumerable<ModuleType> GetModules(string appId, string buildId);
        detailedreport GetDetailedReport(string buildId);
        MitigationInfoIssueType[] GetAllMitigationsForBuild(string buildIds);
        FlawType[] GetFlaws(string buildId);
        SeverityType[] GetSeverity(string buildId);
        DetailedReportModuleType[] GetEntryPoints(string buildId);
        Callstacks GetCallStacks(string buildId, string flawId);
    }
    public class VeracodeRepository : IVeracodeRepository
    {
        private const int FLAW_BATCH_LIMIT = 500;
        private readonly IVeracodeWrapper _wrapper;
        public VeracodeRepository(IOptions<VeracodeConfiguration> config)
        {
            _wrapper = new VeracodeWrapper(new HttpService(config, new CryptoService()));
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
                builds.AddRange(XmlParseHelper.Parse<buildlist>(xml).build);

            var sandboxXml = _wrapper.GetSandboxes(appId);
            if (!string.IsNullOrWhiteSpace(sandboxXml))
            {
                var sandboxes = XmlParseHelper.Parse<sandboxlist>(sandboxXml);
                foreach (var sandbox in sandboxes.sandbox)
                {
                    var sandboxBuildXml = _wrapper.GetBuildListForSandbox(appId, $"{sandbox.sandbox_id}");
                    if (!string.IsNullOrWhiteSpace(sandboxBuildXml))
                        builds.AddRange(XmlParseHelper.Parse<buildlist>(sandboxBuildXml).build);
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

            for(var i = 0; i < flawIds.Length; i += FLAW_BATCH_LIMIT)
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

            return XmlParseHelper.Parse<sandboxlist>(xml).sandbox;
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
    }
}

