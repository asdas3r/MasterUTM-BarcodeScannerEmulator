using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Fare;

using MasterUTM.BarcodeScannerEmulator.Core.Data;

namespace MasterUTM.BarcodeScannerEmulator.Core.Common
{
    public class BarcodeLogic
    {
        private Random random = new Random();
        public IReadOnlyList<BarcodeType> BarcodeTypeList;

        public BarcodeLogic()
        {
            BarcodeTypeList = new List<BarcodeType>(BarcodeTypes.AllList);
        }

        public string GetBarcodeByRegex(string regexPattern)
        {
            Xeger xeger = new Xeger(regexPattern, random);
            return xeger.Generate();
        }

        public BarcodeType ValidateBarcode(string barcode, out string error, bool validateByHardPattern, BarcodeType barcodeType = null)
        {
            error = string.Empty;

            const string hardPatternError = "Штрихкод не прошел валидацию. Введите корректный штрихкод";
            const string softPatternError = "Штрихкод содержит недопустимые символы";
            const string lengthError = "Длина заданного штрихкода не соответствует необходимой";

            if (string.IsNullOrWhiteSpace(barcode))
            {
                error = "Штрихкод не может быть пустым";
                return null;
            }

            if (barcodeType != null)
            {
                if (!ValidateByLength(barcode, barcodeType.Length))
                {
                    error = lengthError;
                    return null;
                }

                if (validateByHardPattern)
                {
                    if (!ValidateByPattern(barcode, barcodeType.HardRegexPattern))
                    {
                        error = hardPatternError;
                        return null;
                    }
                }
                else
                {
                    if (!ValidateByPattern(barcode, barcodeType.SoftRegexPattern))
                    {
                        error = softPatternError;
                        return null;
                    }
                }

                return barcodeType;
            }
            else
            {
                Dictionary<string, int> errorsDictionary = new Dictionary<string, int>();

                string tempError = string.Empty;

                foreach (var bc in BarcodeTypeList)
                {
                    if (ValidateBarcode(barcode, out tempError, validateByHardPattern, bc) != null)
                        return bc;

                    if (errorsDictionary.ContainsKey(tempError))
                        errorsDictionary[tempError]++;
                    else
                        errorsDictionary[tempError] = 0;
                }

                string patternError = validateByHardPattern ? hardPatternError : softPatternError;

                if (errorsDictionary.ContainsKey(patternError))
                {
                    error = patternError;
                    return null;
                }
                else
                {
                    int min = BarcodeTypeList.Min(x => x.Length);
                    if (barcode.Length < min)
                    {
                        error = "Длина введенного штрихкода не соответствует минимальной длине штрихкодов (" + BarcodeTypeList.Min(x => x.Length) + ")";
                        return null;
                    }
                    else
                    {
                        error = lengthError;
                        return null;
                    }
                }
            }
        }

        private bool ValidateByPattern(string barcode, string hardPattern)
        {
            Regex regex = new Regex(hardPattern);
            return regex.IsMatch(barcode);
        }

        private bool ValidateByLength(string barcode, int length)
        {
            return barcode.Length == length;
        }

    }
}
