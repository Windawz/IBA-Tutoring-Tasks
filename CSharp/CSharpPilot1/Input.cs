using System;

namespace CSharpPilot1 {
    static class Input {
        public static string? GetWord() => ExtractWord(GetInput(), IsValidWord);
        private static string GetInput() => Console.ReadLine().Trim();
        private static bool IsValidWord(string word) =>
            !string.IsNullOrEmpty(word) &&
            !word.Contains(' ') &&
            (word.Length >= 8 && word.Length <= 30);
        private static string? ExtractWord(string source, Func<string, bool> validator) =>
            validator(source) ? source : null;
    }
}
