using ECP.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Test.Model
{
    public class WorkflowData
    {

        public static AddWorkflowReq NewWorkflowRequest()
        {
            return new AddWorkflowReq
            {
                Code = "Workflow.Code1",
                Name = "Workflow1",
                Definitionjson = "",
                Previewjson = "",
                Status = -1,
            };
        }


        public static EditWorkflowReq EditWorkflowRequest()
        {
            return new EditWorkflowReq
            {
                Id = 1,
                Code = "Workflow.Code1",
                Name = "Workflow1",
                DefinitionJson = "",
                PreviewJson = "",
            };
        }

    }
}
