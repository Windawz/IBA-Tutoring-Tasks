using System.Linq;
using System.Collections.Generic;

namespace Game.Game {
    class State {
        public State() { }

        public List<Step> History { get; } = new();
        public bool IsOver => History.LastOrDefault()?.Player.IsDefeated ?? false;
    }
}