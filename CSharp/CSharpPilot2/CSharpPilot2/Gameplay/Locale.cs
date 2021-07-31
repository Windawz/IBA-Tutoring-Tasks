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
        public abstract string GetGameOverPressAnyKeyString();

        protected abstract string FormatTime(double seconds);
    }
}
