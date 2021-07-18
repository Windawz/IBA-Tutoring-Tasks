using System;
using System.Linq;
using System.Timers;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharpPilot1.Functional {
    // Convenience extension methods for implementing word comparison logic.
    static class StringExtensions {
        public static int CharacterCount(this string str, char c) =>
            str.Where(x => x == c).Count();

        // Returns a sequence of pairs.
        // Each pair represents a character and the amount of its occurences in the string.
        public static IEnumerable<(char Char, int Count)> CharacterCounts(this string str) =>
            str.Select(c => (Char: c, Count: str.CharacterCount(c))).OrderBy(x => x.Char);
    }

    // A dummy type used in functional programming. Similar to 'void'
    // In functional programming, functions always return something.
    // If the return value is not needed or can't be used, it can be discarded (_ = *something*).
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
    // Since we're working with IO, which is impure, we isolate it into a monad.
    //
    // It's better explained in the following articles:
    //  https://mikhail.io/2018/07/monads-explained-in-csharp-again/
    //  https://blog.ploeh.dk/2019/02/04/how-to-get-the-value-out-of-the-monad/
    partial class IO<T> {
        public IO(T contents) {
            this.contents = contents;
        }

        private readonly T contents;

        // Transforms the contents inside the monad using the function, then returns a new monad with the result.
        public IO<U> Bind<U>(Func<T, IO<U>> f) => f(contents);
    }

    #region IO Functions
    // Wrappers around the standard IO functions that utilize the monad.
    partial class IO {
        public static IO<ConsoleKeyInfo> ReadKey(bool intercept = false) {
            return Console.ReadKey(intercept).AsIO();
        }

        // Reads user input, just like Console.ReadLine().
        // The input is validated using the passed function.
        // The validation result will be returned along with the rest of the input.
        public static IO<InputInfo> ReadLineValidated(Func<string, bool> textValidator) {
            double time = 0.0;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => time += 0.1;

            timer.Start();
            string text = Console.ReadLine() ?? "";
            timer.Stop();

            return new InputInfo(text, time, textValidator(text)).AsIO();
        }

        // Analogous to Console.Write().
        public static IO<Unit> Write(string format, params object[] arg) {
            Console.Write(format, arg);
            return new IO<Unit>(Unit.Instance);
        }

        // Analogous to Console.Write().
        public static IO<Unit> Write(char c) {
            Console.Write(c);
            return new IO<Unit>(Unit.Instance);
        }

        // Analogous to Console.WriteLine().
        public static IO<Unit> WriteLine() {
            Console.WriteLine();
            return new IO<Unit>(Unit.Instance);
        }

        // Analogous to Console.WriteLine().
        public static IO<Unit> WriteLine(string format, params object[] arg) {
            _ = Write(format, arg);
            return WriteLine();
        }

        // Analogous to Console.WriteLine().
        public static IO<Unit> WriteLine(char c) {
            _ = Write(c);
            return WriteLine();
        }
    }
    #endregion

    static class IOExtensions {
        // A helper extension method for better chaining while using the IO monad.
        public static IO<T> AsIO<T>(this T value) => new IO<T>(value);
    }

    class InputInfo {
        public InputInfo(string text, double time, bool valid) =>
            (Text, Time, Valid) = (text, time, valid);

        public string Text { get; }
        // In seconds.
        public double Time { get; }
        public bool Valid { get; }
    }

    class Player {
        public Player(int index, bool defeated) => (Index, Defeated) = (index, defeated);
        public Player(Player player) : this(player.Index, player.Defeated) { }

        public int Index { get; }
        public bool Defeated { get; }
    }

    // The class that game history is composed of.
    // Each valid player action gets recorded in a step.
    class Step {
        public Step(Player player, InputInfo inputInfo) =>
            (Player, InputInfo) = (player, inputInfo);

        public Step(Step step) : this(step.Player, step.InputInfo) { }

        public Player Player { get; }
        public InputInfo InputInfo { get; }
    }

    class State {
        public State(ImmutableList<Step> history) => History = history;
        public State() : this(ImmutableList.Create<Step>()) { }
        public State(State state) : this(state.History) { }


        public ImmutableList<Step> History { get; }
        public Step? Last => History.LastOrDefault();
        public bool Over => Last?.Player.Defeated ?? false;

        public State AddStep(Step step) =>
            new State(History.Add(step));
    }

    static class Rules {
        // Amount of players involved.
        public const int MaxPlayers = 2;
        // Max amount of seconds to enter a word before player loses.
        public const double MaxSeconds = 10.0;
        public const int MinTextLength = 8;
        public const int MaxTextLength = 30;

        public static int NextPlayer(int index) => (index + 1) % MaxPlayers;
        public static bool IsInputTextValid(string text) =>
            !string.IsNullOrWhiteSpace(text) && !text.Contains(' ') && text.Length >= MinTextLength && text.Length <= MaxTextLength;

        // Checks if the input is competent (won't lose) compared to the previous input in terms of text.
        public static bool IsInputCompetentText(InputInfo inputInfo, InputInfo prevInfo) =>
        inputInfo
        .Text
        .ToLowerInvariant()
        .CharacterCounts()
        .SequenceEqual(
            prevInfo
            .Text
            .ToLowerInvariant()
            .CharacterCounts()) && !string.Equals(inputInfo.Text, prevInfo.Text, StringComparison.InvariantCultureIgnoreCase);

        // Checks if the input is competent (won't lose) compared to the previous input in terms of time taken.
        public static bool IsInputCompetentTime(InputInfo inputInfo) =>
            inputInfo.Time <= MaxSeconds;
    }

    class Program {
        static void Main(string[] args) {
            _ = IO.WriteLine(GetIntroductionString());
            _ = IO.ReadKey(intercept: true);
            IO<State> result = Loop(new State());
        }

        // The main game loop.
        // Located in a separate function to allow recursion - the only truly functional way of looping something.
        static IO<State> Loop(State state) => state switch {
            _ when state.Over => IO.WriteLine(GetDefeatedString(state)).Bind(_ =>
                state.AsIO()),
            _ when state.Last is null =>
                new Player(0, false).AsIO().Bind(p =>
                GetInputInfo(p.Index, 0.0, GetInputRequestString(p.Index)).Bind(inputInfo =>
                Loop(state.AddStep(new Step(p, inputInfo)))
                )),
            _ =>
                new Step(state.Last).AsIO().Bind(last =>
                GetInputInfo(
                    Rules.NextPlayer(last.Player.Index),
                    0.0,
                    GetInputRequestString(
                        Rules.NextPlayer(last.Player.Index)
                    )
                ).Bind(inputInfo =>
                new Player(
                    Rules.NextPlayer(last.Player.Index),
                    !Rules.IsInputCompetentText(inputInfo, last.InputInfo) || !Rules.IsInputCompetentTime(inputInfo)
                ).AsIO().Bind(p =>
                Loop(state.AddStep(new Step(p, inputInfo)))
                ))),
        };

        // Keeps on trying to get valid input from player.
        // Will execute over and over on failure.
        // Each attempt the time spent on input will carry over until the input passes.
        // 'startTime': how much time has been already taken by the player to enter input.
        // 'requestString': message to display to the player before requesting input.
        static IO<InputInfo> GetInputInfo(int playerIndex, double startTime, string requestString) =>
            IO.WriteLine($"{requestString} {GetTimeLeftString(Rules.MaxSeconds - startTime)}").Bind(_ =>
            IO.ReadLineValidated(Rules.IsInputTextValid).Bind(inputInfo =>
            inputInfo switch {
                _ when !inputInfo.Valid => GetInputInfo(playerIndex, inputInfo.Time + startTime, GetInputRetryString()),
                _ => new InputInfo(inputInfo.Text, inputInfo.Time + startTime, inputInfo.Valid).AsIO(),
            }
            ));
        static string GetInputRequestString(int playerIndex) =>
            $"Игрок {playerIndex + 1}, введите слово:";
        static string GetInputRetryString() =>
            $"Неверный ввод. Попробуйте ещё раз:";
        static string GetTimeLeftString(double timeLeft) =>
            timeLeft <= 0.0 ? $"(время вышло)" : $"(осталось {timeLeft:F}с)";
        static string GetDefeatedString(State state) {
            string nl = Environment.NewLine;
            Step? lastStep = state.Last;
            Step? prevStep = state.History.SkipLast(1).LastOrDefault();

            return $"{nl}Игрок {state.Last!.Player.Index + 1} проиграл!{nl}{nl}" +
            $"Пред. слово: \"{prevStep?.InputInfo.Text ?? ""}\"{nl}" +
            $"Пред. время: {prevStep?.InputInfo.Time.ToString("F") ?? ""}с{nl}" +
            $"Слово: \"{lastStep?.InputInfo.Text ?? ""}\"{nl}" +
            $"Время: {lastStep?.InputInfo.Time.ToString("F") ?? ""}с";
        }
        static string GetIntroductionString() {
            string nl = Environment.NewLine;
            return $"Добро пожаловать в \"Игру в \"Слова\". Правила игры таковы:{nl}{nl}" +
                $"- {Rules.MaxPlayers} игрока(ов) поочерёдно вводят слова, состоящие из букв предыдущего.{nl}" +
                $"- Введённое слово должно отличаться от первоначального.{nl}" +
                $"- Первый игрок получает карт-бланш.{nl}" +
                $"- Если слово длиной меньше {Rules.MinTextLength} или больше {Rules.MaxTextLength} символов, придётся повторить ввод.{nl}" +
                $"- На ввод даётся {Rules.MaxSeconds} секунд.{nl}" +
                $"- При повторном вводе время не восстанавливается.{nl}{nl}" +
                $"Нажмите любую клавишу, чтобы начать...{nl}";
        }
    }
}