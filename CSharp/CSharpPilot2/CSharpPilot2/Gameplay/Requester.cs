using System;

using CSharpPilot2.IO;

namespace CSharpPilot2.Gameplay
{
    sealed class Requester
    {
        public Requester(RequesterParams parameters)
        {
            _inputSource = parameters.InputSource;
            _inputValidator = parameters.InputValidator;

            _defaultPerformer =
                new CommandPerformerMod(
                    new RetryingPerformerMod(
                        new Performer(_inputSource, parameters.OutputTarget)
                    ),
                    parameters.CommandDetector,
                    parameters.CommandHandler
                );

            _timedPerformer =
                new TimedPerformerMod(
                    _defaultPerformer,
                    parameters.TimeLeftBuilder
                );

            _performer = _defaultPerformer;
        }


        readonly IPerformer _defaultPerformer;
        readonly IPerformer _timedPerformer;
        readonly IInputSource _inputSource;
        readonly Predicate<Input> _inputValidator;

        IPerformer _performer;
        RequesterMode _mode = RequesterMode.Default;

        public RequesterMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                switch (_mode)
                {
                    case RequesterMode.Default:
                        SwitchToDefault();
                        break;
                    case RequesterMode.Timed:
                        SwitchToTimed();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        public Word RequestWord(string prompt, string errorPrompt)
        {
            SayDo before = new()
            {
                Output = new OutputInfo(prompt),
            };
            SayDo<Input> matchedNot = new()
            {
                Output = new OutputInfo(errorPrompt),
            };

            Request request = new()
            {
                Before = before,
                Condition = _inputValidator,
                MatchedNot = matchedNot,
            };

            Input input = _performer.Perform(request);
            return new(input.Text, input.Seconds);
        }
        public string RequestName(string prompt)
        {
            SayDo before = new()
            {
                Output = new OutputInfo(prompt),
            };
            Request request = new()
            {
                Before = before,
            };

            return _performer.Perform(request).Text;
        }
        public void RequestAnyKey(string prompt)
        {
            Request request = new()
            {
                Before = new SayDo(new OutputInfo(prompt)),
            };

            if (_inputSource is ConsoleInputSource cis)
            {
                bool oldIntercept = cis.Intercept;
                cis.Intercept = true;
                _performer.Perform(request);
                cis.Intercept = oldIntercept;
            }
            else
            {
                _performer.Perform(request);
            }
        }

        void SwitchToTimed() =>
            _performer = _timedPerformer;
        void SwitchToDefault() =>
            _performer = _defaultPerformer;
    }
}
