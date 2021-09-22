using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpPilot2.Gameplay
{
    sealed record GameRules
    {
        private GameRules() { }

        public int PlayerCount { get; init; }
        public double MaxWordSeconds { get; init; }
        public int MinWordLength { get; init; }
        public int MaxWordLength { get; init; }

        public static GameRules Default => new()
        { 
            PlayerCount = 2,
            MaxWordSeconds = 10.0,
            MinWordLength = 8,
            MaxWordLength = 30,
        };
    }
}
