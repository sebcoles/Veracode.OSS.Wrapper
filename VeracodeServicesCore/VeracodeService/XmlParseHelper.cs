using System.IO;
using System.Xml.Serialization;

namespace VeracodeService
{
    public static class XmlParseHelper
    {
        public static T Parse<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
