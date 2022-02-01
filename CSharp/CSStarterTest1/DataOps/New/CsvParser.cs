using System;
using System.Collections.Generic;

namespace CSStarterTest1.DataOps.New
{
    /// <summary>
    /// Parses .csv strings into data records
    /// </summary>
    internal class CsvParser
    {
        private static readonly int DataFieldCount = typeof(Data).GetProperties().Length;

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
            if (tokens.Length < DataFieldCount)
            {
                return null;
            }

            Data data;
            try
            {
                data = new Data(
                    int.Parse(tokens[0]),
                    DateTime.Parse(tokens[1]),
                    tokens[2],
                    tokens[3],
                    tokens[4],
                    tokens[5],
                    tokens[6]
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
