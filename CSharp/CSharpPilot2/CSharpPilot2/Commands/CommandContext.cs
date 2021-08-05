﻿
using CSharpPilot2.Gameplay;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Commands
{
    // I hate to couple it with the Gameplay classes but I don't want to take another long detour.
    internal record CommandContext(Locale Locale, State State);
}
