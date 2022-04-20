-- FUNCTION: public.checkduplicate(integer, character varying, character varying)

-- DROP FUNCTION public.checkduplicate(integer, character varying, character varying);

CREATE OR REPLACE FUNCTION workflow.checkduplicate(
	_workflowid integer,
	_applicationid character varying,
	_tenantid character varying)
    RETURNS boolean
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$

BEGIN
	IF exists (select  1
	from workflow.workflowdefinition
	where tenantid = _tenantid 
	and applicationid = _applicationid
	and workflowid <> _workflowid
	and workflowcode = (select workflowcode from workflow.workflowdefinition where workflowid = _workflowid and tenantid = _tenantid and applicationid = _applicationid)
	and status = 1)	-- -1: Pending, 0:InActivate, 1:Activate
  THEN
   return true;
  ELSE
   return false;
  END if;
END; $BODY$;