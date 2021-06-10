using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpPilot1 {
    static class Game {
        // TODO: Finish GetLetters().
        private static Dictionary<char, int> GetLetters(string word) {
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
    }
}
