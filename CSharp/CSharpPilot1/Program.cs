using System;
using System.Collections.Generic;

namespace CSharpPilot1 {
    class Program {
        static void Main(string[] args) {

        }
        static string RequestPlayerInput(string message, string errorMessage) {
            while (true) {
                Println("{0}", message);
                Print(">");
                string? result = GetWord();
                if (result != null) {
                    return result;
                } else {
                    Println("{0}", errorMessage);
                }
            }
        }
        static Dictionary<char, int> GetLetters(string word) {
            var d = new Dictionary<char, int>();
            foreach (char c in word) {
                d[c] = d.ContainsKey(c) ? d[c] + 1 : 1;
            }
            return d;
        }
        static void Println(string format, params object[] arg) {
            Print(format, arg);
            Console.WriteLine();
        }
        static void Print(string format, params object[] arg) =>
            Console.Write(format, arg);
        static string? GetWord() {
            string word = GetInput();
            if (IsValidWord(word)) {
                return word;
            } else {
                return null;
            }
        }
        static string GetInput() =>
            Console.ReadLine().Trim();
        private static bool IsValidWord(string word) =>
            !string.IsNullOrEmpty(word) &&
            !word.Contains(' ') &&
            (word.Length >= 8 && word.Length <= 30);
    }
}
