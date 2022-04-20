using ECP.Export.CSV;
using System;
using System.Collections.Generic;
using System.IO;

namespace Export.CSV.App
{
    public class ExportCSV
    {
        public async void CSVwrite()
        {
            string fileName = $"Receipt_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
            string filePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\", "CSVFile", fileName));
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(filePath);
            }
            List<Receipt> receipt = new List<Receipt>
            {
                new Receipt { BillTo = "Biller1", Shipper = "Shipper1", Amount = 5000 },
                new Receipt { BillTo = "Biller2", Shipper = "Shipper2", Amount = 10000 },
                new Receipt { BillTo = "Biller3", Shipper = "Shipper3", Amount = 15000 },
                new Receipt { BillTo = "Biller4", Shipper = "Shipper4", Amount = 20000 },
                new Receipt { BillTo = "Biller5", Shipper = "Shipper5", Amount = 25000 },
                new Receipt { BillTo = "Biller6", Shipper = "Shipper6", Amount = 30000 },
                new Receipt { BillTo = "Biller7", Shipper = "Shipper7", Amount = 35000 },
                new Receipt { BillTo = "Biller8", Shipper = "Shipper8", Amount = 40000 },
                new Receipt { BillTo = "Biller9", Shipper = "Shipper9", Amount = 45000 },
                new Receipt { BillTo = "Biller10", Shipper = "Shipper10", Amount = 45000 },
            };

            CSVExporter csvExporter = new CSVExporter();
            await csvExporter.ExportToCSV<Receipt>(new FileStream(filePath, FileMode.Create),receipt);
            Console.WriteLine("Export CSV Completed");
            Console.WriteLine("File Path");
            Console.WriteLine(filePath);
            Console.WriteLine("press any key for stop application");
            Console.ReadKey();
            
        }
    }
}
