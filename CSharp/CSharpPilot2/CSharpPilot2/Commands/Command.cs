namespace CSharpPilot2.Input
{
    internal record Command(string Name, string Description, CommandAction Action)
    {
        public static readonly string Prefix = "/";

        public static bool IsValidCommandName(string name) =>
            name.StartsWith(Prefix);
    };
}
