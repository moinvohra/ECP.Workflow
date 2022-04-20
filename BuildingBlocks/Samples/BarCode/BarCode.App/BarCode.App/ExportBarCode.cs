using ECP.BarCode;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace BarCode.App
{
    public class ExportBarCode
    {
        public void BarCodewrite(string[] args, ImageFormat imageFormat, Color foregroundcolor, Color backgroundcolor)
        {
            BarCodeGenerator barCodeGenerator = new BarCodeGenerator();
            try
            {
                barCodeGenerator.Generate(args, imageFormat, foregroundcolor, backgroundcolor);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
