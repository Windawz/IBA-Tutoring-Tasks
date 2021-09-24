using System;
using System.Collections.Generic;
using System.Globalization;

namespace CSStarterTest1.DataOps
{
    public sealed class DataParser
    {
        private static readonly int _dataFieldCount = Enum.GetValues(typeof(DataField)).Length;

        private readonly CSVParser _parser = new();

        public Data[] Parse(string data)
        {
            string[][] table = _parser.Parse(data);
            List<Data> dataList = new();
            foreach (string[] line in table)
            {
                Data? parsed = TryParseLine(line);
                if (parsed is null)
                {
                    continue;
                }
                dataList.Add(parsed);
            }
            return dataList.ToArray();
        }
        private Data? TryParseLine(string[] line)
        {
            if (line.Length != _dataFieldCount)
            {
                return null;
            }

            if (!DateTime.TryParse(line[(int)DataField.Date], CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return null;
            }

            return new(
                date, 
                line[(int)DataField.FirstName],
                line[(int)DataField.SecondName],
                line[(int)DataField.FathersName],
                line[(int)DataField.CityName],
                line[(int)DataField.CountryName]
            );
        }
        private enum DataField
        {
            Date = 0,
            FirstName,
            SecondName,
            FathersName,
            CityName,
            CountryName,
        }
    }
}
