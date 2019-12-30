using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Prism.Events;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Helpers;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;
using MasterUTM.BarcodeScannerEmulator.Core.Data;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Services
{
    public class TransferService : ITransferService
    {
        private const string readyStatus = "Готово к запуску";
        private const string progressStatus = "Идет выполнение...";
        private const string successStatus = "Успешно!";
        private const string failureStatus = "Неудача.";

        private Process workProcess;
        private bool isProcessGood;

        private readonly IEventAggregator _eventAggregator;

        public TransferService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public event EventHandler StatusChanged;

        private string _status = readyStatus;
        public string Status
        {
            get { return _status; }
            set 
            {
                _status = value;
                StatusChanged?.Invoke(null, new EventArgs());
            }
        }

        public async void StartTransfer(Process process, ICollection<string> data, double interval, BarcodeEnding barcodeEnding)
        {
            if (process == null || process.HasExited)
                return;

            if (data == null)
                return;

            if (isProcessGood)
                return;

            Status = progressStatus;
            workProcess = process;
            workProcess.Exited += WorkProcess_Exited;
            isProcessGood = true;

            for (int i = 0; (i < data.Count) && isProcessGood; i++){
                _eventAggregator.GetEvent<TransferedBarcodeIndexEvent>().Publish(i);
                await Task.Run(() => SendMessage(process, data.ElementAt(i), barcodeEnding.StringValue));
                if (i < data.Count - 1)
                    await TimerHelper.WaitForSeconds(interval);
            }
            
            if (isProcessGood)
                StopTransfer(true);
        }

        private void WorkProcess_Exited(object sender, EventArgs e)
        {
            StopTransfer(false);
        }

        private void SendMessage(Process process, params string[] message)
        {
            if (process != null)
            {
                HandleWindow(process);

                if (string.IsNullOrEmpty(message[0]))
                    return;

                for (int i = 0; i < message.Length; i++)
                {
                    InputHelper.SendText(message[i]);
                    //SendKeys.SendWait(message[i]);
                }
            }
        }

        private void HandleWindow(Process process)
        {
            IntPtr handler = process.MainWindowHandle;
            if (WinFunc.IsIconic(handler))
            {
                WinFunc.ShowWindow(handler, 9);
            }

            IntPtr topWindow = WinFunc.GetWindow(handler, WinFunc.GetWindow_Cmd.GW_ENABLEDPOPUP);
            if (topWindow == IntPtr.Zero)
                topWindow = handler;

            /*if (!WinFunc.IsWindowEnabled(topWindow))
            {
                foreach (var winHandler in WinFunc.EnumWindowsByProcess(process.Id))
                {
                    if (WinFunc.IsWindowEnabled(winHandler))
                    {
                        topWindow = winHandler;
                        MessageBox.Show(string.Join(",", WinFunc.EnumChildWindowsList(topWindow)));
                        //break;
                    }
                }
            }*/

            WinFunc.SetForegroundWindow(topWindow);

            return;
        }

        public void StopTransfer(bool isSuccess)
        {
            if (workProcess == null)
                return;

            isProcessGood = false;
            workProcess.Exited -= WorkProcess_Exited;
            workProcess = null;

            if (isSuccess)
                Status = successStatus;
            else
                Status = failureStatus;
        }

        public void RefreshTransfer()
        {
            Status = readyStatus;
            isProcessGood = false;
        }
    }
}
