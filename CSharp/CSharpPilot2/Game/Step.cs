namespace Game {
    // The class that game history is composed of.
    // Each valid player action gets recorded in a step.
    class Step {
        public Step(Player player, InputInfo inputInfo) =>
            (Player, InputInfo) = (player, inputInfo);

        public Step(Step step) : this(step.Player, step.InputInfo) { }

        public Player Player { get; }
        public InputInfo InputInfo { get; }
    }
}