using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;

namespace MasterUTM.BarcodeScannerEmulator.Mvvm.Dialogs.ViewModels
{
    public class OkDialogViewModel : BaseViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public OkDialogViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            OkCommand = new DelegateCommand(OnOkCommand);
        }

        private void OnOkCommand()
        {
            _eventAggregator.GetEvent<CustomMessageBoxButtonEvent>().Publish(ButtonResult.OK);
        }

        public DelegateCommand OkCommand { get; }

        private string _okString = "ОК";
        public string OkString
        {
            get { return _okString; }
            set { SetProperty(ref _okString, value); }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            string ok = (string)navigationContext.Parameters["okString"];
            if (ok != null)
                OkString = ok;
        }
    }
}
