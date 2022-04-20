CREATE TABLE workflow.transactionqueueoutbound
(
    transactionqueueoutboundid integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    eventtype character varying COLLATE pg_catalog."default" NOT NULL,
    payload character varying COLLATE pg_catalog."default" NOT NULL,
    servicename character varying COLLATE pg_catalog."default" NOT NULL,
    senttoexchange boolean,
    datecreated timestamp with time zone,
    datesenttoexchange timestamp with time zone,
    datetoresendtoexchange timestamp with time zone,
    CONSTRAINT transactionqueueoutbound_pkey PRIMARY KEY (transactionqueueoutboundid)
)