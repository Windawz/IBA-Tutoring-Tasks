namespace CSharpPilot2.IO
{
    interface IInputSource
    {
        InputSourceReadMode ReadMode { get; set; }

        Input Get();
    }
}
