using ECP.BarCode.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ECP.BarCode.Validator
{
    class CODE_93Validator : BarCodeValidatorBase
    {
        string REGEX_MESSAGE = "Input data contains unsupported characters select another barcode type or correct your input data";
        public override bool Validate(string value)
        {
            //check length, regex etc...
            var CODE_93Match = Regex.Match(value, RegularExpressionConstant.CODE_93Regex);

            if (CODE_93Match.Success)
            {
                return true;

            }
            ErrorMessage = REGEX_MESSAGE;
            return string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
