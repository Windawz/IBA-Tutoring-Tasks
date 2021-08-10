using CSharpPilot2.Commands;
using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay.RequestCreators
{
    internal abstract class ConcreteRequestCreator<TParam> : ConcreteRequestCreatorBase
    {
        public ConcreteRequestCreator(CommandManager commandManager, InputSource inputSource, Locale locale)
            : base(commandManager, inputSource, locale) { }

        protected TParam ReceivedParameter { get; private set; } = default!;

        public Request CreateRequest(TParam parameter)
        {
            ReceivedParameter = parameter;
            Request request = CreateRequestBase();
            return request;
        }
    }
}
