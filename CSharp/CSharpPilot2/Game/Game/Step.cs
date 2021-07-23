using Game.Input;

namespace Game.Game {
    // The type that game history is composed of.
    // Each valid player action gets recorded in a step.
    record Step(Player Player, InputInfo InputInfo);
}