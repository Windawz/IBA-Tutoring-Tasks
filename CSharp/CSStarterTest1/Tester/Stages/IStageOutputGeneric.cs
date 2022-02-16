using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSStarterTest1.Tester.Stages
{
    internal interface IStageOutput<out T> : IStageOutput
    {
        new T? Data { get; }

        object? IStageOutput.Data => Data;
    }
}
