
using System;
using System.Collections.Generic;

namespace CSStarterTest1.Tester.Stages
{
    internal abstract class Stage<TIn, TOut> : IStage<TIn, TOut>
    {
        public virtual string GetMessage(StageMessage messageKind) => "";
        public abstract IStageOutput<TOut>[] Process(TIn[] input);

        IStageOutput[] IStage.Process(object[] input) =>
            Process(Array.ConvertAll(input, o => (TIn)o));
    }
}
