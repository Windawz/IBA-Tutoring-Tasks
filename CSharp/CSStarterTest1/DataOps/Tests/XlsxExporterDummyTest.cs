using System;
using System.IO;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.DataOps.Tests
{
    public sealed class XlsxExporterDummyTest : Test
    {
        public XlsxExporterDummyTest(TextWriter writer) : base(writer) { }

        public override TestResult Perform()
        {
            Data data = new(new(2021, 03, 14), "FirstName", "SecondName", "FathersName", "CityName", "CountryName");
            string fileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}{Path.DirectorySeparatorChar}testExcelFile"; 
            
            using (XlsxExporter exporter = new(fileName))
            {
                exporter.Export(data);
            }

            

            return TestResult.Success;
        }
    }
}
