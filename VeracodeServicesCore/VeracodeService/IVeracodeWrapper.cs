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
        private readonly IHttpService _httpService;

        public VeracodeWrapper(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public string GetAppList()
        {
            var nameValueCollection = new NameValueCollection();
            return _httpService.Get(VeracodeEndpoints.GET_APP_LIST, nameValueCollection);
        }

        public string GetBuildList(string app_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id }
            };

            return _httpService.Get(VeracodeEndpoints.GET_BUILD_LIST, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.GET_BUILD_LIST, nameValueCollection);
        }

        public string GetDetailedResults(string build_id)
        {
            if (build_id == null)
                throw new ArgumentException(build_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(build_id), build_id }
            };

            return _httpService.Get(VeracodeEndpoints.GET_DETAILED_REPORT, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.GET_MITIGATION_INFO, nameValueCollection);
        }

        public string GetAppInfo(string app_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id }
            };

            return _httpService.Get(VeracodeEndpoints.GET_APP_INFO, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.GET_FILE_LIST, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.GET_PRE_SCAN_RESULTS, nameValueCollection);
        }

        public string GetSandboxes(string app_id)
        {
            if (app_id == null)
                throw new ArgumentException(app_id);

            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id }
            };

            return _httpService.Get(VeracodeEndpoints.GET_SANDBOX_LIST, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.GET_BUILD_INFO, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.GET_CALL_STACKS, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.CREATE_APP, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.UPDATE_APP, nameValueCollection);
        }

        public string DeleteApp(long app_id)
        {            
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), $"{app_id}" },
            };

            return _httpService.Get(VeracodeEndpoints.DELETE_APP, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.CREATE_BUILD, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.UPDATE_BUILD, nameValueCollection);
        }

        public string DeleteBuild(string app_id, string sandbox_id)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), $"{app_id}" },
                { nameof(sandbox_id), $"{sandbox_id}" },
            };

            return _httpService.Get(VeracodeEndpoints.DELETE_BUILD, nameValueCollection);
        }

        public string DeleteTeam(string team_id)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(team_id), $"{team_id}" }
            };

            return _httpService.Get(VeracodeEndpoints.DELETE_TEAM, nameValueCollection);
        }

        public string DeleteUser(string username)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(username), $"{username}" }
            };

            return _httpService.Get(VeracodeEndpoints.DELETE_USER, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.CREATE_USER, nameValueCollection);
        }

        public string CreateTeam(string team_name)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(team_name), $"{team_name}" }
            };

            return _httpService.Get(VeracodeEndpoints.CREATE_TEAM, nameValueCollection);
        }

        public string UpdateTeam(string team_id, string team_name)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(team_id), $"{team_id}" },
                { nameof(team_name), $"{team_name}" }
            };

            return _httpService.Get(VeracodeEndpoints.UPDATE_TEAM, nameValueCollection);
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

            return _httpService.Get(VeracodeEndpoints.UPDATE_USER, nameValueCollection);
        }

        public string GetTeamList()
        {
            var nameValueCollection = new NameValueCollection();
            return _httpService.Get(VeracodeEndpoints.GET_TEAM_LIST, nameValueCollection);
        }

        public string GetUserList()
        {
            var nameValueCollection = new NameValueCollection();
            return _httpService.Get(VeracodeEndpoints.GET_USER_LIST, nameValueCollection);
        }

        public string GetUserDetail(string username)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(username), username }
            };
            
            return _httpService.Get(VeracodeEndpoints.GET_USER_INFO, nameValueCollection);
        }

        public string UploadFileForPrescan(string app_id, string filepath, string filename)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(filename), filename }
            };

            return _httpService.PostFile(VeracodeEndpoints.UPLOAD_FILE, nameValueCollection, filepath);
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

            return _httpService.Get(VeracodeEndpoints.START_PRESCAN, nameValueCollection);
        }

        public string StartScan(string app_id, string modules)
        {
            var nameValueCollection = new NameValueCollection
            {
                { nameof(app_id), app_id },
                { nameof(modules), $"{modules}" }
            };

            return _httpService.Get(VeracodeEndpoints.START_SCAN, nameValueCollection);
        }
    }
}
