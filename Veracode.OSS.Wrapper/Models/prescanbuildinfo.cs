using System.Xml.Serialization;
namespace Veracode.OSS.Wrapper.Models
{
	[XmlRoot(ElementName = "analysis_unit", Namespace = "https://analysiscenter.veracode.com/schema/2.0/buildinfo")]
	public class Analysis_unit
	{
		[XmlAttribute(AttributeName = "analysis_type")]
		public string Analysis_type { get; set; }
		[XmlAttribute(AttributeName = "status")]
		public string Status { get; set; }
	}

	[XmlRoot(ElementName = "build", Namespace = "https://analysiscenter.veracode.com/schema/2.0/buildinfo")]
	public class PrescanBuild
	{
		[XmlElement(ElementName = "analysis_unit", Namespace = "https://analysiscenter.veracode.com/schema/2.0/buildinfo")]
		public Analysis_unit Analysis_unit { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "build_id")]
		public string Build_id { get; set; }
		[XmlAttribute(AttributeName = "submitter")]
		public string Submitter { get; set; }
		[XmlAttribute(AttributeName = "platform")]
		public string Platform { get; set; }
		[XmlAttribute(AttributeName = "lifecycle_stage")]
		public string Lifecycle_stage { get; set; }
		[XmlAttribute(AttributeName = "results_ready")]
		public string Results_ready { get; set; }
		[XmlAttribute(AttributeName = "policy_name")]
		public string Policy_name { get; set; }
		[XmlAttribute(AttributeName = "policy_version")]
		public string Policy_version { get; set; }
		[XmlAttribute(AttributeName = "policy_compliance_status")]
		public string Policy_compliance_status { get; set; }
		[XmlAttribute(AttributeName = "rules_status")]
		public string Rules_status { get; set; }
		[XmlAttribute(AttributeName = "grace_period_expired")]
		public string Grace_period_expired { get; set; }
		[XmlAttribute(AttributeName = "scan_overdue")]
		public string Scan_overdue { get; set; }
	}

	[XmlRoot(ElementName = "buildinfo", Namespace = "https://analysiscenter.veracode.com/schema/2.0/buildinfo")]
	public class PrescanBuildinfo
	{
		[XmlElement(ElementName = "build", Namespace = "https://analysiscenter.veracode.com/schema/2.0/buildinfo")]
		public PrescanBuild Build { get; set; }
		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string SchemaLocation { get; set; }
		[XmlAttribute(AttributeName = "account_id")]
		public string Account_id { get; set; }
		[XmlAttribute(AttributeName = "app_id")]
		public string App_id { get; set; }
		[XmlAttribute(AttributeName = "build_id")]
		public string Build_id { get; set; }
	}

}
