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
            bool isOver = false;
            int player = 0;
            Input? lastInput = null;

            while (!isOver) {
                Console.WriteLine($"Игрок {player + 1}, введите слово:");

                double timeElapsed = 0.0;
                Input input;
                InputError inputError;
                do {
                    input = GetInput(timeElapsed);
                    inputError = GetInputError(input);

                    if (inputError != InputError.None) {
                        Console.WriteLine($"{GetInputErrorString(inputError)}. Попробуйте ещё раз (осталось {MaxSeconds - input.Seconds} секунд):");
                        timeElapsed = input.Seconds;
                    }
                } while (inputError != InputError.None);

                if (lastInput is null) {
                    player = GetNextPlayer(player);
                } else {
                    if (InputMeetsWinConditions(input, lastInput)) {
                        player = GetNextPlayer(player);
                    } else {
                        isOver = true;
                    }
                }

                lastInput = input;
            }

            Console.WriteLine($"Игрок {player + 1} проиграл! Слово: {lastInput!.Word}; Времени затрачено: {lastInput!.Seconds}с.");
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
        static bool InputMeetsWinConditions(Input input, Input lastInput) =>
            (input.Seconds < MaxSeconds) && HasSameLetters(input.Word, lastInput.Word);
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
            double seconds = 0.0;

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
