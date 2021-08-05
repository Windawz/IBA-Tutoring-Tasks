using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Input
{
    internal record Interceptor(Predicate<InputInfo> Condition, Action<InputInfo> Action);
}
