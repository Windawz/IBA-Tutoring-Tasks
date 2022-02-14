
using System;
using System.Collections.Generic;

namespace CSStarterTest1.Tester.Stages
{
    internal abstract class Stage<TIn, TOut> : IStage<TIn, TOut>, IStage
    {
        public Type In => typeof(TIn);
        public Type Out => typeof(TOut);

        public virtual string GetMessage(StageMessage messageKind) => "";
        public abstract IEnumerable<TOut> Process(IEnumerable<TIn> input);
        public IEnumerable<object> Process(IEnumerable<object> input) =>
            (IEnumerable<object>)Process((IEnumerable<TIn>)input);
    }
}
