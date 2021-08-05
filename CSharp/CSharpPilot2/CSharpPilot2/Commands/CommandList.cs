using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal class CommandList
    {
        public CommandList(Dictionary<string, ParsedCommand> commands) =>
            Commands = commands;

        public Dictionary<string, ParsedCommand> Commands { get; }
    }
}
