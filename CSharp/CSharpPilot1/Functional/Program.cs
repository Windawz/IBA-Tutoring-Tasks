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
    class Unit {
        private Unit() { }
        public static readonly Unit Instance = new Unit();
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
    partial class IO<T> {
        public IO(T input) {
            this.input = input;
        }

        private readonly T input;

        public IO<U> Bind<U>(Func<T, IO<U>> f) => f(input);
    }

    #region IO Functions
    partial class IO {
        public static IO<InputInfo> ReadLineValidatedTimed(Func<string, bool> textValidator, double startTime) {
            double time = 0.0;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => time += 0.1;

            timer.Start();
            string text = Console.ReadLine() ?? "";
            timer.Stop();

            return new InputInfo(text, time, textValidator(text)).AsIO();
        }
        public static IO<Unit> Write(string format, params object[] arg) {
            Console.Write(format, arg);
            return new IO<Unit>(Unit.Instance);
        }
        public static IO<Unit> Write(char c) {
            Console.Write(c);
            return new IO<Unit>(Unit.Instance);
        }
        public static IO<Unit> WriteLine() {
            Console.WriteLine();
            return new IO<Unit>(Unit.Instance);
        }
        public static IO<Unit> WriteLine(string format, params object[] arg) {
            _ = Write(format, arg);
            return WriteLine();
        }
        public static IO<Unit> WriteLine(char c) {
            _ = Write(c);
            return WriteLine();
        }
    }
    #endregion

    static class IOExtensions {
        public static IO<T> AsIO<T>(this T value) => new IO<T>(value);
    }
    class InputInfo {
        public InputInfo(string text, double time, bool valid) =>
            (Text, Time, Valid) = (text, time, valid);

        public string Text { get; }
        public double Time { get; }
        public bool Valid { get; }
    }
    class Player {
        public const int MaxPlayers = 2;

        public Player(int index, bool defeated, InputInfo inputInfo) => (Index, Defeated, InputInfo) = (index, defeated, inputInfo);
        public Player(Player player) : this(player.Index, player.Defeated, player.InputInfo) { }

        public int Index { get; }
        public bool Defeated { get; }
        public InputInfo InputInfo { get; }
    }

    class State {
        public State(ImmutableList<Player> history) => History = history;
        public State() : this(ImmutableList.Create<Player>()) { }
        public State(State state) : this(state.History) { }


        public ImmutableList<Player> History { get; }
        public Player? Last => History.LastOrDefault();
        public bool Over => Last?.Defeated ?? false;

        public override string ToString() =>
            $"Steps: {History};\nLastStep: {Last?.ToString() ?? "null"};\nOver: {Over};\n";
    }
    static class Logic {
        public const int MaxPlayers = 2;
        public const double MaxSeconds = 10.0;
        public const int MinTextLength = 8;
        public const int MaxTextLength = 30;

        public static int NextPlayer(int index) => (index + 1) % MaxPlayers;
        public static bool IsInputTextValid(string text) =>
            !string.IsNullOrWhiteSpace(text) && !text.Contains(' ') && text.Length >= MinTextLength && text.Length <= MaxTextLength;
        public static Player AdvancePlayer(Player? last, InputInfo inputInfo) =>
            last switch {
                null => new Player(0, false, inputInfo),
                _ when !last.InputInfo.Valid => new Player(last.Index, last.Defeated, inputInfo),
                _ when last.Defeated => new Player(last),
                _ => new Player(
                    NextPlayer(last.Index),
                    !IsInputCompetentText(inputInfo, last.InputInfo) || !IsInputCompetentTime(inputInfo),
                    inputInfo),
            };
        public static State AdvanceState(State last, Player player) =>
            new State(last.History.Add(player));
        private static bool IsInputCompetentText(InputInfo inputInfo, InputInfo prevInfo) =>
            inputInfo
            .Text
            .CharacterCounts()
            .SequenceEqual(
                prevInfo
                .Text
                .CharacterCounts());
        private static bool IsInputCompetentTime(InputInfo inputInfo) =>
            inputInfo.Time <= MaxSeconds;

    }

    class Program {
        static void Main(string[] args) {
            IO<State> result = Loop(new State().AsIO());
            _ = result.Bind(st => { Console.WriteLine(st.ToString()); return st.AsIO(); });
        }
        // This is currently bugged and doesn't work right.
        static IO<State> Loop(IO<State> state) => state.Bind(
                st => st switch {
                    _ when st.Over => st.AsIO(),
                    _ => Loop(
                            IO.WriteLine(
                                GetInputRequestString(st.Last is null ? 0 : Logic.NextPlayer(st.Last.Index))
                            ).Bind(_ =>
                            (st.Last is null ? Unit.Instance.AsIO() : IO.WriteLine(
                                " " + GetTimeLeftString(GetTimeLeft(st)))
                            ).Bind(_ =>
                            IO.ReadLineValidatedTimed(
                                Logic.IsInputTextValid,
                                st.Last is null || st.Last.InputInfo.Valid ? 0.0 : Logic.MaxSeconds - st.Last.InputInfo.Time
                            ).Bind(inputInfo =>
                            (inputInfo.Valid ? Unit.Instance.AsIO() : IO.WriteLine(
                                GetInputRetryString() + " " + GetTimeLeftString(GetTimeLeft(st)))
                            ).Bind(_ =>
                            Logic.AdvancePlayer(st.Last, inputInfo).AsIO().Bind(p =>
                            Logic.AdvanceState(st, p).AsIO()
                        )))))),
                }
            );
        static double GetTimeLeft(State state) =>
            state.Last is null ? Logic.MaxSeconds : Logic.MaxSeconds - state.Last.InputInfo.Time;
        static string GetInputRequestString(int playerIndex) =>
            $"Игрок {playerIndex + 1}, введите слово:";
        static string GetInputRetryString() =>
            $"Неверный ввод. Попробуйте ещё раз:";
        static string GetTimeLeftString(double timeLeft) =>
            $"(осталось {timeLeft:F}с)";
        static string GetDefeatedString(State state) =>
            $"";
    }
}