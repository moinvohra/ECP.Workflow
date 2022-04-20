using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECP.Workflow.Model
{
    public class WorkflowListRecord
    {
        [Column("workflowid")]
        public int Id { get; set; }

        [Column("workflowname")]
        public string Name { get; set; }

        [Column("workflowcode")]
        public string Code { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [NotMapped]
        public string StatusText
        {
            get
            {
                return ((WorkflowStatus)Status).ToString();
            }
        }

        [Column("startdate")]
        public DateTime? StartDate { get; set; }

        [Column("enddate")]
        public DateTime? EndDate { get; set; }


        [Column("previewjson")]
        public string PreviewJson { get; set; }
    }

    enum WorkflowStatus
    {
        Active = 1,
        Deactive = 0,
        Pending = -1
    }
}