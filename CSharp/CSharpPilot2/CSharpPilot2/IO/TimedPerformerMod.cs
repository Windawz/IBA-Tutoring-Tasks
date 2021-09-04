using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CSharpPilot2.IO {
    sealed partial class TimedPerformerMod : PerformerMod {
        public TimedPerformerMod(IPerformer performer) : this(performer, null) { }
        public TimedPerformerMod(IPerformer performer, TimeLeftBuilder? timeLeftBuilder) : base(performer) =>
            TimeLeftBuilder = timeLeftBuilder;

        public TimeLeftBuilder? TimeLeftBuilder { get; init; }

        public override Input Perform(Request request) {
            TimeAccumulator acc = new();
            
            var newRequest = request with {
                Anyway = AttachAccumulator(AppendSuffix(request.Anyway, acc), acc),
                Before = AppendSuffix(request.Before, acc),
                Matched = AppendSuffix(request.Matched, acc),
                MatchedNot = AppendSuffix(request.MatchedNot, acc),
            };

            return base.Perform(newRequest) with { Seconds = acc.Seconds };
        }
        private TSayDo? AppendSuffix<TSayDo>(TSayDo? sayDo, TimeAccumulator acc) where TSayDo : SayDoBase {
            if (TimeLeftBuilder is null || sayDo?.Output is null) {
                return sayDo;
            }

            Func<OutputInfo> generator = () => {
                double secondsSpend = acc.Seconds;
                var info = sayDo.Output.Info;
                return info with {
                    Text = info.Text + TimeLeftBuilder(secondsSpend),
                };
            };

            return sayDo with { Output = new GeneratedOutput(generator) };
        }
        private SayDo<Input> AttachAccumulator(SayDo<Input>? sayDo, TimeAccumulator acc) {
            Action<Input> action = i => acc.Add(i.Seconds);
            if (sayDo is null) {
                return new SayDo<Input>(action);
            } else {
                return sayDo with { Action = sayDo.Action + action };
            }
        }
    }
}
