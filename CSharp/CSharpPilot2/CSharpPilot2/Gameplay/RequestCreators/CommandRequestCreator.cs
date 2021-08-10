using System.Collections.Generic;
using System.Linq;

using CSharpPilot2.Commands;
using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay.RequestCreators
{
    internal abstract class CommandRequestCreator : RequestCreator
    {
        public CommandRequestCreator(CommandManager commandManager, InputSource inputSource, Locale locale)
            : base(inputSource, locale) => CommandManager = commandManager;

        protected CommandManager CommandManager { get; }

        protected override IEnumerable<Interceptor> GetDefaultInterceptors() =>
            base.GetDefaultInterceptors().Append(GetCommandInterceptor());
        private Interceptor GetCommandInterceptor()
        {
            InterceptorCondition condition = i =>
                i.Text.StartsWith(CommandManager.Options.CommandPrefix);
            InterceptorAction action = i =>
                CommandManager.Execute(i.Text);
            Interceptor interceptor = new(condition, action)
            {
                Behaviour = InterceptorBehavior.Retry,
                Priority = InterceptorPriority.Highest,
            };
            return interceptor;
        }
    }
}
