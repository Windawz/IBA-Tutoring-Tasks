using System.Collections.Generic;

namespace CSharpPilot2.Commands
{
    internal record ParsedCommand(string Name, IDictionary<string, ParsedParameter> Parameters);
}
