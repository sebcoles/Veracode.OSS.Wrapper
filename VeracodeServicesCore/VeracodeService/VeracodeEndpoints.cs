using System;
using System.Collections.Generic;
using System.Text;

namespace VeracodeService
{
    public static class VeracodeEndpoints
    {
        public static string GET_DETAILED_REPORT = "/api/5.0/detailedreport.do";
        public static string GET_CALL_STACKS = "/api/5.0/getcallstacks.do";
        public static string GET_SANDBOX_LIST = "/api/5.0/getsandboxlist.do";
        public static string GET_APP_INFO = "/api/5.0/getappinfo.do";
        public static string CREATE_APP = "/api/5.0/createapp.do";
        public static string DELETE_APP = "/api/5.0/deleteapp.do";
        public static string UPDATE_APP = "/api/5.0/updateapp.do";
        public static string CREATE_BUILD = "/api/5.0/createbuild.do";
        public static string DELETE_BUILD = "/api/5.0/deletebuild.do";
        public static string UPDATE_BUILD = "/api/5.0/updatebuild.do";
        public static string GET_APP_LIST = "/api/5.0/getapplist.do";
        public static string GET_BUILD_INFO = "/api/5.0/getbuildinfo.do";
        public static string GET_BUILD_LIST = "/api/5.0/getbuildlist.do";
        public static string GET_FILE_LIST = "/api/5.0/getfilelist.do";
        public static string GET_PRE_SCAN_RESULTS = "/api/5.0/getprescanresults.do";
        public static string UPLOAD_FILE = "/api/5.0/uploadlargefile.do";
        public static string GET_MITIGATION_INFO = "/api/getmitigationinfo.do";
        public static string UPDATE_MITIGATION_INFO = "/api/updatemitigationinfo.do";
        public static string GET_TEAM_LIST = "/api/3.0/getteamlist.do";
        public static string CREATE_TEAM = "/api/3.0/createteam.do";
        public static string DELETE_TEAM = "/api/3.0/deleteteam.do";
        public static string UPDATE_TEAM = "/api/3.0/updateteam.do";
        public static string CREATE_USER = "/api/3.0/createuser.do";
        public static string DELETE_USER = "/api/3.0/deleteuser.do";
        public static string UPDATE_USER = "/api/3.0/updateuser.do";
        public static string GET_USER_LIST = "/api/3.0/getuserlist.do";
        public static string GET_USER_INFO = "/api/3.0/getuserinfo.do";
        public static string START_PRESCAN = "/api/3.0/beginprescan.do";
        public static string START_SCAN = "/api/5.0/beginscan.do";
    }
}
