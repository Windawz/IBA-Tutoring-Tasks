
using CSharpPilot2.Commands;
using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay.RequestCreators
{
    internal class NameRequestCreator : ConcreteRequestCreator<int>
    {
        public NameRequestCreator(CommandManager commandManager, InputSource inputSource, Locale locale)
            : base(commandManager, inputSource, locale) { }

        protected override RequestStartedEventHandler GetDefaultStartedHandler() =>
            base.GetDefaultStartedHandler().Combine(GetStartedHandler());

        protected virtual string GetStartedHandlerMessageString() =>
            Locale.GetNameRequestString(ReceivedParameter);

        private RequestStartedEventHandler GetStartedHandler() =>
            (sender, e) => GetStartedHandlerMessageString();
    }
}
