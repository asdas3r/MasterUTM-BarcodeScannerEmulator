using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Prism.Commands;
using Prism.Services.Dialogs;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;

namespace MasterUTM.BarcodeScannerEmulator.ViewModels
{
    public class ApplicationChoiceViewModel : DialogBaseViewModel
    {
        private readonly IProcessLeaderService _processService;
        private readonly IFileService<List<string>> _textFileService;

        private ObservableCollection<Process> activeProcesses = new ObservableCollection<Process>();
        public ReadOnlyObservableCollection<Process> ActiveProcesses { get; }

        public ApplicationChoiceViewModel(IProcessLeaderService processService, IFileService<List<string>> textFileService)
        {
            _processService = processService;
            _textFileService = textFileService;

            RefreshList();
            Title = "Выбор процесса";

            ActiveProcesses = new ReadOnlyObservableCollection<Process>(activeProcesses);

            RefreshCommand = new DelegateCommand(RefreshList);
            SetCurrentProcessCommand = new DelegateCommand<object>(SetCurrentProcess);
        }

        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand<object> SetCurrentProcessCommand { get; }

        private Process selectedProcess;
        public Process SelectedProcess
        {
            get { return selectedProcess; }
            set { SetProperty(ref selectedProcess, value); }
        }

        private void SetCurrentProcess(object selectedProcess)
        {
            var getProcess = selectedProcess as Process; 
            if (getProcess != null)
            {
                _processService.CurrentProcess = getProcess;
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            } 
        }

        private void RefreshList()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "AllowedProcesses.txt");
            List<string> fileList = _textFileService.GetFromFile(filePath);
            InitActiveProcessesCollection(fileList);
        }

        void InitActiveProcessesCollection(string name)
        {
            activeProcesses.Clear();
            foreach (var p in _processService.GetProcessList(name))
            {
                activeProcesses.Add(p);
            }
        }

        void InitActiveProcessesCollection(List<string> names)
        {
            activeProcesses.Clear();
            foreach (var p in _processService.GetProcessList(names))
            {
                activeProcesses.Add(p);
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            string processName = parameters.GetValue<string>("processName");

            RefreshList();

            if (processName == null)
                return;

            foreach (var p in ActiveProcesses)
            {
                if (processName.Equals(p.ProcessName))
                {
                    SelectedProcess = p;
                    break;
                }
            }
        }
    }
}
