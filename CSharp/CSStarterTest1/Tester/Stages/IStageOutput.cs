using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSStarterTest1.Tester.Stages
{
    internal interface IStageOutput
    {
        object? Data { get; } // We may have failed to acquire data, hence nullable.
                              // Examples: Failed to create instance of a test type, or
                              //           a test resulted in a failure.
        
        StageOutputDisplayInfo GetDisplayInfo();
    }
}
