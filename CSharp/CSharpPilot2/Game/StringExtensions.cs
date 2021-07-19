using System.Linq;
using System.Collections.Generic;

namespace Game {
    // Convenience extension methods for implementing word comparison logic.
    static class StringExtensions {
        public static int CharacterCount(this string str, char c) =>
            str.Where(x => x == c).Count();

        // Returns a sequence of pairs.
        // Each pair represents a character and the amount of its occurences in the string.
        public static IEnumerable<(char Char, int Count)> CharacterCounts(this string str) =>
            str.Select(c => (Char: c, Count: str.CharacterCount(c))).OrderBy(x => x.Char);
    }
}