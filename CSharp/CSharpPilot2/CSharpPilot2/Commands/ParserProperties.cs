﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Commands
{
    internal record ParserProperties(
        string CommandPrefix,
        string ParameterPrefix,
        string[] Delimiters
    );
}
