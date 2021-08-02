using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal class Parser
    {
        public Parser(ParserProperties properties) =>
            Properties = properties;

        public ParserProperties Properties { get; }

        /// <exception cref="ParseException"></exception>
        public ParsedCommand Parse(string input)
        {
            string[] tokens = input.Split(
                Properties.Delimiters,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            );

            if (tokens.Length == 0)
            {
                throw new ParseException("No tokens");
            }
            else if (!tokens[0].StartsWith(Properties.CommandPrefix))
            {
                throw new ParseException("No command prefix");
            }

            string name = tokens[0].Substring(1);

            if (tokens.Length == 1)
            {
                return new ParsedCommand(Name: name, Parameters: new ParsedParameter[0]);
            }

            IEnumerable<string>     remaining       = tokens.Skip(1);
            List<ParsedParameter>   parameters      = new();

            List<string>            args            = new();
            string?                 currentParam    = null;

            foreach (string token in remaining)
            {
                if (token.StartsWith(Properties.ParameterPrefix))
                {
                    if (args.Count > 0 && currentParam is not null)
                    {
                        parameters.Add(new ParsedParameter(currentParam, args.ToArray()));
                        args.Clear();
                    }
                    currentParam = token;
                }
                else
                {
                    args.Add(token);
                }
            }

            return new ParsedCommand(name, parameters.ToArray());
        }
    }
}
