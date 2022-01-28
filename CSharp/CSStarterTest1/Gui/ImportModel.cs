using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

using CSStarterTest1.DataOps;

namespace CSStarterTest1.Gui
{
    /// <summary>
    /// Represents the data imported from a .csv file.
    /// </summary>
    internal sealed class ImportModel
    {
        /// <exception cref="ArgumentException">
        /// Failure has been caused by invalid file path, or the file's extension is not .csv.
        /// </exception>
        /// <exception cref="FormatException">
        /// Thrown if the .csv file has invalid syntax.
        /// </exception>
        /// <exception cref="SystemException">
        /// Thrown if a system error occurs during importing.
        /// </exception>
        public ImportModel(string path)
        {
            Data = ReadCsv(path);
        }

        public Data[] Data { get; }



        private static Data[] ReadCsv(string path)
        {
            using StreamReader reader = OpenCsv(path);
            string text = ReadCsvTextData(reader);
            if (String.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<Data>();
            }
            Data[] datas = new DataParser().Parse(text);
            return datas;
        }
        private static StreamReader OpenCsv(string path)
        {
            string? ext = Path.GetExtension(path);
            if (String.IsNullOrWhiteSpace(ext) || !ext.Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("File is not .csv", nameof(path));
            }

            StreamReader? reader;
            try
            {
                reader = File.OpenText(path);
            }
            catch (Exception e) when (e is ArgumentException or
                                           PathTooLongException or
                                           DirectoryNotFoundException or
                                           FileNotFoundException or
                                           NotSupportedException)
            {
                throw new ArgumentException("Invalid path", nameof(path), e);
            }
            catch (SystemException)
            {
                throw;
            }

            return reader;
        }
        private static string ReadCsvTextData(StreamReader csvReader)
        {
            string text;
            try
            {
                text = csvReader.ReadToEnd();
            }
            catch (SystemException)
            {
                throw;
            }
            return text;
        }
    }
}
