﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    delegate ExecutionResult CommandAction(CommandContext context, IDictionary<string, ParsedParameter> parameters);
}