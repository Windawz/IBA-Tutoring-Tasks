using System.Collections.Generic;
using System.Linq;

using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay.RequestCreators
{
    internal abstract class RequestCreator
    {
        public RequestCreator(InputSource inputSource, Locale locale)
        {
            InputSource = inputSource;
            Locale = locale;
        }

        protected InputSource InputSource { get; }
        protected Locale Locale { get; }

        protected virtual RequestStartedEventHandler GetDefaultStartedHandler() =>
            delegate { };
        protected virtual IEnumerable<Interceptor> GetDefaultInterceptors() =>
            Enumerable.Empty<Interceptor>();
    }
}
