using System.Collections.Generic;
using System.ComponentModel;

using Prism.Events;
using Prism.Commands;
using Prism.Regions;

using MasterUTM.BarcodeScannerEmulator.Core.Data;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Commands;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;

namespace MasterUTM.BarcodeScannerEmulator.ViewModels
{
    public class OptionsGeneralViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IApplicationCommands _applicationCommands;
        private readonly IDataService _optionsDataService;

        public OptionsGeneralViewModel(IEventAggregator eventAggregator, IApplicationCommands applicationCommands, IDataService optionsDataService)
        {
            _eventAggregator = eventAggregator;
            _applicationCommands = applicationCommands;
            _optionsDataService = optionsDataService;

            BarcodeEndingList = BarcodeEndings.AllList;

            InitData();

            StartTransferGeneralOptionsCommand = new DelegateCommand(StartTransfer).ObservesCanExecute(() => IsDataCorrect);
        }

        private void StartTransfer()
        {
        }

        public DelegateCommand StartTransferGeneralOptionsCommand { get; }

        public List<BarcodeEnding> BarcodeEndingList { get; }

        private BarcodeEnding _selectedBarcodeEnding;
        public BarcodeEnding SelectedBarcodeEnding
        {
            get { return _selectedBarcodeEnding; }
            set
            {
                BarcodeEnding setVal;
                if (value != null && BarcodeEndingList.Exists(x => x.Name.Equals(value.Name)))
                    setVal = BarcodeEndingList.FindLast(x => x.Name.Equals(value.Name));
                else
                    setVal = BarcodeEndingList[0];

                SetProperty(ref _selectedBarcodeEnding, setVal);
                _optionsDataService.SetValue("barcodeEnding", setVal);
            }
        }

        private double _interval;
        public double Interval
        {
            get { return _interval; }
            set
            {
                SetProperty(ref _interval, value);
                _optionsDataService.SetValue("interval", value);
            }
        }

        private double _startDelay;
        public double StartDelay
        {
            get { return _startDelay; }
            set
            {
                SetProperty(ref _startDelay, value);
                _optionsDataService.SetValue("startDelay", value);
            }
        }

        private void InitData()
        {
            Interval = _optionsDataService.GetValue<double>("interval");
            StartDelay = _optionsDataService.GetValue<double>("startDelay");
            SelectedBarcodeEnding = _optionsDataService.GetValue<BarcodeEnding>("barcodeEnding");
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _applicationCommands.RunCommand.RegisterCommand(StartTransferGeneralOptionsCommand);
        }

        #region DataError

        public override void ErrorLogic(string columnName, ref string error)
        {
            switch (columnName)
            {
                case "Interval":

                    if (Interval <= 0)
                    {
                        error = "Значение интервала должно быть больше 0";
                    }
                    if (Interval > 120)
                    {
                        error = "Значение интервала должно быть не больше 120";
                    }
                    break;

                case "StartDelay":

                    if (StartDelay < 0)
                    {
                        error = "Значение задержки не должно быть отрицательным";
                    }
                    if (StartDelay > 120)
                    {
                        error = "Значение задержки должно быть не больше 120";
                    }
                    break;
            }
        }

        #endregion DataError
    }
}
