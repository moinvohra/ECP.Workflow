using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Export.CSV
{
    public interface ICSVExporter
    {
        Task ExportToCSV<T>(Stream outputStream, IEnumerable<T> records);
    }
}
