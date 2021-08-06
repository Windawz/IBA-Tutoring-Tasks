using System;

using CSharpPilot2.Input;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay
{
    internal class RequestProvider
    {
        public RequestProvider(Rules rules, Locale locale) =>
            (Rules, Locale) = (rules, locale);

        protected Rules Rules { get; }
        protected Locale Locale { get; }

        public TimedRequest GetWordRequestTimed(Player player)
        {
            var request = new TimedRequest(Rules.InputValidator);

            double GetTimeLeft(TimedRequest request) =>
                Rules.Properties.MaxWordSeconds - request.SecondsPassed;

            request.RequestStarted += (sender, e) =>
            {
                string msg = $"{Locale.GetWordRequestString(player.Name)} ({Locale.GetTimeLeftSuffixString(GetTimeLeft(request))})";
                Console.WriteLine(msg);
            };

            request.InputInfoInvalid += (sender, e) =>
            {
                string msg = $"{Locale.GetInvalidInputString()} ({Locale.GetTimeLeftSuffixString(GetTimeLeft(request))})";
                Console.WriteLine(msg);
            };

            return request;
        }
        public ValidatedRequest GetWordRequest(Player player)
        {
            var request = new ValidatedRequest(Rules.InputValidator);

            request.RequestStarted += (sender, e) =>
            {
                string msg = $"{Locale.GetWordRequestString(player.Name)}";
                Console.WriteLine(msg);
            };

            request.InputInfoInvalid += (sender, e) =>
            {
                string msg = $"{Locale.GetInvalidInputString()}";
                Console.WriteLine(msg);
            };

            return request;
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
