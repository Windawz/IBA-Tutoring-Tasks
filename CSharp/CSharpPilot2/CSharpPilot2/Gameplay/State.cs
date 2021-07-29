using System;
using System.Collections.Generic;

namespace CSharpPilot2.Gameplay
{
    internal class State
    {
        public State() { }

        public List<Step> Steps { get; } = new();
    }
}
