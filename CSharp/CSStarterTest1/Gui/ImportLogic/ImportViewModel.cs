using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using CSStarterTest1.DataOps;

namespace CSStarterTest1.Gui.ImportLogic
{
    internal sealed class ImportViewModel : INotifyPropertyChanged
    {
        private ImportModel? _model;

        public IReadOnlyCollection<Data> Data =>
            _model?.Data ?? Array.Empty<Data>();
        public ICommand ImportCommand

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
