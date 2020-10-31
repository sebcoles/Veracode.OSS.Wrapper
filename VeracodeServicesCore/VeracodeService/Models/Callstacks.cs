/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Veracode.OSS.Wrapper.Models
{
	[XmlRoot(ElementName = "call", Namespace = "https://analysiscenter.veracode.com/schema/2.0/callstacks")]
	public class Call
	{
		[XmlAttribute(AttributeName = "data_path")]
		public string Data_path { get; set; }
		[XmlAttribute(AttributeName = "file_name")]
		public string File_name { get; set; }
		[XmlAttribute(AttributeName = "file_path")]
		public string File_path { get; set; }
		[XmlAttribute(AttributeName = "function_name")]
		public string Function_name { get; set; }
		[XmlAttribute(AttributeName = "line_number")]
		public string Line_number { get; set; }
	}

	[XmlRoot(ElementName = "callstack", Namespace = "https://analysiscenter.veracode.com/schema/2.0/callstacks")]
	public class Callstack
	{
		[XmlElement(ElementName = "call", Namespace = "https://analysiscenter.veracode.com/schema/2.0/callstacks")]
		public List<Call> Call { get; set; }
		[XmlAttribute(AttributeName = "module_name")]
		public string Module_name { get; set; }
		[XmlAttribute(AttributeName = "steps")]
		public string Steps { get; set; }
		[XmlAttribute(AttributeName = "local_path")]
		public string Local_path { get; set; }
		[XmlAttribute(AttributeName = "function_name")]
		public string Function_name { get; set; }
		[XmlAttribute(AttributeName = "line_number")]
		public string Line_number { get; set; }
	}

	[XmlRoot(ElementName = "callstacks", Namespace = "https://analysiscenter.veracode.com/schema/2.0/callstacks")]
	public class Callstacks
	{
		[XmlElement(ElementName = "callstack", Namespace = "https://analysiscenter.veracode.com/schema/2.0/callstacks")]
		public Callstack[] Callstack { get; set; }
		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string SchemaLocation { get; set; }
		[XmlAttribute(AttributeName = "callstacks_version")]
		public string Callstacks_version { get; set; }
		[XmlAttribute(AttributeName = "build_id")]
		public string Build_id { get; set; }
		[XmlAttribute(AttributeName = "flaw_id")]
		public string Flaw_id { get; set; }
	}

}
