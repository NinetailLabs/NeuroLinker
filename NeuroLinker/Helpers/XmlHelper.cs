using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NeuroLinker.Interfaces;
using VaraniumSharp.Attributes;

namespace NeuroLinker.Helpers
{
    /// <summary>
    /// Assist in manipulating XML
    /// </summary>
    [AutomaticContainerRegistration(typeof(IXmlHelper))]
    public class XmlHelper : IXmlHelper
    {
        /// <summary>
        /// Serialize an object to XML
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="data">Data that should be serialized</param>
        /// <returns>Serialized object</returns>
        public string SerializeData<T>(T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var mStream = new Utf8StringWriter())
            using (var streamWriter = XmlWriter.Create(mStream, new XmlWriterSettings { Encoding = Encoding.UTF8 }))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(streamWriter, data, ns);
                return mStream.ToString();
            }
        }
    }
}