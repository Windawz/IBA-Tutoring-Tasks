using System;

namespace CSharpPilot2.IO
{
    sealed record SayDo<TParam> : SayDoBase
    {
        public SayDo() { }
        public SayDo(IOutput output) => Output = output;
        public SayDo(Action<TParam> action) => Action = action;

        public new Action<TParam>? Action
        {
            get => (Action<TParam>?)base.Action;
            init => base.Action = value;
        }
    }
}
