using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace CSStarterTest1.DataOps.New
{
    public class XmlExporter : Exporter
    {
        protected override string Extension => ".xml";

        protected override void ExportImpl(object? data, string path)
        {
            var xDocument = new XDocument(new XElement("TestProgram", new XmlConverter().Convert(data)));
            xDocument.Save(path);
        }
    }
}
