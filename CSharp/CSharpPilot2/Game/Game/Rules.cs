using System;
using System.Linq;

using Game.Input;

namespace Game.Game {
    static class Rules {
        // Amount of players involved.
        public const int MaxPlayers = 2;
        // Max amount of seconds to enter a word before player loses.
        public const double MaxSeconds = 10.0;
        public const int MinTextLength = 8;
        public const int MaxTextLength = 30;

        public static int NextPlayerIndex(int index) => (index + 1) % MaxPlayers;
        public static bool IsInputTextValid(string text) =>
            !string.IsNullOrWhiteSpace(text) && !text.Contains(' ') && text.Length >= MinTextLength && text.Length <= MaxTextLength;

        // Checks if the input is competent (won't lose) compared to the previous input in terms of text.
        public static bool IsInputCompetentText(InputInfo inputInfo, InputInfo prevInfo) =>
        inputInfo
        .Text
        .ToLowerInvariant()
        .CharacterCounts()
        .SequenceEqual(
            prevInfo
            .Text
            .ToLowerInvariant()
            .CharacterCounts()) && !string.Equals(inputInfo.Text, prevInfo.Text, StringComparison.InvariantCultureIgnoreCase);

        // Checks if the input is competent (won't lose) compared to the previous input in terms of time taken.
        public static bool IsInputCompetentTime(InputInfo inputInfo) =>
            inputInfo.Seconds <= MaxSeconds;
    }
}