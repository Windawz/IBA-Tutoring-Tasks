using System.Collections.Generic;


namespace CSharpPilot2.Commands
{
    using CommandsT = IDictionary<string, CommandInfo>;

    internal class CommandList
    {
        public CommandList(CommandsT commands) =>
            Commands = commands;

        public CommandsT Commands { get; }
    }
}
