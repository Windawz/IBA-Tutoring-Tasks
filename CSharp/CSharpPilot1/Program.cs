using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;

namespace CSharpPilot1 {
    class Program {
        const int MinWordLength = 8;
        const int MaxWordLength = 30;
        const int MaxPlayers = 2;
        const double MaxSeconds = 10.0;

        class GameState {
            public GameState(int player, Input input) {
                Player = player;
                Input = input;
            }
            public GameState(GameState other) : this(other.Player, other.Input) { }


            public int Player { get; }
            public Input Input { get; }
        }
        class Input {
            public Input(string word, double seconds) {
                Word = word;
                Seconds = seconds;
            }

            public string Word { get; }
            public double Seconds { get; }
        }
        class Output {
            public Output() {
                sb = new StringBuilder();
            }
            public Output(string format, params object[] arg) : this() {
                sb.Append(string.Format(format, arg));
            }
            public Output(string str) : this() {
                sb.Append(str);
            }

            private StringBuilder sb;

            public static void Print(Output output) =>
                Console.Write(output.ToString());
            public Output Add(string format, params object[] arg) =>
                new Output(this + string.Format(format, arg));
            public Output AddLine() =>
                new Output(this + Environment.NewLine);
            public Output AddLine(string format, params object[] arg) =>
                Add(format, arg).AddLine();
            public override string ToString() => sb.ToString();
        }

        static void Main(string[] args) {
            GameState endState = Play(null);
        }
        static GameState Play(GameState? lastState) {
            Func<GameState, bool> isPlayerDefeated =
                state => (state.Input.Seconds > MaxSeconds) || !HasSameLetters(GetInput().Word, state.Input.Word);
            Func<Input, bool> isInputValid =
                input => !string.IsNullOrEmpty(input.Word) &&
                    !input.Word.Contains(' ') &&
                    (input.Word.Length >= MinWordLength && input.Word.Length <= MaxWordLength);

            return lastState switch {
                null => Play(new GameState(0, GetInput())),
                _ when !isInputValid(lastState.Input) => Play(new GameState(lastState)),
                _ when isPlayerDefeated(lastState) => new GameState(lastState),
                _ => Play(new GameState((lastState.Player + 1) % MaxPlayers, GetInput())),
            };
        }

        static bool HasSameLetters(string word, string lastWord) =>
            word.Length == lastWord.Length && GetLetterCounts(word).Intersect(GetLetterCounts(lastWord)).Any();
        static IEnumerable<KeyValuePair<char, int>> GetLetterCounts(string word) =>
            word.Select(x => new KeyValuePair<char, int>(x, word.Count(y => y == x)));

        // Wrap because the inner function is impure.
        static Input GetInput() {
            double seconds = 0.0;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => seconds += 0.1;

            timer.Start();
            string word = Console.ReadLine();
            timer.Stop();

            return new Input(word, seconds);
        }

    }
}