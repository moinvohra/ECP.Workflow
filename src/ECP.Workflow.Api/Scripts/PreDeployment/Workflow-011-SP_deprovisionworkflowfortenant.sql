-- PROCEDURE: workflow.deprovisionworkflowfortenant(character varying, character varying[])

-- DROP PROCEDURE workflow.deprovisionworkflowfortenant(character varying, character varying[]);

CREATE OR REPLACE PROCEDURE workflow.deprovisionworkflowfortenant(
	_tenantid character varying,
	applications character varying[])
LANGUAGE 'plpgsql'

AS $BODY$	
BEGIN
	
	Delete from workflow.tenantapplication
	where tenantid = _tenantid and applicationid = any(applications);
	
	Delete from workflow.workflowdefinition
	where tenantid = _tenantid and applicationid = any(applications);
	
	COMMIT;
END;
$BODY$;
