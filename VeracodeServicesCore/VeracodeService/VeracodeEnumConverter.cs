using System;
using Veracode.OSS.Wrapper.Enums;
using Veracode.OSS.Wrapper.Models;

namespace Veracode.OSS.Wrapper
{
    public static class VeracodeEnumConverter
    {
        public static string Convert(PolicyComplianceType compliance_status)
        {
            string parsed_compliance_status;
            switch (compliance_status)
            {
                case PolicyComplianceType.Calculating:
                    parsed_compliance_status = "Calculating";
                    break;
                case PolicyComplianceType.ConditionalPass:
                    parsed_compliance_status = "Conditional Pass";
                    break;
                case PolicyComplianceType.DidNotPass:
                    parsed_compliance_status = "Did Not Pass";
                    break;
                case PolicyComplianceType.NotAssessed:
                    parsed_compliance_status = "Not Assessed";
                    break;
                case PolicyComplianceType.Pass:
                    parsed_compliance_status = "Pass";
                    break;
                default:
                    parsed_compliance_status = compliance_status.ToString("g");
                    break;
            }
            return parsed_compliance_status;
        }

        public static string Convert(BusinessCriticalityType business_criticality)
        {
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
            return parsed_business_criticality;
        }

        public static BusinessCriticalityType Convert(string business_criticality)
        {
            BusinessCriticalityType parsed_business_criticality;
            switch (business_criticality)
            {
                case "Very High":
                    parsed_business_criticality = BusinessCriticalityType.VeryHigh;
                    break;
                case "Very Low":
                    parsed_business_criticality = BusinessCriticalityType.VeryLow;
                    break;
                default:
                    parsed_business_criticality = (BusinessCriticalityType)Enum.Parse(typeof(BusinessCriticalityType), business_criticality);
                    break;
            }
            return parsed_business_criticality;
        }

        public static string Convert(Roles role)
        {
            string parsed_role;
            switch (role)
            {
                case Roles.MitigationApprover:
                    parsed_role = "Mitigation Approver";
                    break;
                case Roles.PolicyAdministrator:
                    parsed_role = "Policy Administrator";
                    break;
                case Roles.SecurityLead:
                    parsed_role = "Security Lead";
                    break;
                case Roles.SecurityInsights:
                    parsed_role = "Security Insights";
                    break;
                case Roles.ManualScan:
                    parsed_role = "Manual Scan";
                    break;
                case Roles.StaticScan:
                    parsed_role = "Static Scan";
                    break;
                case Roles.DynamicScan:
                    parsed_role = "Dynamic Scan";
                    break;
                case Roles.AnyScan:
                    parsed_role = "Any Scan";
                    break;
                default:
                    parsed_role = role.ToString("g");
                    break;
            }
            return parsed_role;
        }

        public static string Convert(BuildStatusType status)
        {
            string parsed_status;
            switch (status)
            {
                case BuildStatusType.Incomplete:
                    parsed_status = "Incomplete";
                    break;
                case BuildStatusType.NotSubmittedtoEngine:
                    parsed_status = "Not Submitted to Engine";
                    break;
                case BuildStatusType.SubmittedtoEngine:
                    parsed_status = "Submitted to Engine";
                    break;
                case BuildStatusType.ScanErrors:
                    parsed_status = "Scan Errors";
                    break;
                case BuildStatusType.ScanInProcess:
                    parsed_status = "Scan In Process";
                    break;
                case BuildStatusType.ScanCancelled:
                    parsed_status = "Scan Cancelled";
                    break;
                case BuildStatusType.ScanInternalError:
                    parsed_status = "Scan Internal Error";
                    break;
                case BuildStatusType.PendingInternalReview:
                    parsed_status = "Pending Internal Review";
                    break;
                case BuildStatusType.ResultsReady:
                    parsed_status = "Results Ready";
                    break;
                case BuildStatusType.PreScanSubmitted:
                    parsed_status = "Pre Scan Submitted";
                    break;
                case BuildStatusType.PreScanFailed:
                    parsed_status = "Pre Scan Failed";
                    break;
                case BuildStatusType.PreScanSuccess:
                    parsed_status = "Pre Scan Success";
                    break;
                case BuildStatusType.NoModulesDefined:
                    parsed_status = "No Modules Defined";
                    break;
                case BuildStatusType.PendingVendorConfirmation:
                    parsed_status = "Pending Vendor Confirmation";
                    break;
                case BuildStatusType.VendorReviewing:
                    parsed_status = "Vendor Reviewing";
                    break;
                case BuildStatusType.PreScanCancelled:
                    parsed_status = "Pre Scan Cancelled";
                    break;
                case BuildStatusType.ScanOnHold:
                    parsed_status = "Scan On Hold";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Build status can only be between 0 - 16");
            }
            return parsed_status;
        }
    }
}
