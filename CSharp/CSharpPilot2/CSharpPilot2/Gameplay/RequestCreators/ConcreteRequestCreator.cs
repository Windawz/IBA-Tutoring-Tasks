using CSharpPilot2.Commands;
using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay.RequestCreators
{
    internal class ConcreteRequestCreator : ConcreteRequestCreatorBase
    {
        public ConcreteRequestCreator(CommandManager commandManager, InputSource inputSource, Locale locale)
            : base(commandManager, inputSource, locale) { }

        public Request CreateRequest() => CreateRequestBase();
    }
}
