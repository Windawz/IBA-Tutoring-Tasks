
using System;
using System.Collections.Generic;

namespace CSStarterTest1.Tester.Stages
{
    internal abstract class Stage<TIn, TOut> : IStage<TIn, TOut>
    {
        public virtual string GetMessage(StageMessage messageKind)
        {
            if (!Enum.IsDefined(messageKind))
            {
                throw new ArgumentOutOfRangeException("Enum value out of range", nameof(messageKind));
            }
            throw new ArgumentException("Value not supported", nameof(messageKind));
        }
        public abstract IStageOutput<TOut>[] Process(TIn[] input);

        IStageOutput[] IStage.Process(object[] input) =>
            Process(Array.ConvertAll(input, o => (TIn)o));
    }
}
