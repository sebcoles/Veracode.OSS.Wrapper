using System;
using System.Collections.Generic;
using System.Text;

namespace Veracode.OSS.Wrapper.Security
{
    public class HmacRequest
    {
        public string ApiId { get; set; }
        public string ApiKey { get; set; }
        public string HostName { get; set; }
        public string Url { get; set; }
        public string HttpMethod { get; set; }
    }
}
