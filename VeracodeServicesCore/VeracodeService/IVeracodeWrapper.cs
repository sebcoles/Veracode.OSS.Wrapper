using System;
using System.Collections.Specialized;
using VeracodeService.Http;

namespace VeracodeService
{
    public interface IVeracodeWrapper
    {
        string GetAppList();
        string GetAppInfo(string app_id);
        string GetBuildInfo(string buildId);
        string GetBuildList(string app_id);
        string GetBuildListForSandbox(string app_id, string sandboxId);
        string GetFiles(string app_id, string buildId);
        string GetPreScanResults(string app_id, string buildid);
        string GetSandboxes(string app_id);
        string GetDetailedResults(string buildId);
        string GetMitigationInfo(string build_id, string flaw_id_list);
    }

    public class VeracodeWrapper : IVeracodeWrapper
    {
        public const string DETAILED_REPORT_URI = "/api/5.0/detailedreport.do"; 
        public const string GET_CALL_STACKS_URI = "/api/5.0/getcallstacks.do";
        public const string GET_SANDBOX_LIST_URI = "/api/5.0/getsandboxlist.do";
        public const string GET_APP_INFO_URI = "/api/5.0/getappinfo.do";
        public const string GET_APP_LIST_URI = "/api/5.0/getapplist.do";
        public const string GET_BUILD_INFO_URI = "/api/5.0/getbuildinfo.do";
        public const string GET_BUILD_LIST_URI = "/api/5.0/getbuildlist.do";
        public const string GET_FILE_LIST_URI = "/api/5.0/getfilelist.do";
        public const string GET_PRE_SCAN_RESULTS_URI = "/api/5.0/getprescanresults.do";
        public const string UPLOAD_FILE_URI = "/api/5.0/uploadlargefile.do";
        public const string GET_MITIGATION_INFO_URI = "/api/getmitigationinfo.do";
        public const string UPDATE_MITIGATION_INFO_URI = "/api/updatemitigationinfo.do";
        private readonly IHttpService _httpService;

        public VeracodeWrapper(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public string GetAppList()
        {
            var nameValueCollection = new NameValueCollection();
            return _httpService.Get(GET_APP_LIST_URI, nameValueCollection);
        }

        public string GetBuildList(string app_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id }
            };

            return _httpService.Get(GET_BUILD_LIST_URI, nameValueCollection);
        }

        public string GetBuildListForSandbox(string app_id, string sandboxId)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            if (sandboxId == null)
                throw new ArgumentException(sandboxId);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(sandboxId), sandboxId }
            };

            return _httpService.Get(GET_BUILD_LIST_URI, nameValueCollection);
        }

        public string GetDetailedResults(string build_id)
        {
            if (build_id == null)
                throw new ArgumentException(build_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(build_id), build_id }
            };

            return _httpService.Get(DETAILED_REPORT_URI, nameValueCollection);
        }

        public string GetMitigationInfo(string build_id, string flaw_id_list)
        {
            if (build_id == null)
                throw new ArgumentException(build_id);

            if (flaw_id_list == null)
                throw new ArgumentException(flaw_id_list);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(build_id), build_id },
                { nameof(flaw_id_list), flaw_id_list }
            };

            return _httpService.Get(GET_MITIGATION_INFO_URI, nameValueCollection);
        }

        public string GetAppInfo(string app_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id }
            };

            return _httpService.Get(GET_APP_LIST_URI, nameValueCollection);
        }

        public string GetFiles(string app_id, string build_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            if (build_id == null)
                throw new ArgumentException(build_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(build_id), build_id }
            };

            return _httpService.Get(GET_BUILD_LIST_URI, nameValueCollection);
        }

        public string GetPreScanResults(string app_id, string build_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            if (build_id == null)
                throw new ArgumentException(build_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(build_id), build_id }
            };

            return _httpService.Get(GET_PRE_SCAN_RESULTS_URI, nameValueCollection);
        }

        public string GetSandboxes(string app_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id }
            };

            return _httpService.Get(GET_SANDBOX_LIST_URI, nameValueCollection);
        }

        public string GetBuildInfo(string build_Id)
        {
            if (build_Id == null)
                throw new ArgumentException(build_Id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(build_Id), build_Id }
            };

            return _httpService.Get(GET_BUILD_INFO_URI, nameValueCollection);
        }
    }

}
