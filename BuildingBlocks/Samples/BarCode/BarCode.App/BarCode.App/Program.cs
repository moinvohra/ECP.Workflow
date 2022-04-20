using ECP.BarCode.Common;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BarCode.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bar Code Generated Started");

            args = new string[12];
            args[BarCodeConstant.BarcodeFormat] = Convert.ToString(BarCodeFormatOriginal.UPC_A);
            ImageFormat imageFormat = ImageFormat.Jpeg;
            args[BarCodeConstant.OutFile] = "UPC_A";
            args[BarCodeConstant.Width] = "300";
            args[BarCodeConstant.Height] = "150";
            args[BarCodeConstant.Value] = "12345678912";
            args[BarCodeConstant.Showlabel] = "false";
            args[BarCodeConstant.Path] = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\", "BarCodeFile"));
            args[BarCodeConstant.Fontfamily] = "Times New Roman";
            args[BarCodeConstant.Fontsize] = "20";
            args[BarCodeConstant.Fontstyle] = Convert.ToString(FontStyle.Regular);
            args[BarCodeConstant.Horizontalresolution] = "100";
            args[BarCodeConstant.Verticalresolution] = "100";

            Color foregroundcolor = Color.Red;
            Color backgroundcolor = Color.White;

            ExportBarCode exportBarCode = new ExportBarCode();
            exportBarCode.BarCodewrite(args, imageFormat, foregroundcolor, backgroundcolor);
            Console.WriteLine("Bar Code Generated Completed");
            Console.ReadKey();
        }
    }
}
