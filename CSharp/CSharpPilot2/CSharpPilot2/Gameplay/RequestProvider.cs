using System;

using CSharpPilot2.Commands;
using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay
{
    internal class RequestProvider
    {
        public RequestProvider(Rules rules, Locale locale, InputSource source, CommandManager commandManager)
        {
            _source = source;
            _rules = rules;
            _locale = locale;
            _commandManager = commandManager;
        }

        private readonly InputSource _source;
        private readonly Rules _rules;
        private readonly Locale _locale;
        private readonly CommandManager _commandManager;

        public TimeTrackingRequest GetTimeTrackingWordRequest(Player player)
        {
            TimeTrackingRequest request = GetTimeTrackingRequestBase();

            Func<double> timeLeftGetter = () =>
                _rules.Properties.MaxWordSeconds - request.SecondsSpent;
            InterceptorAction action = inputInfo =>
                GetInvalidWordTimedMessage(timeLeftGetter);

            request.Interceptors.Add(GetInvalidWordInterceptor(action));

            return request;
        }
        public Request GetWordRequest(Player player)
        {
            Request request = GetRequestBase();

            InterceptorAction action = inputInfo =>
                Console.WriteLine(GetInvalidWordMessage());

            request.Interceptors.Add(GetInvalidWordInterceptor(action));

            return request;
        }
        public Request GetNameRequest(int playerIndex)
        {
            Request request = GetRequestBase();

            request.RequestStarted += (sender, e) =>
                Console.WriteLine($"{_locale.GetNameRequestString(playerIndex)}");

            return request;
        }
        private Request GetRequestBase()
        {
            Interceptor commandInterceptor = GetCommandInterceptor();
            return new Request(_source, commandInterceptor);
        }
        private TimeTrackingRequest GetTimeTrackingRequestBase()
        {
            Interceptor commandInterceptor = GetCommandInterceptor();
            return new TimeTrackingRequest(_source, commandInterceptor);
        }
        private InterceptorCondition GetInvalidWordCondition() =>
            inputInfo => !_rules.InputValidator(inputInfo);
        private string GetInvalidWordMessage() =>
            $"{_locale.GetInvalidInputString()}";
        private string GetInvalidWordTimedMessage(Func<double> timeLeftGetter) =>
            $"{GetInvalidWordMessage()} ({timeLeftGetter()})";
        private Interceptor GetCommandInterceptor()
        {
            string commandPrefix = _commandManager.Options.CommandPrefix;

            InterceptorCondition condition = inputInfo =>
                inputInfo.Text.StartsWith(commandPrefix);

            InterceptorAction action = inputInfo =>
            {
                _commandManager.Execute(inputInfo.Text);
            };

            return new Interceptor(condition, action)
            {
                Behaviour = InterceptorBehavior.Retry,
                Priority = InterceptorPriority.Highest,
            };
        }
        private Interceptor GetInvalidWordInterceptor(InterceptorAction action) =>
            new Interceptor(GetInvalidWordCondition(), action)
            {
                Behaviour = InterceptorBehavior.Retry,
            };
    }
}
