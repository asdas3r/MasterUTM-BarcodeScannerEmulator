using System;
using System.Collections.Generic;

namespace MasterUTM.BarcodeScannerEmulator.Core.Data
{
    [Serializable]
    public class BarcodeType
    {
        public string Name { get; set; }
        public string HardRegexPattern { get; set; }
        public string SoftRegexPattern { get; set; }
        public int Length { get; set; }
    }

    public static class BarcodeTypes
    {
        public static BarcodeType Box
        {
            get
            {
                return new BarcodeType()
                {
                    Name = "Штрихкод коробки",
                    HardRegexPattern = @"([1-9]\d{11}|\d([1-9]\d{10}|\d([1-9]\d{9}|\d([1-9]\d{8}|\d([1-9]\d{7}|\d([1-9]\d{6}|\d([1-9]\d{5}|\d([1-9]\d{4}|\d([1-9]\d{3}|\d([1-9]\d{2}|\d[1-9]\d))))))))))([1-4])(\d[1-9]|[1-9]\d)([0-1]\d)([1-9]\d{8}|\d([1-9]\d{7}|\d([1-9]\d{6}|\d([1-9]\d{5}|\d([1-9]\d{4}|\d([1-9]\d{3}|\d([1-9]\d{2}|\d[1-9]\d)))))))",
                    SoftRegexPattern = @"^[0-9]+$",
                    Length = 26
                };
            }
        }

        public static BarcodeType Palette
        {
            get
            {
                return new BarcodeType()
                {
                    Name = "Штрихкод паллеты",
                    HardRegexPattern = @"([1-9]\d{11}|\d([1-9]\d{10}|\d([1-9]\d{9}|\d([1-9]\d{8}|\d([1-9]\d{7}|\d([1-9]\d{6}|\d([1-9]\d{5}|\d([1-9]\d{4}|\d([1-9]\d{3}|\d([1-9]\d{2}|\d[1-9]\d))))))))))([1-9]\d{5}|\d([1-9]\d{4}|\d([1-9]\d{3}|\d([1-9]\d{2}|\d[1-9]\d))))",
                    SoftRegexPattern = @"^[0-9]+$",
                    Length = 18
                };
            }
        }

        public static BarcodeType OldBottle
        {
            get
            {
                return new BarcodeType()
                {
                    Name = "Штрихкод бутылки (старый формат)",
                    HardRegexPattern = @"\d\d[A-Za-z0-9]{21}(\d[0-1])(\d[0-3])(\d{10})([A-Za-z0-9]{31})",
                    SoftRegexPattern = @"^[A-Za-z\d]+$",
                    Length = 68
                };
            }
        }

        public static BarcodeType NewBottle
        {
            get
            {
                return new BarcodeType()
                {
                    Name = "Штрихкод бутылки (новый формат)",
                    HardRegexPattern = @"([1-9]\d{2}|\d([1-9]\d|\d[1-9])){2}([1-9]\d{7}|\d([1-9]\d{6}|\d([1-9]\d{5}|\d([1-9]\d{4}|\d([1-9]\d{3}|\d([1-9]\d{2}|\d([1-9]\d|\d[1-9])))))))(0[1-9]|1[0-2])(1[8-9]|[2-9][0-9])([1-9]\d{2}|\d([1-9]\d|\d[1-9]))[0-9A-Z]{129}",
                    SoftRegexPattern = @"^[0-9A-Z]+$",
                    Length = 150
                };
            }
        }

        public static List<BarcodeType> AllList
        {
            get
            {
                return new List<BarcodeType>()
                {
                    Box,
                    Palette,
                    OldBottle,
                    NewBottle
                };
            }
        }
    }
}
