
using CSharpPilot2.Input;

namespace CSharpPilot2.Gameplay
{
    internal class RussianLocale : Locale
    {
        public override string GetNameRequestString(int playerIndex) =>
            $"Игрок {playerIndex + 1}, введите имя:";
        public override string GetWordRequestString(Player player) =>
            $"{player.Name}, введите слово:";
        public override string GetInvalidWordString(Word word) =>
            $"Слово \"{word.Text}\" не подходит!";
        public override string GetInvalidInputString(InputInfo inputInfo, Player player) =>
            $"Неверный ввод, попробуйте ещё раз:";
        public override string GetTimeLeftSuffixString(double timeLeft) =>
            $"(осталось {FormatTime(timeLeft)})";
        public override string GetGameOverPreviousWordString(Word word) =>
            $"Предыдущее слово: \"{word.Text}\" ({FormatTime(word.Seconds)})";
        public override string GetGameOverCurrentWordString(Word word) =>
            $"Текущее слово: \"{word.Text}\" ({FormatTime(word.Seconds)})";

        protected override string FormatTime(double seconds) =>
            $"{seconds:F}с";
    }
}
