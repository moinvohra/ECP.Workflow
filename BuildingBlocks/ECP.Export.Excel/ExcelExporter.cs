using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ECP.Export.Excel
{
    public class ExcelExporter : IExcelExporter
    {
        public Task ExportToExcel<T>(Stream outputStream, Stream templateStream, IEnumerable<T> records, string sheetName, ExcelExportStyles headerStyles, ExcelExportStyles dataStyles, int startDataRow, int startDataColumn)
        {
            try
            {
                startDataRow = startDataRow > 0 ? startDataRow : 1;
                startDataColumn = startDataColumn > 0 ? startDataColumn : 1;
                int recordCounter = 0;

                int columnCounter = 0;
                using (var package = new ExcelPackage(outputStream, templateStream))
                {
                    var workSheet = package.Workbook.Worksheets[sheetName];

                    var properties = typeof(T).GetProperties()
                                            .Where(prop => prop.IsDefined(typeof(OutputColumn), false)).ToList();

                    if (properties.Count > 0)
                    {
                        //write style
                        if (headerStyles != null)
                            using (var range = workSheet.Cells[startDataRow, startDataColumn, startDataRow, startDataColumn + properties.Count - 1])
                            {
                                range.Style.Font.Bold = headerStyles.fontBold;
                                range.Style.Fill.PatternType = headerStyles.patternType;
                                range.Style.Fill.BackgroundColor.SetColor(headerStyles.backGroundColor);
                                range.Style.Font.Color.SetColor(headerStyles.fontColor);
                                range.Style.Border.Top.Style = headerStyles.borderStyle;
                                range.Style.Border.Left.Style = headerStyles.borderStyle;
                                range.Style.Border.Right.Style = headerStyles.borderStyle;
                                range.Style.Border.Bottom.Style = headerStyles.borderStyle;
                            }

                        string[] sortedProperties = new string[properties.Count];
                        PropertyInfo[] sortedPropertyInfo = new PropertyInfo[properties.Count];

                        //Write header Row
                        foreach (var prop in properties)
                        {
                            OutputColumn firstProperty = (OutputColumn)Attribute.GetCustomAttribute(prop, typeof(OutputColumn));
                            workSheet.Cells[startDataRow, startDataRow + firstProperty.ColumnIndex - 1].Value = firstProperty.ColumnName;
                            sortedProperties[firstProperty.ColumnIndex - 1] = firstProperty.ColumnName;
                            sortedPropertyInfo[firstProperty.ColumnIndex - 1] = prop;
                        }

                        recordCounter = startDataRow + 1;
                        //Write Records row
                        foreach (T item in records)
                        {
                            columnCounter = startDataColumn;
                            foreach (var prop in sortedPropertyInfo)
                            {
                                workSheet.Cells[recordCounter, columnCounter].Value = prop.GetValue(item);
                                columnCounter++;
                            }
                            recordCounter++;
                        }

                        //write style
                        if (dataStyles != null)
                            using (var range = workSheet.Cells[startDataRow + 1, startDataColumn, recordCounter - 1, startDataColumn + properties.Count - 1])
                            {
                                range.Style.Font.Bold = dataStyles.fontBold;
                                range.Style.Fill.PatternType = dataStyles.patternType;
                                range.Style.Fill.BackgroundColor.SetColor(dataStyles.backGroundColor);
                                range.Style.Font.Color.SetColor(dataStyles.fontColor);
                                range.Style.Border.Top.Style = dataStyles.borderStyle;
                                range.Style.Border.Left.Style = dataStyles.borderStyle;
                                range.Style.Border.Right.Style = dataStyles.borderStyle;
                                range.Style.Border.Bottom.Style = dataStyles.borderStyle;
                            }
                    }
                    else
                    {
                        var defaultProperties = typeof(T).GetProperties();

                        string[] sorteddefaultProperties = new string[defaultProperties.ToList().Count];
                        PropertyInfo[] sorteddefaultPropertyInfo = new PropertyInfo[defaultProperties.ToList().Count];

                        columnCounter = 0;
                        int startDataColumnProperties = startDataColumn;
                        foreach (var prop in defaultProperties)
                        {
                            workSheet.Cells[startDataRow, startDataColumnProperties].Value = prop.Name;
                            sorteddefaultProperties[columnCounter] = prop.Name;
                            sorteddefaultPropertyInfo[columnCounter] = prop;
                            columnCounter++;
                            startDataColumnProperties++;
                        }

                        //style
                        if (headerStyles != null)
                            using (var range = workSheet.Cells[startDataRow, startDataColumn, startDataRow, startDataColumn + columnCounter - 1])
                            {
                                range.Style.Font.Bold = headerStyles.fontBold;
                                range.Style.Fill.PatternType = headerStyles.patternType;
                                range.Style.Fill.BackgroundColor.SetColor(headerStyles.backGroundColor);
                                range.Style.Font.Color.SetColor(headerStyles.fontColor);
                                range.Style.Border.Top.Style = headerStyles.borderStyle;
                                range.Style.Border.Left.Style = headerStyles.borderStyle;
                                range.Style.Border.Right.Style = headerStyles.borderStyle;
                                range.Style.Border.Bottom.Style = headerStyles.borderStyle;
                            }


                        recordCounter = startDataRow + 1;
                        //Write Records row
                        foreach (T item in records)
                        {
                            columnCounter = startDataColumn;
                            foreach (var prop in sorteddefaultPropertyInfo)
                            {
                                workSheet.Cells[recordCounter, columnCounter].Value = prop.GetValue(item);
                                columnCounter++;
                            }
                            recordCounter++;
                        }
                        //style
                        if (dataStyles != null)
                            using (var range = workSheet.Cells[startDataRow + 1, startDataColumn, recordCounter - 1, columnCounter - 1])
                            {
                                range.Style.Font.Bold = dataStyles.fontBold;
                                range.Style.Fill.PatternType = dataStyles.patternType;
                                range.Style.Fill.BackgroundColor.SetColor(dataStyles.backGroundColor);
                                range.Style.Font.Color.SetColor(dataStyles.fontColor);
                                range.Style.Border.Top.Style = dataStyles.borderStyle;
                                range.Style.Border.Left.Style = dataStyles.borderStyle;
                                range.Style.Border.Right.Style = dataStyles.borderStyle;
                                range.Style.Border.Bottom.Style = dataStyles.borderStyle;
                            }
                    }
                    package.Save();
                    package.Dispose();
                }
                outputStream.Close();
            }
            catch(Exception ex)
            {
                return Task.FromException<T>(ex);
            }
            return Task.CompletedTask;
        }
    }
}
