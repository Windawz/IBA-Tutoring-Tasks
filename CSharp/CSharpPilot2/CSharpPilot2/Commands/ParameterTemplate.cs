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

            string?                     curParam        = null;
            List<string>                args            = new();
            List<ParameterTemplate>     templates       = new();

            foreach (string token in tokens)
            {
                if (token.StartsWith(options.Prefix))
                {
                    if (args.Count > 0 && curParam is not null)
                    {
                        string name = curParam.Substring(1);
                        ParameterTemplate template = new(name, args.ToArray());
                        templates.Add(template);
                        args.Clear();
                    }

                    curParam = token;
                }
                else
                {
                    args.Add(token);
                }
            }

            return templates.ToArray();
        }
    }
}
