using System.Threading.Tasks;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Helpers
{
    public static class TimerHelper
    {
        public static async Task WaitForSeconds(double time)
        {
            await Task.Delay((int)time*1000);
        }
    }
}
