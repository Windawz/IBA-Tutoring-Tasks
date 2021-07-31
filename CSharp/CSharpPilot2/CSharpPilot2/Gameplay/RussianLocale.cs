
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
        public override string GetPressAnyKeyToContinueString() =>
            $"Нажмите любую клавишу...";
        public override string GetIntroTitleString() =>
            $"Добро пожаловть в \"Игру в \'Слова\"!";
        public override string GetIntroRulesTitleString() =>
            $"Правила таковы:";
        public override string GetIntroRuleFirstString(int playerCount) =>
            $"{playerCount} игрока(ов) поочерёдно вводят слова, состоящие из букв предыдущего.";
        public override string GetIntroRuleSecondString() =>
            $"Введённое слово должно отличаться от первоначального.";
        public override string GetIntroRuleThirdString() =>
            $"Первый игрок получает карт-бланш.";
        public override string GetIntroRuleFourthString(int minWordTextLength, int maxWordTextLength) =>
            $"Если слово длиной меньше {minWordTextLength} или больше {maxWordTextLength} символов, придётся повторить ввод.";
        public override string GetIntroRuleFifthString(double maxSeconds) =>
            $"На ввод даётся {FormatTime(maxSeconds)}.";
        public override string GetIntroRuleSixthString() =>
            $"При неправильном вводе время не восстанавливается, а идёт дальше.";

        protected override string FormatTime(double seconds) =>
            $"{seconds:F}с";
    }
}
