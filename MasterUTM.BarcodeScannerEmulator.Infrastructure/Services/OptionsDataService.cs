using System.IO;
using System.Collections.Generic;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Helpers;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Services
{
    public class OptionsDataService : IDataService
    {
        private const string binaryFileName = "options.b";

        public Dictionary<string, object> DataDictionary { get; set; } = new Dictionary<string, object>();

        public T GetValue<T>(string key)
        {
            if (DataDictionary.ContainsKey(key))
                return (T)DataDictionary[key];
            else
                return default;
        }

        public void SetValue<T>(string key, T value)
        {
            if (DataDictionary.ContainsKey(key))
                DataDictionary[key] = value;
            else
                DataDictionary.Add(key, value);
        }

        public void LoadValues()
        {
            try
            {
                Stream stream = new FileStream(binaryFileName, FileMode.Open, FileAccess.Read);
                DataDictionary = BinarySerializeHelper.Deserialize<Dictionary<string, object>>(stream);
                if (DataDictionary == null)
                    DataDictionary = new Dictionary<string, object>();
            }
            catch
            {
                DataDictionary = new Dictionary<string, object>();
            }
            
        }

        public void SaveValues()
        {
            Stream stream = new FileStream(binaryFileName, FileMode.Create);
            BinarySerializeHelper.Serialize(DataDictionary ,stream);
        }
    }
}
