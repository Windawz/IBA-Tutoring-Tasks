using System;

namespace CSharpPilot2.IO
{
    sealed class GeneratedOutput : IOutput
    {
        public GeneratedOutput(Func<OutputInfo> generator) => _generator = generator;

        readonly Func<OutputInfo> _generator;

        public OutputInfo Info => _generator();
    }
}
