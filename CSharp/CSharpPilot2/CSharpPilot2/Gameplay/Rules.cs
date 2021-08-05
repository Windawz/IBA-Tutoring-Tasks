using CSharpPilot2.Input;

namespace CSharpPilot2.Gameplay
{
    internal record Rules(RulesProperties Properties, WordValidator WordValidator, InputValidator InputValidator, InputForgiver InputForgiver);
}
