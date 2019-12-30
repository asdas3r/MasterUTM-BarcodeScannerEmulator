using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Services
{
    public class TextFileService : IFileService<List<string>>
    {
        public string ErrorString { get; set; }

        public List<string> GetFromFile(string filePath)
        {
            ErrorString = string.Empty;
            try
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                if (fileStream == null)
                {
                    ErrorString = $"Невозможно открыть файл для чтения. Путь файла: {filePath}";
                    return default;
                }

                var list = new List<string>();
                using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                ErrorString = e.Message;
                return default;
            }
        }

        public void SaveToFile(string filePath, List<string> objectToSave)
        {
            // 
        }
    }
}
