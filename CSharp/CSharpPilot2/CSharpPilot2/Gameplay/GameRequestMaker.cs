using System;
using System.Collections.Generic;

using CSharpPilot2.IO;
using CSharpPilot2.IO.PerformerMods;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay
{
    /// <summary>
    /// Used by <see cref="Game"/> for making common requests.
    /// </summary>
    sealed class GameRequestMaker
    {
        public GameRequestMaker(IPerformer performer, GameRules rules, Locale locale)
        {
            _performer = performer;
            _rules = rules;
            _locale = locale;
        }

        private readonly IPerformer _performer;
        private readonly GameRules _rules;
        private readonly Locale _locale;

        public Input RequestName(int playerNumber)
        {
            Request r = new()
            {
                Before = new SayDo(new Output(_locale.NameRequest(playerNumber))),
                Condition = i => !String.IsNullOrWhiteSpace(i.Text),
                MatchedNot = new SayDo<Input>(new Output(_locale.ErrorInvalidName())),
            };

            return _performer.Perform(r);
        }
        public Input RequestWord(string playerName)
        {
            Request r = new()
            {
                Before = new SayDo(new Output(_locale.WordRequest(playerName))),
                Condition = i =>
                    !String.IsNullOrWhiteSpace(i.Text)
                    && i.Text.Length >= _rules.MinWordLength
                    && i.Text.Length <= _rules.MaxWordSeconds,
                MatchedNot = new SayDo<Input>(new Output(_locale.ErrorInvalidInput())),
            };

            return _performer.Perform(r);
        }
        public Input RequestAnyKey()
        {
            Request r = new()
            {
                Before = new SayDo(new Output(_locale.AnyKeyRequest())),
            };

            ReadModeMod anyKeyPerfomer = new(_performer, InputSourceReadMode.Key);
            return anyKeyPerfomer.Perform(r);
        }
        public void DisplayIntro()
        {
            Request r = new()
            {
                Before = new SayDo(new Output(_locale.Intro(_rules.PlayerCount, _rules.MinWordLength, _rules.MaxWordLength, _rules.MaxWordSeconds))),
            };

            new ReadModeMod(_performer, InputSourceReadMode.Skip).Perform(r);
        }
        public void DisplayGameOver(string loserName, Input prev, Input cur)
        {
            Request r = new()
            {
                Before = new SayDo(new Output(_locale.GameOver(loserName, prev.Text, prev.Seconds, cur.Text, cur.Seconds))),
            };

            new ReadModeMod(_performer, InputSourceReadMode.Skip).Perform(r);
        }
    }
}
