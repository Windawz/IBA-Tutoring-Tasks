using System;

namespace CSharpPilot2.IO
{
    sealed class GeneratedOutput : IOutput
    {
        public GeneratedOutput(Func<Output> generator) => _generator = generator;

        readonly Func<Output> _generator;

        public Output Info => _generator();
    }
}
