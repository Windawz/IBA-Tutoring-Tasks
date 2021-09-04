namespace CSharpPilot2.Commands
{
    sealed record CommandOptions(string CommandPrefix, string ParameterPrefix, string[] Delimiters, CommandList CommandList);
}
