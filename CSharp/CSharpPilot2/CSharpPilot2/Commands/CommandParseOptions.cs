using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal record CommandParseOptions(string Prefix, string[] ParameterDelimiters)
    {
        public static bool TryParseCommand(string command, CommandParseOptions options, out CommandTemplate template)
        {
            
        }
    }
}
