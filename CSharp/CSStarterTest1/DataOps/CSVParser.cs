using System;
using System.Collections.Generic;

namespace CSStarterTest1.DataOps
{
    /// <summary>
    /// Parses .csv strings into data records
    /// </summary>
    internal class CsvParser
    {
        public Data[] Parse(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<Data>();
            }
            string[] lines = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var datas = new List<Data>(lines.Length);
            foreach (string line in lines)
            {
                Data? data = ParseLine(line);
                if (data is not null)
                {
                    datas.Add(data);
                }
            }
            return datas.ToArray();
        }
        private Data? ParseLine(string line)
        {
            string[] tokens = line.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (tokens.Length < Data.FieldCount)
            {
                return null;
            }

            Data data;
            var parser = new DataFieldParser();
            try
            {
                data = new Data(
                    (int)parser.Parse(tokens[0], 0),
                    (DateTime)parser.Parse(tokens[1], 1),
                    (string)parser.Parse(tokens[2], 2),
                    (string)parser.Parse(tokens[3], 3),
                    (string)parser.Parse(tokens[4], 4),
                    (string)parser.Parse(tokens[5], 5),
                    (string)parser.Parse(tokens[6], 6)
                );
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                return null;
            }

            return data;
        }
    }
}
