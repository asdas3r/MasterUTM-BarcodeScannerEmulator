using System.Collections.Generic;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces
{
    public interface IFileService <T>
    {
        string ErrorString { get; set; }

        T GetFromFile(string filePath);

        void SaveToFile(string filePath, T objectToSave);
    }
}
