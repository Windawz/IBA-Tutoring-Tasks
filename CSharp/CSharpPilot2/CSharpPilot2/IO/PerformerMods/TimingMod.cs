using System;

namespace CSharpPilot2.IO.PerformerMods
{
    sealed partial class TimingMod : PerformerMod
    {
        public TimingMod(IPerformer performer) : this(performer, null) { }
        public TimingMod(IPerformer performer, TimeLeftBuilder? timeLeftBuilder) : base(performer) =>
            TimeLeftBuilder = timeLeftBuilder;

        public TimeLeftBuilder? TimeLeftBuilder { get; init; }

        public override Input Perform(Request request)
        {
            TimeAccumulator acc = new();

            Request? newRequest = request with
            {
                Anyway = AttachAccumulator(AppendSuffix(request.Anyway, acc), acc),
                Before = AppendSuffix(request.Before, acc),
                Matched = AppendSuffix(request.Matched, acc),
                MatchedNot = AppendSuffix(request.MatchedNot, acc),
            };

            return base.Perform(newRequest) with { Seconds = acc.Seconds };
        }
        TSayDo? AppendSuffix<TSayDo>(TSayDo? sayDo, TimeAccumulator acc) where TSayDo : SayDoBase
        {
            if (TimeLeftBuilder is null || sayDo?.Output is null)
            {
                return sayDo;
            }

            Func<Output> generator = () =>
            {
                double secondsSpent = acc.Seconds;
                Output? info = sayDo.Output.Info;
                return info with
                {
                    Text = info.Text + TimeLeftBuilder(secondsSpent),
                };
            };

            return sayDo with { Output = new GeneratedOutput(generator) };
        }
        SayDo<Input> AttachAccumulator(SayDo<Input>? sayDo, TimeAccumulator acc)
        {
            Action<Input> action = i => acc.Add(i.Seconds);
            if (sayDo is null)
            {
                return new SayDo<Input>(action);
            }
            else
            {
                return sayDo with { Action = sayDo.Action + action };
            }
        }
    }
}
