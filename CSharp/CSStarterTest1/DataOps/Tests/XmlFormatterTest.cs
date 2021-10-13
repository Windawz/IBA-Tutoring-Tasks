using System;
using System.IO;
using System.Text.RegularExpressions;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.DataOps.Tests
{
    public sealed class XmlFormatterTest : Test
    {
        public XmlFormatterTest(TextWriter writer) : base(writer) { }

        public override TestResult Perform()
        {
            Data testData = new(new(2020, 12, 31), "FirstName", "SecondName", "FathersName", "CityName", "CountryName");
            string expectedXml =
@$"<{XmlFormatter.DefaultElementName}>
    <{nameof(Data.Date)}>
        31.12.2020
    </{nameof(Data.Date)}>
    <{nameof(Data.FirstName)}>
        FirstName
    </{nameof(Data.FirstName)}>
    <{nameof(Data.SecondName)}>
        SecondName
    </{nameof(Data.SecondName)}>
    <{nameof(Data.FathersName)}>
        FathersName
    </{nameof(Data.FathersName)}>
    <{nameof(Data.CityName)}>
        CityName
    </{nameof(Data.CityName)}>
    <{nameof(Data.CountryName)}>
        CountryName
    </{nameof(Data.CountryName)}>
</{XmlFormatter.DefaultElementName}>";

            string actualXml = XmlFormatter.Format(testData)!.ToString();

            Regex whitespaceRemover = new(@"\s+");
            expectedXml = whitespaceRemover.Replace(expectedXml, "");
            actualXml = whitespaceRemover.Replace(actualXml, "");

            if (String.Equals(expectedXml, actualXml, StringComparison.Ordinal))
            {
                return TestResult.Success;
            }
            else
            {
                Logger.WriteLine("Expected XML:")
                    .WriteLine(expectedXml)
                    .WriteLine("Actual XML:")
                    .WriteLine(actualXml);

                return TestResult.Failure;
            }
        }
    }
}
