using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Model.Utility
{
    public class Error
    {
        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        public int RecordIndex { get; set; }
    }
}
