using System;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.CoreCompat.System.Drawing;
using ECP.BarCode.Common;
using ECP.BarCode.Validator;
using System.Drawing;
using System.IO;
using ZXing.CoreCompat.Rendering;

namespace ECP.BarCode
{
    public class BarCodeGenerator : IBarCodeGenerator
    {
        bool showlabel = DefaultBarCodeValues.Default_ShowLabelOnly;
        string folderPath = DefaultBarCodeValues.Default_Path;
        string fontfamily = DefaultBarCodeValues.Default_FontFamily;
        float fontsize = DefaultBarCodeValues.Default_FontSize;
        float horizontalresolution = DefaultBarCodeValues.Default_HorizontalResolution;
        float verticalresolution = DefaultBarCodeValues.Default_VerticalResolution;

        [STAThread]
        public void Generate(string[] barcodeparameter, ImageFormat imageFormat, Color foregroundcolor, Color backgroundcolor)
        {
            try
            {
                long startTime = DateTime.Now.Ticks;
                Console.WriteLine(DateTime.Now.Ticks);
                AppDomain.CurrentDomain.UnhandledException +=
                    (sender, exc) => Console.Error.WriteLine(exc.ExceptionObject.ToString());
                if (!string.IsNullOrEmpty(barcodeparameter[BarCodeConstant.Showlabel]))
                {
                    showlabel = Convert.ToBoolean(barcodeparameter[BarCodeConstant.Showlabel]);
                }
                if (!string.IsNullOrEmpty(barcodeparameter[BarCodeConstant.Path]))
                {
                    folderPath = barcodeparameter[BarCodeConstant.Path];
                }
                if (!string.IsNullOrEmpty(barcodeparameter[BarCodeConstant.Fontfamily]))
                {
                    fontfamily = barcodeparameter[BarCodeConstant.Fontfamily];
                }
                if (!string.IsNullOrEmpty(barcodeparameter[BarCodeConstant.Fontsize]))
                {
                    fontsize = Convert.ToInt32(barcodeparameter[BarCodeConstant.Fontsize]);
                }
                if (!string.IsNullOrEmpty(barcodeparameter[BarCodeConstant.Horizontalresolution]))
                {
                    horizontalresolution = Convert.ToInt32(barcodeparameter[BarCodeConstant.Horizontalresolution]);
                }
                if (!string.IsNullOrEmpty(barcodeparameter[BarCodeConstant.Verticalresolution]))
                {
                    verticalresolution = Convert.ToInt32(barcodeparameter[BarCodeConstant.Verticalresolution]);
                }
                var barcodeFormat = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), barcodeparameter[BarCodeConstant.BarcodeFormat]);
                var outFileString = barcodeparameter[BarCodeConstant.OutFile];
                var width = Convert.ToInt32(barcodeparameter[BarCodeConstant.Width]);
                var height = Convert.ToInt32(barcodeparameter[BarCodeConstant.Height]);
                FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), barcodeparameter[BarCodeConstant.Fontstyle]);

                string contents = string.Empty;
                foreach (var arg in barcodeparameter)
                {
                    if (arg == barcodeparameter[BarCodeConstant.Value])
                    {
                        contents = arg;
                        break;
                    }
                }
                var barcodeWriter = new BarcodeWriter
                {
                    Format = barcodeFormat,
                    Renderer = new BitmapRenderer
                    {
                        DpiX = horizontalresolution,
                        DpiY = verticalresolution,
                        Foreground = foregroundcolor,
                        Background = backgroundcolor,
                        TextFont = new Font(fontfamily, fontsize, fontStyle)
                    },
                    Options = new EncodingOptions
                    {
                        Height = height,
                        Width = width,
                        GS1Format = true,
                        PureBarcode = showlabel
                    }
                };
                if (ValidateBarcodeValue(contents, barcodeFormat.ToString()))
                {
                    var bitmap = barcodeWriter.Write(contents);
                    outFileString += '.' + imageFormat.ToString();
                    string file = Path.Combine(folderPath, outFileString);
                    bitmap.Save(file);
                    DateTime dt = new DateTime(startTime);
                    Console.Out.WriteLine(outFileString + " Seconds: " + DateTime.Now.Subtract(dt).Seconds + " Miliseconds: " + DateTime.Now.Subtract(dt).Milliseconds);
                }
                else
                {
                    Console.WriteLine("BarCodeGenerater cannot be created due to format is incorrect");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private bool ValidateBarcodeValue(string value, string barcodetype)
        {
            switch (barcodetype)
            {
                case ValidatorConstant.CODABAR:
                    CODABARValidator cODABARValidator = new CODABARValidator();
                    if (cODABARValidator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(cODABARValidator.ErrorMessage);

                case ValidatorConstant.CODE_39:
                    CODE_39Validator cODE_39Validator = new CODE_39Validator();
                    if (cODE_39Validator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(cODE_39Validator.ErrorMessage);

                case ValidatorConstant.DATA_MATRIX:
                    DATA_MATRIXValidator dATA_MATRIXValidator = new DATA_MATRIXValidator();
                    if (dATA_MATRIXValidator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(dATA_MATRIXValidator.ErrorMessage);

                case ValidatorConstant.CODE_128:
                    CODE_128Validator cODE_128Validator = new CODE_128Validator();
                    if (cODE_128Validator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(cODE_128Validator.ErrorMessage);

                case ValidatorConstant.MSI:
                    MSIValidator mSIValidator = new MSIValidator();
                    if (mSIValidator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(mSIValidator.ErrorMessage);

                case ValidatorConstant.CODE_93:
                    CODE_93Validator cODE_93Validator = new CODE_93Validator();
                    if (cODE_93Validator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(cODE_93Validator.ErrorMessage);

                case ValidatorConstant.UPC_A:
                    UPC_AValidator uPC_AValidator = new UPC_AValidator();
                    if (uPC_AValidator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(uPC_AValidator.ErrorMessage);

                case ValidatorConstant.UPC_E:
                    UPC_EValidator uPC_EValidator = new UPC_EValidator();
                    if (uPC_EValidator.Validate(value))
                    {
                        return true;

                    }
                    throw new ArgumentException(uPC_EValidator.ErrorMessage);

                case ValidatorConstant.EAN_8:
                    EAN_8Validator eAN_8Validator = new EAN_8Validator();
                    if (eAN_8Validator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(eAN_8Validator.ErrorMessage);

                case ValidatorConstant.EAN_13:
                    EAN_13Validator eAN_13Validator = new EAN_13Validator();
                    if (eAN_13Validator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(eAN_13Validator.ErrorMessage);

                case ValidatorConstant.PDF_417:
                    PDF_417Validator pDF_417Validator = new PDF_417Validator();
                    if (pDF_417Validator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(pDF_417Validator.ErrorMessage);

                case ValidatorConstant.PLESSEY:
                    PLESSEYValidator pLESSEYValidator = new PLESSEYValidator();
                    if (pLESSEYValidator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(pLESSEYValidator.ErrorMessage);

                case ValidatorConstant.ITF:
                    ITFValidator iTFValidator = new ITFValidator();
                    if (iTFValidator.Validate(value))
                    {
                        return true;
                    }
                    throw new ArgumentException(iTFValidator.ErrorMessage);

                case ValidatorConstant.QR_CODE:
                    return true;

                case ValidatorConstant.AZTEC:
                    return true;
                default:
                    throw new ArgumentException("BarCodeGenerater format isn't supported.");
            }

        }
    }
}
