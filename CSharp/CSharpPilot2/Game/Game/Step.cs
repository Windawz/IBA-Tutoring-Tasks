using CSharpPilot2.Input;

namespace CSharpPilot2.Game {
    // The type that game history is composed of.
    // Each valid player action gets recorded in a step.
    record Step(Player Player, InputInfo InputInfo);
}