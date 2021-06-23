using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Linq;

namespace CSharpPilot1.Procedural {
    class Program {
        // Amount of players involved.
        const int MaxPlayers = 2;
        // Max amount of seconds to enter a word before player loses.
        const double MaxSeconds = 10.0;
        const int MinWordLength = 8;
        const int MaxWordLength = 30;

        // For easier formatting.
        static readonly string NL = Environment.NewLine;

        class Input {
            public Input(string word, double seconds) {
                Word = word;
                Seconds = seconds;
            }

            public string Word { get; }
            public double Seconds { get; }
        }
        enum InputError {
            None,
            WordNullOrEmpty,
            WordTooLong,
            WordTooShort,
            NotWord,
        }
        static void Main(string[] args) {
            Console.WriteLine(GetIntroductionString());
            Console.ReadKey(true);

            bool isOver = false;
            int player = 0;
            Input? lastInput = null;
            Input? input = null;

            while (!isOver) {
                double timeElapsed = 0.0;
                InputError inputError;

                Console.WriteLine($"Игрок {player + 1}, введите слово {GetTimeRemainingString(timeElapsed)}:");
                do {
                    input = GetInput(timeElapsed);
                    inputError = GetInputError(input);
                    timeElapsed = input.Seconds;

                    if (inputError != InputError.None) {
                        Console.WriteLine($"{GetInputErrorString(inputError)}. Попробуйте ещё раз {GetTimeRemainingString(timeElapsed)}:");
                    }

                } while (inputError != InputError.None && timeElapsed < MaxSeconds);

                if (timeElapsed >= MaxSeconds || lastInput != null && (input.Word == lastInput.Word || !HasSameLetters(input.Word, lastInput.Word))) {
                    isOver = true;
                } else {
                    player = GetNextPlayer(player);
                    lastInput = input;
                }
            }

            Console.WriteLine(GetEndgameString(player, input!, lastInput));
        }
        static string GetIntroductionString() {
            return $"Добро пожаловать в \"Игру в \"Слова\". Правила игры таковы:{NL}{NL}" +
                $"- {MaxPlayers} игрока(ов) поочерёдно вводят слова, состоящие из букв предыдущего.{NL}" +
                $"- Введённое слово должно отличаться от первоначального.{NL}" +
                $"- Первый игрок получает карт-бланш.{NL}" +
                $"- Если слово длиной меньше {MinWordLength} или больше {MaxWordLength} символов, придётся повторить ввод.{NL}" +
                $"- На ввод даётся {MaxSeconds} секунд.{NL}" +
                $"- При повторном вводе время не восстанавливается.{NL}{NL}" +
                $"Нажмите любую клавишу, чтобы начать...{NL}";
        }
        static string GetEndgameString(int loser, Input input, Input? lastInput) {
            return $"\nИгрок {loser + 1} проиграл!{NL}" +
                $"Слово: \"{input!.Word}\";{NL}" +
                $"Предыдущее слово: \"{lastInput?.Word ?? string.Empty}\";{NL}" +
                $"Времени затрачено: {input!.Seconds:f}с.";
        }
        static string GetTimeRemainingString(double timeElapsed) {
            if (timeElapsed >= MaxSeconds) {
                return "(время вышло)";
            } else {
                return $"(осталось {MaxSeconds - timeElapsed:f}с)";
            }
        }
        static int GetNextPlayer(int currentPlayer) => (currentPlayer + 1) % MaxPlayers;
        static string? GetInputErrorString(InputError inputError) => inputError switch {
            InputError.None => null,
            _ when inputError.HasFlag(InputError.WordTooShort) => $"Длина слова ниже {MinWordLength} символов",
            _ when inputError.HasFlag(InputError.WordTooLong) => $"Длина слова ниже {MaxWordLength} символов",
            _ when inputError.HasFlag(InputError.WordNullOrEmpty) => $"Ничего не введено",
            _ when inputError.HasFlag(InputError.NotWord) => $"Нужно ввести СЛОВО",
            _ => throw new ArgumentException("Unknown input error"),
        };
        static InputError GetInputError(Input input) {
            if (string.IsNullOrEmpty(input.Word)) {
                return InputError.WordNullOrEmpty;
            } else {
                if (input.Word.Contains(' ')) {
                    return InputError.NotWord;
                }
                if (input.Word.Length > MaxWordLength) {
                    return InputError.WordTooLong;
                }
                if (input.Word.Length < MinWordLength) {
                    return InputError.WordTooShort;
                }
            }

            return InputError.None;
        }

        static bool HasSameLetters(string word, string lastWord) {
            if (word.Length != lastWord.Length) {
                return false;
            }
            var letters = GetLetterCounts(word);
            var lastLetters = GetLetterCounts(lastWord);
            return lastLetters.All(
                kv => letters.ContainsKey(kv.Key) && kv.Value == letters[kv.Key]
            );
        }
        // The dictionary returned represents all characters in the word and amounts of their occurences in it.
        static Dictionary<char, int> GetLetterCounts(string word) {
            var d = new Dictionary<char, int>();
            foreach (char c in word) {
                if (d.ContainsKey(c)) {
                    d[c] += 1;
                } else {
                    d[c] = 1;
                }
            }
            return d;
        }
        static Input GetInput(double initialTime) {
            double seconds = initialTime;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => seconds += 0.1;
            Console.Write('>');

            timer.Start();
            string word = Console.ReadLine() ?? string.Empty;
            timer.Stop();

            return new Input(word, seconds);
        }
    }
}
