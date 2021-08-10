using CSharpPilot2.Commands;
using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay.RequestCreators
{
    internal abstract class ConcreteRequestCreatorBase : CommandRequestCreator
    {
        public ConcreteRequestCreatorBase(CommandManager commandManager, InputSource inputSource, Locale locale)
            : base(commandManager, inputSource, locale) { }

        protected Request CreateRequestBase()
        {
            Request request = new(InputSource, GetDefaultInterceptors());
            request.RequestStarted += GetDefaultStartedHandler();
            return request;
        }
    }
}
