using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay
{
    internal class Game
    {
        public Game(InputSource inputSource, Rules rules, Locale locale)
        {
            _inputSource = inputSource;
            _locale = locale;
            _state = new State(rules);
            _requestProvider = new RequestProvider(_state.Rules, _locale);
        }

        private readonly InputSource _inputSource;
        private readonly Locale _locale;
        private readonly RequestProvider _requestProvider;
        private readonly State _state;

        public IReadOnlyList<Step> Steps =>
            _state.Steps;

        public void Start()
        {
            Initialize();
            Play();
        }

        private void Initialize()
        {
            Console.WriteLine(GetIntroString());
            RequestAnyKey();
            CreatePlayers();
        }
        private void Play()
        {
            bool isPlaying = true;
            Player currentPlayer = _state.Players[0];
            Player loser;

            while (isPlaying)
            {
                Word? prevWord = _state.Steps.LastOrDefault()?.Word;

                Word word = prevWord is null ?
                    RequestWord(currentPlayer) : RequestWordTimed(currentPlayer);

                _state.Steps.Add(new Step(currentPlayer, word));

                if (prevWord is not null && !_state.Rules.WordValidator(word, prevWord))
                {
                    loser = currentPlayer;
                    isPlaying = false;
                }
                currentPlayer = GetNextPlayer(currentPlayer);
            }

            Console.WriteLine(GetEndGameStatsString());
            RequestAnyKey();
        }
        private void CreatePlayers()
        {
            for (int i = 0; i < _state.Rules.Properties.PlayerCount; i++)
            {
                string name = _requestProvider
                    .GetNameRequest(playerIndex: i)
                    .Perform(_inputSource)
                    .Text;

                _state.Players[i] = new Player(Index: i, Name: name);
            }
        }
        private Word RequestWordTimed(Player requestingPlayer)
        {
            TimeTrackingRequest request = _requestProvider.GetTimeTrackingWordRequest(requestingPlayer);
            InputInfo inputInfo = request.Perform(_inputSource);
            return new Word(inputInfo.Text, inputInfo.Seconds);
        }
        private Word RequestWord(Player requestingPlayer)
        {
            ValidatedRequest request = _requestProvider.GetWordRequest(requestingPlayer);
            InputInfo inputInfo = request.Perform(_inputSource);
            return new Word(inputInfo.Text, inputInfo.Seconds);
        }
        private void RequestAnyKey()
        {
            Console.WriteLine(_locale.GetPressAnyKeyToContinueString());
            Console.ReadKey(intercept: true);
        }
        private Player GetNextPlayer(Player current) =>
            _state.Players[(current.Index + 1) % _state.Players.Length];
        private string GetEndGameStatsString()
        {
            IEnumerable<Step>? twoLastSteps = _state.Steps.TakeLast(2);
            Step curStep = twoLastSteps.Last();
            Step prevStep = twoLastSteps.SkipLast(1).Last();

            StringBuilder sb = new();
            sb.AppendLine()
                .AppendLine($"{_locale.GetGameOverLoserString(curStep.Player.Name)}")
                .AppendLine()
                .AppendLine($"{_locale.GetGameOverPreviousWordString(prevStep.Word.Text, prevStep.Word.Seconds)}")
                .AppendLine($"{_locale.GetGameOverCurrentWordString(curStep.Word.Text, curStep.Word.Seconds)}");

            return sb.ToString();
        }
        private string GetIntroString()
        {
            string bullet = "- ";
            string[]? ruleStrings = new string[6]
            {
                _locale.GetIntroRuleFirstString(_state.Rules.Properties.PlayerCount),
                _locale.GetIntroRuleSecondString(),
                _locale.GetIntroRuleThirdString(),
                _locale.GetIntroRuleFourthString(_state.Rules.Properties.MinWordTextLength, _state.Rules.Properties.MaxWordTextLength),
                _locale.GetIntroRuleFifthString(_state.Rules.Properties.MaxWordSeconds),
                _locale.GetIntroRuleSixthString()
            };

            StringBuilder sb = new();
            sb.Append(_locale.GetIntroTitleString())
                .Append(' ')
                .AppendLine(_locale.GetIntroRulesTitleString());

            foreach (string? str in ruleStrings)
            {
                sb.Append(bullet).AppendLine(str);
            }

            return sb.ToString();
        }
    }
}
