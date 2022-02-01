using System;
using System.Globalization;

namespace CSStarterTest1.DataOps
{
    internal class DataFieldParser
    {
        public object Parse(string fieldValue, int fieldIndex)
        {
            if (0 > fieldIndex || fieldIndex > Data.FieldCount)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldIndex));
            }
            switch (fieldIndex)
            { 
                case 0:
                    return int.Parse(fieldValue);
                case 1:
                    return DateTime.ParseExact(fieldValue, "dd.MM.yyyy", null, DateTimeStyles.AssumeLocal);
                default:
                    return fieldValue;
            }
        }
    }
}
