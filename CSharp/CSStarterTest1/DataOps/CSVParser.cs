using System;
using System.Collections.Generic;
using System.Text;

namespace CSStarterTest1.DataOps
{
    /// <summary>
    /// Parses a string representing a .csv file.
    /// </summary>
    internal sealed class CSVParser
    {
        /// <summary>
        /// Token, a pair of which enforces the text enveloped between to be treated as a single thing.
        /// </summary>
        public string Wrapper { get; set; } = "\"";
        /// <summary>
        /// Token that serves as a separator of fields.
        /// </summary>
        public string Separator { get; set; } = ";";
        /// <summary>
        /// Token that denotes a line to be ignored.
        /// </summary>
        public string Comment { get; set; } = "//";
        /// <summary>
        /// Default <see cref="StringComparison"/> value to use when comparing tokens.
        /// </summary>
        public StringComparison ComparisonType { get; set; } = StringComparison.InvariantCulture;

        /// <summary>
        /// Parses the string representation of a .csv file into a table of tokens.
        /// </summary>
        /// <param name="data">The string representation of a .csv file.</param>
        /// <returns>
        /// A table made up of all tokens extracted from the .csv string.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// May be thrown if a token has exceeded <see cref="StringBuilder.MaxCapacity"/>
        /// </exception>
        public string[][] Parse(string data)
        {
            string[] lines = data.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
            List<string[]> table = new();

            for (int i = 0; i < lines.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }
                string[] parsed = ParseLine(lines[i]);
                if (parsed.Length == 0)
                {
                    continue;
                }

                table.Add(parsed);
            }

            return table.ToArray();
        }
        /// <summary>
        /// Parses a line from a .csv file to an array of fields.
        /// </summary>
        /// <param name="value">The .csv line to parse.</param>
        /// <returns>
        /// Array of fields extracted from the line, or empty array if the line was ignored.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// May be thrown if a token has exceeded <see cref="StringBuilder.MaxCapacity"/>
        /// </exception>
        private string[] ParseLine(string value)
        {
            if (value.StartsWith(Comment))
            {
                return Array.Empty<string>();
            }

            return Split(value);
        }
        /// <summary>
        /// Splits the string into tokens.
        /// <para>
        /// Ignores whitespace by default.
        /// </para>
        /// </summary>
        /// <param name="value">The string to split.</param>
        /// <returns>
        /// An array of tokens.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// May be thrown if a token has exceeded <see cref="StringBuilder.MaxCapacity"/>
        /// </exception>
        private string[] Split(string value)
        {
            List<string> tokens = new();
            StringBuilder token = new();

            (int Left, int Right) wraps;
            int sep;

            void UpdateWraps(int startIndex) =>
                wraps = startIndex >= value.Length ? (-1, -1) : IndicesOfWrappers(value, startIndex);
            void UpdateSep(int startIndex) =>
                sep = startIndex >= value.Length ? -1 : IndexOfSeparator(value, startIndex);

            UpdateWraps(0);
            UpdateSep(0);

            for (int i = 0; i < value.Length; i++)
            {
                // Do wraps even exist at this point?
                bool wrapsExist = wraps.Left != -1 && -1 != wraps.Right;

                // Are we between the starts of the wrappers?
                bool betweenWraps = wrapsExist &&
                                    wraps.Left <= i && i <= wraps.Right;

                // Are we INSIDE the wrapper token itself?
                bool insideWrap = wrapsExist &&
                                  wraps.Left <= i && i < wraps.Left + Wrapper.Length ||
                                  wraps.Right <= i && i < wraps.Right + Wrapper.Length;

                // Are we at the last char of the rightmost wrapper?
                bool atRightWrapEnd = wrapsExist &&
                                      i == wraps.Right + Wrapper.Length - 1;

                // Does a separator even exist at this point?
                bool sepExists = sep != -1;

                // Are we at the start of the separator token?
                bool atSep = sepExists &&
                             i == sep;

                // Are we INSIDE the separator token itself?
                bool insideSep = sepExists &&
                                 sep <= i && i < sep + Separator.Length;

                // Are we at the last char of the separator?
                bool atSepEnd = sepExists &&
                                i == sep + Separator.Length - 1;
                
                // Are we at the last char?
                bool atLast = i == value.Length - 1;

                char c = value[i];

                // Add char if
                // not currently at a wrapper token
                // AND
                // either overriden by wrapper or haven't stumbled upon a separator yet.
                // Don't add the chars that are part of the separator.
                if (!insideWrap && (betweenWraps || !insideSep && !atSep) && !Char.IsWhiteSpace(c))
                {
                    token.Append(c);
                }
                // If that failed, see if we're at least at the separator's start,
                // or if the string is about to end.
                //
                // Flush if so.
                if (atSep || atLast)
                {
                    tokens.Add(token.ToString());
                    token.Clear();
                }

                // Update separator and wrapper indices if necessary
                // REMINDER: out of range cases are handled by the funcs already!!!
                if (atRightWrapEnd)
                {
                    UpdateWraps(i + 1);
                }
                if (atSepEnd)
                {
                    UpdateSep(i + 1);
                }
            }

            return tokens.ToArray();
        }
        /// <summary>
        /// Search for a separator token in the string using the current <see cref="ComparisonType"/>.
        /// </summary>
        /// <param name="haystack">String to search.</param>
        /// <param name="startIndex">Index to start at.</param>
        /// <returns>
        /// Start index of the separator token, -1 if not found.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Caused by invalid <paramref name="startIndex"/>.
        /// Refer to <see cref="String.IndexOf(String, int, StringComparison)"/>.
        /// </exception>
        private int IndexOfSeparator(string haystack, int startIndex)
        {
            return haystack.IndexOf(Separator, startIndex, ComparisonType);
        }
        /// <summary>
        /// Search for a pair of wrapper tokens in the string using the current <see cref="ComparisonType"/>.<br/>
        /// Two such adjacent tokens don't count.
        /// </summary>
        /// <param name="haystack">String to search.</param>
        /// <param name="startIndex">Index to start at.</param>
        /// <returns>
        /// Start indices of the first and the following wrapper, -1 correspondingly if not found.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Caused by invalid <paramref name="startIndex"/>.
        /// Refer to <see cref="String.IndexOf(String, int, StringComparison)"/>.
        /// </exception>
        private (int Left, int Right) IndicesOfWrappers(string haystack, int startIndex)
        {
            int index = haystack.IndexOf(Wrapper, startIndex, ComparisonType);
            int indexAdjacent = index + Wrapper.Length;
            int indexNext = -1;

            if (index != -1 && indexAdjacent < haystack.Length)
            {
                indexNext = haystack.IndexOf(Wrapper, indexAdjacent, ComparisonType);

                if (indexNext != -1 && indexNext == indexAdjacent)
                {
                    indexNext = -1;
                }
            }

            return (index, indexNext);
        }
    }
}
