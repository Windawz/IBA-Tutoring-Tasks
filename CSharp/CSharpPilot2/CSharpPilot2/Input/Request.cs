using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharpPilot2.Input
{
    internal class Request
    {
        private Request(InputSource source) =>
            _source = source;
        public Request(InputSource source, Interceptor interceptor) : this(source) =>
            _interceptors.Add(interceptor);
        public Request(InputSource source, IReadOnlyCollection<Interceptor> interceptors) : this(source) =>
            _interceptors.AddRange(interceptors);

        private readonly List<Interceptor> _interceptors = new();
        private readonly InputSource _source;

        public event EventHandler? RequestStarted;

        public InputInfo Perform()
        {
            InputInfo inputInfo;
            while (true)
            {
                inputInfo = _source();
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
