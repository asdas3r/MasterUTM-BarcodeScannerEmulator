using System.Windows;
using System.Windows.Data;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;

namespace MasterUTM.BarcodeScannerEmulator.Design.Resources
{
    public partial class WindowChromeStyle : ResourceDictionary
    {
        public WindowChromeStyle()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            var vm = (window.DataContext) as DialogBaseViewModel;
            if (vm != null)
            {
                vm.CloseDialog();
            }
            else
            {
                window.Close();
            }
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = WindowState.Minimized;
        }

        private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
                Application.Current.Resources["MaximizeVisibility"] = Visibility.Visible;
                Application.Current.Resources["RestoreVisibility"] = Visibility.Collapsed;
            }
            else if (window.WindowState == WindowState.Normal)
            {
                window.WindowState = WindowState.Maximized;
                Application.Current.Resources["MaximizeVisibility"] = Visibility.Collapsed;
                Application.Current.Resources["RestoreVisibility"] = Visibility.Visible;
            }
        }
    }
}
