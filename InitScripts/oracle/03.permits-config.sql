-- Comment: Please input your actual ID to replace XXXXX.
--Name: permits-config.sql
--description: 
--     1.  ACA configuration for permits 

SET SERVEROUTPUT ON
SET DEFINE OFF
SET ECHO ON

CREATE OR REPLACE PROCEDURE SPC_EXISTS_CHECK(P_PROCEDURE_NAME IN VARCHAR2, P_SPC IN VARCHAR2) IS
   L_COUNT_1 NUMBER;
BEGIN
   SELECT COUNT(*) INTO L_COUNT_1 FROM RSERV_PROV WHERE SERV_PROV_CODE = UPPER(P_SPC);
   IF (L_COUNT_1 <= 0) THEN
      
      --When agency does not exists, it will raise the error message
      RAISE_APPLICATION_ERROR((-20000 - 224), 'Please specify the agency code in EXEC ' || P_PROCEDURE_NAME || '(''' ||
                               UPPER(P_SPC) || ''') of file 03.permits-config.sql');
   END IF;
END SPC_EXISTS_CHECK;
/

create or replace procedure permits_config
(  p_spc             IN VARCHAR2
)
IS
   v_SERV_PROV_CODE VARCHAR2(15);
   v_temp NUMBER(1, 0) := 0;
BEGIN
   v_SERV_PROV_CODE := p_spc;
   -- check agency code
   SPC_EXISTS_CHECK('permits_config', v_SERV_PROV_CODE);

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'JOB_VALUE_ENABLED' ;

   ------ New Permit Features Configuration
   ---- Enable Job Value
   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'JOB_VALUE_ENABLED',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'YES'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'AUTO_POPULATE_STATE' ;

   ---- Enable Auto Populate State
   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'AUTO_POPULATE_STATE',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'YES'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'DESCRIPTION_REQUIRED_FLAG' ;

   ---- In the code, but didn't do anything?
   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'DESCRIPTION_REQUIRED_FLAG',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'YES'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'ALL_USER_SCHEDULE_INSPECTION_ENABLED' ;

   ------ Inspection Features Configuration
   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'ALL_USER_SCHEDULE_INSPECTION_ENABLED',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'YES'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'DISPLAY_OPTIONAL_INSPECTIONS' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'DISPLAY_OPTIONAL_INSPECTIONS',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'Yes'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'INSPECTION_CALENDAR_NAME' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'INSPECTION_CALENDAR_NAME',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'Inspection Blockout'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'V360_WEB_ACTION_URL' ;

   ------ Reporting Features Configuration
   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'V360_WEB_ACTION_URL',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'http://av.beta.accela.com/portlets/'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'V360_WEB_ACTION_USERNAME' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'V360_WEB_ACTION_USERNAME',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'admin'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'V360_WEB_ACTION_PASSWORD' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'ACA_CONFIGS',
                 'V360_WEB_ACTION_PASSWORD',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'testok'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'PRINT_PERMIT_REPORT_BUILDING' ;

   -------- Print Permit Report
   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN
        ( SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERV_PROV_CODE, 'PRINT_PERMIT_REPORT_BUILDING', 0, SYSDATE, 'ADMIN', 'A' );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'PRINT_PERMIT_REPORT_BUILDING'
                                    AND BIZDOMAIN_VALUE = 'REPORT_NAME' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'PRINT_PERMIT_REPORT_BUILDING',
                 'REPORT_NAME',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'Print Permit'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'PRINT_PERMIT_SUMMARY_REPORT_BUILDING' ;

   -------- Print Permit Summary Report
   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN
        ( SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERV_PROV_CODE, 'PRINT_PERMIT_SUMMARY_REPORT_BUILDING', 0, SYSDATE, 'ADMIN', 'A' );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'PRINT_PERMIT_SUMMARY_REPORT_BUILDING'
                                    AND BIZDOMAIN_VALUE = 'REPORT_NAME' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'PRINT_PERMIT_SUMMARY_REPORT_BUILDING',
                 'REPORT_NAME',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'Print Permit Summary'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'PRINT_PAYMENT_RECEIPT_REPORT' ;

   -------- Print Receipt Report
   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN
        ( SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERV_PROV_CODE, 'PRINT_PAYMENT_RECEIPT_REPORT', 0, SYSDATE, 'ADMIN', 'A' );

   END;
   END IF;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'PRINT_PAYMENT_RECEIPT_REPORT'
                                    AND BIZDOMAIN_VALUE = 'Receipt Report' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        ( SELECT MAX(BDV_SEQ_NBR) + 1,
                 v_SERV_PROV_CODE,
                 'PRINT_PAYMENT_RECEIPT_REPORT',
                 'Receipt Report',
                 SYSDATE,
                 'ADMIN',
                 'A',
                 'Receipt Report'
          FROM RBIZDOMAIN_VALUE
            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE );

   END;
   END IF;
	UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  NVL(MAX(BDV_SEQ_NBR),0) + 1 FROM RBIZDOMAIN_VALUE) WHERE SEQUENCE_NAME='RBIZDOMAIN_VALUE_SEQ';
   COMMIT;
END;
/

exec permits_config('XXXXX');

drop procedure permits_config;

drop procedure SPC_EXISTS_CHECK;

