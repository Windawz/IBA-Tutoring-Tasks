namespace CSharpPilot2.Commands
{
    internal sealed record CommandOptions(string CommandPrefix, string ParameterPrefix, string[] Delimiters, CommandList CommandList);
}
