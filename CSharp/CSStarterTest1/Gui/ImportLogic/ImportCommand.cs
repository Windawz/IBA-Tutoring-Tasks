using System;
using System.Windows.Input;

namespace CSStarterTest1.Gui.ImportLogic
{
    internal sealed class ImportCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => throw new NotImplementedException();
        public void Execute(object? parameter) => throw new NotImplementedException();
    }
}
