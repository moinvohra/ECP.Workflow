using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ECP.Export.Excel
{
    public interface IExcelExporter
    {
        Task ExportToExcel<T>(Stream outputStream,Stream templateStream, IEnumerable<T> records, string sheetName,
                              ExcelExportStyles headerStyles, ExcelExportStyles dataStyles, int startDataRow, int startDataColumn);
    }
}

