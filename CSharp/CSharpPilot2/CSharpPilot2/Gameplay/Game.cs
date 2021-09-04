using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpPilot2.Commands;
using CSharpPilot2.IO;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay
{
    internal class Game
    {
        public Game(IInputSource inputSource, Rules rules, Locale locale, CommandOptions commandOptions)
        {
            _locale = locale;
            _state = new State(rules);

            CommandManager commandManager = new(GetCommandContext(), commandOptions);

            RequesterParams requesterParams = new(
                inputSource,
                new ConsoleOutputTarget(),
                _ => false, // placeholder
                _ => { },
                i => rules.InputValidator(i),
                t => locale.GetTimeLeftSuffixString(t)
            );

            _requester = new Requester(requesterParams);
        }

        private readonly Locale _locale;
        private readonly Requester _requester;
        private readonly State _state;

        public IReadOnlyList<Step> Steps =>
            _state.Steps;

        public void Start()
        {
            PrePlay();
            Play();
            PostPlay();
        }

        private void PrePlay()
        {
            Console.WriteLine(GetIntroString());
            _requester.Mode = RequesterMode.Default;
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
                if (prevWord is not null) {
                    _requester.Mode = RequesterMode.Timed;
                }

                Word word = RequestWord(currentPlayer.Name);

                _state.Steps.Add(new Step(currentPlayer, word));

                if (prevWord is not null && !_state.Rules.WordValidator(word, prevWord))
                {
                    loser = currentPlayer;
                    isPlaying = false;
                }
                currentPlayer = GetNextPlayer(currentPlayer);
            }
        }
        private void PostPlay() {
            _requester.Mode = RequesterMode.Default;
            Console.WriteLine(GetEndGameStatsString());
            RequestAnyKey();
        }
        private CommandContext GetCommandContext() => 
            new CommandContext(_locale, _state);
        private void CreatePlayers()
        {
            for (int i = 0; i < _state.Rules.Properties.PlayerCount; i++)
            {
                _state.Players[i] = new Player(Index: i, Name: RequestName(i));
            }
        }
        private string RequestName(int playerIndex) =>
            _requester.RequestName(_locale.GetNameRequestString(playerIndex));
        private Word RequestWord(string playerName) =>
            _requester.RequestWord(_locale.GetWordRequestString(playerName), _locale.GetInvalidInputString());
        private void RequestAnyKey() =>
            _requester.RequestAnyKey(_locale.GetPressAnyKeyToContinueString());
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
