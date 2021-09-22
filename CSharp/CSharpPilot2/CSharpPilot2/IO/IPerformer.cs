namespace CSharpPilot2.IO
{
    interface IPerformer
    {
        Input Perform(Request request);

        IInputSource InputSource { get; set; }
        IOutputTarget OutputTarget { get; set; }
    }
}
