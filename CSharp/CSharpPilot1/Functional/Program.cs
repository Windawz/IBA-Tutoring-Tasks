using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;

/*
 *      Warning
 * 
 *  This code is in a broken state and should not be taken seriously.
 *  
 *  You've been warned.
 */

/*
namespace CSharpPilot1 {
    class ProgramFunctional {
        const int MinWordLength = 8;
        const int MaxWordLength = 30;
        const int MaxPlayers = 2;
        const double TimerInterval = 0.1;
        const double MaxSeconds = 10.0;
        const string InputPromptFormatString = "Игрок {0}, введите слово:";
        const string InvalidInputFormatString = "Ошибка ввода: {0}. Повторите ввод:";

        class GameState {
            public GameState(int player, Input input, Output output, GameState? lastState) {
                Player = player;
                Input = input;
                Output = output;
                LastState = lastState;
            }

            public int Player { get; }
            public Input Input { get; }
            public Output Output { get; }
            public GameState? LastState { get; }

            public GameState Copy() => new GameState(Player, Input, Output, LastState);
        }
        class Input {
            public Input(string word, double seconds) {
                Word = word;
                Seconds = seconds;
            }

            public string Word { get; }
            public double Seconds { get; }

            public Input Copy() => new Input(Word, Seconds);
        }
        class Output {
            public Output(string? str = null) {
                contents = new StringBuilder(str);
            }

            private StringBuilder contents;

            public Output Add(string format, params object[] arg) =>
                new Output(string.Format(format, arg));
            public Output AddLine() =>
                Add(Environment.NewLine);
            public Output AddLine(string format, params object[] arg) =>
                Add(format, arg).AddLine();
            public Output Print() {
                string str = contents.ToString();
                Console.Write(str);
                return new Output(str);
            }
            public Output Copy() => new Output(contents.ToString());
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
                        new Output(string.Format(InputPromptFormatString, 1)).AddLine().Print(),
                        null
                    )
                ),
                _ when state.LastState is null => Play(
                    new GameState(
                        GetNextPlayer(state.Player),
                        GetInput(),
                        state.Output.AddLine(string.Format(InputPromptFormatString, GetNextPlayer(state.Player) + 1)).Print(),
                        state
                    )
                ),
                _ when !isInputValid(state.Input) => Play(
                    new GameState(
                        state.Player,
                        GetInput(),
                        new Output().Print(),
                        state
                    )
                ),
                _ when !isInputCompetent(state.Input, state.LastState.Input) => state.Copy(),
                _ => Play(
                    new GameState(
                        GetNextPlayer(state.Player),
                        GetInput(),
                        state
                    )
                ),
            };
        }
        static int GetNextPlayer(int playerIndex) =>
            (playerIndex + 1) % MaxPlayers;

        static bool HasSameLetters(string word, string lastWord) =>
            word.Length == lastWord.Length && GetLetterCounts(word).Intersect(GetLetterCounts(lastWord)).Any();
        static IEnumerable<KeyValuePair<char, int>> GetLetterCounts(string word) =>
            word.Select(x => new KeyValuePair<char, int>(x, word.Count(y => y == x)));

        static Input GetInput() {
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
*/