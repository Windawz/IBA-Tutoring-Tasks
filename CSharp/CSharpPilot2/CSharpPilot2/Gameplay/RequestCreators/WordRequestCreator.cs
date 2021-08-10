using System;
using System.Collections.Generic;
using System.Linq;

using CSharpPilot2.Commands;
using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay.RequestCreators
{
    internal class WordRequestCreator : ConcreteRequestCreator<string>
    {
        public WordRequestCreator(InputValidator inputValidator, CommandManager commandManager, InputSource inputSource, Locale locale)
            : base(commandManager, inputSource, locale) => InputValidator = inputValidator;

        protected InputValidator InputValidator { get; }

        protected override RequestStartedEventHandler GetDefaultStartedHandler() =>
            base.GetDefaultStartedHandler().Combine(GetStartedHandler());
        protected override IEnumerable<Interceptor> GetDefaultInterceptors() =>
            base.GetDefaultInterceptors().Append(GetInputInterceptor());

        protected virtual string GetStartedHandlerMessageString() =>
            Locale.GetWordRequestString(ReceivedParameter);
        protected virtual string GetInputInterceptorMessageString() =>
            Locale.GetInvalidInputString();

        private RequestStartedEventHandler GetStartedHandler() =>
            (sender, e) => GetStartedHandlerMessageString();
        private Interceptor GetInputInterceptor() =>
            new Interceptor(i => !InputValidator(i), _ => Console.WriteLine(GetInputInterceptorMessageString()));
    }
}
