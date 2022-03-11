using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RSInvoiceLibrary.Helpers
{
    public static class HelperClass
    {
        public static object GetNewObject<T>(this T obj, string xml)
        {
            string objectName = typeof(T).Name;
            string? assemblyName = typeof(T).Assembly.GetName().Name;
            string? ObjectfullName = GetObjectFullName(objectName, ref assemblyName);

            Type? type = Type.GetType($"{ObjectfullName}, {assemblyName}");
            object? instance = Activator.CreateInstance(type);

            return instance.GetObjectFromXml(xml);
        }

        private static T GetObjectFromXml<T> (this T obj , string xml)
        {
            var tempXml = RemoveAllNamespaces(xml);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(tempXml); 

            foreach (XmlNode item in xmlDocument.FirstChild.FirstChild.ChildNodes)
            {
                foreach (XmlNode childNode in item.ChildNodes)
                {
                    childNode.Attributes?.RemoveAll();
                    childNode.InnerText = childNode.InnerText.Trim();
                }
            }
            string json = JsonConvert.SerializeXmlNode(xmlDocument.FirstChild.FirstChild);

            string? rame = xmlDocument.FirstChild.FirstChild.ToString();
            T newObj = rame.DeserializeFromXml();

           // T newObj = (T)JsonConvert.DeserializeObject<T>(json);


            //JObject jObj = newObj as JObject;
            //T newObj2 = jObj.ToObject<T>();
            //var o = JObject.Parse(json);
            //var jsonType = (String)o["_type"];
            //var rame =   o.ToObject<Type>();
            

            var name = newObj.GetType().Name;

            return newObj;
        }
        public static T DeserializeFromXml<T>(this string xml)
        {
            XmlSerializer serializer = new(typeof(T));

            using StringReader reader = new(xml);
            T result = (T)serializer.Deserialize(reader);

            return result;
        }

        public static string GetObjectFullName(string objectname, ref string assemblyName)
        {
            assemblyName +=  ".Models";
            string strPart1 = assemblyName;
            string strPart2 = string.Join("", objectname.Split('_').ToList()
            .ConvertAll(word =>
                    word.Substring(0, 1).ToUpper() + word.Substring(1)
            ));
              return strPart1 + "." + strPart2;
        }
        #region Helper Methods
        //Implemented based on interface, not part of algorithm
        private static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));
            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        #endregion

    }
}
