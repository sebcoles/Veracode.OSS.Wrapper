using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Enums;
using VeracodeService.Models;

namespace VeracodeService
{
    public static class EnumToStringConverter
    {
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
    }
}
