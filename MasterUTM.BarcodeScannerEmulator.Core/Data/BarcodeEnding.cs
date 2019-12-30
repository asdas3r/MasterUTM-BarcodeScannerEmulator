using System;
using System.Collections.Generic;

namespace MasterUTM.BarcodeScannerEmulator.Core.Data
{
    [Serializable]
    public class BarcodeEnding
    {
        public string Name { get; set; }

        public string StringValue { get; set; }
    }

    public static class BarcodeEndings
    {
        public static BarcodeEnding LineFeed
        {
            get
            {
                return new BarcodeEnding()
                {
                    Name = "Перевод строки",
                    StringValue = "\n"
                };
            }
        }

        public static BarcodeEnding CarriageReturn
        {
            get
            {
                return new BarcodeEnding()
                {
                    Name = "Возврат каретки",
                    StringValue = "\r"
                };
            }
        }

        public static BarcodeEnding Enter
        {
            get
            {
                return new BarcodeEnding()
                {
                    Name = "Клавиша 'Enter'",
                    StringValue = "{ENTER}"
                };
            }
        }

        public static BarcodeEnding None
        {
            get
            {
                return new BarcodeEnding()
                {
                    Name = "Отсутствует",
                    StringValue = null
                };
            }
        }

        public static List<BarcodeEnding> AllList
        {
            get
            {
                return new List<BarcodeEnding>()
                {
                    None,
                    CarriageReturn,
                    LineFeed,
                    Enter
                };
            }
        }
    }
}
