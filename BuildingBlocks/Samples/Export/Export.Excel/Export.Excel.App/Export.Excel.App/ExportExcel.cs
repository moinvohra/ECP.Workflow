using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ECP.Export.Excel;
using OfficeOpenXml.Style;

namespace Export.Excel.App
{
    public class ExportExcel
    {

        public async void Excelwrite()
        {
            string fileName = $"Receipt_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            string filePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\", "ExcelFile", fileName));
            string templatefileName = "TestTemplate.xlsx";
            string templateFilepath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\", "ExcelFile", templatefileName));
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(filePath);
            }
            List<Receipt> receipt = new List<Receipt>
            {
                new Receipt { BillTo = "Biller1",  Shipper = "Shipper1",  Amount = 5000  },
                new Receipt { BillTo = "Biller2",  Shipper = "Shipper2",  Amount = 10000 },
                new Receipt { BillTo = "Biller3",  Shipper = "Shipper3",  Amount = 15000 },
                new Receipt { BillTo = "Biller4",  Shipper = "Shipper4",  Amount = 20000 },
                new Receipt { BillTo = "Biller5",  Shipper = "Shipper5",  Amount = 25000 },
                new Receipt { BillTo = "Biller6",  Shipper = "Shipper6",  Amount = 30000 },
                new Receipt { BillTo = "Biller7",  Shipper = "Shipper7",  Amount = 35000 },
                new Receipt { BillTo = "Biller8",  Shipper = "Shipper8",  Amount = 40000 },
                new Receipt { BillTo = "Biller9",  Shipper = "Shipper9",  Amount = 45000 },
                new Receipt { BillTo = "Biller10", Shipper = "Shipper10", Amount = 45000 },
            };
            ExcelExportStyles headerStyles = new ExcelExportStyles();
            headerStyles.fontBold = true;
            headerStyles.patternType = ExcelFillStyle.Solid;
            headerStyles.backGroundColor = Color.YellowGreen;
            headerStyles.fontColor = Color.Black;
            headerStyles.borderStyle = ExcelBorderStyle.Thick;

            ExcelExportStyles dataStyles = new ExcelExportStyles();
            dataStyles.fontBold = true;
            dataStyles.patternType = ExcelFillStyle.DarkDown;
            dataStyles.backGroundColor = Color.White;
            dataStyles.fontColor = Color.Black;
            dataStyles.borderStyle = ExcelBorderStyle.Thin;
            int startDataRow = 8;
            int startDataColumn = 8;
            ExcelExporter excelExporter = new ExcelExporter();
           await excelExporter.ExportToExcel<Receipt>
           (new FileStream(filePath, FileMode.Create),
            new FileStream(templateFilepath, FileMode.Open),
            receipt, "summary", headerStyles, dataStyles, startDataRow, startDataColumn
           );
            Console.WriteLine("Export Excel Completed");
            Console.WriteLine("File Path");
            Console.WriteLine(filePath);
            Console.WriteLine("press any key for stop application");
            Console.ReadKey();
            
        }
    }
}
