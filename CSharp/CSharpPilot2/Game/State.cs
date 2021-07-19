using System.Linq;
using System.Collections.Immutable;

namespace Game {
    class State {
        public State(ImmutableList<Step> history) => History = history;
        public State() : this(ImmutableList.Create<Step>()) { }
        public State(State state) : this(state.History) { }


        public ImmutableList<Step> History { get; }
        public Step? Last => History.LastOrDefault();
        public bool IsOver => Last?.Player.IsDefeated ?? false;

        public State AddStep(Step step) =>
            new State(History.Add(step));
    }
}