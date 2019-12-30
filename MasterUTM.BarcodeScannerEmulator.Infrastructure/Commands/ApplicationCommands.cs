using Prism.Commands;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Commands
{
    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand RunCommand { get; } = new CompositeCommand();
    }
}
