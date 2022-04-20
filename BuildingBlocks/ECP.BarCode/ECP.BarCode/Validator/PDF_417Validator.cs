using ECP.BarCode.Common;
using System.Text.RegularExpressions;

namespace ECP.BarCode.Validator
{
    class PDF_417Validator : BarCodeValidatorBase
    {
        string REGEX_MESSAGE = "Input data contains unsupported characters select another barcode type or correct your input data";
        public override bool Validate(string value)
        {
            //check length, regex etc...
            var PDF417Match = Regex.Match(value, RegularExpressionConstant.PDF417Regex);

            if (PDF417Match.Success)
            {
                return true;

            }
            ErrorMessage = REGEX_MESSAGE;
            return string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
