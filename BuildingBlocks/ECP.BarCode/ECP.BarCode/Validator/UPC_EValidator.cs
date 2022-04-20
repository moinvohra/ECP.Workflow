using ECP.BarCode.Common;
using System.Text.RegularExpressions;

namespace ECP.BarCode.Validator
{
    class UPC_EValidator: BarCodeValidatorBase
    {
        const int REQUIRED_LEGNTH = 7;
        string INVALID_LENGTH_MESSAGE = "BarCodeGenerater needs 7 number of input characters ";
        string INVALID_ActualLENGTH_MESSAGE = " numbers of input characters";
        string REGEX_MESSAGE = "Input data contains unsupported characters select another barcode type or correct your input data";
        public override bool Validate(string value)
        {
            //check length, regex etc...
            if (value.Length == REQUIRED_LEGNTH)
            {
                var UPCEMatch = Regex.Match(value, RegularExpressionConstant.UPCERegex);

                if (UPCEMatch.Success)
                {
                    return true;

                }
                ErrorMessage = REGEX_MESSAGE;
                return string.IsNullOrEmpty(ErrorMessage);
            }
            //set error message
            ErrorMessage = string.Format(INVALID_LENGTH_MESSAGE + value.Length + INVALID_ActualLENGTH_MESSAGE, REQUIRED_LEGNTH);

            return string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
