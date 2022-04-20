-- PROCEDURE: workflow.cloneactiveworkflow(integer, character varying, character varying, character varying, character varying, jsonb, jsonb, integer, character varying)

-- DROP PROCEDURE workflow.cloneactiveworkflow(integer, character varying, character varying, character varying, character varying, jsonb, jsonb, integer, character varying);

CREATE OR REPLACE PROCEDURE workflow.cloneactiveworkflow(
	_sourceworkflowid integer,
	_workflowname character varying,
	_workflowcode character varying,
	_tenantid character varying,
	_applicationid character varying,
	_definitionjson jsonb,
	_previewjson jsonb,
	_status integer)
LANGUAGE 'plpgsql'

AS $BODY$
BEGIN
--- Cloning the workflow
if not exists (select 1 from workflow.workflowdefinition where tenantid in
(SELECT tenantid FROM workflow.tenantapplication WHERE applicationid = _applicationid )
AND applicationid = _applicationId AND workflowcode = _workflowcode AND tenantid <> _tenantid)
THEN
INSERT INTO workflow.workflowdefinition(
tenantid,
applicationid,
workflowname,
workflowcode,
definitionjson,
previewjson,
status,
createdby,
startdate
)
SELECT 
WorkflowTenant.tenantid,
_applicationid,
_workflowname,
_workflowcode,
_definitionjson,
_previewjson,
_status,
wd.createdby,
wd.startdate
FROM workflow.workflowdefinition wd
CROSS JOIN (
SELECT wt.tenantid
FROM workflow.tenantapplication wt
WHERE wt.applicationid = _applicationid and tenantid <> _tenantid
) AS WorkflowTenant
WHERE wd.workflowid = _sourceworkflowid;
END IF;

COMMIT;
END;
$BODY$;
