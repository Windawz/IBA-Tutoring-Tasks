using System;
using System.Collections.Generic;

namespace CSStarterTest1.Tester.Stages
{
    internal interface IStage
    {
        Type In { get; }
        Type Out { get; }

        string GetMessage(StageMessage messageKind);
        IStageOutput[] Process(object[] input);
    }
}
