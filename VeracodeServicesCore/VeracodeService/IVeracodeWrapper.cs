using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
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
        string NewBuild(string app_id, string version);
        string UpdateBuild(string app_id, long build_id, string version);
        string DeleteBuild(string app_id, string sandbox_id);
        string UpdateTeam(string team_id, string team_name);
        string UpdateUser(string username, string first_name, string last_name, string email_address, string roles, string teams);
        string GetTeamList();
        string GetUserList();
        string CreateTeam(string team_name);
        string CreateUser(string first_name, string last_name, string email_address, string roles, string teams);
        string DeleteUser(string username);
        string DeleteTeam(string team_id);
        string GetUserDetail(string username);
        string UploadFileForPrescan(string app_id, string filepath, string filename);
        string StartPrescan(string app_id);
        string StartScan(string app_id, string modules);
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
        public const string CREATE_BUILD_URI = "/api/5.0/createbuild.do";
        public const string DELETE_BUILD_URI = "/api/5.0/deletebuild.do";
        public const string UPDATE_BUILD_URI = "/api/5.0/updatebuild.do";
        public const string GET_APP_LIST_URI = "/api/5.0/getapplist.do";
        public const string GET_BUILD_INFO_URI = "/api/5.0/getbuildinfo.do";
        public const string GET_BUILD_LIST_URI = "/api/5.0/getbuildlist.do";
        public const string GET_FILE_LIST_URI = "/api/5.0/getfilelist.do";
        public const string GET_PRE_SCAN_RESULTS_URI = "/api/5.0/getprescanresults.do";
        public const string UPLOAD_FILE_URI = "/api/5.0/uploadlargefile.do";
        public const string GET_MITIGATION_INFO_URI = "/api/getmitigationinfo.do";
        public const string UPDATE_MITIGATION_INFO_URI = "/api/updatemitigationinfo.do";
        public const string GET_TEAM_LIST_URI = "/api/3.0/getteamlist.do";
        public const string CREATE_TEAM_URI = "/api/3.0/createteam.do";
        public const string DELETE_TEAM_URI = "/api/3.0/deleteteam.do";
        public const string UPDATE_TEAM_URI = "/api/3.0/updateteam.do";
        public const string CREATE_USER_URI = "/api/3.0/createuser.do";
        public const string DELETE_USER_URI = "/api/3.0/deleteuser.do";
        public const string UPDATE_USER_URI = "/api/3.0/updateuser.do";
        public const string GET_USER_LIST_URI = "/api/3.0/getuserlist.do";
        public const string GET_USER_INFO_URI = "/api/3.0/getuserinfo.do";
        public const string START_PRESCAN_URI = "/api/3.0/beginprescan.do";
        public const string START_SCAN_URI = "/api/5.0/beginscan.do";
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

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_name), app_name },
                { nameof(business_criticality), EnumToStringConverter.Convert(business_criticality)}
            };

            return _httpService.Get(CREATE_APP_URI, nameValueCollection);
        }

        public string UpdateApp(long app_id, string app_name, BusinessCriticalityType business_criticality)
        {
            if (app_name == null)
                throw new ArgumentException(app_name);            

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), $"{app_id}" },
                { nameof(app_name), app_name },
                { nameof(business_criticality), EnumToStringConverter.Convert(business_criticality)}
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

        public string NewBuild(string app_id, string version)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(version), version}
            };

            return _httpService.Get(CREATE_BUILD_URI, nameValueCollection);
        }

        public string UpdateBuild(string app_id, long build_id, string version)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(build_id), $"{build_id}" },
                { nameof(version), version}
            };

            return _httpService.Get(UPDATE_BUILD_URI, nameValueCollection);
        }

        public string DeleteBuild(string app_id, string sandbox_id)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), $"{app_id}" },
                { nameof(sandbox_id), $"{sandbox_id}" },
            };

            return _httpService.Get(DELETE_BUILD_URI, nameValueCollection);
        }

        public string DeleteTeam(string team_id)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(team_id), $"{team_id}" }
            };

            return _httpService.Get(DELETE_TEAM_URI, nameValueCollection);
        }

        public string DeleteUser(string username)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(username), $"{username}" }
            };

            return _httpService.Get(DELETE_USER_URI, nameValueCollection);
        }

        public string CreateUser(string first_name, string last_name,
              string email_address, string roles, string teams)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(first_name), first_name},
                { nameof(last_name), last_name},
                { nameof(email_address), email_address },
                { nameof(roles), roles },
                { nameof(teams), teams }
            };

            return _httpService.Get(CREATE_USER_URI, nameValueCollection);
        }

        public string CreateTeam(string team_name)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(team_name), $"{team_name}" }
            };

            return _httpService.Get(CREATE_TEAM_URI, nameValueCollection);
        }

        public string UpdateTeam(string team_id, string team_name)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(team_id), $"{team_id}" },
                { nameof(team_name), $"{team_name}" }
            };

            return _httpService.Get(UPDATE_TEAM_URI, nameValueCollection);
        }

        public string UpdateUser(string username, string first_name, string last_name, 
            string email_address, string roles, string teams)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(username), username },
                { nameof(first_name), first_name },
                { nameof(last_name), last_name },
                { nameof(email_address), email_address },
                { nameof(roles), roles },
                { nameof(teams), teams }
            };

            return _httpService.Get(UPDATE_USER_URI, nameValueCollection);
        }

        public string GetTeamList()
        {
            var nameValueCollection = new NameValueCollection();
            return _httpService.Get(GET_TEAM_LIST_URI, nameValueCollection);
        }

        public string GetUserList()
        {
            var nameValueCollection = new NameValueCollection();
            return _httpService.Get(GET_USER_LIST_URI, nameValueCollection);
        }

        public string GetUserDetail(string username)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(username), username }
            };
            
            return _httpService.Get(GET_USER_INFO_URI, nameValueCollection);
        }

        public string UploadFileForPrescan(string app_id, string filepath, string filename)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(filename), filename }
            };

            return _httpService.PostFile(UPLOAD_FILE_URI, nameValueCollection, filepath);
        }

        public string StartPrescan(string app_id)
        {
            var autoscan = false;
            var scan_all_nonfatal_top_level_modules = false;
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(autoscan), $"{autoscan}" },
                { nameof(scan_all_nonfatal_top_level_modules), $"{scan_all_nonfatal_top_level_modules}" }
            };

            return _httpService.Get(START_PRESCAN_URI, nameValueCollection);
        }

        public string StartScan(string app_id, string modules)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(modules), $"{modules}" }
            };

            return _httpService.Get(START_SCAN_URI, nameValueCollection);
        }
    }
}
