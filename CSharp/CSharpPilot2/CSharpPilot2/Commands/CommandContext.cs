
using CSharpPilot2.LegacyGameplay;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Commands
{
    // I hate to couple it with the Gameplay classes but I don't want to take another long detour.
    sealed record CommandContext(Locale Locale, State State);
}
