using Base.EditorFactories.XAML;
using Base.EditorFactories.XAML.View;
using Base.Utils;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;

namespace VS_Extension___CSharp_Editor_Designer_Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    [Guid("3093ca7c-c764-4547-a7ae-12055b139bdf")]
    public class OptionsDialogPage : UIElementDialogPage
    {
        private OptionsView _options;

        protected override UIElement Child => _options ?? (_options = new OptionsView());

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            _options.Settings = Site.GetMefService<IAvaloniaVSSettings>();
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            _options?.Settings?.Save();
        }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            _options?.Settings?.Load();
        }
    }
}