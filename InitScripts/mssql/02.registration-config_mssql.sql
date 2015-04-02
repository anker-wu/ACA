-- Comment: Please input your actual ID to replace XXXXX.
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'SPC_EXISTS_CHECK' AND XTYPE = 'P')
  DROP PROCEDURE dbo.SPC_EXISTS_CHECK
GO

CREATE PROCEDURE dbo.SPC_EXISTS_CHECK
   @P_PROCEDURE_NAME  VARCHAR(2000),
   @P_SPC             VARCHAR(20)
AS 
DECLARE
   @V_COUNT_1 TINYINT
BEGIN
    SET @V_COUNT_1 = 0
    SELECT @V_COUNT_1 = COUNT(*) FROM RSERV_PROV WHERE SERV_PROV_CODE = UPPER(@P_SPC)
	IF (@V_COUNT_1 <= 0)
	BEGIN
	   DECLARE @ERR_MSG NVARCHAR(200)
		SET @ERR_MSG = 'Please specify the agency code in EXEC ' + COALESCE(@P_PROCEDURE_NAME, '') + '(''' + UPPER(@P_SPC) + ''') of file 02.registration-config_mssql.sql'
      RAISERROR(@ERR_MSG, 16, 1) with nowait 
      --exit when the agency not exists
      RAISERROR('interrupt', 20, 127, '1', '1') with log  
    END
END
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[REGISTRATION_CONFIG]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
   drop procedure [dbo].[REGISTRATION_CONFIG]
END
GO

CREATE PROCEDURE DBO.REGISTRATION_CONFIG
   @SERV_PROV_CODE VARCHAR(15)
AS
BEGIN
   -- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @SERV_PROV_CODE
DECLARE @ENABLE_EMAIL_VERIFICATION VARCHAR(3)
SET @ENABLE_EMAIL_VERIFICATION = 'YES'
DECLARE @ENABLE_AGENCY_NOTIFICATION VARCHAR(3)
SET @ENABLE_AGENCY_NOTIFICATION = 'YES'
DECLARE @AGENCY_NOTIFICATION_EMAIL VARCHAR(100)
SET @AGENCY_NOTIFICATION_EMAIL = 'support@accela.com'
DECLARE @RCOUNT BIGINT
DECLARE @BDV_SEQ_NBR NUMERIC(9)
DECLARE @SEQ NUMERIC(9)

-- Do cleanup
--DELETE
  --FROM RBIZDOMAIN_VALUE
  --WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
        --BIZDOMAIN_VALUE = 'PA_EMAIL_VERIFICATION'
--DELETE
  --FROM RBIZDOMAIN_VALUE
  --WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
        --BIZDOMAIN = 'ACA_AGENCY_EMAIL_REGISTRATION_TO'
--DELETE
  --FROM RBIZDOMAIN
  --WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
        --BIZDOMAIN = 'ACA_AGENCY_EMAIL_REGISTRATION_TO'

--DELETE 
  --FROM RBIZDOMAIN_VALUE
  --WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND 
		--BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_FROM'

--DELETE
  --FROM RBIZDOMAIN
  --WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
        --BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_FROM'
		
--DELETE 
  --FROM RBIZDOMAIN_VALUE
  --WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND 
		--BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_TO'

--DELETE
  --FROM RBIZDOMAIN
  --WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
        --BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_TO'		
		 	
--insert ACA_CONFIGS bizdomain if it does not exist, added by achievo
SELECT @RCOUNT = COUNT(*)
FROM RBIZDOMAIN
WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND BIZDOMAIN = 'ACA_CONFIGS'
IF @RCOUNT = 0
    BEGIN
    Insert
      into RBIZDOMAIN(SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE,
                      REC_FUL_NAM, REC_STATUS)
      Values (@SERV_PROV_CODE, 'ACA_CONFIGS', 0, GETDATE(), 'ADMIN', 'A')
    END


-- Insert the PA_EMAIL_VERIFICATION value
SELECT @BDV_SEQ_NBR = MAX(BDV_SEQ_NBR) + 1 FROM RBIZDOMAIN_VALUE
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'PA_EMAIL_VERIFICATION')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      VALUES (@BDV_SEQ_NBR, @SERV_PROV_CODE, 'ACA_CONFIGS',
              'PA_EMAIL_VERIFICATION', GETDATE(), 'ADMIN', 'A',
              @ENABLE_EMAIL_VERIFICATION)
     SELECT @BDV_SEQ_NBR = MAX(BDV_SEQ_NBR) + 1 FROM RBIZDOMAIN_VALUE
    END


IF @ENABLE_AGENCY_NOTIFICATION = 'YES'
    BEGIN
	--insert the ACA_AGENCY_EMAIL_REGISTRATION_TO
    IF NOT EXISTS (SELECT 1
                     FROM RBIZDOMAIN
                    WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                          BIZDOMAIN = 'ACA_AGENCY_EMAIL_REGISTRATION_TO')
        BEGIN
        INSERT
          INTO RBIZDOMAIN(SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE,
                          REC_FUL_NAM, REC_STATUS)
          VALUES (@SERV_PROV_CODE, 'ACA_AGENCY_EMAIL_REGISTRATION_TO', 0,
                  GETDATE(), 'ADMIN', 'A')
        END

    IF NOT EXISTS (SELECT 1
                     FROM RBIZDOMAIN_VALUE
                    WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                          BIZDOMAIN = 'ACA_AGENCY_EMAIL_REGISTRATION_TO' AND
                          BIZDOMAIN_VALUE = @AGENCY_NOTIFICATION_EMAIL)
        BEGIN
        INSERT
          INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                                BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM,
                                REC_STATUS, VALUE_DESC)
          VALUES (@BDV_SEQ_NBR, @SERV_PROV_CODE,
                  'ACA_AGENCY_EMAIL_REGISTRATION_TO',
                  @AGENCY_NOTIFICATION_EMAIL, GETDATE(), 'ADMIN', 'A', NULL)
          SELECT @BDV_SEQ_NBR = MAX(BDV_SEQ_NBR) + 1 FROM RBIZDOMAIN_VALUE
        END

	--insert the ACA_EMAIL_REGISTRATION_FROM
    IF NOT EXISTS (SELECT 1
                     FROM RBIZDOMAIN
                    WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                          BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_FROM')
        BEGIN
        INSERT
          INTO RBIZDOMAIN(SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE,
                          REC_FUL_NAM, REC_STATUS)
          VALUES (@SERV_PROV_CODE, 'ACA_EMAIL_REGISTRATION_FROM', 0,
                  GETDATE(), 'ADMIN', 'A')
        END

    IF NOT EXISTS (SELECT 1
                     FROM RBIZDOMAIN_VALUE
                    WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                          BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_FROM')
        BEGIN
        INSERT
          INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                                BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM,
                                REC_STATUS, VALUE_DESC)
          VALUES (@BDV_SEQ_NBR, @SERV_PROV_CODE,
                  'ACA_EMAIL_REGISTRATION_FROM',
                  @AGENCY_NOTIFICATION_EMAIL, GETDATE(), 'ADMIN', 'A', @AGENCY_NOTIFICATION_EMAIL)
          SELECT @BDV_SEQ_NBR = MAX(BDV_SEQ_NBR) + 1 FROM RBIZDOMAIN_VALUE
        END

	--insert the ACA_EMAIL_REGISTRATION_TO
    IF NOT EXISTS (SELECT 1
                     FROM RBIZDOMAIN
                    WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                          BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_TO')
        BEGIN
        INSERT
          INTO RBIZDOMAIN(SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE,
                          REC_FUL_NAM, REC_STATUS)
          VALUES (@SERV_PROV_CODE, 'ACA_EMAIL_REGISTRATION_TO', 0,
                  GETDATE(), 'ADMIN', 'A')
        END

    IF NOT EXISTS (SELECT 1
                     FROM RBIZDOMAIN_VALUE
                    WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                          BIZDOMAIN = 'ACA_EMAIL_REGISTRATION_TO' AND
                          BIZDOMAIN_VALUE = @AGENCY_NOTIFICATION_EMAIL)
        BEGIN
        INSERT
          INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                                BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM,
                                REC_STATUS, VALUE_DESC)
          VALUES (@BDV_SEQ_NBR, @SERV_PROV_CODE,
                  'ACA_EMAIL_REGISTRATION_TO',
                  @AGENCY_NOTIFICATION_EMAIL, GETDATE(), 'ADMIN', 'A', NULL)
          SELECT @BDV_SEQ_NBR = MAX(BDV_SEQ_NBR) + 1 FROM RBIZDOMAIN_VALUE
        END
		
    END

------ This is the email sent to the citizen user when they register
SELECT @SEQ = ISNULL(MAX(TEMPLATE_ID),0) + 1
FROM BCUSTOMIZED_CONTENT
WHERE SERV_PROV_CODE = @SERV_PROV_CODE
IF NOT EXISTS (SELECT 1
                 FROM BCUSTOMIZED_CONTENT
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                      CONTENT_TYPE = 'ACA_ACTIVATION_EMAIL_SUBJECT')
    BEGIN
    INSERT
      INTO BCUSTOMIZED_CONTENT(SERV_PROV_CODE,TEMPLATE_ID,CONTENT_TYPE,CONTENT_TEXT,
        BRIEF_DESC,REC_DATE,REC_FUL_NAM,REC_STATUS)
      VALUES (@SERV_PROV_CODE, @SEQ, 'ACA_ACTIVATION_EMAIL_SUBJECT', '1', 'Default config',
              GETDATE(), 'ADMIN', 'A')
    END

SELECT @SEQ = ISNULL(MAX(TEMPLATE_ID),0) + 1
FROM BCUSTOMIZED_CONTENT
WHERE SERV_PROV_CODE = @SERV_PROV_CODE
IF NOT EXISTS (SELECT 1
                 FROM BCUSTOMIZED_CONTENT
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                      CONTENT_TYPE = 'ACA_ACTIVATION_EMAIL_CONTENT')
    BEGIN
    INSERT
      INTO BCUSTOMIZED_CONTENT(SERV_PROV_CODE,TEMPLATE_ID,CONTENT_TYPE,CONTENT_TEXT,
        BRIEF_DESC,REC_DATE,REC_FUL_NAM,REC_STATUS)
      VALUES (@SERV_PROV_CODE, @SEQ, 'ACA_ACTIVATION_EMAIL_CONTENT', '1', 'Default config',
              GETDATE(), 'ADMIN', 'A')
    END

------ This is the email sent to the citizen user when they register
UPDATE BCUSTOMIZED_CONTENT
   SET CONTENT_TEXT = 'Welcome to the City of Bridview''s Citizen Portal'
 WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
       CONTENT_TYPE = 'ACA_ACTIVATION_EMAIL_SUBJECT'
UPDATE BCUSTOMIZED_CONTENT
   SET CONTENT_TEXT =
       '<p>Welcome $$firstName$$  $$lastName$$ to the CIty of Bridgeview''s Citizen Portal!</P>
	<P>Detail information of the Account<BR>Account Name $$userID$$<BR>User name $$firstName$$ $$middleName$$ $$lastName$$!<BR>Agency and City  $$servProvCode$$   $$city$$<BR>Business $$businessName$$  <BR>State $$state$$  <BR>Address  $$address$$  <BR>Zip  $$zip$$  </P>'
 WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
       CONTENT_TYPE = 'ACA_ACTIVATION_EMAIL_CONTENT'

------ This is the email sent to the licensed professional when they register
SELECT @SEQ = ISNULL(MAX(TEMPLATE_ID),0) + 1
FROM BCUSTOMIZED_CONTENT
WHERE SERV_PROV_CODE = @SERV_PROV_CODE
IF NOT EXISTS (SELECT 1
                 FROM BCUSTOMIZED_CONTENT
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                      CONTENT_TYPE = 'ACA_INACTIVATION_EMAIL_SUBJECT')
    BEGIN
    INSERT
      INTO BCUSTOMIZED_CONTENT(SERV_PROV_CODE,TEMPLATE_ID,CONTENT_TYPE,CONTENT_TEXT,
        BRIEF_DESC,REC_DATE,REC_FUL_NAM,REC_STATUS)
      VALUES (@SERV_PROV_CODE, @SEQ, 'ACA_INACTIVATION_EMAIL_SUBJECT', '1', 'Default config',
              GETDATE(), 'ADMIN', 'A')
    END

SELECT @SEQ = ISNULL(MAX(TEMPLATE_ID),0) + 1
FROM BCUSTOMIZED_CONTENT
WHERE SERV_PROV_CODE = @SERV_PROV_CODE
IF NOT EXISTS (SELECT 1
                 FROM BCUSTOMIZED_CONTENT
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                      CONTENT_TYPE = 'ACA_INACTIVATION_EMAIL_CONTENT')
    BEGIN
    INSERT
      INTO BCUSTOMIZED_CONTENT(SERV_PROV_CODE,TEMPLATE_ID,CONTENT_TYPE,CONTENT_TEXT,
        BRIEF_DESC,REC_DATE,REC_FUL_NAM,REC_STATUS)
      VALUES (@SERV_PROV_CODE, @SEQ, 'ACA_INACTIVATION_EMAIL_CONTENT', '1', 'Default config',
              GETDATE(), 'ADMIN', 'A')
    END

SELECT @SEQ = ISNULL(MAX(TEMPLATE_ID),0) + 1
FROM BCUSTOMIZED_CONTENT
WHERE SERV_PROV_CODE = @SERV_PROV_CODE
IF NOT EXISTS (SELECT 1
                 FROM BCUSTOMIZED_CONTENT
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                      CONTENT_TYPE = 'ACA_AGENCY_ACTIVATION_EMAIL_SUBJECT')
    BEGIN
    INSERT
      INTO BCUSTOMIZED_CONTENT(SERV_PROV_CODE,TEMPLATE_ID,CONTENT_TYPE,CONTENT_TEXT,
        BRIEF_DESC,REC_DATE,REC_FUL_NAM,REC_STATUS)
      VALUES (@SERV_PROV_CODE, @SEQ, 'ACA_AGENCY_ACTIVATION_EMAIL_SUBJECT', '1', 'Default config',
              GETDATE(), 'ADMIN', 'A')
    END

SELECT @SEQ = ISNULL(MAX(TEMPLATE_ID),0) + 1
FROM BCUSTOMIZED_CONTENT
WHERE SERV_PROV_CODE = @SERV_PROV_CODE
IF NOT EXISTS (SELECT 1
                 FROM BCUSTOMIZED_CONTENT
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
                      CONTENT_TYPE = 'ACA_AGENCY_ACTIVATION_EMAIL_CONTENT')
    BEGIN
    INSERT
      INTO BCUSTOMIZED_CONTENT(SERV_PROV_CODE,TEMPLATE_ID,CONTENT_TYPE,CONTENT_TEXT,
        BRIEF_DESC,REC_DATE,REC_FUL_NAM,REC_STATUS)
      VALUES (@SERV_PROV_CODE, @SEQ, 'ACA_AGENCY_ACTIVATION_EMAIL_CONTENT', '1', 'Default config',
              GETDATE(), 'ADMIN', 'A')
    END


------ This is the email sent to the licensed professional when they register
UPDATE BCUSTOMIZED_CONTENT
   SET CONTENT_TEXT = 'Welcome to the City of Bridgview''s Contractor Portal'
 WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
       CONTENT_TYPE = 'ACA_INACTIVATION_EMAIL_SUBJECT'
UPDATE BCUSTOMIZED_CONTENT
   SET CONTENT_TEXT =
       '<P>$$firstName$$ $$middleName$$ $$lastName$$</P>
	<P>Thank you for registering for an online account.  Your account requires validation. Once your account has been validated and activated you will receive an email.</P>
	<P>Detail information of the Account<BR>Account Name $$userID$$<BR></P>
	<P>Business $$businessName$$ </P>
	<P>User name $$firstName$$ $$middleName$$ $$lastName$$<BR>City   $$city$$<BR>State $$state$$<BR>Address  $$address$$<BR>Zip  $$zip$$</P>
	<P>Thank you for registration our site. </P>'
 WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
       CONTENT_TYPE = 'ACA_INACTIVATION_EMAIL_CONTENT'
UPDATE BCUSTOMIZED_CONTENT
   SET CONTENT_TEXT = 'New user needs activated'
 WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
       CONTENT_TYPE = 'ACA_AGENCY_ACTIVATION_EMAIL_SUBJECT'
UPDATE BCUSTOMIZED_CONTENT
   SET CONTENT_TEXT =
       '<P>The following new user needs activated</P>
	<p>W$$firstName$$  $$lastName$$ to the CIty of Bridgeview''s Citizen Portal!</P>
	<P>Detail information of the Account<BR>Account Name $$userID$$<BR>User name $$firstName$$ $$middleName$$ $$lastName$$!<BR>Agency and City  $$servProvCode$$   $$city$$<BR>Business $$businessName$$  <BR>State $$state$$  <BR>Address  $$address$$  <BR>Zip  $$zip$$  </P>'
 WHERE SERV_PROV_CODE = @SERV_PROV_CODE AND
       CONTENT_TYPE = 'ACA_AGENCY_ACTIVATION_EMAIL_CONTENT'
END
GO

EXEC DBO.REGISTRATION_CONFIG 'XXXXX'
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[REGISTRATION_CONFIG]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
   drop procedure [dbo].[REGISTRATION_CONFIG]
END
GO


--reset sequence
UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  ISNULL(MAX(BDV_SEQ_NBR),0) + 1 FROM RBIZDOMAIN_VALUE) WHERE SEQUENCE_NAME='RBIZDOMAIN_VALUE_SEQ'

UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  ISNULL(MAX(TEMPLATE_ID),0) + 1 FROM BCUSTOMIZED_CONTENT) WHERE SEQUENCE_NAME='BCUSTOMIZED_CONTENT_SEQ'
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'SPC_EXISTS_CHECK' AND XTYPE = 'P')
  DROP PROCEDURE dbo.SPC_EXISTS_CHECK
GO
