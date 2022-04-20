using ECP.BarCode.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ECP.BarCode.Validator
{
    class CODABARValidator:BarCodeValidatorBase
    {
       string REGEX_MESSAGE = "Input data contains unsupported characters select another barcode type or correct your input data";
        public override bool Validate(string value)
        {
            //check length, regex etc...
           var CODABARMatch = Regex.Match(value, RegularExpressionConstant.CODABARRegex);

                if (CODABARMatch.Success)
                {
                    return true;

                }
                ErrorMessage = REGEX_MESSAGE;
          return string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
