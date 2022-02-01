
using System;

namespace CSStarterTest1.DataOps
{
    internal class DataFieldFormatter
    {
        public string Format(object fieldValue)
        {
            switch (fieldValue)
            {
                case int intValue:
                    return String.Format("{0}", intValue);
                case DateTime dtValue:
                    return dtValue.ToString("dd.MM.yyyy");
                case string stringValue:
                    return stringValue;
                default:
                    throw new ArgumentException("Invalid field value type");
            }
        }
    }
}
