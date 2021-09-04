namespace CSharpPilot2.IO
{
    // Lambdas don't play well with ref variables, so
    // I had to write a reference type just for accumulating time.
    // Lambdas still rock, though.
    sealed class TimeAccumulator
    {
        const double InitialValue = 0.0;

        public double Seconds { get; private set; } = InitialValue;

        public void Add(double seconds) => Seconds += seconds;
        public void Reset() => Seconds = InitialValue;
    }
}
