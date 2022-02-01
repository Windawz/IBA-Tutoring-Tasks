﻿using System;
using System.IO;

namespace CSStarterTest1.DataOps.New
{
    public class CsvImporter : IImporter
    {
        public Data[] Import(string path)
        {
            string text = File.ReadAllText(path);
            return new CsvParser().Parse(text);
        }
    }
}
