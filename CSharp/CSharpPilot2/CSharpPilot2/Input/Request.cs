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
        public Request(InputSource source, IReadOnlyCollection<Interceptor> interceptors) : this(source) =>
            Interceptors.AddRange(interceptors);

        private readonly InputSource _source;
        
        protected List<Interceptor> Interceptors { get; } = new();

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
        public void AddInterceptor(Interceptor interceptor) =>
            Interceptors.Add(interceptor);
        public void AddInterceptorRange(IEnumerable<Interceptor> interceptors) =>
            Interceptors.AddRange(interceptors);
        protected virtual void AddDefaultInterceptors() { }
        private Interceptor? FindInterceptor(InputInfo inputInfo) =>
            Interceptors.Where(i => i.Condition(inputInfo)).FirstOrDefault();
    }
}
