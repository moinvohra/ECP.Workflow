using ECP.BarCode.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ECP.BarCode.Validator
{
    class ITFValidator : BarCodeValidatorBase
    {
        const int REQUIRED_LEGNTH = 14;
        string INVALID_LENGTH_MESSAGE = "BarCodeGenerater needs 14 number of input characters ";
        string INVALID_ActualLENGTH_MESSAGE = " numbers of input characters";
        string REGEX_MESSAGE = "Input data contains unsupported characters select another barcode type or correct your input data";
        public override bool Validate(string value)
        {
            //check length, regex etc...
            if (value.Length == REQUIRED_LEGNTH)
            {
                var ITFMatch = Regex.Match(value, RegularExpressionConstant.ITFRegex);

                if (ITFMatch.Success)
                {
                    return true;

                }
                ErrorMessage = REGEX_MESSAGE;
                return string.IsNullOrEmpty(ErrorMessage);
            }
            ErrorMessage = string.Format(INVALID_LENGTH_MESSAGE + value.Length + INVALID_ActualLENGTH_MESSAGE, REQUIRED_LEGNTH);
            return string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
