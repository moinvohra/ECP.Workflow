
namespace ECP.BarCode.Validator
{
    public abstract class BarCodeValidatorBase
    {
        public string ErrorMessage { get; set; }
        public abstract bool Validate(string value);
    }
}
