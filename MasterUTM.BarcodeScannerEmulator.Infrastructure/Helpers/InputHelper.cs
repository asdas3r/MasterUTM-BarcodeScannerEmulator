using WindowsInput;
using WindowsInput.Native;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Helpers
{
    public static class InputHelper
    {
        public static void SendText(string text)
        {
            InputSimulator inputSimulator = new InputSimulator();

            if (string.IsNullOrEmpty(text))
                return;

            if (text.Equals("{ENTER}"))
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            else if (text.Equals("\r"))
                inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_M);
            else if (text.Equals("\n"))
                inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_J);
            else
                inputSimulator.Keyboard.TextEntry(text);
        }
    }
}
