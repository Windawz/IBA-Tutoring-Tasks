using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Gameplay
{
    internal class EnglishLocale : Locale
    {
        public override string GetNameRequestString(int playerIndex) =>
            $"Player {playerIndex + 1}, enter name:";
        public override string GetWordRequestString(string playerName) =>
            $"\"{playerName}\", enter word:";
        public override string GetInvalidWordString(string word) =>
            $"Word \"{word}\" is invalid!";
        public override string GetInvalidInputString() =>
            $"Invalid input, try again:";
        public override string GetTimeLeftSuffixString(double timeLeft) =>
            $"You have {FormatTime(timeLeft)} left";
        public override string GetGameOverLoserString(string loserName) =>
            $"Player \"{loserName}\" has lost!";
        public override string GetGameOverPreviousWordString(string word, double seconds) =>
            $"Previous word: \"{word}\" ({FormatTime(seconds)})";
        public override string GetGameOverCurrentWordString(string word, double seconds) =>
            $"Current word: \"{word}\" ({FormatTime(seconds)})";
        public override string GetPressAnyKeyToContinueString() =>
            $"Press any key to continue...";
        public override string GetIntroTitleString() =>
            $"Welcome to \"Game of \'Words\"!";
        public override string GetIntroRulesTitleString() =>
            $"The rules are:";
        public override string GetIntroRuleFirstString(int playerCount) =>
            $"{playerCount} player(s), one after another, enter their word consisting of the previous word's letters.";
        public override string GetIntroRuleSecondString() =>
            $"The word entered must differ from the previous.";
        public override string GetIntroRuleThirdString() =>
            $"The first player receives carte blanche.";
        public override string GetIntroRuleFourthString(int minWordTextLength, int maxWordTextLength) =>
            $"If the word's length is less than {minWordTextLength} or greater than {maxWordTextLength} characters, the input must be retried.";
        public override string GetIntroRuleFifthString(double maxSeconds) =>
            $"You have {FormatTime(maxSeconds)} for input.";
        public override string GetIntroRuleSixthString() =>
            $"On invalid input time isn't restored and keeps going.";
        public override string GetErrorParsingCommand(string command, string reason) =>
            $"Error parsing command \"{command}\". Reason: \"{reason}\".";
        public override string GetErrorCommandNotFound(string command) =>
            $"Error: command \"{command}\" not found.";

        protected override string FormatTime(double seconds) =>
            $"{seconds:F}s";
    }
}
