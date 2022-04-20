using System.Drawing;
using System.Drawing.Imaging;

namespace ECP.BarCode
{
    public interface IBarCodeGenerator
    {
        void Generate(string[] barcodeparameter, ImageFormat imageFormat, Color foregroundcolor, Color backgroundcolor);
    }
}
