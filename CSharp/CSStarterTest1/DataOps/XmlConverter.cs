using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace CSStarterTest1.DataOps
{
    /// <summary>
    /// Converts data records to XElements.
    /// </summary>
    internal class XmlConverter
    {
        public XElement? Convert(object? data)
        {
            if (data is null)
            {
                return null;
            }

            XElement xElement;

            if (data is IEnumerable elems)
            {
                xElement = new XElement("Records");
                foreach (var elem in elems)
                {
                    xElement.Add(ConvertImpl(elem));
                }
            }
            else
            {
                xElement = ConvertImpl(data);
            }

            return xElement;
        }
        private static XElement ConvertImpl(object data)
        {
            var xElement = new XElement("Record");

            var formatter = new DataFieldFormatter();
            var kvs = data
                .GetType()
                .GetProperties()
                .Select(p => (Key: XmlConvert.EncodeName(p.Name), Value: formatter.Format(p.GetValue(data)!)));

            foreach (var (key, value) in kvs)
            {
                string idKeyName = nameof(Data.Id);
                if (key == idKeyName && xElement.Attribute(idKeyName) is null)
                {
                    xElement.Add(new XAttribute(idKeyName, value));
                    continue;
                }
                xElement.Add(new XElement(key, value));
            }
            return xElement;
        }
    }
}
