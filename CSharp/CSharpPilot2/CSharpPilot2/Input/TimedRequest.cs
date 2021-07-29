namespace CSharpPilot2.Input
{
    internal class TimedRequest : ValidatedRequest
    {
        public TimedRequest(InputValidator validator, InputExceptor exceptor) : base(validator, exceptor) =>
            RequestEnded += (sender, e) => SecondsPassed += e.Seconds;

        public double SecondsPassed { get; private set; } = 0.0;

        protected override InputInfo PerformImpl(InputSource source) => base.PerformImpl(source) with { Seconds = SecondsPassed };
    }
}
