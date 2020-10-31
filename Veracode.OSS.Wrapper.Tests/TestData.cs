using Veracode.OSS.Wrapper.Models;

namespace Veracode.OSS.WrappersCoreTests
{
    public class TestData
    {
        public string AppId { get; set; }
        public string BuildId { get; set; }
        public string FlawId { get; set; }
        public string NewAppName { get; set; }
        public string UpdatedAppName { get; set; }
        public BusinessCriticalityType NewAppCriticality { get; set; }
        public BusinessCriticalityType UpdatedAppCriticality { get; set; }
        public string NewBuildVersion { get; set; }
        public string UpdatedBuildVersion { get; set; }
        public string NewUserName { get; set; }
        public string NewFirstName { get; set; }
        public string NewLastName { get; set; }
        public string NewUserEmail { get; set; }
        public string NewUserTeam { get; set; }
        public int[] NewUserRoles { get; set; }
        public int[] UpdatedUserRoles { get; set; }
        public string UpdatedFirstName { get; set; }
        public string UpdatedLastName { get; set; }
        public string NewTeamName { get; set; }
        public string UpdatedTeamName { get; set; }
        public string Policyname { get; set; }
        public string Policydescription { get; set; }
        public int Policysca_blacklist_grace_period { get; set; }
        public int Policyscore_grace_period { get; set; }
        public int Policysev0_grace_period { get; set; }
        public int Policysev1_grace_period { get; set; }
        public int Policysev2_grace_period { get; set; }
        public int Policysev3_grace_period { get; set; }
        public int Policysev4_grace_period { get; set; }
        public int Policysev5_grace_period { get; set; }
        public string Policytype { get; set; }
        public bool Policyvendor_policy { get; set; }
        public string Policynameupdated { get; set; }
        public string MitigationFlawId { get; set; }
        public string MitigationAction { get; set; }
        public string MitigationActionApproved { get; set; }
        public string MitigationComment1 { get; set; }
        public string MitigationComment2 { get; set; }
        public string SandboxName { get; set; }
    }
}
