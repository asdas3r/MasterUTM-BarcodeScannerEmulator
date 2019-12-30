using System;
using System.Collections.Generic;
using System.Diagnostics;

using MasterUTM.BarcodeScannerEmulator.Core.Data;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces
{
    public interface ITransferService
    {
        string Status { get; }

        event EventHandler StatusChanged;

        void StartTransfer(Process process, ICollection<string> data, double interval, BarcodeEnding barcodeEnding);

        void StopTransfer(bool isSuccess);

        void RefreshTransfer();
    }
}
