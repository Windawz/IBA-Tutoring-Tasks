using System.Collections.Generic;

namespace CSharpPilot2.Locales
{
    partial class StringTable
    {
        public static readonly StringTable Russian =
            new StringTable
            (
                new Dictionary<TableIndex, string>
                {
                    [TableIndex.IntroTitle] = @"Добро пожаловть в ""Игру в 'Слова""",
                    [TableIndex.IntroRuleTitle] = @"Правила таковы",
                    [TableIndex.IntroRule1] = @"{0} игрока(ов) поочерёдно вводят слова, состоящие из букв предыдущего",
                    [TableIndex.IntroRule2] = @"Введённое слово должно отличаться от первоначального",
                    [TableIndex.IntroRule3] = @"Первый игрок получает вводит любое слово и не ограничен во времени",
                    [TableIndex.IntroRule4] = @"Если слово длиной меньше {0} или больше {1} символов, придётся повторить ввод",
                    [TableIndex.IntroRule5] = @"На ввод даётся {0}",
                    [TableIndex.IntroRule6] = @"При неправильном вводе время не восстанавливается, а идёт дальше",
                    [TableIndex.GameOverLoser] = @"Игрок {0} проиграл",
                    [TableIndex.GameOverPrevWord] = @"Пред. слово",
                    [TableIndex.GameOverCurWord] = @"Текущ. слово",
                    [TableIndex.NameRequest] = @"Игрок {0}, представьтесь",
                    [TableIndex.WordRequest] = @"{0}, введите слово",
                    [TableIndex.AnyKeyRequest] = @"Нажмите любую клавишу, чтобы продолжить",
                    [TableIndex.ErrorInvalidName] = @"Такое имя не подходит, попробуйте ещё раз",
                    [TableIndex.ErrorInvalidInput] = @"Неверный ввод, попробуйте ещё раз",
                    [TableIndex.ErrorParsingCommand] = @"Неверный вызов комманды {0}, причина: {1}. Попробуйте ещё раз",
                    [TableIndex.ErrorCommandNotFound] = @"Комманда {0} не найдена, попробуйте ещё раз",
                    [TableIndex.FormatSeconds] = @"{0:0.00}с",
                    [TableIndex.TimeRemaining] = @"Осталось {0}",
                }
            );
    }
}
