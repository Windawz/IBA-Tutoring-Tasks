using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CSStarterTest1.DataOps
{
    /// <summary>
    /// Parses text in .csv format into <see cref="Data"/>.
    /// </summary>
    public sealed class DataParser
    {
        private static readonly int _dataFieldCount = Enum.GetValues(typeof(DataField)).Length;

        private readonly CSVParser _parser = new();

        /// <summary>
        /// Parses a string of representing a .csv file into an array of <see cref="Data"/> objects.
        /// </summary>
        /// <param name="data">The .csv file string.</param>
        /// <returns>
        /// An array of parsed <see cref="Data"/> objects.
        /// </returns>
        /// <exception cref="FormatException">Thrown if parsing a line fails.</exception>
        public Data[] Parse(string data)
        {
            string[][] table = _parser.Parse(data);
            List<Data> dataList = new();
            foreach (string[] line in table)
            {
                Data parsed = ParseLine(line);
                dataList.Add(parsed);
            }
            return dataList.ToArray();
        }

        /// <summary>
        /// Parses an array of tokens into a <see cref="Data"/> object.
        /// </summary>
        /// <param name="line">The array of tokens to parse.</param>
        /// <returns>
        /// The resulting <see cref="Data"/> object..
        /// </returns>
        /// <exception cref="FormatException">Thrown if parsing fails.</exception>
        private static Data ParseLine(string[] line)
        {
            Data? data = TryParseLine(line);
            if (data is null)
            {
                throw new FormatException($"Failed to parse line: \"{line.Aggregate((s1, s2) => s1 + s2)}\"");
            }
            return data;
        }
        /// <summary>
        /// Parses an array of tokens into a <see cref="Data"/> object.
        /// </summary>
        /// <param name="line">The array of tokens to parse.</param>
        /// <returns>
        /// The resulting <see cref="Data"/> object, or null on failure.
        /// </returns>
        private static Data? TryParseLine(string[] line)
        {
            if (line.Length != _dataFieldCount)
            {
                return null;
            }

            if (!DateTime.TryParse(line[(int)DataField.Date], CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
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
        /// <summary>
        /// Indices corresponding to the fields of a <see cref="Data"/> object.
        /// </summary>
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
