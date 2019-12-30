using System.Collections.Generic;
using System.Diagnostics;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces
{
    public interface IProcessLeaderService
    {
        Process CurrentProcess { get; set; }

        IEnumerable<Process> GetProcessList(string nameSubString);

        IEnumerable<Process> GetProcessList(List<string> nameSubStrings);
    }
}
