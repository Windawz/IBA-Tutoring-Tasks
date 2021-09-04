using System.Collections.Generic;

namespace CSharpPilot2.Commands
{
    internal sealed record ParsedCommand(string Name, IDictionary<string, ParsedParameter> Parameters);
}
