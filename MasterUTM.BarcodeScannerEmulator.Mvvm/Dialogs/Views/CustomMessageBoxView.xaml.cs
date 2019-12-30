using Prism.Regions;
using System.Windows.Controls;

namespace MasterUTM.BarcodeScannerEmulator.Mvvm.Dialogs.Views
{
    public partial class CustomMessageBoxView : UserControl
    {
        public CustomMessageBoxView(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionManager(dialogRegionContent, regionManager);
        }
    }
}
