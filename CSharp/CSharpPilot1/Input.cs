using System;

namespace CSharpPilot1 {
    static class Input {
        private static string GetInput() {
            return Console.ReadLine().Trim();
        }
        private static bool IsValidWord(string word) {
            return 
                !string.IsNullOrEmpty(word) && 
                !word.Contains(' ') &&
                (word.Length >= 8 && word.Length <= 30);
        }
        public static string? GetWord() {
            string word = GetInput();
            if (IsValidWord(word)) {
                return word;
            } else {
                return null;
            }
        }
    }
}
