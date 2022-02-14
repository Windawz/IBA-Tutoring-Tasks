using System.Collections.Generic;

namespace CSStarterTest1.Tester.Stages
{
    internal interface IStage<in TIn, out TOut> : IStage
    {
        IEnumerable<TOut> Process(IEnumerable<TIn> input);
    }
}
