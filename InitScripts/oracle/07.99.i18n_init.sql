-- Comment: Please input your actual ID to replace XXXXX.
--Name: 99.i18n_init.sql
--description: 
--     1.  Setup ACA_CONFIGS Standard Choice Entry
--     2.  Add TRANSACTION_LOG_INTERVAL entry to the ACA_CONFIGS
--     3.  Enable Home Tab
--     4.  Insert standard password reset email content

SET SERVEROUTPUT ON SIZE 1000000
SET DEFINE OFF

CREATE OR REPLACE PROCEDURE SPC_EXISTS_CHECK(P_PROCEDURE_NAME IN VARCHAR2, P_SPC IN VARCHAR2) IS
   L_COUNT_1 NUMBER;
BEGIN
   SELECT COUNT(*) INTO L_COUNT_1 FROM RSERV_PROV WHERE SERV_PROV_CODE = UPPER(P_SPC);
   IF (L_COUNT_1 <= 0) THEN
      
      --When agency does not exists, it will raise the error message
      RAISE_APPLICATION_ERROR((-20000 - 224), 'Please specify the agency code in EXEC ' || P_PROCEDURE_NAME || '(''' ||
                               UPPER(P_SPC) || ''') of file 99.i18n_init.sql');
   END IF;
END SPC_EXISTS_CHECK;
/

-- Format LANG_ID like xx_XX
CREATE OR REPLACE FUNCTION GET_LANG_ID(P_LANG_ID IN VARCHAR2) RETURN VARCHAR2 IS
   RESULT    VARCHAR2(10);
   V_LANG_ID VARCHAR2(10) := TRIM(P_LANG_ID);
   V_LEN     NUMBER := LENGTH(V_LANG_ID);
BEGIN
   IF V_LEN = 2 THEN
      RESULT := LOWER(V_LANG_ID);
   ELSIF V_LEN = 5 AND INSTR(V_LANG_ID, '_') = 3 THEN
      RESULT := LOWER(SUBSTR(V_LANG_ID, 1, 2)) || '_' || UPPER(SUBSTR(V_LANG_ID, -2, 2));
   ELSE
      RESULT := V_LANG_ID;
   END IF;
   RETURN(RESULT);
END GET_LANG_ID;
/

--migration to generate resource table records
--generate resource column value for main table
--execute clean main language resource table if you choose overwrite
--execute copy main table to resource table
--reset resource sequence
CREATE OR REPLACE PROCEDURE PD_INITIALIZE_I18N_TAB
(
   MAIN_TAB_NAME  IN VARCHAR2,
   --main table (R3APPTYP)
   I18N_TAB_NAME  IN VARCHAR2,
   --I18N table (R3APPTYP_I18N)
   SEQ_NAME       IN VARCHAR2,
   --I18N resource table sequence (R3APPTYP_RES_SEQ)
   P_SPC          IN VARCHAR2,
   LANG_CODE      IN VARCHAR2,
   --primary language code
   P_OVERWRITE    IN VARCHAR2 
   --overwrite old resource table
) IS
   --MAIN_TAB_NAME : main table G3STAFF
   --I18N_TAB_NAME : resource table G3STAFF_I18N
   --SEQ_NAME      : resource sequence name G3STAFF_RES_SEQ
   --P_SPC         : agency name
   --LANG_CODE     : main language of agency
   --P_OVERWRITE   : main language of agency  
   MAIN_TAB_NAME_ VARCHAR2(4000) := UPPER(MAIN_TAB_NAME);
   I18N_TAB_NAME_ VARCHAR2(4000) := UPPER(I18N_TAB_NAME);
   SEQ_NAME_      VARCHAR2(4000) := UPPER(SEQ_NAME);
   LANG_CODE_      VARCHAR2(4000) := LANG_CODE;
   MAX_RES_ID_     NUMBER := 1;
   SQL_GEN_RES_ID_   VARCHAR2(4000);
   SQL_CLS_I18N_TAB_ VARCHAR2(4000);
   SQL_INS_I18N_     VARCHAR2(4000);
   SQL_UPD_SEQ_      VARCHAR2(4000);
   I18N_TAB_INS_COLUMNS_ VARCHAR2(4000);
   MAIN_TAB_SEL_COLUMNS_ VARCHAR2(4000);
   MAIN_TAB_SEL_VALUE_   VARCHAR2(4000);
   L_INS_WHERE           VARCHAR2(4000);
   CURSOR GET_COLUMNS IS
      SELECT TC.COLUMN_NAME
        FROM USER_TAB_COLUMNS TC
       WHERE TC.TABLE_NAME = I18N_TAB_NAME_;
BEGIN
   -- get table resource sequence number
   SELECT LAST_NUMBER INTO MAX_RES_ID_ FROM AA_SYS_SEQ WHERE SEQUENCE_NAME=SEQ_NAME;
   -- update table resource sequence column based on maximum resource sequence
   SQL_GEN_RES_ID_   := 'UPDATE ' || MAIN_TAB_NAME_ || ' SET RES_ID = ROWNUM + ' || TO_CHAR(MAX_RES_ID_) || ' WHERE RES_ID IS NULL ';
   -- delete old resource record if overwrite is true
   SQL_CLS_I18N_TAB_ := 'DELETE FROM ' || I18N_TAB_NAME_ || ' WHERE LANG_ID = ' || '''' || LANG_CODE_ || '''' ;
   IF(UPPER(P_OVERWRITE) != 'YES') THEN
      L_INS_WHERE := ' WHERE RES_ID > ' || TO_CHAR(MAX_RES_ID_) ;
   ELSE
      L_INS_WHERE := ' WHERE RES_ID IS NOT NULL ';
   END IF;
   -- get all I18N table columns
   FOR C_1 IN GET_COLUMNS LOOP
      IF I18N_TAB_INS_COLUMNS_ IS NOT NULL THEN
         I18N_TAB_INS_COLUMNS_ := I18N_TAB_INS_COLUMNS_ || ',' ||
                                  C_1.COLUMN_NAME;
      ELSE
         I18N_TAB_INS_COLUMNS_ := C_1.COLUMN_NAME;
      END IF;
      -- if column has agency code
      IF C_1.COLUMN_NAME = 'SERV_PROV_CODE' THEN
         MAIN_TAB_SEL_VALUE_ := 'SERV_PROV_CODE';
           -- only generate resource id for specified agency
		   SQL_GEN_RES_ID_ := SQL_GEN_RES_ID_ || ' AND SERV_PROV_CODE = ''' || P_SPC || ''' ';
		   -- for table R2CHCKBOX start: only initialize R1_CHECKBOX_GROUP IN ('APPLICATION','WORKFLOW TASK','FEEATTACHEDTABLE')
         IF(MAIN_TAB_NAME_ = 'R2CHCKBOX') THEN
            SQL_GEN_RES_ID_ := SQL_GEN_RES_ID_ || ' AND R1_CHECKBOX_GROUP IN (''APPLICATION'',''WORKFLOW TASK'',''FEEATTACHEDTABLE'')';         
         END IF;
         -- for table R2CHCKBOX end
         -- only clear resource table for specified agency
         SQL_CLS_I18N_TAB_ := SQL_CLS_I18N_TAB_ || ' AND SERV_PROV_CODE = ''' || P_SPC || ''' ';
         -- only insert resource table for specified agency
         L_INS_WHERE := L_INS_WHERE || ' AND SERV_PROV_CODE = ''' || P_SPC || ''' ';
      ELSIF C_1.COLUMN_NAME = 'LANG_ID' THEN
         MAIN_TAB_SEL_VALUE_ := '''' || LANG_CODE_ || '''';
      ELSIF C_1.COLUMN_NAME = 'REC_DATE' THEN
         MAIN_TAB_SEL_VALUE_ := 'SYSDATE';
      ELSIF C_1.COLUMN_NAME = 'REC_FUL_NAM' THEN
         MAIN_TAB_SEL_VALUE_ := '''I18N_INIT''';
      ELSIF C_1.COLUMN_NAME = 'REC_STATUS' THEN
         MAIN_TAB_SEL_VALUE_ := '''A''';
      ELSE
         MAIN_TAB_SEL_VALUE_ := C_1.COLUMN_NAME;
      END IF;
      IF MAIN_TAB_SEL_COLUMNS_ IS NOT NULL THEN
         MAIN_TAB_SEL_COLUMNS_ := MAIN_TAB_SEL_COLUMNS_ || ',' ||
                                  MAIN_TAB_SEL_VALUE_;
      ELSE
         MAIN_TAB_SEL_COLUMNS_ := MAIN_TAB_SEL_VALUE_;
      END IF;
   END LOOP;
   --SQL Start: INSERT INTO ... SELECT ... FORM ...
   SQL_INS_I18N_ := 'INSERT INTO ' || I18N_TAB_NAME_ || ' (' ||
                    I18N_TAB_INS_COLUMNS_ || ')';
   SQL_INS_I18N_ := SQL_INS_I18N_ || CHR(10) || 'SELECT ' ||
                    MAIN_TAB_SEL_COLUMNS_ || ' FROM ' || MAIN_TAB_NAME_ ;
   SQL_INS_I18N_ := SQL_INS_I18N_ || CHR(10) || L_INS_WHERE;
   --SQL End: INSERT INTO ... SELECT ... FORM ...
   SQL_UPD_SEQ_ := 'UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT GREATEST(COUNT(*),COALESCE(MAX(RES_ID),1)) FROM ' || MAIN_TAB_NAME_ || ') WHERE SEQUENCE_NAME = ''' || SEQ_NAME_ || '''';
   --execute generate resource id
   dbms_output.put_line( SQL_GEN_RES_ID_);
   EXECUTE IMMEDIATE SQL_GEN_RES_ID_;
   --execute clean main language resource table
   if(UPPER(P_OVERWRITE) = 'YES') THEN
	   dbms_output.put_line( SQL_CLS_I18N_TAB_);
	   EXECUTE IMMEDIATE SQL_CLS_I18N_TAB_;
   end if;
   --execute resource table data
   dbms_output.put_line( SQL_INS_I18N_);
   EXECUTE IMMEDIATE SQL_INS_I18N_;
   --reset sequence
   dbms_output.put_line( SQL_UPD_SEQ_);
   EXECUTE IMMEDIATE SQL_UPD_SEQ_;
   COMMIT;
EXCEPTION  
-- exception handlers begin
-- Only one of the WHEN blocks is executed.
   WHEN OTHERS THEN  
   -- handles all other errors
      DBMS_OUTPUT.PUT_LINE('Process table ' || MAIN_TAB_NAME || ': Error code ' || SQLCODE || ': ' || SUBSTR(SQLERRM, 1 , 2000));
END PD_INITIALIZE_I18N_TAB;
/

--EXEC PD_INITIALIZE_I18N_TAB('BCUSTOMIZED_CONTENT', 'BCUSTOMIZED_CONTENT_I18N', 'BCUSTOMIZED_RES_SEQ', 'FLAGSTAFF', 'en', 'YES');
--DELETE FROM G3STAFFS_I18N WHERE SERV_PROV_CODE = 'QA';
--UPDATE G3STAFFS SET RES_ID = NULL WHERE SERV_PROV_CODE = 'QA';

CREATE OR REPLACE PROCEDURE i18n_mig
(
   P_OVERWRITE    IN VARCHAR2,
   --owerwrite old resource data
   P_MAIN_TABLE   IN VARCHAR2,
   --main table (R3APPTYP)
   P_SPC          IN VARCHAR2
)
IS
   --P_OVERWRITE  : main language of agency
   --P_MAIN_TABLE : main table, '%' for All tables
   --P_SPC        : agency, '%' for All agencies
   L_CNT          NUMBER := 0;
   L_CNT2         NUMBER := 0;
   L_LANG         VARCHAR2(10) := 'en_US';
   L_MAIN_TAB     VARCHAR2(4000) := '';
   L_RES_NAM      VARCHAR2(4000) := '';
   P_MAIN_TABLE_  VARCHAR2(128)  := UPPER(SUBSTR(P_MAIN_TABLE, 1, 25));
   P_SPC_         VARCHAR2(32)   := UPPER(P_SPC);
   --get I18N resource tables
   CURSOR CUR_I18N IS
      SELECT TABLE_NAME FROM USER_TABLES WHERE TABLE_NAME LIKE P_MAIN_TABLE_ || '_I18N';
   CURSOR CUR_SPC IS
      SELECT SERV_PROV_CODE FROM RSERV_PROV WHERE SERV_PROV_CODE <> 'STANDARDDATA' AND SERV_PROV_CODE LIKE P_SPC_;
BEGIN
   FOR REC_SPC IN CUR_SPC LOOP
      --check MULTI-LANGUAGE SUPPORT ENABLE switch
      SELECT COUNT(*) INTO L_CNT FROM RBIZDOMAIN_VALUE 
	  WHERE SERV_PROV_CODE = REC_SPC.SERV_PROV_CODE
	    AND BIZDOMAIN = 'I18N_SETTINGS'
		AND BIZDOMAIN_VALUE = 'MULTI-LANGUAGE SUPPORT ENABLE'
		AND UPPER(VALUE_DESC) = 'YES';
      --check main language
      SELECT COUNT(*) INTO L_CNT2 FROM RBIZDOMAIN_VALUE 
	  WHERE SERV_PROV_CODE = REC_SPC.SERV_PROV_CODE
	    AND BIZDOMAIN = 'I18N_SETTINGS'
		AND BIZDOMAIN_VALUE = 'I18N_DEFAULT_LANGUAGE';
      --get default main language
	  IF (L_CNT2 <= 0) THEN
	     L_LANG := 'en_US';
	  ELSE
        SELECT VALUE_DESC INTO L_LANG FROM RBIZDOMAIN_VALUE 
	    WHERE SERV_PROV_CODE = REC_SPC.SERV_PROV_CODE
	      AND BIZDOMAIN = 'I18N_SETTINGS'
		  AND BIZDOMAIN_VALUE = 'I18N_DEFAULT_LANGUAGE';
	  END IF;
      IF (L_CNT > 0) THEN
      --do copy operation for each I18N table
       FOR REC_I18N IN CUR_I18N LOOP
		   IF (REC_I18N.TABLE_NAME = 'REXPRESSION_LOOKUP_CRITER_I18N') THEN
		      L_MAIN_TAB := 'REXPRESSION_LOOKUP_CRITERIA';
			  L_RES_NAM := L_MAIN_TAB||'_RES_SEQ';
		   ELSIF (REC_I18N.TABLE_NAME = 'RRATING_CONDITION_CRITERI_I18N') THEN
		      L_MAIN_TAB := 'RRATING_CONDITION_CRITERIA';
			  L_RES_NAM := L_MAIN_TAB||'_RES_SEQ';
		   ELSIF (REC_I18N.TABLE_NAME = 'RRATING_EXPRESSION_CRITER_I18N') THEN
		      L_MAIN_TAB := 'RRATING_EXPRESSION_CRITERIA';
			  L_RES_NAM := L_MAIN_TAB||'_RES_SEQ';
		   ELSIF (REC_I18N.TABLE_NAME = 'REXPRESSION_FUNCTION_CRIT_I18N') THEN
		      L_MAIN_TAB := 'REXPRESSION_FUNCTION_CRITERIA';
			  L_RES_NAM := L_MAIN_TAB||'_RES_SEQ';
		   ELSE
		      L_MAIN_TAB := SUBSTR(REC_I18N.TABLE_NAME, 0, length(REC_I18N.TABLE_NAME) - 4 - 1);
			  L_RES_NAM := L_MAIN_TAB||'_RES_SEQ';
		   END IF;
		   SELECT COUNT(*) INTO L_CNT2 FROM AA_SYS_SEQ WHERE SEQUENCE_NAME=L_RES_NAM;
		   IF (L_CNT2 > 0) THEN
		        dbms_output.put_line ('Process table '|| L_MAIN_TAB || ' for agency '|| REC_SPC.SERV_PROV_CODE || ' begin! ');
				PD_INITIALIZE_I18N_TAB(L_MAIN_TAB, REC_I18N.TABLE_NAME, L_RES_NAM, REC_SPC.SERV_PROV_CODE, L_LANG, P_OVERWRITE);
		        dbms_output.put_line ('Process table '|| L_MAIN_TAB || ' for agency '|| REC_SPC.SERV_PROV_CODE || ' end! ');
		   END IF;
		   COMMIT;
--		   dbms_output.put_line (L_MAIN_TAB || ' ' || REC_I18N.TABLE_NAME || ' ' || L_MAIN_TAB||'_RES_SEQ' || ' ' || REC_SPC.SERV_PROV_CODE || ' ' || L_LANG);
      END LOOP;
	  END IF;
   END LOOP;
END i18n_mig;
/

CREATE OR REPLACE PROCEDURE ACA_I18N_INIT(P_SPC IN VARCHAR2) IS
   V_SERVPROVCODE    VARCHAR2(15);
   V_CNT_DFLT_LANG   NUMBER;
   V_CNT_I18N_ENABLE NUMBER;
BEGIN
   V_SERVPROVCODE := UPPER(P_SPC);
   -- check agency code
   SPC_EXISTS_CHECK('ACA_I18N_INIT', V_SERVPROVCODE);
   --
   SELECT COUNT(*)
     INTO V_CNT_DFLT_LANG
     FROM RBIZDOMAIN_VALUE
    WHERE SERV_PROV_CODE = V_SERVPROVCODE
      AND BIZDOMAIN = 'I18N_SETTINGS'
      AND BIZDOMAIN_VALUE = 'I18N_DEFAULT_LANGUAGE'
      AND REC_STATUS = 'A';
   SELECT COUNT(*)
     INTO V_CNT_I18N_ENABLE
     FROM RBIZDOMAIN_VALUE
    WHERE SERV_PROV_CODE = V_SERVPROVCODE
      AND BIZDOMAIN = 'I18N_SETTINGS'
      AND BIZDOMAIN_VALUE = 'MULTI-LANGUAGE SUPPORT ENABLE'
      AND UPPER(VALUE_DESC) = 'YES'
      AND REC_STATUS = 'A';
   IF (V_CNT_DFLT_LANG > 0 AND V_CNT_I18N_ENABLE > 0) THEN
      I18N_MIG('NO', 'G3STAFFS', V_SERVPROVCODE);
      I18N_MIG('NO', 'RBIZDOMAIN_VALUE', V_SERVPROVCODE);
      I18N_MIG('NO', 'BCUSTOMIZED_CONTENT', V_SERVPROVCODE);
      I18N_MIG('NO', 'CALENDAR', V_SERVPROVCODE);
      I18N_MIG('NO', 'CALENDAR_EVENT', V_SERVPROVCODE);
      I18N_MIG('NO', 'XPOLICY', V_SERVPROVCODE);
   END IF;
END ACA_I18N_INIT;
/

EXEC ACA_I18N_INIT('XXXXX');

drop procedure SPC_EXISTS_CHECK;

drop procedure ACA_I18N_INIT;

