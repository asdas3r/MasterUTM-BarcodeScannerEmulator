using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Prism.Events;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Services
{
    public class ProcessLeaderService : IProcessLeaderService
    {
        private readonly IEventAggregator _eventAggregator;
        private Process _currentProcess;

        public ProcessLeaderService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        private void OnProcessExitEvent(object sender, EventArgs e)
        {
            CurrentProcess.Exited -= OnProcessExitEvent;
            OnProcessExit();
        }

        private void OnProcessExit()
        {
            _eventAggregator.GetEvent<ActiveProcessExitedEvent>().Publish(CurrentProcess.ProcessName);
            CurrentProcess = null;
        }

        public Process CurrentProcess
        {
            get { return _currentProcess; }
            set 
            {
                _currentProcess = value;
                if (_currentProcess!= null)
                {
                    if (_currentProcess.HasExited)
                    {
                        OnProcessExit();
                        return;
                    }
                    _currentProcess.EnableRaisingEvents = true;
                    _currentProcess.Exited += OnProcessExitEvent;
                }
                _eventAggregator.GetEvent<UpdateActiveProcessEvent>().Publish(_currentProcess?.ProcessName);
            }
        }

        public IEnumerable<Process> GetProcessList(string nameSubString)
        {
            Process thisProcess = Process.GetCurrentProcess();
            List<Process> processList = new List<Process>(Process.GetProcesses());
            return processList.Where(x => x.ProcessName.ToLower().Contains(nameSubString.Trim().ToLower()) && x.Id != thisProcess.Id && x.MainWindowHandle != IntPtr.Zero).ToList();
        }

        public IEnumerable<Process> GetProcessList(List<string> nameSubStrings)
        {
            Process thisProcess = Process.GetCurrentProcess();
            List<Process> processList = new List<Process>(Process.GetProcesses());
            return processList.Where(x => nameSubStrings.Exists(y => x.ProcessName.ToLower().Contains(y.Trim().ToLower())) && x.Id != thisProcess.Id && x.MainWindowHandle != IntPtr.Zero).ToList();
        }
    }
}
