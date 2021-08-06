using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharpPilot2.Input
{
    /// <summary>
    /// Represents an input request from the specified source. The input info will
    /// belong to the specified player.
    /// </summary>
    internal class Request
    {
        public Request(InputSource source, IList<Interceptor> interceptors)
        {
            _interceptors = interceptors.ToImmutableList();
            Source = source;
        }

        private readonly ImmutableList<Interceptor> _interceptors;

        protected InputSource Source { get; }

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
