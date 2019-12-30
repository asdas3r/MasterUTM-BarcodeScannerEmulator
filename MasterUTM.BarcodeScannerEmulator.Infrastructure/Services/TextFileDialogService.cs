using System;
using System.IO;
using Microsoft.Win32;

using Prism.Services.Dialogs;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Services
{
    public class TextFileDialogService : IFileDialogService
    {
        private readonly IDialogService _dialogService;

        public TextFileDialogService(IDialogService dialogService) 
        {
            _dialogService = dialogService;
        }

        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                DefaultExt = "txt",
                Filter = "Text Files (*.txt) | *.txt"
            };

            if (openFile.ShowDialog() == true)
            {
                if (!string.Equals(Path.GetExtension(openFile.FileName), ".txt", StringComparison.OrdinalIgnoreCase))
                {
                    string message = "Ошибка формата исходного файла";
                    _dialogService.ShowDialog("CustomMessageBoxView", new DialogParameters($"message={message}&systemIcon=null&dialogButtons=Ok&title=Ошибка"), null);
                    return false;
                }
                else
                {
                    FilePath = openFile.FileName;
                    return true;
                }
            }

            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFile = new SaveFileDialog
            {
                DefaultExt = "txt",
                Filter = "Text Files (*.txt) | *.txt"
            };

            if (saveFile.ShowDialog() == true)
            {
                if (!string.Equals(Path.GetExtension(saveFile.FileName), ".txt", StringComparison.OrdinalIgnoreCase))
                {
                    FilePath = string.Format(saveFile.FileName + ".txt");
                }
                else
                {
                    FilePath = saveFile.FileName;
                }
                return true;
            }

            return false;
        }
    }
}
