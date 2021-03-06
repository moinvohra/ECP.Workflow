using ECP.BarCode.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ECP.BarCode.Validator
{
    class DATA_MATRIXValidator : BarCodeValidatorBase
    {
        string REGEX_MESSAGE = "Input data contains unsupported characters select another barcode type or correct your input data";
        public override bool Validate(string value)
        {
            //check length, regex etc...
            var DATA_MATRIXMatch = Regex.Match(value, RegularExpressionConstant.DATA_MATRIXRegex);

            if (DATA_MATRIXMatch.Success)
            {
                return true;

            }
            ErrorMessage = REGEX_MESSAGE;
            return string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
