-- Comment: Please input your actual ID to replace XXXXX.
--Name: standard-config.sql
--description: 
--     1.  Setup ACA_CONFIGS Standard Choice Entry
--     2.  Add TRANSACTION_LOG_INTERVAL entry to the ACA_CONFIGS
--     3.  Enable Home Tab
--     4.  Insert standard password reset email content

SET SERVEROUTPUT ON
SET DEFINE OFF

CREATE OR REPLACE PROCEDURE SPC_EXISTS_CHECK(P_PROCEDURE_NAME IN VARCHAR2, P_SPC IN VARCHAR2) IS
   L_COUNT_1 NUMBER;
BEGIN
   SELECT COUNT(*) INTO L_COUNT_1 FROM RSERV_PROV WHERE SERV_PROV_CODE = UPPER(P_SPC);
   IF (L_COUNT_1 <= 0) THEN
      
      --When agency does not exists, it will raise the error message
      RAISE_APPLICATION_ERROR((-20000 - 224), 'Please specify the agency code in EXEC ' || P_PROCEDURE_NAME || '(''' ||
                               UPPER(P_SPC) || ''') of file 01.standard_config.sql');
   END IF;
END SPC_EXISTS_CHECK;
/

create or replace procedure standard_config
(  p_spc             IN VARCHAR2
)
IS
   v_SERVPROVCODE VARCHAR2(15);
   v_DEFAULTMODULE VARCHAR2(15);
   v_RCOUNT NUMBER(19,0);
   v_SEQ NUMBER(19,0);
   v_PRINTVARIABLE VARCHAR2(4000);
   v_MODULES VARCHAR2(2000);
   CURSOR GET_MODULES
     IS SELECT DISTINCT (MODULE_NAME) MODULE
     FROM PPROV_GROUP
     WHERE SERV_PROV_CODE = v_SERVPROVCODE
     AND REC_STATUS = 'A';
BEGIN
   v_SERVPROVCODE := p_spc;
   
   -- check agency code
   SPC_EXISTS_CHECK('standard_config', v_SERVPROVCODE);
   
   v_DEFAULTMODULE := 'Building';

   -- Check if the anonymous user record exists in the publicuser table.  If it doesn't then create the record
   SELECT COUNT(*)
     INTO v_RCOUNT
     FROM PUBLICUSER
     WHERE USER_ID = 'anonymous';

   IF v_RCOUNT = 0 THEN
   BEGIN
      v_PRINTVARIABLE := 'Inserting anonymous user record into the PUBLICUSER table';

      DBMS_OUTPUT.PUT_LINE(v_PrintVariable);

      INSERT INTO PUBLICUSER
        ( USER_SEQ_NBR, USER_ID, PASSWORD, FNAME, LNAME, EMAIL_ID, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( 0, 'anonymous', '0a92fab3230134cca6eadd9898325b9b2ae67998', 'anonymous', 'anonymous', 'anonymous', SYSDATE, 'ADMIN', 'A' );

   END;
   END IF;

   -- Check if the PUBLICUSER0 record exists in the PUSER table for this agency
   SELECT COUNT(*)
     INTO v_RCOUNT
     FROM PUSER
     WHERE SERV_PROV_CODE = v_SERVPROVCODE
             AND USER_NAME = 'PUBLICUSER0';

   IF v_RCOUNT = 0 THEN
   BEGIN
      v_PRINTVARIABLE := 'Inserting PUBLICUSER0 record into the PUSER table';

      DBMS_OUTPUT.PUT_LINE(v_PrintVariable);

      INSERT INTO PUSER
        ( SERV_PROV_CODE, USER_NAME, PASSWORD, DISP_NAME, STATUS, REC_DATE, REC_STATUS, REC_FUL_NAM, FNAME, LNAME, GA_USER_ID, EMPLOYEE_ID )
        VALUES ( v_SERVPROVCODE, 'PUBLICUSER0', '822f6dd22fdb753eb748edc705008331d9859267', 'PUBLICUSER0', 'ENABLE', SYSDATE, 'A', 'ADMIN', 'Public', 'User', '0', '0' );

   END;
   END IF;

   -- Check if the G3STAFFS record exists in the for PUBLICUSER0 in this agency
   SELECT COUNT(*)
     INTO v_RCOUNT
     FROM G3STAFFS
     WHERE SERV_PROV_CODE = v_SERVPROVCODE
             AND GA_USER_ID = '0';

   IF v_RCOUNT = 0 THEN
   BEGIN
      v_PRINTVARIABLE := 'Inserting G3STAFFS record';

      DBMS_OUTPUT.PUT_LINE(v_PrintVariable);

      INSERT INTO G3STAFFS
        ( GA_USER_ID, SERV_PROV_CODE, GA_FNAME, GA_LNAME, GA_BUREAU_CODE, GA_DIVISION_CODE, GA_OFFICE_CODE, GA_SECTION_CODE, GA_GROUP_CODE, REC_DATE, REC_FUL_NAM, REC_STATUS, USER_NAME, GA_EMAIL, GA_AGENCY_CODE, GA_IVR_SEQ )
        VALUES ( '0', v_SERVPROVCODE, 'Public', 'User', 'NA', 'NA', 'NA', 'NA', 'NA', SYSDATE, 'ADMIN', 'A', 'PUBLICUSER0', 'anonymous@anonymous.com', SUBSTR(v_SERVPROVCODE, 0, 7), 145 );

   END;
   END IF;

   -- Check if the XPUBLICUSER_SERVPROV record exists in the for PUBLICUSER0 in this agency
   SELECT COUNT(*)
     INTO v_RCOUNT
     FROM XPUBLICUSER_SERVPROV
     WHERE SERV_PROV_CODE = v_SERVPROVCODE
             AND USER_NAME = 'PUBLICUSER0';

   IF v_RCOUNT = 0 THEN
   BEGIN
      v_PRINTVARIABLE := 'Inserting XPUBLICUSER_SERVPROV record';

      DBMS_OUTPUT.PUT_LINE(v_PrintVariable);

      INSERT INTO XPUBLICUSER_SERVPROV
        ( USER_SEQ_NBR, SERV_PROV_CODE, USER_NAME, STATUS, REC_DATE, REC_FUL_NAM, REC_STATUS, AGENCY_PIN )
        VALUES ( 0, v_SERVPROVCODE, 'PUBLICUSER0', 'ACTIVE', SYSDATE, 'ADMIN', 'A', '9876' );

   END;
   END IF;

   OPEN GET_MODULES;

   FETCH GET_MODULES INTO v_MODULES;

   WHILE ( GET_MODULES%FOUND)
   LOOP
      BEGIN
         SELECT COUNT(*)
           INTO v_RCOUNT
           FROM PPROV_GROUP
           WHERE SERV_PROV_CODE = v_SERVPROVCODE
                   AND DISP_TEXT = NVL(v_MODULES, '') || 'PublicUser';

         IF v_RCOUNT = 0 THEN
         BEGIN
            v_PRINTVARIABLE := 'Inserting PPROV_GROUP record for group' || '+' || 'PublcUser';

            DBMS_OUTPUT.PUT_LINE(v_PrintVariable);

            SELECT MAX(GROUP_SEQ_NBR) + 1
              INTO v_SEQ
              FROM PPROV_GROUP ;

            INSERT INTO PPROV_GROUP
              ( GROUP_SEQ_NBR, SERV_PROV_CODE, DISP_TEXT, STATUS, REC_DATE, REC_STATUS, REC_FUL_NAM, MODULE_NAME )
              VALUES ( v_SEQ, v_SERVPROVCODE, NVL(v_MODULES, '') || 'PublicUser', 'ENABLE', SYSDATE, 'A', 'ADMIN', v_MODULES );

         END;
         END IF;

         FETCH GET_MODULES INTO v_MODULES;

      END;
   END LOOP;

   CLOSE GET_MODULES;

   -- Create default record
   SELECT COUNT(*)
     INTO v_RCOUNT
     FROM R1SERVER_CONSTANT
     WHERE SERV_PROV_CODE = v_SERVPROVCODE
             AND CONSTANT_NAME = 'PA_DEFAULT_MODULE';

   IF v_RCOUNT = 0 THEN
   BEGIN
      INSERT INTO R1SERVER_CONSTANT
        ( SERV_PROV_CODE, CONSTANT_NAME, CONSTANT_VALUE, VISIBLE, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERVPROVCODE, 'PA_DEFAULT_MODULE', v_DEFAULTMODULE, 'Y', SYSDATE, 'ADMIN', 'A' );

   END;
   END IF;

   SELECT COUNT(*)
     INTO v_RCOUNT
                          FROM RBIZDOMAIN
                            WHERE SERV_PROV_CODE = v_SERVPROVCODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS' ;

   -- Create the ACA_CONFIGS Standard Choice Entry
   IF v_RCOUNT = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN
        ( SERV_PROV_CODE, BIZDOMAIN, DESCRIPTION, VALUE_SIZE, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERVPROVCODE, 'ACA_CONFIGS', 'ACA Configuration', 0, SYSDATE, 'ADMIN', 'A' );

   END;
   END IF;

   -- Set the TRANSACTION_LOG_INTERVAL - Default is 120
   SELECT MAX(BDV_SEQ_NBR) + 1
     INTO v_SEQ
     FROM RBIZDOMAIN_VALUE
     WHERE SERV_PROV_CODE = v_SERVPROVCODE;

   SELECT COUNT(*)
     INTO v_RCOUNT
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERVPROVCODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'TRANSACTION_LOG_INTERVAL' ;

   IF v_RCOUNT = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        VALUES ( v_SEQ, v_SERVPROVCODE, 'ACA_CONFIGS', 'TRANSACTION_LOG_INTERVAL', SYSDATE, 'ADMIN', 'A', '120' );

   END;
   END IF;

   -- Password Reset Emails
   SELECT NVL(MAX(TEMPLATE_ID), 0) + 1
     INTO v_SEQ
     FROM BCUSTOMIZED_CONTENT
     WHERE SERV_PROV_CODE = v_SERVPROVCODE;

   SELECT COUNT(*)
     INTO v_RCOUNT
                          FROM BCUSTOMIZED_CONTENT
                            WHERE SERV_PROV_CODE = v_SERVPROVCODE
                                    AND CONTENT_TYPE = 'ACA_EMAIL_SENDPASSWORD_SUBJECT' ;

   IF v_RCOUNT = 0 THEN
   BEGIN
      INSERT INTO BCUSTOMIZED_CONTENT
        ( SERV_PROV_CODE, TEMPLATE_ID, CONTENT_TYPE, CONTENT_TEXT, BRIEF_DESC, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERVPROVCODE, v_SEQ, 'ACA_EMAIL_SENDPASSWORD_SUBJECT', '1', 'Default config', SYSDATE, 'ADMIN', 'A' );

   END;
   END IF;

   SELECT NVL(MAX(TEMPLATE_ID), 0) + 1
     INTO v_SEQ
     FROM BCUSTOMIZED_CONTENT
     WHERE SERV_PROV_CODE = v_SERVPROVCODE;

   SELECT COUNT(*)
     INTO v_RCOUNT
                          FROM BCUSTOMIZED_CONTENT
                            WHERE SERV_PROV_CODE = v_SERVPROVCODE
                                    AND CONTENT_TYPE = 'ACA_EMAIL_SENDPASSWORD_CONTENT' ;

   IF v_RCOUNT = 0 THEN
   BEGIN
      INSERT INTO BCUSTOMIZED_CONTENT
        ( SERV_PROV_CODE, TEMPLATE_ID, CONTENT_TYPE, CONTENT_TEXT, BRIEF_DESC, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERVPROVCODE, v_SEQ, 'ACA_EMAIL_SENDPASSWORD_CONTENT', '1', 'Default config', SYSDATE, 'ADMIN', 'A' );

   END;
   END IF;
   
   UPDATE BCUSTOMIZED_CONTENT
      SET CONTENT_TEXT = 'Reset Password For $$firstName$$ $$lastName$$'
      WHERE SERV_PROV_CODE = v_SERVPROVCODE
     AND CONTENT_TYPE = 'ACA_EMAIL_SENDPASSWORD_SUBJECT';
   
   UPDATE BCUSTOMIZED_CONTENT
      SET CONTENT_TEXT =
          '<P>Your reset password is $$password$$.</P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">userID=$$userID$$<?xml:namespace prefix = o ns = "urn:schemas-microsoft-com:office:office" /><o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">servProvCode=$$servProvCode$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">firstName=$$firstName$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">middleName=$$middleName$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">lastName=$$lastName$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">password=$$password$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">email=$$email$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">businessName=$$businessName$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">state=$$state$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">city=$$city$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">address=$$address$$<o:p></o:p></SPAN></FONT></P>
                  <P class=MsoNormal><FONT face=Arial size=1><SPAN lang=EN-US style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">zip=$$zip$$</SPAN></FONT></P>'
    WHERE SERV_PROV_CODE = v_SERVPROVCODE AND
          CONTENT_TYPE = 'ACA_EMAIL_SENDPASSWORD_CONTENT';
	
	UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  NVL(MAX(BDV_SEQ_NBR),0) + 1 FROM RBIZDOMAIN_VALUE) WHERE SEQUENCE_NAME='RBIZDOMAIN_VALUE_SEQ';

	COMMIT;
END;
/

exec standard_config ('XXXXX');

drop procedure standard_config;

drop procedure SPC_EXISTS_CHECK;

