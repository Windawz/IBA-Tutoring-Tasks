using System;
using System.Collections.Generic;

namespace CSharpPilot2.Gameplay
{
    internal class State
    {
        public State(Rules rules)
        {
            Rules = rules;
            Players = new Player[rules.Properties.PlayerCount];
        }

        public List<Step> Steps { get; } = new();
        public Player[] Players { get; }
        public Rules Rules { get; }
    }
}
