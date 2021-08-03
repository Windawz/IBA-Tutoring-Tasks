using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal record CommandTemplate(string Name, ParameterTemplate[] Parameters)
    {
        /// <exception cref="ArgumentException"></exception>
        public static CommandTemplate ParseCommand(string command, CommandParseOptions options)
        {
            if (String.IsNullOrEmpty(command))
            {
                throw new ArgumentException($"Command string is empty", nameof(command));
            }
            if (!command.StartsWith(options.Prefix))
            {
                throw new ArgumentException($"Command string doesn't start with {options.Prefix}", nameof(command));
            }


        }
    }
}
