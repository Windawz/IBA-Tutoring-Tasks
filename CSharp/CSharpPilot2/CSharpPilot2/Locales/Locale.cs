using System;
using System.Text;

namespace CSharpPilot2.Locales
{
    class Locale
    {
        public Locale(StringTable table)
        {
            _table = table;
        }

        private readonly StringTable _table;

        public string Intro(int playerCount, int minWordTextLength, int maxWordTextLength, double maxSeconds) =>
            new StringBuilder()
                .Append(_table.Retrieve(TableIndex.IntroTitle))
                .AppendLine("!")

                .Append(_table.Retrieve(TableIndex.IntroRuleTitle))
                .AppendLine(":")

                .Append("- ").AppendLine(_table.Retrieve(TableIndex.IntroRule1, playerCount))
                .Append("- ").AppendLine(_table.Retrieve(TableIndex.IntroRule2))
                .Append("- ").AppendLine(_table.Retrieve(TableIndex.IntroRule3))
                .Append("- ").AppendLine(_table.Retrieve(TableIndex.IntroRule4, minWordTextLength, maxWordTextLength))
                .Append("- ").AppendLine(_table.Retrieve(TableIndex.IntroRule5, maxSeconds))
                .Append("- ").AppendLine(_table.Retrieve(TableIndex.IntroRule6))

                .ToString();
        public string GameOver(string loserName, string prevWord, double prevSeconds, string curWord, double curSeconds) =>
            new StringBuilder()
                .Append(_table.Retrieve(TableIndex.GameOverLoser, $"\"{loserName}\"")).AppendLine("!")
                .AppendLine()

                .Append(_table.Retrieve(TableIndex.GameOverPrevWord))
                .Append(": ")
                .Append($"\"{prevWord}\"")
                .AppendLine($"({FormatTime(prevSeconds)})")

                .Append(_table.Retrieve(TableIndex.GameOverCurWord))
                .Append(": ")
                .Append($"\"{curWord}\"")
                .AppendLine($"({FormatTime(curSeconds)})")
                
                .ToString();

        public string NameRequest(int playerNumber) =>
            $"{_table.Retrieve(TableIndex.NameRequest, playerNumber)}:";
        public string WordRequest(string playerName) =>
            $"{_table.Retrieve(TableIndex.WordRequest, playerName)}:";
        public string SecondsLeftSuffix(double secondsLeft) =>
            $"({_table.Retrieve(TableIndex.TimeRemaining, FormatTime(secondsLeft))})";
        public string AnyKeyRequest() =>
            $"{_table.Retrieve(TableIndex.AnyKeyRequest)}...";
        public string ErrorInvalidName() =>
            $"{_table.Retrieve(TableIndex.ErrorInvalidName)}:";
        public string ErrorInvalidInput() =>
            $"{_table.Retrieve(TableIndex.ErrorInvalidInput)}:";
        public string ErrorParsingCommand(string command, string reason) =>
            $"{_table.Retrieve(TableIndex.ErrorParsingCommand, $"\"{command}\"", $"\"{reason}\"")}:";
        public string ErrorCommandNotFound(string command) =>
            $"{_table.Retrieve(TableIndex.ErrorCommandNotFound, $"\"{command}\"")}:";
        private string FormatTime(double seconds) =>
            _table.Retrieve(TableIndex.FormatSeconds, seconds);
    }
}
