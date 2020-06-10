using System;
using System.Collections.Generic;
using System.Text;

namespace VeracodeService.Security
{
    public class HmacRequest
    {
        public string ApiId { get; set; }
        public string ApiKey { get; set; }
        public string HostName { get; set; }
        public string UriString { get; set; }
        public string UrlQueryParams { get; set; }
        public string HttpMethod { get; set; }
    }
}
