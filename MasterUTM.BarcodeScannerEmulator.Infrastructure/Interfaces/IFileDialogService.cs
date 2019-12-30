namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces
{
    public interface IFileDialogService
    {
        string FilePath { get; set; }

        bool OpenFileDialog();

        bool SaveFileDialog();
    }
}
