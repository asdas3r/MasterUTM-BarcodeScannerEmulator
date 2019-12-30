using System.Collections.ObjectModel;
using System.ComponentModel;

using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Commands;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;

namespace MasterUTM.BarcodeScannerEmulator.ViewModels
{
    class OptionsInputManualViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IApplicationCommands _applicationCommands;

        public OptionsInputManualViewModel(IDialogService dialogService, IEventAggregator eventAggregator, IApplicationCommands applicationCommands)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _applicationCommands = applicationCommands;

            BarcodeCollection = new ObservableCollection<string>();

            EditBarcodeListCommand = new DelegateCommand(OpenBarcodeDialog);
            AllowStartManualOptionsCommand = new DelegateCommand(AllowStart).ObservesCanExecute(() => IsDataCorrect);
        }

        public ObservableCollection<string> BarcodeCollection { get; }

        public DelegateCommand EditBarcodeListCommand { get; }
        public DelegateCommand AllowStartManualOptionsCommand { get; }

        public int BarcodesInCollection
        {
            get { return BarcodeCollection.Count; }
        }

        private void OpenBarcodeDialog()
        {
            DialogParameters parameters = new DialogParameters
            {
                { "barcodeCollection", BarcodeCollection }
            };
            _dialogService.ShowDialog("BarcodeInputView", parameters, CloseBarcodeDialogCallback);
        }

        void CloseBarcodeDialogCallback(IDialogResult result)
        {
            ObservableCollection<string> recievedCollection = result.Parameters.GetValue<ObservableCollection<string>>("barcodeCollection");
            if (recievedCollection == null)
                return;

            BarcodeCollection.Clear();
            foreach (var bc in recievedCollection)
                BarcodeCollection.Add(bc);

            RaisePropertyChanged("BarcodesInCollection");
        }

        private void AllowStart()
        {
            _eventAggregator.GetEvent<NotifyAllowStartEvent>().Publish(BarcodeCollection);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _applicationCommands.RunCommand.RegisterCommand(AllowStartManualOptionsCommand);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _applicationCommands.RunCommand.UnregisterCommand(AllowStartManualOptionsCommand);
        }

        #region DataError

        public override void ErrorLogic(string columnName, ref string error)
        {
            switch (columnName)
            {
                case "BarcodesInCollection":

                    if (BarcodesInCollection <= 0)
                    {
                        error = "Количество штрихкодов должно быть больше 0";
                    }

                    break;
            }
        }

        #endregion DataError
    }
}
