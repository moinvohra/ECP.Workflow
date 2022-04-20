using System;

namespace Export.CSV.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Export CSV Started");
            ExportCSV exportCSV = new ExportCSV();
            exportCSV.CSVwrite();
        }
    }
}
