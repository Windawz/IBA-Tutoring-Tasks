using System.Collections.Generic;

using CSharpPilot2.Gameplay;

namespace CSharpPilot2.Input
{
    internal class TimedRequest : Request
    {
        public TimedRequest(InputSource source) : base(source) { }
        public TimedRequest(InputSource source, Interceptor interceptor) : base(source, interceptor) { }
        public TimedRequest(InputSource source, IReadOnlyCollection<Interceptor> interceptors) : base(source, interceptors) { }

        public double SecondsSpent { get; private set; } = 0.0;

        protected override void AddDefaultInterceptors()
        {
            InterceptCondition condition = _ => true;
            InterceptAction action = x =>
            {
                SecondsSpent += x.Seconds;
                return InterceptResult.Continue;
            };
            Interceptors.Add(new Interceptor(condition, action));
        }
    }
}
