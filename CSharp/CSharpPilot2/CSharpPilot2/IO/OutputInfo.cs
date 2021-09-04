namespace CSharpPilot2.IO
{
    sealed record OutputInfo(string Text) : IOutput
    {
        public OutputColor? Color { get; init; }
        public bool NewLine { get; init; } = true;

        OutputInfo IOutput.Info => this;
    }
}
