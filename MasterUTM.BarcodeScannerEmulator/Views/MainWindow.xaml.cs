using System.Windows;
using System.Windows.Controls;

using Prism.Regions;
using CommonServiceLocator;
using DevExpress.Xpf.Core;

using MasterUTM.BarcodeScannerEmulator.Infrastructure;

namespace MasterUTM.BarcodeScannerEmulator
{
    public partial class MainWindow : Window
    {
        private readonly IRegionManager _regionManager;

        public MainWindow()
        {
            InitializeComponent();

            _regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            RegionManager.SetRegionManager(mainRegionContent, _regionManager);

            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            DXSplashScreen.Close();
            _regionManager.RequestNavigate(RegionNames.MainRegion, "MainView");
        }
    }
}
