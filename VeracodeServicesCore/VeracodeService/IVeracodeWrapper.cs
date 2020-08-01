using System;
using System.Collections.Specialized;
using VeracodeService.Http;
using VeracodeService.Models;

namespace VeracodeService
{
    public interface IVeracodeWrapper
    {
        string GetAppList();
        string GetAppInfo(string app_id);
        string NewApp(string app_name, BusinessCriticalityType business_criticality);
        string GetBuildInfo(string app_id, string build_Id);
        string GetBuildList(string app_id);
        string GetBuildListForSandbox(string app_id, string sandboxId);
        string GetFiles(string app_id, string buildId);
        string GetPreScanResults(string app_id, string buildid);
        string GetSandboxes(string app_id);
        string GetDetailedResults(string buildId);
        string GetMitigationInfo(string build_id, string flaw_id_list);
        string GetCallStack(string build_id, string flaw_id);
        string UpdateApp(long app_id, string app_name, BusinessCriticalityType business_criticality);
        string DeleteApp(long app_id);
    }

    public class VeracodeWrapper : IVeracodeWrapper
    {
        public const string DETAILED_REPORT_URI = "/api/5.0/detailedreport.do";
        public const string GET_CALL_STACKS_URI = "/api/5.0/getcallstacks.do";
        public const string GET_SANDBOX_LIST_URI = "/api/5.0/getsandboxlist.do";
        public const string GET_APP_INFO_URI = "/api/5.0/getappinfo.do";
        public const string CREATE_APP_URI = "/api/5.0/createapp.do";
        public const string DELETE_APP_URI = "/api/5.0/deleteapp.do";
        public const string UPDATE_APP_URI = "/api/5.0/updateapp.do";
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

        public string GetBuildListForSandbox(string app_id, string sandbox_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            if (sandbox_id == null)
                throw new ArgumentException(sandbox_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(sandbox_id), sandbox_id }
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

            return _httpService.Get(GET_APP_INFO_URI, nameValueCollection);
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

            return _httpService.Get(GET_FILE_LIST_URI, nameValueCollection);
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

        public string GetBuildInfo(string app_id, string build_Id)
        {
            if (build_Id == null)
                throw new ArgumentException(build_Id);

            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(build_Id), build_Id },
            };

            return _httpService.Get(GET_BUILD_INFO_URI, nameValueCollection);
        }

        public string GetCallStack(string build_id, string flaw_id)
        {
            if (build_id == null)
                throw new ArgumentException(build_id);

            if (flaw_id == null)
                throw new ArgumentException(flaw_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(build_id), build_id },
                { nameof(flaw_id), flaw_id }
            };

            return _httpService.Get(GET_CALL_STACKS_URI, nameValueCollection);
        }

        public string NewApp(string app_name, BusinessCriticalityType business_criticality)
        {
            if (app_name == null)
                throw new ArgumentException(app_name);

            string parsed_business_criticality;
            switch (business_criticality)
            {
                case BusinessCriticalityType.VeryHigh:
                    parsed_business_criticality = "Very High";
                    break;
                case BusinessCriticalityType.VeryLow:
                    parsed_business_criticality = "Very Low";
                    break;
                default:
                    parsed_business_criticality = business_criticality.ToString("g");
                    break;
            }

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_name), app_name },
                { nameof(business_criticality), parsed_business_criticality}
            };

            return _httpService.Get(CREATE_APP_URI, nameValueCollection);
        }

        public string UpdateApp(long app_id, string app_name, BusinessCriticalityType business_criticality)
        {
            if (app_name == null)
                throw new ArgumentException(app_name);

            string parsed_business_criticality;
            switch (business_criticality)
            {
                case BusinessCriticalityType.VeryHigh:
                    parsed_business_criticality = "Very High";
                    break;
                case BusinessCriticalityType.VeryLow:
                    parsed_business_criticality = "Very Low";
                    break;
                default:
                    parsed_business_criticality = business_criticality.ToString("g");
                    break;
            }

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), $"{app_id}" },
                { nameof(app_name), app_name },
                { nameof(business_criticality), parsed_business_criticality}
            };

            return _httpService.Get(UPDATE_APP_URI, nameValueCollection);
        }

        public string DeleteApp(long app_id)
        {            
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), $"{app_id}" },
            };

            return _httpService.Get(DELETE_APP_URI, nameValueCollection);
        }
    }
}
