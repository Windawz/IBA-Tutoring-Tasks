using System;

namespace CSharpPilot2.IO
{
    sealed class CommandPerformerMod : PerformerMod
    {
        public CommandPerformerMod(IPerformer performer, Predicate<string> commandDetector, Action<string> commandHandler) : base(performer)
        {
            _commandDetector = commandDetector;
            _commandHandler = commandHandler;
        }

        readonly Predicate<string> _commandDetector;
        readonly Action<string> _commandHandler;

        public override Input Perform(Request request)
        {
            Input input;

            while (true)
            {
                input = base.Perform(request);
                if (_commandDetector(input.Text))
                {
                    _commandHandler(input.Text);
                }
                else
                {
                    break;
                }
            }

            return input;
        }
    }
}
