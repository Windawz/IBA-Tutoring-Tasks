using System;
using System.Linq;
using System.Timers;

namespace Game {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine(GetIntroductionString());
            Console.ReadKey(intercept: true);
            State finalState = Play(new State());
        }

        // Main game logic.
        // Recursive.
        // Returns the final state of the game once it has played out to the end.
        static State Play(State state) {
            if (state.IsOver) {
                Console.WriteLine(GetDefeatedString(state));

                return state;
            } else if (state.LastStep is null) {
                var player = new Player(0, false);
                var inputInfo = GetInputInfo(player.Index, 0.0, GetInputRequestString(player.Index));
                var step = new Step(player, inputInfo);
                var newState = state.AddStep(step);

                return Play(newState);
            } else {
                int playerIndex = Rules.NextPlayerIndex(state.LastStep.Player.Index);
                var inputInfo = GetInputInfo(playerIndex, 0.0, GetInputRequestString(playerIndex));
                bool isDefeated =
                    !Rules.IsInputCompetentText(inputInfo, state.LastStep.InputInfo) ||
                    !Rules.IsInputCompetentTime(inputInfo);
                var player = new Player(playerIndex, isDefeated);
                var step = new Step(player, inputInfo);
                var newState = state.AddStep(step);

                return Play(newState);
            }
        }

        // Keeps on trying to get valid input from player.
        // Will execute over and over on failure.
        // Each attempt the time spent on input will carry over until the input passes.
        // 'startTime': how much time has been already taken by the player to enter input.
        // 'requestString': message to display to the player before requesting input.
        static InputInfo GetInputInfo(int playerIndex, double startSeconds, string requestString) {
            Console.WriteLine($"{requestString} {GetTimeLeftString(Rules.MaxSeconds - startSeconds)}");

            var inputInfo = Dialogue.ReadLineValidated(Rules.IsInputTextValid);
            double accumulated = inputInfo.Seconds + startSeconds;

            if (!inputInfo.IsValid) {
                return GetInputInfo(playerIndex, accumulated, GetInputRetryString());
            } else {
                return inputInfo with { Seconds = accumulated };
            }
        }
        static string GetInputRequestString(int playerIndex) =>
            $"Игрок {playerIndex + 1}, введите слово:";
        static string GetInputRetryString() =>
            $"Неверный ввод. Попробуйте ещё раз:";
        static string GetTimeLeftString(double timeLeft) =>
            timeLeft <= 0.0 ? $"(время вышло)" : $"(осталось {timeLeft:F}с)";
        static string GetDefeatedString(State state) {
            string nl = Environment.NewLine;
            Step? lastStep = state.LastStep;
            Step? prevStep = state.History.SkipLast(1).LastOrDefault();

            return $"{nl}Игрок {state.LastStep!.Player.Index + 1} проиграл!{nl}{nl}" +
            $"Пред. слово: \"{prevStep?.InputInfo.Text ?? ""}\"{nl}" +
            $"Пред. время: {prevStep?.InputInfo.Seconds.ToString("F") ?? ""}с{nl}" +
            $"Слово: \"{lastStep?.InputInfo.Text ?? ""}\"{nl}" +
            $"Время: {lastStep?.InputInfo.Seconds.ToString("F") ?? ""}с";
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