using ECP.BarCode.Common;
using System.Text.RegularExpressions;

namespace ECP.BarCode.Validator
{
    public class UPC_AValidator : BarCodeValidatorBase
    {
        const int REQUIRED_LEGNTH = 11;
        string INVALID_LENGTH_MESSAGE = "BarCodeGenerater needs 11 numbers of input characters you are providing ";
        string INVALID_ActualLENGTH_MESSAGE = " numbers of input characters";
        string REGEX_MESSAGE = "Input data contains unsupported characters select another barcode type or correct your input data";
        public override bool Validate(string value)
        {
            //check length, regex etc...
            if (value.Length == REQUIRED_LEGNTH)
            {
                var UPCAMatch = Regex.Match(value, RegularExpressionConstant.UPCARegex);

                if (UPCAMatch.Success)
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
