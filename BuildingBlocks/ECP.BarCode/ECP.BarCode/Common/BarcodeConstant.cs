
namespace ECP.BarCode.Common
{
    public static class BarCodeConstant
    {
        public const int BarcodeFormat = 0;
        public const int OutFile = 1;
        public const int Width = 2;
        public const int Height = 3;
        public const int Value = 4;
        public const int Showlabel = 5;
        public const int Path = 6;
        public const int Fontfamily = 7;
        public const int Fontsize = 8;
        public const int Fontstyle = 9;
        public const int Horizontalresolution = 10;
        public const int Verticalresolution = 11;
    }
    public static class ImageConstant
    {
        public const string bmp = "bmp";
        public const string emf = "emf";
        public const string gif = "gif";
        public const string icon = "icon";
        public const string jpeg = "jpeg";
        public const string jpg = "jpg";
        public const string png = "png";
        public const string tiff = "tiff";
        public const string wmf = "wmf";
        public const string exif = "exif";
    }
    public static class RegularExpressionConstant
    {
        public const string CODABARRegex = @"^[A-D][0-9\+$:\-/.]*[A-D]$";
        public const string CODE_39Regex = @"^[0-9A-Z\-.$/\+%\*\s]*$";
        public const string DATA_MATRIXRegex = @"^[\x00-\xff]*$";
        public const string CODE_128Regex = @"^[\000-\177]*$";
        public const string MSIRegex = @"^\d{1,}$";
        public const string CODE_93Regex = @"^[0-9A-Z\-.$/\+%\*\s]*$";
        public const string UPCARegex = @"^\d{1,}$";
        public const string UPCERegex = @"^\d{1,}$";
        public const string EAN_8Regex = @"^\d{1,}$";
        public const string EAN_13Regex = @"^\d{1,}$";
        public const string PDF417Regex = @"^[\011\012\015\040-\177]*$";
        public const string ITFRegex = @"^[0-9]*$";
        public const string PLESSEYRegex = @"^[0-9,A-F]*$";
        
    }
    public static class ValidatorConstant
    {
        public const string CODABAR = "CODABAR";
        public const string CODE_39 = "CODE_39";
        public const string DATA_MATRIX = "DATA_MATRIX";
        public const string CODE_128 = "CODE_128";
        public const string MSI = "MSI";
        public const string CODE_93 = "CODE_93";
        public const string UPC_A = "UPC_A";
        public const string UPC_E = "UPC_E";
        public const string EAN_8 = "EAN_8";
        public const string EAN_13 = "EAN_13";
        public const string PDF_417 = "PDF_417";
        public const string PLESSEY = "PLESSEY";
        public const string QR_CODE = "QR_CODE";
        public const string AZTEC = "AZTEC";
        public const string ITF = "ITF";
    }
}
