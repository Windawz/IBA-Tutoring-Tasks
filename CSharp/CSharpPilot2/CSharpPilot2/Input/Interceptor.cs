using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Input
{
    internal record Interceptor(InterceptorCondition Condition, InterceptorAction Action)
    {
        public InterceptorCondition Condition { get; init; } = Condition;
        public InterceptorAction Action { get; init; } = Action;
        public InterceptorBehavior Behaviour { get; init; } = InterceptorBehavior.Pass;
        public InterceptorPriority Priority { get; init; } = InterceptorPriority.Normal;
    }
}
