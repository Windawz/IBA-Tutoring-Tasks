namespace CSharpPilot2.Input
{
    internal class TimedRequest : ValidatedRequest
    {
        public TimedRequest(InputValidator validator, InputForgiver forgiver) : base(validator, forgiver) =>
            InputInfoReceived += (sender, e) => SecondsPassed += e.Seconds;

        public double SecondsPassed { get; private set; } = 0.0;

        protected override InputInfo PerformImpl(InputSource source) => base.PerformImpl(source) with { Seconds = SecondsPassed };
    }
}
