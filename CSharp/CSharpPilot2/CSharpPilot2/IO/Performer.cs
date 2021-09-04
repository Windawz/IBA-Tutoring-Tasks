using System;

namespace CSharpPilot2.IO
{
    sealed class Performer : IPerformer
    {
        public Performer(IInputSource inputSource, IOutputTarget outputTarget) =>
            (InputSource, OutputTarget) = (inputSource, outputTarget);

        public IInputSource InputSource { get; set; }
        public IOutputTarget OutputTarget { get; set; }

        public Input Perform(Request request)
        {
            HandleSayDo(request.Before, null);

            Input input = InputSource.Get();

            HandleSayDo(request.Anyway, input);

            if (request.Condition?.Invoke(input) ?? true)
            {
                HandleSayDo(request.Matched, input);
            }
            else
            {
                HandleSayDo(request.MatchedNot, input);
            }

            return input;
        }

        void HandleSayDo(SayDoBase? sayDo, Input? input)
        {
            if (sayDo is not null)
            {
                if (sayDo.Output is not null)
                {
                    OutputTarget.Put(sayDo.Output);
                }
                switch (sayDo)
                {
                    case SayDo saydo:
                        saydo.Action?.Invoke();
                        break;
                    case SayDo<Input> saydoWithInput:
                        saydoWithInput.Action?.Invoke(input ?? throw new ArgumentNullException(nameof(input)));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
