using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal record ParameterTemplate(string Name, string[] Args)
    {
        public static ParameterTemplate[] ParseParameters(string parameters, ParameterParseOptions options)
        {
            string[] tokens = parameters.Split(
                options.ArgDelimiters,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            );

            string?                     lastParam       = null;
            string?                     curParam        = lastParam;
            List<string>                args            = new();
            List<ParameterTemplate>     templates       = new();

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (token.StartsWith(options.Prefix))
                {
                    curParam = token;
                }
                else
                {
                    args.Add(token);
                }

                // If curParam has changed or token is the last one left:
                if (curParam != lastParam || i == tokens.Length - 1)
                {
                    if (curParam is not null)
                    {
                        templates.Add(new ParameterTemplate(curParam[1..], args.ToArray()));
                    }
                    args.Clear();
                }

                lastParam = curParam;
            }

            return templates.ToArray();
        }
    }
}
