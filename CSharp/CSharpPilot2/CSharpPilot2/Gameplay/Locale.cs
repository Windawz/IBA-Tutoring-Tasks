using CSharpPilot2.Input;

namespace CSharpPilot2.Gameplay
{
    internal abstract class Locale
    {
        public abstract string GetNameRequestString(int playerIndex);
        public abstract string GetWordRequestString(Player player);
        public abstract string GetInvalidWordString(Word word);
        public abstract string GetInvalidInputString(InputInfo inputInfo, Player player);
        public abstract string GetTimeLeftSuffixString(double timeLeft);
        public abstract string GetGameOverPreviousWordString(Word prevWord);
        public abstract string GetGameOverCurrentWordString(Word currentWord);

        protected abstract string FormatTime(double seconds);
    }
}
