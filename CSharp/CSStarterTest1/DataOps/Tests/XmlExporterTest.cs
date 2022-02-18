using System;
using System.IO;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.DataOps.Tests
{
    public class XmlExporterTest : Test
    {
        public XmlExporterTest(TextWriter writer) : base(writer) { }

        protected override TestResult PerformImpl()
        {
            var data = new Data(1, DateTime.Now, "FirstName", "LastName", "SurName", "City", "Country");
            new XmlExporter().Export(data, $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}{Path.DirectorySeparatorChar}testExcelFile");

            return new TestResult(TestStatus.Success);
        }
    }
}
