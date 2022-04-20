-- PROCEDURE: workflow.copyworkflow(integer, character varying, character varying, character varying, character varying)

-- DROP PROCEDURE workflow.copyworkflow(integer, character varying, character varying, character varying, character varying);

CREATE OR REPLACE PROCEDURE workflow.copyworkflow(
	sourceworkflowid integer,
	_workflowname character varying,
	_workflowcode character varying,
	_tenantid character varying,
	_applicationid character varying)
LANGUAGE 'plpgsql'

AS $BODY$
BEGIN
  --- Cloning the workflow
  if not exists  (select 1 from workflow.workflowdefinition where tenantid = _tenantid and applicationid = _applicationId and workflowcode = _workflowcode)
  THEN 
   	insert into workflow.workflowdefinition
    (tenantid,applicationid,workflowname,workflowcode,definitionjson,previewjson)
   	select _tenantid,_applicationid,_workflowname,_workflowcode,definitionjson,previewjson
  	from workflow.workflowdefinition 
	  where tenantid = _tenantid
  	and applicationid = _applicationid and workflowid = sourceworkflowid;
   END IF;
  
    COMMIT;
END;
$BODY$;
