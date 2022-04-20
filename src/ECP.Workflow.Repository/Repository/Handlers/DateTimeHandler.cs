using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECP.Workflow.Repository.Handlers
{
    public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>

    {
        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            parameter.Value = value;
        }

        public override DateTime Parse(object value)
        {
            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
        }

    }
}
