using System;
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
                bool isIntercepted = false;
                inputInfo = Source();
                foreach (var interceptor in _interceptors)
                {
                    if (interceptor.Condition(inputInfo))
                    {
                        isIntercepted = true;
                        interceptor.Action(inputInfo);
                    }
                }
                if (!isIntercepted)
                {
                    break;
                }
            }
            return inputInfo;
        }
    }
}
