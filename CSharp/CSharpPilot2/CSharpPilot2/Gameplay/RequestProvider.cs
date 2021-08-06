using System;

using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay
{
    internal class RequestProvider
    {
        public RequestProvider(Rules rules, Locale locale, InputSource source)
        {
            _source = source;
            _rules = rules;
            _locale = locale;
        }

        private readonly InputSource _source;
        private readonly Rules _rules;
        private readonly Locale _locale;

        public Request GetWordRequestTimed(Player player)
        {
           
        }
        public Request GetWordRequest(Player player)
        {
            Interceptor interceptor = new(x => _rules.InputValidator(x), x => Console.WriteLine($"{_locale.GetInvalidInputString()}"));
            return new Request(_source, { interceptor });
        }
        public Request GetNameRequest(int playerIndex)
        {
            var request = new Request();

            request.RequestStarted += (sender, e) =>
                Console.WriteLine($"{Locale.GetNameRequestString(playerIndex)}");

            return request;
        }
    }
}
