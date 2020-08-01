using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;

namespace VeracodeServicesCoreTests
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
    }
}
