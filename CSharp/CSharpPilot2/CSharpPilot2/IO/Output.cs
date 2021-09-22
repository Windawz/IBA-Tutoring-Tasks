namespace CSharpPilot2.IO
{
    sealed record Output(string Text) : IOutput
    {
        public OutputColor? Color { get; init; }
        public bool NewLine { get; init; } = true;

        Output IOutput.Info => this;
    }
}
