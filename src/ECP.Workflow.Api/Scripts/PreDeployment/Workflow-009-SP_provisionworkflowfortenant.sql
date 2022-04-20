-- PROCEDURE: workflow.provisionworkflowfortenant(character varying, character varying, character varying[])

-- DROP PROCEDURE workflow.provisionworkflowfortenant(character varying, character varying, character varying[]);

CREATE OR REPLACE PROCEDURE workflow.provisionworkflowfortenant(
	_primarytenantid character varying,
	_tenantid character varying,
	applications character varying[])
LANGUAGE 'plpgsql'

AS $BODY$	
BEGIN	
	CREATE TEMP TABLE tmptenantapplication(tenantid CHARACTER VARYING, applicationid CHARACTER VARYING);
	
	--Getting new tenant's applications

	insert into tmptenantapplication(tenantid,applicationid)  (
		select  _tenantid, unnest(applications) as applicationid
			except
		select tenantid, applicationid from workflow.tenantapplication where tenantid = _tenantid
	 );
	
	-- TenantApplication
	insert into workflow.tenantapplication(tenantid,applicationid) 
	select tenantid,applicationid from tmptenantapplication;
	
	
	   -- Clone active workflow details
	   insert into workflow.workflowdefinition (tenantid,applicationid,workflowname,workflowcode,
	   definitionjson,	previewjson,status,	startdate,enddate,createddate,modifieddate,createdby,modifiedby)
	  select
	  _tenantid as tenantid,	applicationid,	workflowname,	workflowcode,
	  definitionjson,	previewjson,status,	startdate,enddate,createddate,modifieddate,createdby,modifiedby
	  from workflow.workflowdefinition
	  where status = 1 and tenantid = _primarytenantid
	  and applicationid in (select applicationid from tmptenantapplication);
	
	DISCARD TEMP;

COMMIT;
END;
$BODY$;