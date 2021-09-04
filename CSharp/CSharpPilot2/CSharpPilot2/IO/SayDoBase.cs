using System;
using System.Collections.Immutable;

namespace CSharpPilot2.IO
{
    abstract record SayDoBase
    {
        public Delegate? Action { get; init; }
        public IOutput? Output { get; init; }
    }
}
