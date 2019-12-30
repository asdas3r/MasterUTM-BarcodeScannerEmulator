using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Prism.Events;
using Prism.Services.Dialogs;

using MasterUTM.BarcodeScannerEmulator.Core.Common;
using MasterUTM.BarcodeScannerEmulator.Core.Data;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Helpers;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;

namespace MasterUTM.BarcodeScannerEmulator.ViewModels
{
    class TransferViewModel : DialogBaseViewModel
    {
        private readonly ITransferService _transferService;
        private readonly IProcessLeaderService _processService;
        private readonly IEventAggregator _eventAggregator;

        public TransferViewModel(IProcessLeaderService processService, ITransferService transferService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _processService = processService;
            _transferService = transferService;

            _eventAggregator.GetEvent<TransferedBarcodeIndexEvent>().Subscribe(OnTransferedBarcode);

            Title = "Передача штрихкодов";

            BarcodeCollection = new ObservableCollection<string>();
        }

        private string _statusText;
        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        private string _activeBarcode;
        public string ActiveBarcode
        {
            get { return _activeBarcode; }
            set { SetProperty(ref _activeBarcode, value); }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public ObservableCollection<string> BarcodeCollection { get; }

        private void OnTransferedBarcode(int num)
        {
            ActiveBarcode = BarcodeCollection[num];
            SelectedItem = BarcodeCollection[num];
        }

        private void TransferService_StatusChanged(object sender, EventArgs e)
        {
            StatusText = _transferService.Status;
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            _transferService.StatusChanged += TransferService_StatusChanged;
            StatusText = "Ожидание загрузки данных...";
            BarcodeCollection.Clear();

            var barcodeEnding = parameters.GetValue<BarcodeEnding>("barcodeEnding");
            var interval = parameters.GetValue<double>("interval");
            var startDelay = parameters.GetValue<double>("startDelay");

            var inputCollection = await Task.Run(() => OnAfterDialogOpenedAsync(parameters));

            foreach (var bc in inputCollection)
                BarcodeCollection.Add(bc);

            _transferService.RefreshTransfer();

            await TimerHelper.WaitForSeconds(startDelay);

            await Task.Run(() =>_transferService.StartTransfer(_processService.CurrentProcess, BarcodeCollection, interval, barcodeEnding));
        }

        private ObservableCollection<string> OnAfterDialogOpenedAsync(IDialogParameters parameters)
        {
            var barcodeType = parameters.GetValue<BarcodeType>("barcodeType");
            var barcodesAmount = parameters.GetValue<int>("barcodesAmount");
            var recievedCollection = parameters.GetValue<ObservableCollection<string>>("barcodeCollection");

            if (recievedCollection == null)
            {
                BarcodeLogic barcodeLogic = new BarcodeLogic();
                recievedCollection = new ObservableCollection<string>();
                for (int i = 0; i < barcodesAmount;)
                {
                    var randomBarcode = barcodeLogic.GetBarcodeByRegex(barcodeType.HardRegexPattern);
                    if (!recievedCollection.Contains(randomBarcode))
                    {
                        recievedCollection.Add(randomBarcode);
                        i++;
                    }
                }
            }

            return recievedCollection;
        }

        public override void OnDialogClosed()
        {
            _eventAggregator.GetEvent<TransferedBarcodeIndexEvent>().Unsubscribe(OnTransferedBarcode);
            _transferService.StatusChanged -= TransferService_StatusChanged;
            _transferService.StopTransfer(false);
        }
    }
}
