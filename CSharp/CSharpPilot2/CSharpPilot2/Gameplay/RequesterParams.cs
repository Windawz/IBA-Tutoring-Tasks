using System;

using CSharpPilot2.IO;

namespace CSharpPilot2.Gameplay
{
    // FIXME: Temporary, until I come up with a better design.
    readonly struct RequesterParams
    {
        public RequesterParams(IInputSource inputSource, IOutputTarget outputTarget, Predicate<string> commandDetector, Action<string> commandHandler,
            Predicate<Input> inputValidator, TimeLeftBuilder timeLeftBuilder) {
            InputSource = inputSource;
            OutputTarget = outputTarget;
            CommandDetector = commandDetector;
            CommandHandler = commandHandler;
            InputValidator = inputValidator;
            TimeLeftBuilder = timeLeftBuilder;
        }

        public IInputSource InputSource { get; }
        public IOutputTarget OutputTarget { get; }
        public Predicate<string> CommandDetector { get; }
        public Action<string> CommandHandler { get; }
        public Predicate<Input> InputValidator { get; }
        public TimeLeftBuilder TimeLeftBuilder { get; }
    }
}
