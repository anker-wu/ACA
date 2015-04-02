-- Comment: Please input your actual ID to replace XXXXX.
--Name: registration-config.sql
--description: 
--     1.  ACA register setting

SET SERVEROUTPUT ON
SET DEFINE OFF

CREATE OR REPLACE PROCEDURE SPC_EXISTS_CHECK(P_PROCEDURE_NAME IN VARCHAR2, P_SPC IN VARCHAR2) IS
   L_COUNT_1 NUMBER;
BEGIN
   SELECT COUNT(*) INTO L_COUNT_1 FROM RSERV_PROV WHERE SERV_PROV_CODE = UPPER(P_SPC);
   IF (L_COUNT_1 <= 0) THEN
      
      --When agency does not exists, it will raise the error message
      RAISE_APPLICATION_ERROR((-20000 - 224), 'Please specify the agency code in EXEC ' || P_PROCEDURE_NAME || '(''' ||
                               UPPER(P_SPC) || ''') of file 02.registration-config.sql');
   END IF;
END SPC_EXISTS_CHECK;
/

create or replace procedure registration_config
(  p_spc             IN VARCHAR2
)
IS
   v_SERV_PROV_CODE VARCHAR2(15);
   v_ENABLE_EMAIL_VERIFICATION VARCHAR2(3);
   v_ENABLE_AGENCY_NOTIFICATION VARCHAR2(3);
   v_AGENCY_NOTIFICATION_EMAIL VARCHAR2(100);
   v_RCOUNT NUMBER(19,0);
   v_BDV_SEQ_NBR NUMBER(9);
   v_seq NUMBER(19,0);
   v_temp NUMBER(1, 0) := 0;
BEGIN
   v_SERV_PROV_CODE := p_spc;
   -- check agency code
   SPC_EXISTS_CHECK('registration_config', v_SERV_PROV_CODE);

   v_ENABLE_EMAIL_VERIFICATION := 'YES';

   v_ENABLE_AGENCY_NOTIFICATION := 'YES';

   v_AGENCY_NOTIFICATION_EMAIL := 'support@accela.com';

   -- Do cleanup
   --DELETE RBIZDOMAIN_VALUE
     --WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
             --AND BIZDOMAIN_VALUE = 'PA_EMAIL_VERIFICATION';

   --DELETE RBIZDOMAIN_VALUE
     --WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
             --AND BIZDOMAIN = 'ACA_AGENCY_EMAIL_REGISTRATION_TO';

   --DELETE RBIZDOMAIN
     --WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
             --AND BIZDOMAIN = 'ACA_AGENCY_EMAIL_REGISTRATION_TO';

   --DELETE RBIZDOMAIN_VALUE
     --WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
             --AND BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_FROM';

   --DELETE RBIZDOMAIN
     --WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
             --AND BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_FROM';
			 
   --DELETE RBIZDOMAIN_VALUE
     --WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
             --AND BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_TO';

   --DELETE RBIZDOMAIN
     --WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
             --AND BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_TO';
			 
   --insert ACA_CONFIGS bizdomain if it does not exist, added by achievo
   SELECT COUNT(*)
     INTO v_RCOUNT
     FROM RBIZDOMAIN
     WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
             AND BIZDOMAIN = 'ACA_CONFIGS';

   IF v_RCOUNT = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN
        ( SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERV_PROV_CODE, 'ACA_CONFIGS', 0, SYSDATE, 'ADMIN', 'A' );
   END;
   END IF;

   -- Insert the PA_EMAIL_VERIFICATION value
   SELECT MAX(BDV_SEQ_NBR) + 1
     INTO v_BDV_SEQ_NBR
     FROM RBIZDOMAIN_VALUE ;

   SELECT COUNT(*) INTO v_temp
                          FROM RBIZDOMAIN_VALUE
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND BIZDOMAIN = 'ACA_CONFIGS'
                                    AND BIZDOMAIN_VALUE = 'PA_EMAIL_VERIFICATION' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO RBIZDOMAIN_VALUE
        ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
        VALUES ( v_BDV_SEQ_NBR, v_SERV_PROV_CODE, 'ACA_CONFIGS', 'PA_EMAIL_VERIFICATION', SYSDATE, 'ADMIN', 'A', v_ENABLE_EMAIL_VERIFICATION );
   END;
   END IF;

   --insert the ACA_AGENCY_EMAIL_REGISTRATION_TO
   IF v_ENABLE_AGENCY_NOTIFICATION = 'YES' THEN
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN
	   SELECT COUNT(*) INTO v_temp
                             FROM RBIZDOMAIN
                               WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                       AND BIZDOMAIN = 'ACA_AGENCY_EMAIL_REGISTRATION_TO' ;

      IF v_temp = 0 THEN
      BEGIN
         INSERT INTO RBIZDOMAIN
           ( SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE, REC_FUL_NAM, REC_STATUS )
           VALUES ( v_SERV_PROV_CODE, 'ACA_AGENCY_EMAIL_REGISTRATION_TO', 0, SYSDATE, 'ADMIN', 'A' );

         
      END;
      END IF;
      
      SELECT MAX(v_BDV_SEQ_NBR) + 1
           INTO v_BDV_SEQ_NBR
           FROM RBIZDOMAIN_VALUE ;
           
	   SELECT COUNT(*) INTO v_temp
                             FROM RBIZDOMAIN_VALUE
                               WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                       AND BIZDOMAIN = 'ACA_AGENCY_EMAIL_REGISTRATION_TO'
                                       AND BIZDOMAIN_VALUE = v_AGENCY_NOTIFICATION_EMAIL ;

      IF v_temp = 0 THEN
      BEGIN
         INSERT INTO RBIZDOMAIN_VALUE
           ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
           VALUES ( v_BDV_SEQ_NBR, v_SERV_PROV_CODE, 'ACA_AGENCY_EMAIL_REGISTRATION_TO', v_AGENCY_NOTIFICATION_EMAIL, SYSDATE, 'ADMIN', 'A', NULL );
      END;
      END IF;
   END;
   END IF;

   --insert the ACA_EMAIL_REGISTRATION_FROM
   IF v_ENABLE_AGENCY_NOTIFICATION = 'YES' THEN
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN
	   SELECT COUNT(*) INTO v_temp
                             FROM RBIZDOMAIN
                               WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                       AND BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_FROM' ;

      IF v_temp = 0 THEN
      BEGIN
         INSERT INTO RBIZDOMAIN
           ( SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE, REC_FUL_NAM, REC_STATUS )
           VALUES ( v_SERV_PROV_CODE, 'ACA_EMAIL_REGISTRATION_FROM', 0, SYSDATE, 'ADMIN', 'A' );

         
      END;
      END IF;

      SELECT MAX(v_BDV_SEQ_NBR) + 1
           INTO v_BDV_SEQ_NBR
           FROM RBIZDOMAIN_VALUE ;
           
	   SELECT COUNT(*) INTO v_temp
                             FROM RBIZDOMAIN_VALUE
                               WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                       AND BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_FROM'
                                       AND BIZDOMAIN_VALUE = v_AGENCY_NOTIFICATION_EMAIL ;
      IF v_temp = 0 THEN
      BEGIN
         INSERT INTO RBIZDOMAIN_VALUE
           ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
           VALUES ( v_BDV_SEQ_NBR, v_SERV_PROV_CODE, 'ACA_EMAIL_REGISTRATION_FROM', v_AGENCY_NOTIFICATION_EMAIL, SYSDATE, 'ADMIN', 'A', NULL );
      END;
      END IF;

   END;
   END IF;

   --insert the ACA_EMAIL_REGISTRATION_TO
   IF v_ENABLE_AGENCY_NOTIFICATION = 'YES' THEN
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN
	   SELECT COUNT(*) INTO v_temp
                             FROM RBIZDOMAIN
                               WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                       AND BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_TO' ;

      IF v_temp = 0 THEN
      BEGIN
         INSERT INTO RBIZDOMAIN
           ( SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE, REC_FUL_NAM, REC_STATUS )
           VALUES ( v_SERV_PROV_CODE, 'ACA_EMAIL_REGISTRATION_TO', 0, SYSDATE, 'ADMIN', 'A' );
      END;
      END IF;

         SELECT MAX(v_BDV_SEQ_NBR) + 1
           INTO v_BDV_SEQ_NBR
           FROM RBIZDOMAIN_VALUE ;

	   SELECT COUNT(*) INTO v_temp
                             FROM RBIZDOMAIN_VALUE
                               WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                       AND BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_TO'
                                       AND BIZDOMAIN_VALUE = v_AGENCY_NOTIFICATION_EMAIL ;

      IF v_temp = 0 THEN
      BEGIN
         INSERT INTO RBIZDOMAIN_VALUE
           ( BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN, BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS, VALUE_DESC )
           VALUES ( v_BDV_SEQ_NBR, v_SERV_PROV_CODE, 'ACA_EMAIL_REGISTRATION_TO', v_AGENCY_NOTIFICATION_EMAIL, SYSDATE, 'ADMIN', 'A', NULL );
      END;
      END IF;

   END;
   END IF;
   
   
   ------ This is the email sent to the citizen user when they register
     SELECT NVL(MAX(TEMPLATE_ID), 0) + 1
     INTO v_SEQ
     FROM BCUSTOMIZED_CONTENT
     WHERE SERV_PROV_CODE = v_SERV_PROV_CODE;

	   SELECT COUNT(*) INTO v_temp
                          FROM BCUSTOMIZED_CONTENT
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND CONTENT_TYPE = 'ACA_INACTIVATION_EMAIL_SUBJECT' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO BCUSTOMIZED_CONTENT
        ( SERV_PROV_CODE, TEMPLATE_ID, CONTENT_TYPE, CONTENT_TEXT, BRIEF_DESC, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERV_PROV_CODE, v_SEQ, 'ACA_INACTIVATION_EMAIL_SUBJECT', '1', 'Default config', SYSDATE, 'ADMIN', 'A' );
   END;
   END IF;

   SELECT NVL(MAX(TEMPLATE_ID), 0) + 1
     INTO v_SEQ
     FROM BCUSTOMIZED_CONTENT
     WHERE SERV_PROV_CODE = v_SERV_PROV_CODE;

	   SELECT COUNT(*) INTO v_temp
                          FROM BCUSTOMIZED_CONTENT
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND CONTENT_TYPE = 'ACA_INACTIVATION_EMAIL_CONTENT' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO BCUSTOMIZED_CONTENT
        ( SERV_PROV_CODE, TEMPLATE_ID, CONTENT_TYPE, CONTENT_TEXT, BRIEF_DESC, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERV_PROV_CODE, v_SEQ, 'ACA_INACTIVATION_EMAIL_CONTENT', '1', 'Default config', SYSDATE, 'ADMIN', 'A' );
   END;
   END IF;

   SELECT NVL(MAX(TEMPLATE_ID), 0) + 1
     INTO v_SEQ
     FROM BCUSTOMIZED_CONTENT
     WHERE SERV_PROV_CODE = v_SERV_PROV_CODE;

	   SELECT COUNT(*) INTO v_temp
                          FROM BCUSTOMIZED_CONTENT
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND CONTENT_TYPE = 'ACA_AGENCY_ACTIVATION_EMAIL_SUBJECT' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO BCUSTOMIZED_CONTENT
        ( SERV_PROV_CODE, TEMPLATE_ID, CONTENT_TYPE, CONTENT_TEXT, BRIEF_DESC, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERV_PROV_CODE, v_SEQ, 'ACA_AGENCY_ACTIVATION_EMAIL_SUBJECT', '1', 'Default config', SYSDATE, 'ADMIN', 'A' );
   END;
   END IF;

   SELECT NVL(MAX(TEMPLATE_ID), 0) + 1
     INTO v_SEQ
     FROM BCUSTOMIZED_CONTENT
     WHERE SERV_PROV_CODE = v_SERV_PROV_CODE;

	   SELECT COUNT(*) INTO v_temp
                          FROM BCUSTOMIZED_CONTENT
                            WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
                                    AND CONTENT_TYPE = 'ACA_AGENCY_ACTIVATION_EMAIL_CONTENT' ;

   IF v_temp = 0 THEN
   BEGIN
      INSERT INTO BCUSTOMIZED_CONTENT
        ( SERV_PROV_CODE, TEMPLATE_ID, CONTENT_TYPE, CONTENT_TEXT, BRIEF_DESC, REC_DATE, REC_FUL_NAM, REC_STATUS )
        VALUES ( v_SERV_PROV_CODE, v_SEQ, 'ACA_AGENCY_ACTIVATION_EMAIL_CONTENT', '1', 'Default config', SYSDATE, 'ADMIN', 'A' );
   END;
   END IF;

   UPDATE BCUSTOMIZED_CONTENT
      SET CONTENT_TEXT = 'Welcome to the City of Bridview''s Citizen Portal'
      WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
     AND CONTENT_TYPE = 'ACA_ACTIVATION_EMAIL_SUBJECT';

   UPDATE BCUSTOMIZED_CONTENT
      SET CONTENT_TEXT = '<p>Welcome $$firstName$$  $$lastName$$ to the CIty of Bridgeview''s Citizen Portal!</P>
      <P>Detail information of the Account<BR>Account Name $$userID$$<BR>User name $$firstName$$ $$middleName$$ $$lastName$$!<BR>Agency and City  $$servProvCode$$   $$city$$<BR>Business $$businessName$$  <BR>State $$state$$  <BR>Address  $$address$$  <BR>Zip  $$zip$$  </P>'
    WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
      AND CONTENT_TYPE = 'ACA_ACTIVATION_EMAIL_CONTENT';

   ------ This is the email sent to the licensed professional when they register
   UPDATE BCUSTOMIZED_CONTENT
      SET CONTENT_TEXT = 'Welcome to the City of Bridgview''s Contractor Portal'
      WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
     AND CONTENT_TYPE = 'ACA_INACTIVATION_EMAIL_SUBJECT';

   UPDATE BCUSTOMIZED_CONTENT
   SET CONTENT_TEXT =
       '<P>$$firstName$$ $$middleName$$ $$lastName$$</P>
   <P>Thank you for registering for an online account.  Your account requires validation. Once your account has been validated and activated you will receive an email.</P>
   <P>Detail information of the Account<BR>Account Name $$userID$$<BR></P>
   <P>Business $$businessName$$ </P>
   <P>User name $$firstName$$ $$middleName$$ $$lastName$$<BR>City   $$city$$<BR>State $$state$$<BR>Address  $$address$$<BR>Zip  $$zip$$</P>
   <P>Thank you for registration our site. </P>'
 WHERE SERV_PROV_CODE = v_SERV_PROV_CODE 
   AND CONTENT_TYPE = 'ACA_INACTIVATION_EMAIL_CONTENT';

   UPDATE BCUSTOMIZED_CONTENT
      SET CONTENT_TEXT = 'New user needs activated'
      WHERE SERV_PROV_CODE = v_SERV_PROV_CODE
     AND CONTENT_TYPE = 'ACA_AGENCY_ACTIVATION_EMAIL_SUBJECT';
   UPDATE BCUSTOMIZED_CONTENT
      SET CONTENT_TEXT =
          '<P>The following new user needs activated</P>
      <p>W$$firstName$$  $$lastName$$ to the CIty of Bridgeview''s Citizen Portal!</P>
      <P>Detail information of the Account<BR>Account Name $$userID$$<BR>User name $$firstName$$ $$middleName$$ $$lastName$$!<BR>Agency and City  $$servProvCode$$   $$city$$<BR>Business $$businessName$$  <BR>State $$state$$  <BR>Address  $$address$$  <BR>Zip  $$zip$$  </P>'
    WHERE SERV_PROV_CODE = v_SERV_PROV_CODE AND
          CONTENT_TYPE = 'ACA_AGENCY_ACTIVATION_EMAIL_CONTENT';
	UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  NVL(MAX(BDV_SEQ_NBR),0) + 1 FROM RBIZDOMAIN_VALUE) WHERE SEQUENCE_NAME='RBIZDOMAIN_VALUE_SEQ';

	COMMIT;
END;
/

exec registration_config('XXXXX');

drop procedure registration_config;

drop procedure SPC_EXISTS_CHECK;

