using System.Collections.Generic;

using CSharpPilot2.Gameplay;

namespace CSharpPilot2.Input
{
    internal class TimeTrackingRequest : Request
    {
        public TimeTrackingRequest(InputSource source) : base(source) { }
        public TimeTrackingRequest(InputSource source, Interceptor interceptor) : base(source, interceptor) { }
        public TimeTrackingRequest(InputSource source, IEnumerable<Interceptor> interceptors) : base(source, interceptors) { }

        public double SecondsSpent { get; private set; } = 0.0;

        protected override void AddDefaultInterceptors()
        {
            InterceptorCondition condition = _ => true;
            InterceptorAction action = inputInfo =>
            {
                SecondsSpent += inputInfo.Seconds;
            };
            Interceptor interceptor = new(condition, action);
            Interceptors.Add(interceptor);
        }
    }
}
