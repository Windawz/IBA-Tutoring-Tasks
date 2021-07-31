
using CSharpPilot2.Input;

namespace CSharpPilot2.Gameplay
{
    internal class RussianLocale : Locale
    {
        public override string GetNameRequestString(int playerIndex) =>
            $"Игрок {playerIndex + 1}, введите имя:";
        public override string GetWordRequestString(string playerName) =>
            $"{playerName}, введите слово:";
        public override string GetInvalidWordString(string word) =>
            $"Слово \"{word}\" не подходит!";
        public override string GetInvalidInputString() =>
            $"Неверный ввод, попробуйте ещё раз:";
        public override string GetTimeLeftSuffixString(double timeLeft) =>
            $"(осталось {FormatTime(timeLeft)})";
        public override string GetGameOverLoserString(string loserName) =>
            $"Игрок \"{loserName}\" проиграл!";
        public override string GetGameOverPreviousWordString(string word, double seconds) =>
            $"Предыдущее слово: \"{word}\" ({FormatTime(seconds)})";
        public override string GetGameOverCurrentWordString(string word, double seconds) =>
            $"Текущее слово: \"{word}\" ({FormatTime(seconds)})";
        public override string GetGameOverPressAnyKeyString() =>
            $"Нажмите любую клавишу...";

        protected override string FormatTime(double seconds) =>
            $"{seconds:F}с";
    }
}
