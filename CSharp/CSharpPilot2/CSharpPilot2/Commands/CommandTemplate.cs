using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal record CommandTemplate(string Name, ParameterTemplate[] Parameters)
    {
        public static bool TryParseCommand(string command, CommandParseOptions options, out CommandTemplate template)
        {

        }
    }
}
