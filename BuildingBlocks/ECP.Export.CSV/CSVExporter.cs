using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ECP.Export.CSV
{
    public class CSVExporter: ICSVExporter
    {
        public Task ExportToCSV<T>(Stream outputStream, IEnumerable<T> records)
        {
           using (var writer = new StreamWriter(outputStream))
           using (var csv = new CsvWriter(writer))
           {
               csv.Configuration.TypeConverterCache.AddConverter<List<Custom>>(new MyTypeConverter());
               csv.WriteRecords(records);
               writer.Flush();
               return Task.CompletedTask;
           }
        }
        private class MyClass
        {
            public string Id { get; set; }

            [Index(1, 2)]
            [Name("Property", "Property1")]
            public List<Custom> MyCustoms { get; set; }
        }
        private class Custom
        {
            public string Property1 { get; set; }

            public string Property2 { get; set; }
        }
        private class MyTypeConverter : DefaultTypeConverter
        {
          public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
          {
              var list = (List<Custom>)value;
              var custom = list[0];
              row.WriteField(custom.Property1);
              row.WriteField(custom.Property2);
              return null;
          }

        }
    }
}
