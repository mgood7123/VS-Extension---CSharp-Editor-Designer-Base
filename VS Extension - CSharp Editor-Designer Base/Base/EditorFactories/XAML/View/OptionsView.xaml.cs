using System.Windows.Controls;

namespace Base.EditorFactories.XAML.View
{
    public partial class OptionsView : UserControl
    {
        public OptionsView()
        {
            InitializeComponent();
        }

        public IAvaloniaVSSettings Settings
        {
            get => DataContext as IAvaloniaVSSettings;
            set => DataContext = value;
        }
    }
}
