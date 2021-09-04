using System;

namespace CSharpPilot2.IO
{
    sealed record SayDo : SayDoBase
    {
        public SayDo() { }
        public SayDo(IOutput output) => Output = output;
        public SayDo(Action action) => Action = action;

        public new Action? Action
        {
            get => (Action?)base.Action;
            init => base.Action = value;
        }
    }
}
