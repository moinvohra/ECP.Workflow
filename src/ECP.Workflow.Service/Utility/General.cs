namespace ECP.Workflow.Service.Model.Utility
{
    public class General
    {
        public enum Status
        {
            Pending = -1,
            InActive = 0,
            Active = 1,

        }
        public const string ContentType = "application/json";

        #region CommonMessage
        public const string NoDataFound = "Data not found";
        public const string Unauthorized = "You are unauthorized to use this feature";
        public const string Forbidden = "You are not allowed to use this application";
        public const string BadRequest = "Please check your request format";
        public const string InternalServerError = "Something went wrong, please try again after some time";
        public const string Success = "Success";
        public const string NotificationNotSent = "Fail";
        public const string Duplicate = "Record already exists.";
        #endregion

        public const string WorkflowCreated = "Workflow {WorkflowName} created successfully";
        public const string WorkflowCopy = "Workflow copied successfully";
        public const string WorkflowCanNotUpdated = "Workflow can not be updated";
        public const string WorkflowUpdated = "Workflow {WorkflowName} updated successfully";
        public const string WorkflowDeleted = "Workflow deleted successfully";
        public const string WorkflowCanNotDeleted = "Workflow can not deleted";
        public const string WorkflowActivated = "Workflow activated successfully";
        public const string WorkflowInActivated = "Workflow deactivated successfully";
        public const string WorkflowInActivatedFail = "Workflow can not be deactivated";
    }

    public class Feature
    {
        public const string WorkflowList = "workflow.list";
        public const string WorkflowCreate = "workflow.create";
        public const string WorkflowClone = "workflow.clone";
        public const string WorkflowActivate = "workflow.activate";
        public const string WorkflowInActivate = "workflow.inactivate";
        public const string WorkflowUpdate = "workflow.update";
        public const string WorkflowDelete = "workflow.delete";
    }
}