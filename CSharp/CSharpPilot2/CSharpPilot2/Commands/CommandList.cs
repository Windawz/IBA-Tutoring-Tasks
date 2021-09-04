﻿using System.Collections.Generic;


namespace CSharpPilot2.Commands
{
    using CommandsT = IDictionary<string, CommandInfo>;

    sealed class CommandList
    {
        public CommandList(CommandsT commands) =>
            Commands = commands;

        public CommandsT Commands { get; }
    }
}
