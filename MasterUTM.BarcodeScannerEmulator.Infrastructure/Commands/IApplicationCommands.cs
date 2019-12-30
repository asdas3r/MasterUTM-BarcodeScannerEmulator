using Prism.Commands;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Commands
{
    public interface IApplicationCommands
    {
        CompositeCommand RunCommand { get; }
    }
}
