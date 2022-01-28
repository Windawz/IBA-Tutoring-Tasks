using System;
using System.Windows.Input;

namespace CSStarterTest1.Gui
{
    internal class RelayCommand : ICommand
    {
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;
        
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object? parameter) =>
            _execute(parameter);
        public bool CanExecute(object? parameter) =>
            _canExecute?.Invoke(parameter) ?? true;
    }
}
