using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using Veracode.OSS.Wrapper.Http;

namespace Veracode.OSS.Wrapper.Rest
{
    public interface IPolicyClient
    {
        System.Threading.Tasks.Task<bool> Delete(string policyGuid, bool? replace_with_default_policy, string replacement_GUID);
        System.Threading.Tasks.Task<PolicyVersion> Update(PolicyVersion policy, string policyGuid);
        System.Threading.Tasks.Task<PagedResourceOfPolicies> Get(string name, bool? name_exact);
        System.Threading.Tasks.Task<PolicyVersion> GetById(string policyGuid);
        System.Threading.Tasks.Task<PolicyVersion> Create(SendPolicyVersion policy);
    }

    public partial class PolicyClient : IPolicyClient
    {
        private Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;
        private readonly IHttpService _httpService;

        public PolicyClient(IHttpService httpService)
        {
            _settings = new Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
            _httpService = httpService;
        }

        private Newtonsoft.Json.JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }
        protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }
        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);

        public async System.Threading.Tasks.Task<PagedResourceOfPolicies> Get(string name, bool? name_exact)
        {
            var nameValueCollection = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(name))
                nameValueCollection.Add(new NameValueCollection { { nameof(name), $"{name}" } });

            if (name_exact != null)
                nameValueCollection.Add(new NameValueCollection { { nameof(name_exact), $"{name_exact}" } });

            try
            {
                var response_ = await _httpService.RestGet("/appsec/v1/policies", nameValueCollection).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<PagedResourceOfPolicies>(response_, headers_).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        }
                        return objectResponse_.Object;
                    }
                    else
                    {
                        DealWithErrors(status_, response_, headers_);
                        return null;
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }
            finally
            {
            }
        }

        public async System.Threading.Tasks.Task<PolicyVersion> Create(SendPolicyVersion policy)
        {
            if (policy == null)
                throw new ArgumentNullException("policy");

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(policy);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response_ = await _httpService.RestPost("/appsec/v1/policies", new NameValueCollection(), content).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<PolicyVersion>(response_, headers_).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        }
                        return objectResponse_.Object;
                    }
                    else
                    {
                        DealWithErrors(status_, response_, headers_);
                        return null;
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }
            finally
            {
            }
        }

        public async System.Threading.Tasks.Task<PolicyVersion> GetById(string policyGuid)
        {
            if (policyGuid == null)
                throw new ArgumentNullException("policyGuid");

            try
            {
                var response_ = await _httpService.RestGet($"/appsec/v1/policies/{policyGuid}", new NameValueCollection()).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<PolicyVersion>(response_, headers_).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        }
                        return objectResponse_.Object;
                    }
                    else
                    {
                        DealWithErrors(status_, response_, headers_);
                        return null;
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }
            finally
            {
            }
        }

        public async System.Threading.Tasks.Task<PolicyVersion> Update(PolicyVersion policy, string policyGuid)
        {
            if (policyGuid == null)
                throw new ArgumentNullException("policyGuid");

            if (policy == null)
                throw new ArgumentNullException("policy");

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(policy);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response_ = await _httpService.RestPut($"/appsec/v1/policies/{policyGuid}", new NameValueCollection(), content).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<PolicyVersion>(response_, headers_).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        }
                        return objectResponse_.Object;
                    }
                    else
                    {
                        DealWithErrors(status_, response_, headers_);
                        return null;
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }
            finally
            {
            }
        }

        public async System.Threading.Tasks.Task<bool> Delete(string policyGuid, bool? replace_with_default_policy, string replacement_GUID)
        {
            var nameValueCollection = new NameValueCollection();

            if (replace_with_default_policy != null)
                nameValueCollection.Add(new NameValueCollection { { nameof(replace_with_default_policy), $"{replace_with_default_policy}" } });

            if (replacement_GUID != null)
                nameValueCollection.Add(new NameValueCollection { { nameof(replacement_GUID), $"{replacement_GUID}" } });

            try
            {
                var response_ = await _httpService.RestDelete($"/appsec/v1/policies/{policyGuid}", nameValueCollection).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        return true;
                    }
                    else
                    {
                        DealWithErrors(status_, response_, headers_);
                        return false;
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }
            finally
            {
            }
        }

        private async void DealWithErrors(int status_, HttpResponseMessage response_, Dictionary<string, IEnumerable<string>> headers_)
        {
            if (status_ == 400)
            {
                string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ApiException("Invalid request. Verify all the components of your request and try again.", status_, responseText_, headers_, null);
            }
            else
if (status_ == 401)
            {
                string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ApiException("You are not authorized to perform this action.", status_, responseText_, headers_, null);
            }
            else
if (status_ == 403)
            {
                string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ApiException("Access denied. You are not authorized to make this request.", status_, responseText_, headers_, null);
            }
            else
if (status_ == 404)
            {
                string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ApiException("This page does not exist. Verify the URI and try again.", status_, responseText_, headers_, null);
            }
            else
if (status_ == 429)
            {
                string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ApiException("Request limit exceeded. You have sent too many requests in a single time period. Submit your request again later.", status_, responseText_, headers_, null);
            }
            else
if (status_ == 500)
            {
                string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ApiException("Server-side error. Please try again later.", status_, responseText_, headers_, null);
            }
            else
            {
                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
            }
        }

        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                this.Object = responseObject;
                this.Text = responseText;
            }
            public T Object { get; }
            public string Text { get; }
        }
        public bool ReadResponseAsString { get; set; }

        protected virtual async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default(T), string.Empty);
            }

            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var streamReader = new System.IO.StreamReader(responseStream))
                    using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(streamReader))
                    {
                        var serializer = Newtonsoft.Json.JsonSerializer.Create(JsonSerializerSettings);
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }
    }

    public enum Scan_type
    {
        [System.Runtime.Serialization.EnumMember(Value = @"STATIC")]
        STATIC = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"DYNAMIC")]
        DYNAMIC = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"MANUAL")]
        MANUAL = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"SCA")]
        SCA = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"MOBILE")]
        MOBILE = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"ALL")]
        ALL = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"DYNAMICMP")]
        DYNAMICMP = 6
    }
    public enum FindingRuleType
    {
        [System.Runtime.Serialization.EnumMember(Value = @"FAIL_ALL")]
        FAIL_ALL = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"CWE")]
        CWE = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"CATEGORY")]
        CATEGORY = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"MAX_SEVERITY")]
        MAX_SEVERITY = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"CVSS")]
        CVSS = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"CVE")]
        CVE = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"BLACKLIST")]
        BLACKLIST = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"MIN_SCORE")]
        MIN_SCORE = 7,

        [System.Runtime.Serialization.EnumMember(Value = @"SECURITY_STANDARD")]
        SECURITY_STANDARD = 8,

        [System.Runtime.Serialization.EnumMember(Value = @"LICENSE_RISK")]
        LICENSE_RISK = 9,
    }
    public enum PolicyVersionType
    {
        [System.Runtime.Serialization.EnumMember(Value = @"BUILTIN")]
        BUILTIN = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"VERACODELEVEL")]
        VERACODELEVEL = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"CUSTOMER")]
        CUSTOMER = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"STANDARD")]
        STANDARD = 3
    }

    public enum ScanFrequencyFrequency
    {
        [System.Runtime.Serialization.EnumMember(Value = @"NOT_REQUIRED")]
        NOT_REQUIRED = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ONCE")]
        ONCE = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"WEEKLY")]
        WEEKLY = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"MONTHLY")]
        MONTHLY = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"QUARTERLY")]
        QUARTERLY = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"SEMI_ANNUALLY")]
        SEMI_ANNUALLY = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"ANNUALLY")]
        ANNUALLY = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"EVERY_18_MONTHS")]
        EVERY_18_MONTHS = 7,

        [System.Runtime.Serialization.EnumMember(Value = @"EVERY_2_YEARS")]
        EVERY_2_YEARS = 8,

        [System.Runtime.Serialization.EnumMember(Value = @"EVERY_3_YEARS")]
        EVERY_3_YEARS = 9,

        [System.Runtime.Serialization.EnumMember(Value = @"SET_BY_VL_POLICY")]
        SET_BY_VL_POLICY = 10,

        [System.Runtime.Serialization.EnumMember(Value = @"SET_BY_POLICY_RULE")]
        SET_BY_POLICY_RULE = 11,
    }

    public enum ScanFrequencyScan_type
    {
        [System.Runtime.Serialization.EnumMember(Value = @"STATIC")]
        STATIC = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"DYNAMIC")]
        DYNAMIC = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"MANUAL")]
        MANUAL = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"SCA")]
        SCA = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"ANY")]
        ANY = 4,
    }

    public class ScanFrequencyRule
    {
        public string scan_type { get; set; }
        public string frequency { get; set; }
    }

    public class FindingRule
    {
        public string type { get; set; }
        public List<string> scan_type { get; set; }
        public string value { get; set; }
    }

    public class CustomSeverity
    {
        public int cwe { get; set; }
        public int severity { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class PolicyVersion
    {
        public int sev5_grace_period { get; set; }
        public int sev4_grace_period { get; set; }
        public int sev3_grace_period { get; set; }
        public int sev2_grace_period { get; set; }
        public int sev1_grace_period { get; set; }
        public int sev0_grace_period { get; set; }
        public int score_grace_period { get; set; }
        public int sca_blacklist_grace_period { get; set; }
        public string guid { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public int version { get; set; }
        public List<ScanFrequencyRule> scan_frequency_rules { get; set; }
        public List<FindingRule> finding_rules { get; set; }
        public bool vendor_policy { get; set; }
        public List<CustomSeverity> custom_severities { get; set; }
        public DateTime created { get; set; }
        public int? organization_id { get; set; }
        public Links _links { get; set; }
        public List<object> capabilities { get; set; }
    }

    public class SendPolicyVersion
    {
        public int sev5_grace_period { get; set; }
        public int sev4_grace_period { get; set; }
        public int sev3_grace_period { get; set; }
        public int sev2_grace_period { get; set; }
        public int sev1_grace_period { get; set; }
        public int sev0_grace_period { get; set; }
        public int score_grace_period { get; set; }
        public int sca_blacklist_grace_period { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public List<ScanFrequencyRule> scan_frequency_rules { get; set; }
        public List<FindingRule> finding_rules { get; set; }
        public bool vendor_policy { get; set; }
    }

    public class Embedded
    {
        public List<PolicyVersion> policy_versions { get; set; }
    }

    public class First
    {
        public string href { get; set; }
    }

    public class Self2
    {
        public string href { get; set; }
        public bool templated { get; set; }
    }

    public class Next
    {
        public string href { get; set; }
    }

    public class Last
    {
        public string href { get; set; }
    }

    public class Links2
    {
        public First first { get; set; }
        public Self2 self { get; set; }
        public Next next { get; set; }
        public Last last { get; set; }
    }

    public class Page
    {
        public int size { get; set; }
        public int total_elements { get; set; }
        public int total_pages { get; set; }
        public int number { get; set; }
    }

    public class PagedResourceOfPolicies
    {
        public Embedded _embedded { get; set; }
        public Links2 _links { get; set; }
        public Page page { get; set; }
        public List<string> capabilities { get; set; }
    }
    public partial class ApiException : Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }
    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }
}

