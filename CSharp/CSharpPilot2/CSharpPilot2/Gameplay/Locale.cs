using CSharpPilot2.Input;

namespace CSharpPilot2.Gameplay
{
    internal abstract class Locale
    {
        public abstract string GetNameRequestString(int playerIndex);
        public abstract string GetWordRequestString(string playerName);
        public abstract string GetInvalidWordString(string word);
        public abstract string GetInvalidInputString();
        public abstract string GetTimeLeftSuffixString(double timeLeft);
        public abstract string GetGameOverLoserString(string loserName);
        public abstract string GetGameOverPreviousWordString(string word, double seconds);
        public abstract string GetGameOverCurrentWordString(string word, double seconds);
        public abstract string GetPressAnyKeyToContinueString();
        public abstract string GetIntroTitleString();
        public abstract string GetIntroRulesTitleString();
        public abstract string GetIntroRuleFirstString(int playerCount);
        public abstract string GetIntroRuleSecondString();
        public abstract string GetIntroRuleThirdString();
        public abstract string GetIntroRuleFourthString(int minWordTextLength, int maxWordTextLength);
        public abstract string GetIntroRuleFifthString(double maxSeconds);
        public abstract string GetIntroRuleSixthString();
        public abstract string GetErrorParsingCommand(string command, string reason);

        protected abstract string FormatTime(double seconds);
    }
}
