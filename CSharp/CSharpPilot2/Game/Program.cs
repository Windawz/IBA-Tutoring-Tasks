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
            if (state.Over) {
                Console.WriteLine(GetDefeatedString(state));
                return state;
            } else if (state.Last is null) {
                var newPlayer = new Player(0, false);
                var inputInfo = GetInputInfo(newPlayer.Index, 0.0, GetInputRequestString(newPlayer.Index));
                var newStep = new Step(newPlayer, inputInfo);
                var newState = state.AddStep(newStep);
                return Play(newState);
            } else {
                int newPlayerIndex = Rules.NextPlayerIndex(state.Last.Player.Index);
                var inputInfo = GetInputInfo(newPlayerIndex, 0.0, GetInputRequestString(newPlayerIndex));
                bool defeated =
                    !Rules.IsInputCompetentText(inputInfo, state.Last.InputInfo) ||
                    !Rules.IsInputCompetentTime(inputInfo);
                var newPlayer = new Player(newPlayerIndex, defeated);
                var newStep = new Step(newPlayer, inputInfo);
                var newState = state.AddStep(newStep);
                return Play(newState);
            }
        }

        // Keeps on trying to get valid input from player.
        // Will execute over and over on failure.
        // Each attempt the time spent on input will carry over until the input passes.
        // 'startTime': how much time has been already taken by the player to enter input.
        // 'requestString': message to display to the player before requesting input.
        static InputInfo GetInputInfo(int playerIndex, double startTime, string requestString) {
            Console.WriteLine($"{requestString} {GetTimeLeftString(Rules.MaxSeconds - startTime)}");
            var inputInfo = ReadLineValidated(Rules.IsInputTextValid);
            double accumulatedTime = inputInfo.Time + startTime;
            if (!inputInfo.Valid) {
                return GetInputInfo(playerIndex, inputInfo.Time + startTime, GetInputRetryString());
            } else {
                return new InputInfo(inputInfo.Text, inputInfo.Time + startTime, inputInfo.Valid);
            }
        }
        // Reads user input, just like Console.ReadLine().
        // The input is validated using the passed function.
        // The validation result will be returned along with the rest of the input.
        static InputInfo ReadLineValidated(Func<string, bool> textValidator) {
            double time = 0.0;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => time += 0.1;

            timer.Start();
            string text = Console.ReadLine() ?? "";
            timer.Stop();

            return new InputInfo(text, time, textValidator(text));
        }
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