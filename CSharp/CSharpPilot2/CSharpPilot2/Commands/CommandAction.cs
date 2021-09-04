using System.Collections.Generic;

namespace CSharpPilot2.Commands
{
    delegate ExecutionResult CommandAction(CommandContext context, IDictionary<string, ParsedParameter> parameters);
}
