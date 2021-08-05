using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSharpPilot2.Commands
{
    using CommandsT = Dictionary<string, CommandInfo>;

    internal class CommandList
    {
        public CommandList(CommandsT commands) =>
            Commands = commands;

        public CommandsT Commands { get; }
    }
}
