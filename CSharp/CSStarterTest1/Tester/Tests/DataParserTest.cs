using System;
using System.IO;

using CSStarterTest1.DataOps;

namespace CSStarterTest1.Tester.Tests
{
    internal sealed class DataParserTest : Test
    {
        public DataParserTest(TextWriter logWriter) : base(logWriter) { }

        public override TestResult Perform()
        {
            Data reference = new(
                new DateTime(2021, 9, 16),
                "Name",
                "Surname",
                "Fathername",
                "SomeCity",
                "SomeCountry"
            );
            string text = "16.09.2021;Name;Surname;Fathername;SomeCity;SomeCountry";

            DataParser parser = new();
            Data[] datas;
            try
            {
                datas = parser.Parse(text);
            }
            catch (FormatException e)
            {
                Logger.WriteLine($"Parse failure: \"{e.Message}\"");
                return TestResult.Failure;
            }
            if (datas.Length != 1)
            {
                Logger.WriteLine($"Extracted {datas.Length} data objects instead of 1");
                return TestResult.Failure;
            }

            Data data = datas[0];
            if (data == reference)
            {
                return TestResult.Success;
            }
            else
            {
                Logger.WriteLine("Resulting data isn't equal to reference");
                return TestResult.Failure;
            }
        }
    }
}
