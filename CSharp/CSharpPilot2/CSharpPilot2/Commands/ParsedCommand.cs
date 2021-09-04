using System.Collections.Generic;

namespace CSharpPilot2.Commands
{
    sealed record ParsedCommand(string Name, IDictionary<string, ParsedParameter> Parameters);
}
