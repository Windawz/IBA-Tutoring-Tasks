using System;
using System.Linq;
using System.Timers;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharpPilot1.Functional {
    static class StringExtensions {
        public static int CharacterCount(this string str, char c) =>
            str.Where(x => x == c).Count();
        public static IEnumerable<(char Char, int Count)> CharacterCounts(this string str) =>
            str.Select(c => (Char: c, Count: str.CharacterCount(c))).OrderBy(x => x.Char);
    }

    // This pattern is called a monad.
    // It contains a given value, but you cannot extract the value afterwards.
    // That means you can't perform operations on the value directly.
    // Instead you 'Bind' operations to it by using Bind().
    // The function passed to Bind() transforms the value inside into a monad with the new value.
    // You 'inject' the desied behaviour into the monad.
    //
    // 
    //
    // Since we're working with IO, which is impure, we isolate it into a monad.
    //
    // It's better explained in the following articles:
    //  https://mikhail.io/2018/07/monads-explained-in-csharp-again/
    //  https://blog.ploeh.dk/2019/02/04/how-to-get-the-value-out-of-the-monad/
    class IO<T> {
        public IO(T input) {
            this.input = input;
        }

        private readonly T input;

        public IO<U> Bind<U>(Func<T, IO<U>> f) => f(input);
    }

    static class IOExtensions {
        public static IO<T> AsIO<T>(this T value) => new IO<T>(value);
    }

    class InputInfo {
        public InputInfo(string input, double time) {
            Text = input;
            Time = time;
        }

        public string Text { get; }
        public double Time { get; }
    }

    class Player {
        public const int MaxPlayers = 2;

        public Player(int index, bool defeated, InputInfo inputInfo) {
            Index = index;
            Defeated = defeated;
            InputInfo = inputInfo;
        }

        public int Index { get; }
        public bool Defeated { get; }
        public InputInfo InputInfo { get; }

        public static int Next(int index) => (index + 1) % MaxPlayers;
    }

    class State {
        public State() {
            Steps = ImmutableList.Create<Player>();
        }
        private State(State prev, Player newStep) {
            Steps = prev.Steps.Add(newStep);
        }

        public ImmutableList<Player> Steps { get; }
        public Player? LastStep => Steps.IsEmpty ? null : Steps.Last();
        public bool Over => LastStep?.Defeated ?? false;

        private static bool IsInputCompetent(InputInfo inputInfo, InputInfo prevInfo) =>
            inputInfo
            .Text
            .CharacterCounts()
            .SequenceEqual(
                prevInfo
                .Text
                .CharacterCounts());
        public State Advance(InputInfo input) => new State(this, Steps switch {
            _ when Steps.IsEmpty => new Player(0, false, input),
            _ => new Player(
                Player.Next(Steps.Last().Index),
                !IsInputCompetent(input, LastStep!.InputInfo),
                input),
        });
    }

    class Program {
        static void Main(string[] args) {
            IO<State> result = Loop(new State().AsIO()).Bind(
                st => st switch {
                    _ when st.Over => st.AsIO(),
                    _ => Loop(st.AsIO()),
                }
            );
        }
        static bool IsInputTextValid(string text) {
            return !string.IsNullOrWhiteSpace(text) && !text.Contains(' ') && text.Length >= 8 && text.Length <= 30;
        }
        static IO<State> Loop(IO<State> prevState) =>
            ReadUntilValidTimed(IsInputTextValid)
            .Bind(
                info => prevState.Bind(
                    state => state.Advance(info).AsIO()
                )
            );
        // Impure input functions
        static IO<InputInfo> ReadUntilValidTimed(Func<string, bool> textValidator) {
            double time = 0.0;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => time += 0.1;

            string text;
            do {
                timer.Start();
                text = Console.ReadLine() ?? "";
                timer.Stop();

            } while (!textValidator(text));

            return new InputInfo(text, time).AsIO();
        }
    }
}