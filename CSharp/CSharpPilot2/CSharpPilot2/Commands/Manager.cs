using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal class Manager
    {
        public Manager(Context context, ParseOptions options, IDictionary<string, Action> dictionary)
        {
            _context        = context;
            _options        = options;
            _dictionary     = dictionary;
        }

        private readonly Context                        _context;
        private readonly ParseOptions                   _options;
        private readonly IDictionary<string, Action>    _dictionary;

        public ExecutionResult Execute(string command)
        {
            ParsedCommand parsedCommand;
            try
            {
                parsedCommand = ParseCommandTemplate(command, _options);
            }
            catch (ArgumentException e)
            {
                return new ExecutionResult(
                    HasFailed: true, 
                    FailMessage: $"{_context.Locale.GetErrorParsingCommand(command, e.Message)}"
                );
            }

            if (_dictionary.TryGetValue(parsedCommand.Name, out Action? action))
            {
                return action.Invoke(_context, parsedCommand.Parameters);
            }
            else
            {
                return new ExecutionResult(
                    HasFailed: true,
                    FailMessage: $"{_context.Locale.GetErrorCommandNotFound(parsedCommand.Name)}"
                );
            }
        }
    
        private static ParsedCommand ParseCommandTemplate(string command, ParseOptions options)
        {
            if (!command.StartsWith(options.CommandPrefix))
            {
                throw new ArgumentException($"Command string doesn't start with {options.CommandPrefix}", nameof(command));
            }

            var tokens = command.Split(
                options.Delimiters,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            ).Select(x => x.ToLowerInvariant());

            if (!tokens.Any())
            {
                throw new ArgumentException("Command string has no tokens", nameof(command));
            }

            var         paramTokens     = tokens.Skip(1);
            string      commandName     = tokens.First();
            string?     lastParam       = null;
            string?     curParam        = lastParam;
            var         args            = new List<ParsedArg>();
            var         parsedParams    = new Dictionary<string, ParsedParameter>();

            foreach (string paramToken in paramTokens)
            {
                if (paramToken.StartsWith(options.ParameterPrefix))
                {
                    string refinedToken = paramToken[1..];
                    if (!String.IsNullOrWhiteSpace(refinedToken) && !refinedToken.StartsWith(options.ParameterPrefix))
                    {
                        curParam = refinedToken;
                    }
                }
                else
                {
                    args.Add(new ParsedArg(paramToken));
                }

                // If curParam has changed or token is the last one left:
                if (curParam != lastParam || paramToken == paramTokens.Last())
                {
                    if (curParam is not null)
                    {
                        parsedParams[curParam] = new ParsedParameter(curParam, args.ToArray());
                    }
                    args.Clear();
                }

                lastParam = curParam;
            }

            return new ParsedCommand(commandName, parsedParams);
        }
    }
}
