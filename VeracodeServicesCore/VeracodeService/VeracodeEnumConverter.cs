using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Enums;
using VeracodeService.Models;

namespace VeracodeService
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
                    parsed_status = "NotSubmittedtoEngine";
                    break;
                case BuildStatusType.SubmittedtoEngine:
                    parsed_status = "SubmittedtoEngine";
                    break;
                case BuildStatusType.ScanErrors:
                    parsed_status = "ScanErrors";
                    break;
                case BuildStatusType.ScanInProcess:
                    parsed_status = "ScanInProcess";
                    break;
                case BuildStatusType.ScanCancelled:
                    parsed_status = "ScanCancelled";
                    break;
                case BuildStatusType.ScanInternalError:
                    parsed_status = "ScanInternalError";
                    break;
                case BuildStatusType.PendingInternalReview:
                    parsed_status = "PendingInternalReview";
                    break;
                case BuildStatusType.ResultsReady:
                    parsed_status = "ResultsReady";
                    break;
                case BuildStatusType.PreScanSubmitted:
                    parsed_status = "PreScanSubmitted";
                    break;
                case BuildStatusType.PreScanFailed:
                    parsed_status = "PreScanFailed";
                    break;
                case BuildStatusType.PreScanSuccess:
                    parsed_status = "PreScanSuccess";
                    break;
                case BuildStatusType.NoModulesDefined:
                    parsed_status = "NoModulesDefined";
                    break;
                case BuildStatusType.PendingVendorConfirmation:
                    parsed_status = "PendingVendorConfirmation";
                    break;
                case BuildStatusType.VendorReviewing:
                    parsed_status = "VendorReviewing";
                    break;
                case BuildStatusType.PreScanCancelled:
                    parsed_status = "PreScanCancelled";
                    break;
                case BuildStatusType.ScanOnHold:
                    parsed_status = "ScanOnHold";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Build status can only be between 0 - 16");
            }
            return parsed_status;
        }
    }
}
