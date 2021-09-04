namespace CSharpPilot2.Commands
{
    sealed record ExecutionResult(bool HasFailed, string FailMessage);
}
