using System;

namespace CSharpPilot2.IO {
    sealed class GeneratedOutput : IOutput {
        public GeneratedOutput(Func<OutputInfo> generator) {
            _generator = generator;
        }

        private readonly Func<OutputInfo> _generator;

        public OutputInfo Info {
            get {
                return _generator();
            }
        }
    }
}
