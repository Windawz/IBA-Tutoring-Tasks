using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Linq;

namespace CSharpPilot1 {
    class Program {
        const int MinWordLength = 8;
        const int MaxWordLength = 30;
        const int MaxPlayers = 2;
        const double MaxSeconds = 10.0;

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
            RunIntroduction();

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

            Console.WriteLine(
$@"
Игрок {player + 1} проиграл!
Слово: ""{input!.Word}"";
Предыдущее слово: ""{lastInput?.Word ?? string.Empty}"";
Времени затрачено: {input!.Seconds:f}с."
            );
        }
        static void RunIntroduction() {
            Console.WriteLine(
$@" Добро пожаловать в ""Игру в ""Слова"". Правила игры таковы:

- {MaxPlayers} игрока(ов) поочерёдно вводят слова, состоящие из букв предыдущего.
- Введённое слово должно отличаться от первоначального.
- Первый игрок получает карт-бланш.
- Если слово длиной меньше {MinWordLength} или больше {MaxWordLength} символов, придётся повторить ввод.
- На ввод даётся {MaxSeconds} секунд.
- При повторном вводе время не восстанавливается.

Нажмите любую клавишу, чтобы начать...
"
            );
            Console.ReadKey(true);
        }
        static string GetTimeRemainingString(double timeElapsed) {
            if (timeElapsed >= MaxSeconds) {
                return "(время вышло)";
            } else {
                return $"(осталось {MaxSeconds - timeElapsed:f}с)";
            }
        }
        static int GetNextPlayer(int currentPlayer) {
            return (currentPlayer + 1) % MaxPlayers;
        }
        static string? GetInputErrorString(InputError inputError) {
            if (inputError == InputError.None) {
                return null;
            }
            return inputError switch {
                _ when inputError.HasFlag(InputError.WordTooShort) => $"Длина слова ниже {MinWordLength} символов",
                _ when inputError.HasFlag(InputError.WordTooLong) => $"Длина слова ниже {MaxWordLength} символов",
                _ when inputError.HasFlag(InputError.WordNullOrEmpty) => $"Ничего не введено",
                _ when inputError.HasFlag(InputError.NotWord) => $"Нужно ввести СЛОВО",
                _ => throw new ArgumentException("Unknown input error"),
            };
        }
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
            string word = Console.ReadLine();
            timer.Stop();

            return new Input(word, seconds);
        }
    }
}
