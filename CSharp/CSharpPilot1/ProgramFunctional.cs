using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;

namespace CSharpPilot1 {
    class ProgramFunctional {
        const int MinWordLength = 8;
        const int MaxWordLength = 30;
        const int MaxPlayers = 2;
        const double MaxSeconds = 10.0;

        class GameState {
            public GameState(int player, Input input, GameState? lastState) {
                Player = player;
                Input = input;
                LastState = lastState;
            }

            public int Player { get; }
            public Input Input { get; }
            public GameState? LastState { get; }
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

        static GameState Play(GameState? state) {
            Func<Input, Input, bool> isInputCompetent =
                (input, lastInput) => (input.Seconds < MaxSeconds) && HasSameLetters(input.Word, lastInput.Word);
            Func<Input, bool> isInputValid =
                input => !string.IsNullOrEmpty(input.Word) &&
                    !input.Word.Contains(' ') &&
                    (input.Word.Length >= MinWordLength && input.Word.Length <= MaxWordLength);

            return state switch {
                null => Play(
                    new GameState(
                        0,
                        GetInput(),
                        null
                    )
                ),
                _ when state.LastState is null => Play(
                    new GameState(
                        IncrementPlayerIndex(state.Player),
                        GetInput(),
                        state
                    )
                ),
                _ when !isInputValid(state.Input) => Play(
                    new GameState(
                        state.Player,
                        GetInput(),
                        state
                    )
                ),
                _ when !isInputCompetent(state.Input, state.LastState.Input) => new GameState(
                    state.Player, 
                    state.Input, 
                    state.LastState
                ),
                _ => Play(
                    new GameState(
                        IncrementPlayerIndex(state.Player),
                        GetInput(),
                        state
                    )
                ),
            };
        }
        static int IncrementPlayerIndex(int playerIndex) =>
            (playerIndex + 1) % MaxPlayers;

        static bool HasSameLetters(string word, string lastWord) =>
            word.Length == lastWord.Length && GetLetterCounts(word).Intersect(GetLetterCounts(lastWord)).Any();
        static IEnumerable<KeyValuePair<char, int>> GetLetterCounts(string word) =>
            word.Select(x => new KeyValuePair<char, int>(x, word.Count(y => y == x)));

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