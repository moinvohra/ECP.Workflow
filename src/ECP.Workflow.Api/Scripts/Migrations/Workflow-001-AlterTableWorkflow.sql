ALTER TABLE workflow.workflowdefinition
    ALTER COLUMN startdate TYPE timestamp with time zone,
    ALTER COLUMN enddate TYPE timestamp with time zone;
