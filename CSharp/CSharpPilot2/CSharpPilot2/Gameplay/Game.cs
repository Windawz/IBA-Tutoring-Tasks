using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpPilot2.Input;

namespace CSharpPilot2.Gameplay
{
    internal class Game
    {
        public Game(InputSource inputSource, Rules rules, Locale locale)
        {
            _inputSource        = inputSource;
            _rules              = rules;
            _locale             = locale;
            _requestProvider    = new RequestProvider(_rules, _locale);
            _players            = new Player[_rules.Properties.PlayerCount];
        }

        private readonly InputSource        _inputSource;
        private readonly Rules              _rules;
        private readonly Locale             _locale;
        private readonly RequestProvider    _requestProvider;
        private readonly State              _state = new();
        private readonly Player[]           _players;

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
            Player currentPlayer = _players[0];
            Player loser;

            while (isPlaying)
            {
                Word? prevWord = _state.Steps.LastOrDefault()?.Word;

                Word word = RequestWord(currentPlayer);
                _state.Steps.Add(new Step(currentPlayer, word));

                if (prevWord is not null && !_rules.WordValidator(word, prevWord))
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
            for (int i = 0; i < _rules.Properties.PlayerCount; i++) {
                string name = _requestProvider
                    .GetNameRequest(playerIndex: i)
                    .Perform(_inputSource)
                    .Text;

                _players[i] = new Player(Index: i, Name: name);
            }
        }
        private Word RequestWord(Player requestingPlayer)
        {
            Request request = _requestProvider.GetWordRequest(requestingPlayer);
            InputInfo inputInfo = request.Perform(_inputSource);
            return new Word(inputInfo.Text, inputInfo.Seconds);
        }
        private void RequestAnyKey()
        {
            Console.WriteLine(_locale.GetPressAnyKeyToContinueString());
            Console.ReadKey(intercept: true);
        }
        private Player GetNextPlayer(Player current) => 
            _players[(current.Index + 1) % _players.Length];
        private string GetEndGameStatsString()
        {
            var twoLastSteps = _state.Steps.TakeLast(2);
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
            var ruleStrings = new string[6]
            {
                _locale.GetIntroRuleFirstString(_rules.Properties.PlayerCount),
                _locale.GetIntroRuleSecondString(),
                _locale.GetIntroRuleThirdString(),
                _locale.GetIntroRuleFourthString(_rules.Properties.MinWordTextLength, _rules.Properties.MaxWordTextLength),
                _locale.GetIntroRuleFifthString(_rules.Properties.MaxWordSeconds),
                _locale.GetIntroRuleSixthString()
            };

            StringBuilder sb = new();
            sb.Append(_locale.GetIntroTitleString())
                .Append(' ')
                .AppendLine(_locale.GetIntroRulesTitleString());

            foreach (var str in ruleStrings)
            {
                sb.Append(bullet).AppendLine(str);
            }

            return sb.ToString();
        }
    }
}
