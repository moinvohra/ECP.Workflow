using System;

namespace ECP.Export.Excel
{
    [AttributeUsage(AttributeTargets.All)]
    public class OutputColumn : Attribute
    {
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
        public OutputColumn(int columnIndex, string columnName)
        {
            this.ColumnIndex = columnIndex;
            this.ColumnName = columnName;
        }
    }
}
