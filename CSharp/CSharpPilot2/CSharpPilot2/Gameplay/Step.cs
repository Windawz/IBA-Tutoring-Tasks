namespace CSharpPilot2.Gameplay
{
    // The type that game history is composed of.
    // Each valid player action gets recorded in a step.
    sealed record Step(Player Player, Word Word);
}
