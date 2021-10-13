using System;
using System.Reflection;

namespace CSStarterTest1.DataOps
{
    /// <summary>
    /// Used for formatting objects of types used in <see cref="Data"/>.
    /// </summary>
    internal static class DataFieldFormatter
    {
        private static readonly string NullString = "NULL";

        /// <exception cref="ArgumentException">
        /// Thrown if the type of value is not supported,
        /// indicated by <see cref="CanFormat(object?)"/> returning false.
        /// </exception>
        public static string Format(object? value)
        {
            if (value is null)
            {
                return NullString;
            }

            string formatted = value switch 
            {
                null => NullString,
                DateTime dt => dt.ToString(FormatSettings.MainDateTimeFormatString),
                var val when CanFormat(val) => val.ToString() ?? String.Empty,
                _ => throw new ArgumentException("Failed to format due to unsupported type", nameof(value)),
            };

            return formatted;
        }
        public static bool CanFormat(object? value) =>
            value is null || value.GetType().IsPrimitive || value is
                Enum or
                Guid or
                string or
                DateTime;
    }
}
