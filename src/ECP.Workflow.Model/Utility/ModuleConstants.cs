using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Service.Model.Utility
{
    public static class ModuleConstants
    {
        public const string RecordTypeCreate = "Create";
        public const string RecordTypeClone = "Copy";
        public const string RecordTypeUpdate = "Update";
        public const string RecordTypeActive = "Active";
        public const string RecordTypeInActive = "Deactive";
        public const string RecordTypeDelete = "Delete";

        public const string WorkflowCreate = "workflow.create";
        public const string WorkflowClone = "workflow.copy";
        public const string WorkflowActive = "workflow.active";
        public const string WorkflowInActive = "workflow.deactive";
        public const string WorkflowUpdate = "workflow.update";
        public const string WorkflowDelete = "workflow.delete";
    }
}
