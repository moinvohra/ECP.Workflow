CREATE TABLE workflow.workflowdefinition
(
     workflowid integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
	tenantid character varying COLLATE pg_catalog."default" NOT NULL,
	applicationid character varying COLLATE pg_catalog."default" NOT NULL,
    workflowname character varying COLLATE pg_catalog."default" NOT NULL,
    workflowcode character varying COLLATE pg_catalog."default" NOT NULL,
    definitionjson jsonb,
    previewjson jsonb,
    status smallint DEFAULT '-1'::integer,
    startdate date,
    enddate date,
    createddate timestamp with time zone DEFAULT timezone('utc'::text, now()),
    modifieddate timestamp with time zone,
    createdby character varying COLLATE pg_catalog."default",
    modifiedby character varying COLLATE pg_catalog."default",
    CONSTRAINT "Pk_workflowid" PRIMARY KEY (workflowid)
)