using com.veracode.apiwrapper;
using Microsoft.Extensions.Options;
using VeracodeWebhooks.Configuration;

namespace VeracodeService
{
    public interface IVeracodeWrapper
    {
        string GetAppList();
        string GetAppInfo(string appId);
        string GetBuildInfo(string buildId);
        string GetBuildList(string appId);
        string GetBuildListForSandbox(string appId, string sandboxId);
        string GetFiles(string appId, string buildId);
        string GetPreScanResults(string appId, string buildid);
        string GetSandboxes(string appId);
        string GetDetailedResults(string buildId);
        string GetSummaryResults(string buildId);
        string GetMitigationInfo(string build_id, string flaw_id_list);
    }

    public class VeracodeWrapper : IVeracodeWrapper
    {
        private UploadAPIWrapper _uploadAPIWrapper;
        private MitigationAPIWrapper _mitigationAPIWrapper;
        private ResultsAPIWrapper _resultsAPIWrapper;
        private SandboxAPIWrapper _sandboxApiWrapper;

        public VeracodeWrapper(IOptions<VeracodeConfiguration> config)
        {
            _resultsAPIWrapper = CreateWrapper<ResultsAPIWrapper>(config.Value.ApiId, config.Value.ApiKey);
            _uploadAPIWrapper = CreateWrapper<UploadAPIWrapper>(config.Value.ApiId, config.Value.ApiKey);
            _mitigationAPIWrapper = CreateWrapper<MitigationAPIWrapper>(config.Value.ApiId, config.Value.ApiKey);
            _sandboxApiWrapper = CreateWrapper<SandboxAPIWrapper>(config.Value.ApiId, config.Value.ApiKey);
        }
        private T CreateWrapper<T>(string apiId, string apiKey) where T : AbstractAPIWrapper, new()
        {
            var wrapper = new T();
            wrapper.SetUpApiCredentials(apiId, apiKey);
            return wrapper;
        }

        public string GetAppList()
        {
            return _uploadAPIWrapper.GetAppList();
        }

        public string GetBuildList(string appId)
        {
            return _uploadAPIWrapper.GetBuildList(appId);
        }

        public string GetBuildListForSandbox(string appId, string sandboxId)
        {
            return _uploadAPIWrapper.GetBuildList(appId, sandboxId);
        }

        public string GetDetailedResults(string buildId)
        {
            return _resultsAPIWrapper.DetailedReport(buildId);
        }

        public string GetSummaryResults(string buildId)
        {
            return _resultsAPIWrapper.SummaryReport(buildId);
        }

        public string GetMitigationInfo(string build_id, string flaw_id_list)
        {
            return _mitigationAPIWrapper.GetMitigationInfo(build_id, flaw_id_list);
        }

        public string GetAppInfo(string appId)
        {
            return _uploadAPIWrapper.GetAppInfo(appId);
        }

        public string GetFiles(string appId, string buildId)
        {
            return _uploadAPIWrapper.GetFileList(appId, buildId);
        }

        public string GetPreScanResults(string appId, string buildId)
        {
            return _uploadAPIWrapper.GetPreScanResults(appId, buildId);
        }

        public string GetSandboxes(string appId)
        {
            return _sandboxApiWrapper.GetSandboxList(appId);
        }

        public string GetBuildInfo(string buildId)
        {
            return _uploadAPIWrapper.GetBuildInfo(buildId);
        }
    }

}
