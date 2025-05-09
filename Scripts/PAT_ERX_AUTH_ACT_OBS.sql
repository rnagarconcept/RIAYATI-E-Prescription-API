CREATE SEQUENCE PAT_ERX_AUTH_ACT_OBS_SEQ  START WITH 1 INCREMENT BY 1 MINVALUE 1 CACHE 20 NOCYCLE NOORDER;

CREATE TABLE HOSPITAL.PAT_ERX_AUTH_ACT_OBS
(
ID		NUMBER NOT NULL,
ACTIVITY_ID	NUMBER ,
REQ_ID	NUMBER NOT NULL,
OBS_TYPE NVARCHAR2(255),
Code NVARCHAR2(255),
OBS_VALUE NVARCHAR2(255),
ValueType NVARCHAR2(255)
)
TABLESPACE USERS
PCTUSED    0
PCTFREE    10
INITRANS   1
MAXTRANS   255
STORAGE    (
            PCTINCREASE      0
            BUFFER_POOL      DEFAULT
           )
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

CREATE UNIQUE INDEX PAT_ERX_AUTH_ACT_OBS_PK ON PAT_ERX_AUTH_ACT_OBS
(ID)
LOGGING
TABLESPACE USERS
PCTFREE    10
INITRANS   2
MAXTRANS   255
STORAGE    (
            PCTINCREASE      0
            BUFFER_POOL      DEFAULT
           )
NOPARALLEL;

CREATE OR REPLACE TRIGGER PAT_ERX_AUTH_ACT_OBS 
BEFORE INSERT
ON PAT_ERX_AUTH_ACT_OBS 
REFERENCING NEW AS New OLD AS Old
FOR EACH ROW
DECLARE
tmpVar NUMBER;

BEGIN
   tmpVar := 0;

   SELECT PAT_ERX_AUTH_ACT_OBS_SEQ.NEXTVAL INTO tmpVar FROM dual;
   :NEW.ID := tmpVar;

   EXCEPTION
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RAISE;
END ;

ALTER TABLE PAT_ERX_AUTH_ACT_OBS ADD (
  CONSTRAINT PAT_ERX_AUTH_ACT_OBS_PK
 PRIMARY KEY
 (ID));
 