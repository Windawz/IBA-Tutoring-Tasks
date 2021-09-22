using System;
using System.Collections.Generic;
using System.Linq;

using CSharpPilot2.IO;
using CSharpPilot2.IO.PerformerMods;

namespace CSharpPilot2.Gameplay
{
    sealed class Game : Module
    {
        public Game(App app, GameRules rules) : base(app)
        {
            _rules = rules;
            _historyLogger = new(_history);
            _players = new Player[rules.PlayerCount];

            // Add retry functionality to app performer.
            app.Performer = new RetryingMod(app.Performer);

            // Keep the app-provided performer.
            _originalPerformer = app.Performer;
            // Used after the first word was entered.
            _timingMod = new TimingMod(_originalPerformer, spent => app.Locale.SecondsLeftSuffix(rules.MaxWordSeconds - spent));
            // Mutable field for local performer modification.
            _currentPerformer = _originalPerformer;
        }

        private readonly GameRules _rules;
        private readonly List<Step> _history = new();
        private readonly HistoryLogger _historyLogger;
        private readonly Player[] _players;
        private readonly IPerformer _originalPerformer;
        private readonly TimingMod _timingMod;

        private IPerformer _currentPerformer;
        private int _curPlayerIndex = 0;

        public IReadOnlyList<Step> History =>
            _history;

        protected override void InitImpl()
        {
            GameRequestMaker requestMaker = GetRequestMaker();

            requestMaker.DisplayIntro();
            requestMaker.RequestAnyKey();

            for (int i = 0; i < _players.Length; i++)
            {
                Input input = requestMaker.RequestName(i + 1);
                _players[i] = new Player(input.Text);
            }
        }
        protected override void ActImpl()
        {
            GameRequestMaker requestMaker = GetRequestMaker();

            Step? lastStep = _history.LastOrDefault();
            Player curPlayer = _players[_curPlayerIndex];

            Input input = requestMaker.RequestWord(curPlayer.Name);
            _historyLogger.LogPlayerInput(curPlayer, input);

            if (lastStep is not null && !CheckWordCompetence(input, lastStep.Input))
            {
                App.Exit = true;
            }
            else
            {
                AdvancePlayerIndex();
                _currentPerformer = _timingMod;
            }
        }
        protected override void FinishImpl()
        {
            _currentPerformer = _originalPerformer;

            GameRequestMaker requestMaker = GetRequestMaker();

            Step prev = _history.SkipLast(1).Last();
            Step cur = _history.Last();

            requestMaker.DisplayGameOver(cur.Player.Name, prev.Input, cur.Input);
            requestMaker.RequestAnyKey();
        }
        private bool CheckWordCompetence(Input word, Input lastWord)
        {
            static IEnumerable<(char Char, int Count)> PrepareForComparison(string str) =>
                str.ToLowerInvariant().CharacterCounts();

            return 
                word.Seconds <= _rules.MaxWordSeconds
                && word.Text != lastWord.Text
                && PrepareForComparison(word.Text).SequenceEqual(PrepareForComparison(lastWord.Text));
        }
        private void AdvancePlayerIndex() =>
            _curPlayerIndex = (_curPlayerIndex + 1) % _rules.PlayerCount;
        private GameRequestMaker GetRequestMaker() =>
            new(_currentPerformer, _rules, App.Locale);
    }
}
