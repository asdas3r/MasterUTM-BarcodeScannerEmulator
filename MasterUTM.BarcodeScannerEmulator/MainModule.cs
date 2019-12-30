using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using MasterUTM.BarcodeScannerEmulator.Infrastructure;
using MasterUTM.BarcodeScannerEmulator.Views;
using MasterUTM.BarcodeScannerEmulator.ViewModels;

namespace MasterUTM.BarcodeScannerEmulator
{
    class MainModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(MainView));
            //_regionManager.RegisterViewWithRegion(RegionNames.InputOptionsRegion, typeof(OptionsInputManualView));
            //_regionManager.RegisterViewWithRegion(RegionNames.GeneralOptionsRegion, typeof(OptionsGeneralView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView>("MainView");
            containerRegistry.RegisterForNavigation<OptionsInputAutoView>("OptionsInputAutoView");
            containerRegistry.RegisterForNavigation<OptionsInputManualView>("OptionsInputManualView");
            containerRegistry.RegisterForNavigation<OptionsGeneralView>("OptionsGeneralView");

            containerRegistry.RegisterDialog<ApplicationChoiceView, ApplicationChoiceViewModel>("ApplicationChoiceView");
            containerRegistry.RegisterDialog<BarcodeInputView, BarcodeInputViewModel>("BarcodeInputView");
            containerRegistry.RegisterDialog<TransferView, TransferViewModel>("TransferView");
        }
    }
}
