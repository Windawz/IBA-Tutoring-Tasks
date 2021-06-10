using System;
using System.Collections.Generic;

namespace CSharpPilot1 {
    class Program {
        static void Main(string[] args) {
        }
    }
    static class Game {
        public static Dictionary<char, int> GetLetters(string word) {
            switch (word.Length) {
                case 0:
                    return new Dictionary<char, int>();

                default:
                    string s = word.Substring(0, word.Length - 1);
                    char c = word[word.Length - 1];
                    var d = new Dictionary<char, int>(GetLetters(s));
                    d[c] = d.ContainsKey(c) ? d[c] + 1 : 1;
                    return d;
            }
        }
        public static string? GetWord() {
            string word = GetInput();
            if (IsValidWord(word)) {
                return word;
            } else {
                return null;
            }
        }
        private static string GetInput() {
            return Console.ReadLine().Trim();
        }
        private static bool IsValidWord(string word) {
            return
                !string.IsNullOrEmpty(word) &&
                !word.Contains(' ') &&
                (word.Length >= 8 && word.Length <= 30);
        }
    }
}
