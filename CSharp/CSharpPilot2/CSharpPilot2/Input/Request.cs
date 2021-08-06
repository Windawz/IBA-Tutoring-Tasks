using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharpPilot2.Input
{
    internal class Request
    {
        public Request(InputSource source) : this(source, Array.Empty<Interceptor>()) { }
        public Request(InputSource source, Interceptor interceptor) : this(source, new Interceptor[1] { interceptor }) { }
        public Request(InputSource source, Interceptor[] interceptors)
        {
            Source = source;
            _interceptors = interceptors.ToImmutableArray();
        }

        private readonly ImmutableArray<Interceptor> _interceptors;

        protected InputSource Source { get; init; }

        public event EventHandler? RequestStarted;

        public InputInfo Perform()
        {
            InputInfo inputInfo;
            while (true)
            {
                inputInfo = Source();
                Interceptor? interceptor = FindInterceptor(inputInfo);
                if (interceptor is not null)
                {
                    interceptor.Action(inputInfo);
                }
                else
                {
                    break;
                }
            }
            return inputInfo;
        }
        private Interceptor? FindInterceptor(InputInfo inputInfo) =>
            _interceptors.Where(i => i.Condition(inputInfo)).FirstOrDefault();
    }
}
