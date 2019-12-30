using System.Windows;

using Prism.Services.Dialogs;

namespace MasterUTM.BarcodeScannerEmulator.Mvvm.Views
{
    public partial class CustomDialogWindow : IDialogWindow
    {
        public CustomDialogWindow()
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }

    }
}
