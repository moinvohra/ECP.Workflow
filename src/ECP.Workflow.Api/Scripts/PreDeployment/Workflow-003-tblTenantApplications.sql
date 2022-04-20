-- Table: workflow.tenantapplication

-- DROP TABLE workflow.tenantapplication;

CREATE TABLE workflow.tenantapplication
(
    tenantapplicationid integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    tenantid character varying COLLATE pg_catalog."default" NOT NULL,
    applicationid character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Pk_tenantapplication" PRIMARY KEY (tenantapplicationid)
);
