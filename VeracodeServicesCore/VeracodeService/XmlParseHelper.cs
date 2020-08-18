using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace VeracodeService
{
    public static class XmlParseHelper
    {
        public static T Parse<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);
            T item;
            try
            {
                item = (T)serializer.Deserialize(reader);
            } catch(Exception e)
            {
                throw new Exception($"{e.Message} using xml payload {xml}");
            }
            return item;
        }

        public static string GetDecodedXmlResponse(string xmlString, bool indentXml)
        {
            string response = string.Empty;

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = indentXml,
                Encoding = new UTF8Encoding(false),
                ConformanceLevel = ConformanceLevel.Document
            };

            using (var writer = new StringWriterWithEncoding(Encoding.UTF8))
            {
                using var w = XmlWriter.Create(writer, settings);
                xmlDocument.Save(w);
                response = writer.ToString();
            }

            return response;
        }

        public class StringWriterWithEncoding : StringWriter
        {
            private Encoding encoding;

            public override Encoding Encoding
            {
                get
                {
                    return this.encoding;
                }
            }

            public StringWriterWithEncoding(Encoding encoding)
            {
                this.encoding = encoding;
            }
        }
    }
}
