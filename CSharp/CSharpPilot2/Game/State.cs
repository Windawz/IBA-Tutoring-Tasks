using System.Linq;
using System.Collections.Immutable;

namespace Game {
    class State {
        public State(ImmutableList<Step> history) => History = history;
        public State() : this(ImmutableList.Create<Step>()) { }

        public ImmutableList<Step> History { get; }
        public Step? LastStep => History.LastOrDefault();
        public bool IsOver => LastStep?.Player.IsDefeated ?? false;

        public State AddStep(Step step) =>
            new State(History.Add(step));
    }
}