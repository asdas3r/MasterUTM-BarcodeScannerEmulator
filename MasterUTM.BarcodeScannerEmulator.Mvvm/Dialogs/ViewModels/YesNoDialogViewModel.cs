using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;

namespace MasterUTM.BarcodeScannerEmulator.Mvvm.Dialogs.ViewModels
{
    public class YesNoDialogViewModel : BaseViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public YesNoDialogViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator; 

            YesCommand = new DelegateCommand(OnYesCommand);
            NoCommand = new DelegateCommand(OnNoCommand);
        }

        private void OnYesCommand()
        {
            _eventAggregator.GetEvent<CustomMessageBoxButtonEvent>().Publish(ButtonResult.Yes);
        }

        private void OnNoCommand()
        {
            _eventAggregator.GetEvent<CustomMessageBoxButtonEvent>().Publish(ButtonResult.No);
        }

        public DelegateCommand YesCommand { get; }
        public DelegateCommand NoCommand { get; }

        private string _yesString = "Да";
        public string YesString
        {
            get { return _yesString; }
            set { SetProperty(ref _yesString, value); }
        }

        private string _noString = "Нет";
        public string NoString
        {
            get { return _noString; }
            set { SetProperty(ref _noString, value); }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            string yes = (string)navigationContext.Parameters["yesString"];
            string no = (string)navigationContext.Parameters["noString"];
            if (yes != null)
                YesString = yes;
            if (no != null)
                NoString = no;
        }
    }
}
