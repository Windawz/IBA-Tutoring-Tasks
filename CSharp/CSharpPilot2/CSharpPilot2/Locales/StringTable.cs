using System;
using System.Collections.Generic;

namespace CSharpPilot2.Locales
{
    partial class StringTable
    {
        public StringTable(IReadOnlyDictionary<TableIndex, string> stringDict)
        {
            _stringDict = stringDict;
        }

        private readonly IReadOnlyDictionary<TableIndex, string> _stringDict;

        /// <summary>
        /// <para>
        /// Gets the string at the specified index and formats it with the supplied args.
        /// </para>
        /// <para>
        /// Note: 
        /// </para>
        /// </summary>
        /// <param name="index"><see cref="TableIndex"/> of the requested string.</param>
        /// <param name="formatArgs">Args to attempt to format the string with.</param>
        /// <returns>The string corresponding to the <see cref="TableIndex"/>, retrieved and formatted.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is invalid or no string is found at it.</exception>
        /// <exception cref="FormatException">Thrown if formatting fails due to bad <paramref name="formatArgs"/> or other errors.</exception>
        public string Retrieve(TableIndex index, params object[] formatArgs)
        {
            if (!Enum.IsDefined(index))
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Unknown {nameof(TableIndex)} value");
            }

            try
            {
                string entry = _stringDict[index];
                return String.Format(entry, formatArgs);
            }
            catch (KeyNotFoundException e)
            {
                throw new ArgumentOutOfRangeException($"String at index {index} does not exist in this {nameof(StringTable)}", e);
            }
            catch (FormatException)
            {
                throw;
            }
        }
    }
}
