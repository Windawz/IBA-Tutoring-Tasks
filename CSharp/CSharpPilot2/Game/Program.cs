using System;
using System.Linq;

namespace Game {

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