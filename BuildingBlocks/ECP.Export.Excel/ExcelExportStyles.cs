using OfficeOpenXml.Style;
using System.Drawing;

namespace ECP.Export.Excel
{
    public class ExcelExportStyles
    {
        public bool fontBold { get; set; }
        public ExcelFillStyle patternType { get; set; }
        public Color backGroundColor { get; set; }
        public Color fontColor { get; set; }
        public ExcelBorderStyle borderStyle { get; set; }
    }
}
