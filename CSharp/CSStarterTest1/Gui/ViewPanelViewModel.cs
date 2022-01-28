using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

using CSStarterTest1.DataOps;

namespace CSStarterTest1.Gui
{
    internal abstract class ViewPanelViewModel<TData>
    {
        public abstract ObservableCollection<TData> DataCollection { get; }

        public IReadOnlyList<ViewPanelCommandInfo> CommandInfos { get; init; } =
            Array.Empty<ViewPanelCommandInfo>();
    }
}
