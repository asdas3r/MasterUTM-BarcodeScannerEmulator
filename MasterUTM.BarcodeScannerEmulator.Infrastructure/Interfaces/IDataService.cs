using System.Collections.Generic;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces
{
    public interface IDataService
    {
        Dictionary<string, object> DataDictionary { get; set; }

        T GetValue<T>(string key);

        void SetValue<T>(string key, T value);

        void LoadValues();

        void SaveValues();
    }
}
