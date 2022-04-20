using System;

namespace Export.Excel.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Export Excel Started");
            ExportExcel exportExcel = new ExportExcel();
            exportExcel.Excelwrite();
        }
    }
}
