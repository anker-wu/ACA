-- Comment: Please input your actual ID to replace XXXXX.
CREATE OR REPLACE PROCEDURE SPC_EXISTS_CHECK(P_PROCEDURE_NAME IN VARCHAR2, P_SPC IN VARCHAR2) IS
   L_COUNT_1 NUMBER;
BEGIN
   SELECT COUNT(*) INTO L_COUNT_1 FROM RSERV_PROV WHERE SERV_PROV_CODE = UPPER(P_SPC);
   IF (L_COUNT_1 <= 0) THEN
      
      --When agency does not exists, it will raise the error message
      RAISE_APPLICATION_ERROR((-20000 - 224), 'Please specify the agency code in EXEC ' || P_PROCEDURE_NAME || '(''' ||
                               UPPER(P_SPC) || ''') of file 06.contact_display_controls.sql');
   END IF;
END SPC_EXISTS_CHECK;
/

CREATE OR REPLACE PROCEDURE ACA_BIZ_TO_POLICY_CONTACT_TYPE(V_SERV_PROV_CODE IN VARCHAR2) IS
   V_POLICYSEQ NUMBER;

   CURSOR CUR_CONTACT_TYPE_MODULE(IV_SERV_PROV_CODE VARCHAR2) IS
      SELECT B.BIZDOMAIN_VALUE AS MODULE_NAME,
             CASE
                WHEN B.VALUE_DESC = '1' THEN
                 '1000000000'
                WHEN B.VALUE_DESC = '2' THEN
                 '0100000000'
                WHEN B.VALUE_DESC = '3' THEN
                 '0110000000'
             END USER_ROLE_CODE
        FROM RBIZDOMAIN_VALUE B, RBIZDOMAIN A
       WHERE A.SERV_PROV_CODE = B.SERV_PROV_CODE
         AND A.BIZDOMAIN = B.BIZDOMAIN
         AND A.REC_STATUS = 'A'
         AND UPPER(B.BIZDOMAIN) = 'ACA_APPLICANT_DISPLAY_RULE'
         AND B.SERV_PROV_CODE = IV_SERV_PROV_CODE
		 AND NOT EXISTS (SELECT 1 FROM XPOLICY 
	        WHERE SERV_PROV_CODE = IV_SERV_PROV_CODE
	        AND POLICY_NAME = 'ACA_CONTACT_TYPE_USER_ROLES' 
	        AND UPPER(LEVEL_TYPE)='MODULE'
	        AND LEVEL_DATA = B.BIZDOMAIN_VALUE
	        AND RIGHT_GRANTED = 'ACA'
	        AND DATA1='Applicant');

   ROW_CONTACT_TYPE_MODULE CUR_CONTACT_TYPE_MODULE%ROWTYPE;

BEGIN
   -- check agency code
   SPC_EXISTS_CHECK('ACA_BIZ_TO_POLICY_CONTACT_TYPE', V_SERV_PROV_CODE);
   OPEN CUR_CONTACT_TYPE_MODULE(V_SERV_PROV_CODE);

   SELECT LAST_NUMBER
     INTO V_POLICYSEQ
     FROM AA_SYS_SEQ
    WHERE SEQUENCE_NAME = 'XPOLICY_SEQ';

   LOOP
      FETCH CUR_CONTACT_TYPE_MODULE
         INTO ROW_CONTACT_TYPE_MODULE;
   
      EXIT WHEN CUR_CONTACT_TYPE_MODULE%NOTFOUND;
       V_POLICYSEQ := V_POLICYSEQ + 1;
   
      INSERT INTO XPOLICY
         (SERV_PROV_CODE, POLICY_NAME,
           --ACA_CONTACT_TYPE_USER_ROLES       
          LEVEL_TYPE,
           --'MODULE'       
          LEVEL_DATA, POLICY_SEQ, DATA1,
           --Contact Type 'Applicant'       
          DATA3,
           --User Role Code       
          RIGHT_GRANTED,
           --'ACA'       
          STATUS,
           --'Y'       
          REC_DATE,
           --GETDATE()       
          REC_FUL_NAM, REC_STATUS, MENU_LEVEL)
      VALUES
         (V_SERV_PROV_CODE, 'ACA_CONTACT_TYPE_USER_ROLES', 'MODULE',
          ROW_CONTACT_TYPE_MODULE.MODULE_NAME, V_POLICYSEQ, 'Applicant',
          ROW_CONTACT_TYPE_MODULE.USER_ROLE_CODE, 'ACA', 'Y', SYSDATE, 'ADMIN',
          'A', '0');
   
     
   
   END LOOP;

   CLOSE CUR_CONTACT_TYPE_MODULE;

   UPDATE AA_SYS_SEQ
      SET LAST_NUMBER = V_POLICYSEQ
    WHERE SEQUENCE_NAME = 'XPOLICY_SEQ';

   COMMIT;

END;
/

EXEC ACA_BIZ_TO_POLICY_CONTACT_TYPE('XXXXX');

DROP PROCEDURE ACA_BIZ_TO_POLICY_CONTACT_TYPE;

CREATE OR REPLACE PROCEDURE ACA_BIZ_TO_POLICY_CONTACT_TYPE(V_SERV_PROV_CODE IN VARCHAR2) IS
   V_POLICYSEQ NUMBER;

   CURSOR CUR_CONTACT_TYPE_MODULE(IV_SERV_PROV_CODE VARCHAR2) IS
      SELECT C.CONSTANT_NAME AS MODULE_NAME, B.BIZDOMAIN_VALUE AS CONTACT_TYPE
        FROM RBIZDOMAIN A, RBIZDOMAIN_VALUE B, R1SERVER_CONSTANT C
       WHERE A.SERV_PROV_CODE = B.SERV_PROV_CODE
		AND C.SERV_PROV_CODE = B.SERV_PROV_CODE
		AND A.BIZDOMAIN = B.BIZDOMAIN
		AND A.REC_STATUS = 'A'
		AND UPPER(B.BIZDOMAIN) = 'CONTACT TYPE'
		AND C.DESCRIPTION = 'Module'
		AND C.REC_STATUS = 'A'
		AND B.SERV_PROV_CODE = IV_SERV_PROV_CODE
		AND NOT EXISTS (SELECT 1 FROM XPOLICY
			WHERE SERV_PROV_CODE = IV_SERV_PROV_CODE
				AND POLICY_NAME = 'ACA_CONTACT_TYPE_USER_ROLES'
				AND UPPER(LEVEL_TYPE) = 'MODULE'
				AND LEVEL_DATA = C.CONSTANT_NAME
				AND DATA1 = B.BIZDOMAIN_VALUE
				AND RIGHT_GRANTED = 'ACA');

   ROW_CONTACT_TYPE_MODULE CUR_CONTACT_TYPE_MODULE%ROWTYPE;

BEGIN
   -- check agency code
   SPC_EXISTS_CHECK('ACA_BIZ_TO_POLICY_CONTACT_TYPE', V_SERV_PROV_CODE);
   
   OPEN CUR_CONTACT_TYPE_MODULE(V_SERV_PROV_CODE);

   SELECT LAST_NUMBER
     INTO V_POLICYSEQ
     FROM AA_SYS_SEQ
    WHERE SEQUENCE_NAME = 'XPOLICY_SEQ';

   LOOP
      FETCH CUR_CONTACT_TYPE_MODULE
         INTO ROW_CONTACT_TYPE_MODULE;
   
      EXIT WHEN CUR_CONTACT_TYPE_MODULE%NOTFOUND;
      V_POLICYSEQ := V_POLICYSEQ + 1;
      
      INSERT INTO XPOLICY
         (SERV_PROV_CODE, POLICY_NAME,
           --ACA_CONTACT_TYPE_USER_ROLES       
          LEVEL_TYPE,
           --'MODULE'       
          LEVEL_DATA, POLICY_SEQ, DATA1,
           --Contact Type 'Applicant'       
          DATA3,
           --User Role Code       
          RIGHT_GRANTED,
           --'ACA'       
          STATUS,
           --'Y'       
          REC_DATE,
           --GETDATE()       
          REC_FUL_NAM, REC_STATUS, MENU_LEVEL)
      VALUES
         (V_SERV_PROV_CODE, 'ACA_CONTACT_TYPE_USER_ROLES', 'MODULE',
          ROW_CONTACT_TYPE_MODULE.MODULE_NAME, V_POLICYSEQ,
          ROW_CONTACT_TYPE_MODULE.CONTACT_TYPE, '1000000000', 'ACA', 'Y',
          SYSDATE, 'ADMIN', 'A', '0');
   
      
   
   END LOOP;

   CLOSE CUR_CONTACT_TYPE_MODULE;

   UPDATE AA_SYS_SEQ
      SET LAST_NUMBER = V_POLICYSEQ
    WHERE SEQUENCE_NAME = 'XPOLICY_SEQ';

   COMMIT;

END;
/

EXEC ACA_BIZ_TO_POLICY_CONTACT_TYPE('XXXXX');

DROP PROCEDURE ACA_BIZ_TO_POLICY_CONTACT_TYPE;

drop procedure SPC_EXISTS_CHECK;

