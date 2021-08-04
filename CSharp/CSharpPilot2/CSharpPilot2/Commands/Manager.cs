using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal class Manager
    {
        public Manager(Context context) =>
            _context = context;

        private readonly Context _context;

        public ExecutionResult Execute(string command)
        {
            
        }
    
        private static CommandTemplate ParseCommandTemplate(string command, ParseOptions options)
        {
            if (!command.StartsWith(options.CommandPrefix))
            {
                throw new ArgumentException($"Command string doesn't start with {options.CommandPrefix}", nameof(command));
            }

            var tokens = command.Split(
                options.Delimiters,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            );

            if (!tokens.Any())
            {
                throw new ArgumentException("Command string has no tokens", nameof(command));
            }

            var                         paramTokens     = tokens.Skip(1);
            string                      commandName     = tokens.First();
            string?                     lastParam       = null;
            string?                     curParam        = lastParam;
            List<string>                args            = new();
            List<ParameterTemplate>     paramTemplates  = new();

            foreach (string token in paramTokens)
            {
                if (token.StartsWith(options.ParameterPrefix))
                {
                    curParam = token;
                }
                else
                {
                    args.Add(token);
                }

                // If curParam has changed or token is the last one left:
                if (curParam != lastParam || token == paramTokens.Last())
                {
                    if (curParam is not null)
                    {
                        paramTemplates.Add(new ParameterTemplate(curParam[1..], args.ToArray()));
                    }
                    args.Clear();
                }

                lastParam = curParam;
            }

            return new CommandTemplate(commandName, paramTemplates.ToArray());
        }
    }
}
