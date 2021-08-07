using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharpPilot2.Input
{
    internal class Request
    {
        public Request(InputSource source)
        {
            _source = source;
            AddDefaultInterceptors();
        }
        public Request(InputSource source, Interceptor interceptor) : this(source) =>
            Interceptors.Add(interceptor);
        public Request(InputSource source, IEnumerable<Interceptor> interceptors) : this(source) =>
            Interceptors.AddRange(interceptors);

        private readonly InputSource _source;

        public List<Interceptor> Interceptors { get; } = new();

        public event EventHandler? RequestStarted;

        public InputInfo Perform()
        {
            var ordered = GetOrdered(Interceptors);
            InputInfo inputInfo;
            bool doRetry;
            do
            {
                doRetry = false;
                inputInfo = _source();
                var matching = GetMatching(ordered, inputInfo);
                foreach (Interceptor i in matching)
                {
                    i.Action(inputInfo);
                    if (IsRetrier(i))
                    {
                        doRetry = true;
                        break;
                    }
                }
            }
            while (doRetry);
            return inputInfo;
        }
        protected virtual void AddDefaultInterceptors() { }
        private static IEnumerable<Interceptor> GetOrdered(IEnumerable<Interceptor> interceptors) =>
            interceptors.OrderBy(i => i.Behaviour).ThenByDescending(i => i.Priority);
        private static IEnumerable<Interceptor> GetMatching(IEnumerable<Interceptor> interceptors, InputInfo inputInfo) =>
            interceptors.Where(i => i.Condition(inputInfo));
        private static bool IsRetrier(Interceptor interceptor) =>
            interceptor.Behaviour == InterceptorBehavior.Retry;
    }
}
