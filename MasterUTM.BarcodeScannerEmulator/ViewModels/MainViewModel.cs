using System;
using System.ComponentModel;
using System.Windows;

using Prism.Services.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Infrastructure;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Commands;
using MasterUTM.BarcodeScannerEmulator.Core.Data;
using System.Collections.ObjectModel;

namespace MasterUTM.BarcodeScannerEmulator.ViewModels
{
    public class MainViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IProcessLeaderService _processService;
        private readonly IDialogService _dialogService;
        private readonly IDataService _optionsDataService;

        private const string emptyProcess = "отсутствует";

        public MainViewModel(IProcessLeaderService processService, IDialogService dialogService, IEventAggregator eventAggregator, IRegionManager regionManager, IApplicationCommands applicationCommands, IDataService optionsDataService)
        {
            _regionManager = regionManager;
            _processService = processService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _optionsDataService = optionsDataService;
            ApplicationCommands = applicationCommands;

            _eventAggregator.GetEvent<UpdateActiveProcessEvent>().Subscribe(OnUpdateActiveEvent);
            _eventAggregator.GetEvent<ActiveProcessExitedEvent>().Subscribe(OnActiveProcessExited);
            _eventAggregator.GetEvent<NotifyAllowStartEvent>().Subscribe(RunOnDataRecieved);

            ChooseProcessCommand = new DelegateCommand<string>(OpenChooseProcessDialog);
            RunCommand = new DelegateCommand(StartTransfer).ObservesCanExecute(() => IsDataCorrect);

            ApplicationCommands.RunCommand.RegisterCommand(RunCommand);

            _optionsDataService.LoadValues();
        }

        public DelegateCommand<string> ChooseProcessCommand { get; }
        public DelegateCommand RunCommand { get; }

        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        public string CurrentProcessName
        {
            get { return _processService.CurrentProcess == null? emptyProcess : _processService.CurrentProcess.ProcessName; }
        }

        private int _selectedInputMethod;
        public int SelectedInputMethod
        {
            get { return _selectedInputMethod; }
            set
            {
                SetProperty(ref _selectedInputMethod, value);
                SetInputMethod(_selectedInputMethod);
                _optionsDataService.SetValue("inputMethodIndex", value);
            }
        }

        private void SetInputMethod(int selectedMethod)
        {
            switch (selectedMethod)
            {
                case 0:
                    _regionManager.Regions[RegionNames.InputOptionsRegion].RequestNavigate("OptionsInputManualView");
                    break;

                case 1:
                    _regionManager.Regions[RegionNames.InputOptionsRegion].RequestNavigate("OptionsInputAutoView");
                    break;
            }
        }

        private void RunOnDataRecieved(ObservableCollection<string> recievedCollection)
        {
            DialogParameters parameters = new DialogParameters();
            
            if (recievedCollection == null)
            {
                parameters.Add("barcodeType", _optionsDataService.GetValue<BarcodeType>("barcodeType"));
                parameters.Add("barcodesAmount", _optionsDataService.GetValue<int>("barcodesAmount"));
            }
            else
            {
                parameters.Add("barcodeCollection", recievedCollection);
            }

            parameters.Add("barcodeEnding", _optionsDataService.GetValue<BarcodeEnding>("barcodeEnding"));
            parameters.Add("interval", _optionsDataService.GetValue<double>("interval"));
            parameters.Add("startDelay", _optionsDataService.GetValue<double>("startDelay"));

            _optionsDataService.SaveValues();

            _dialogService.ShowDialog("TransferView", parameters, null);
        }

        private void OnUpdateActiveEvent(string obj)
        {
            RaisePropertyChanged("CurrentProcessName");
            RunCommand.RaiseCanExecuteChanged();
        }

        private void OnActiveProcessExited(string processName)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => ProcessExitedDialog(processName)));
        }

        private void ProcessExitedDialog(string processName)
        {
            string msg = $"Процесс выбранной программы ({processName}) преждевременно завершился!";
            _dialogService.ShowDialog("CustomMessageBoxView", new DialogParameters($"message={msg}&systemIcon=Information&dialogButtons=Ok&title=Процесс завершен"), null);
        }

        private void StartTransfer()
        {
        }

        private void OpenChooseProcessDialog(string processName)
        {
            _dialogService.ShowDialog("ApplicationChoiceView", new DialogParameters($"processName={processName}"), null);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedInputMethod = _optionsDataService.GetValue<int>("inputMethodIndex");
            _regionManager.Regions[RegionNames.GeneralOptionsRegion].RequestNavigate("OptionsGeneralView");
        }

        #region DataError

        public override void ErrorLogic(string columnName, ref string error)
        {
            switch (columnName)
            {
                case "CurrentProcessName":

                    if (CurrentProcessName.Equals(emptyProcess))
                    {
                        error = "Необходимо выбрать приложение!";
                    }
                    break;
            }
        }

        #endregion DataError
    }
}
