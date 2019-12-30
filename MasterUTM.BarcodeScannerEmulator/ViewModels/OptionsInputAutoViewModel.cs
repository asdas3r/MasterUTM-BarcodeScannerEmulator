using System.Collections.Generic;
using System.ComponentModel;

using Prism.Commands;
using Prism.Events;
using Prism.Regions;

using MasterUTM.BarcodeScannerEmulator.Core.Data;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Commands;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;

namespace MasterUTM.BarcodeScannerEmulator.ViewModels
{
    class OptionsInputAutoViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IApplicationCommands _applicationCommands;
        private readonly IDataService _optionsDataService;

        public OptionsInputAutoViewModel(IEventAggregator eventAggregator, IApplicationCommands applicationCommands, IDataService optionsDataService)
        {
            _eventAggregator = eventAggregator;
            _applicationCommands = applicationCommands;
            _optionsDataService = optionsDataService;

            BarcodeTypeList = BarcodeTypes.AllList;

            InitData();

            AllowStartAutoOptionsCommand = new DelegateCommand(AllowStart).ObservesCanExecute(() => IsDataCorrect);
        }

        public DelegateCommand AllowStartAutoOptionsCommand { get; }

        public List<BarcodeType> BarcodeTypeList { get; }

        private BarcodeType _selectedBarcodeType;
        public BarcodeType SelectedBarcodeType
        {
            get { return _selectedBarcodeType; }
            set 
            {
                BarcodeType setVal;
                if (value != null && BarcodeTypeList.Exists(x => x.Name.Equals(value.Name)))
                    setVal = BarcodeTypeList.FindLast(x => x.Name.Equals(value.Name));
                else
                    setVal = BarcodeTypeList[0];

                SetProperty(ref _selectedBarcodeType, setVal);
                _optionsDataService.SetValue("barcodeType", setVal);
            }
        }

        private int _barcodesAmount;
        public int BarcodesAmount
        {
            get { return _barcodesAmount; }
            set
            {
                SetProperty(ref _barcodesAmount, value);
                _optionsDataService.SetValue("barcodesAmount", value);
            }
        }

        private void AllowStart()
        {
            _eventAggregator.GetEvent<NotifyAllowStartEvent>().Publish(null);
        }

        private void InitData()
        {
            SelectedBarcodeType = _optionsDataService.GetValue<BarcodeType>("barcodeType");
            BarcodesAmount = _optionsDataService.GetValue<int>("barcodesAmount");
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _applicationCommands.RunCommand.RegisterCommand(AllowStartAutoOptionsCommand);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _applicationCommands.RunCommand.UnregisterCommand(AllowStartAutoOptionsCommand);
        }

        #region DataError

        public override void ErrorLogic(string columnName, ref string error)
        {
            switch (columnName)
            {
                case "BarcodesAmount":

                    if (BarcodesAmount <= 0)
                    {
                        error = "Количество штрихкодов должно быть больше 0";
                    }
                    if (BarcodesAmount >= 1000)
                    {
                        error = "Количество штрихкодов должно быть меньше 1000";
                    }

                    break;
            }
        }

        #endregion DataError
    }
}
