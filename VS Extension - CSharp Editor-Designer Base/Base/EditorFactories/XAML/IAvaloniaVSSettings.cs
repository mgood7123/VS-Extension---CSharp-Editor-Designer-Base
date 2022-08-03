using System.ComponentModel;
using System.Windows.Controls;
using Base.EditorFactories.XAML.View;
using Serilog.Events;

namespace Base.EditorFactories.XAML
{
    public interface IAvaloniaVSSettings : INotifyPropertyChanged
    {
        Orientation DesignerSplitOrientation { get; set; }
        AvaloniaDesignerView DesignerView { get; set; }
        LogEventLevel MinimumLogVerbosity { get; set; }
        void Save();
        void Load();
    }
}