using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal record ParsedCommand(string Name, IDictionary<string, string[]> Parameters);
}
