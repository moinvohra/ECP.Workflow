namespace ECP.Workflow.Model.Utility
{
    public static class General
    {
        public enum WorkflowStatus
        {
            Pending = -1,
            InActive = 0,
            Active = 1,

        }



        public enum ReturnType
        {
            InvalidWorkflow = -2,
            Duplicate = -1,
            Success = 1,
            OperationNotDone = 0,
            NotFound = 2,
            AlreadyActivated = 3,
            AlreadyDeActivated = 4,

        }
        public const string ContentType = "application/json";

        #region CommonMessage
        public const string NoDataFound = "Data not found";
        public const string Unauthorized = "You are unauthorized to use this feature";
        public const string Forbidden = "You are not allowed to use this application";
        public const string BadRequest = "Please check your request format";
        public const string OperationNotDone = "Something went wrong, please try again after some time";
        public const string Success = "Success";
        public const string NotificationNotSent = "Fail";
        public const string Duplicate = "Record already exists.";
        public const string Invalid = "InValid Workflow.";
        #endregion


        // <summary>
        /// General message
        /// </summary>
        public const string NEWWORKFLOWCREATED = "NewWorkflowCreated";
        public const string WORKFLOWUPDATED = "WorkflowUpdated";
        public const string WORKFLOWACTIVATED = "WorkflowActivated";
        public const string WORKFLOWINACTIVATED = "WorkflowInActivated";
        public const string WORKFLOWDELETED = "WorkflowDeleted";

        public const string WorkflowCreated = "Workflow {0} created successfully";
        public const string WorkflowUpdated = "Workflow {0} updated successfully";
        public const string WorkflowUpdatedActivated = "Workflow {0} updated and activated successfully";
        public const string WorkflowCopy = "Workflow {0} copied successfully";

        public const string WorkflowCanNotUpdated = "Workflow {0} can not be updated";
        public const string WorkflowDuplicate = "Workflow code {0} already exists";
        public const string WorkflowInvalid = "Workflow  {0} is invalid";
        public const string WorkflowDeleted = "Workflow deleted successfully";
        public const string WorkflowCanNotDeleted = "Workflow can not deleted";

        public const string WorkflowAlreadyActivated = "Workflow {0} has the same workflow code, so you cannot activate the workflow.";
        public const string WorkflowActivated = "Workflow activated successfully";
        public const string WorkflowInActivated = "Workflow deactivated successfully";
        public const string WorkflowInActivatedFail = "Workflow can not be deactivated";
        public const string WorkflowActivationFail = "Workflow can not be re-activated again";
    }

    public static class Feature
    {
        public const string WorkflowList = "workflow.list";
        public const string WorkflowCreate = "workflow.create";
        public const string WorkflowClone = "workflow.clone";
        public const string WorkflowActivate = "workflow.activate";
        public const string WorkflowInActivate = "workflow.inactivate";
        public const string WorkflowUpdate = "workflow.update";
        public const string WorkflowDelete = "workflow.delete";
    }


    public static class Errors
    {
        public const string WorkflowDetailsNotFound = "WorkflowDetailsNotFound";
        public const string WorkflowDuplicate = "WorkflowAlreadyExists";
        public const string WorkflowCanNotDeleted = "WorkflowCanNotDeleted";
    }
}