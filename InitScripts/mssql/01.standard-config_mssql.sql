-- Comment: Please input your actual ID to replace XXXXX.
/**
	1.  Setup ACA_CONFIGS Standard Choice Entry
	2.  Add TRANSACTION_LOG_INTERVAL entry to the ACA_CONFIGS
	3.  Enable Home Tab
	4.  Insert standard password reset email content
	**/

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
		SET @ERR_MSG = 'Please specify the agency code in EXEC ' + COALESCE(@P_PROCEDURE_NAME, '') + '(''' + UPPER(@P_SPC) + ''') of file 01.standard-config_mssql.sql'
      RAISERROR(@ERR_MSG, 16, 1) with nowait 
      --exit when the agency not exists
      RAISERROR('interrupt', 20, 127, '1', '1') with log  
    END
END
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[STANDARD_CONFIG]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
   drop procedure [dbo].[STANDARD_CONFIG]
END
GO

CREATE PROCEDURE DBO.STANDARD_CONFIG
   @SERVPROVCODE VARCHAR(15)
AS
BEGIN
   -- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @SERVPROVCODE
   
DECLARE @DEFAULTMODULE VARCHAR(15)
SET @DEFAULTMODULE = 'Building'
DECLARE @RCOUNT BIGINT
DECLARE @SEQ BIGINT
DECLARE @PRINTVARIABLE VARCHAR(4000)

-- Check if the anonymous user record exists in the publicuser table.  If it doesn't then create the record
SELECT @RCOUNT = COUNT(*) FROM PUBLICUSER WHERE USER_ID = 'anonymous'
IF @RCOUNT = 0
    BEGIN
    SET @PRINTVARIABLE = 'Inserting anonymous user record into the PUBLICUSER table'
    PRINT @PRINTVARIABLE
    INSERT
      INTO PUBLICUSER(USER_SEQ_NBR, USER_ID, PASSWORD, FNAME, LNAME, EMAIL_ID,
                      REC_DATE, REC_FUL_NAM, REC_STATUS)
      VALUES (0, 'anonymous', '0a92fab3230134cca6eadd9898325b9b2ae67998',
              'anonymous', 'anonymous', 'anonymous', GETDATE(), 'ADMIN', 'A')
    END


-- Check if the PUBLICUSER0 record exists in the PUSER table for this agency
SELECT @RCOUNT = COUNT(*) FROM PUSER WHERE SERV_PROV_CODE = @SERVPROVCODE AND
                                           USER_NAME = 'PUBLICUSER0'
IF @RCOUNT = 0
    BEGIN
    SET @PRINTVARIABLE = 'Inserting PUBLICUSER0 record into the PUSER table'
    PRINT @PRINTVARIABLE
    INSERT
      INTO PUSER(SERV_PROV_CODE, USER_NAME, PASSWORD, DISP_NAME, STATUS,
                 REC_DATE, REC_STATUS, REC_FUL_NAM, FNAME, LNAME, GA_USER_ID,
                 EMPLOYEE_ID)
      VALUES (@SERVPROVCODE, 'PUBLICUSER0',
              '822f6dd22fdb753eb748edc705008331d9859267', 'PUBLICUSER0',
              'ENABLE', GETDATE(), 'A', 'ADMIN', 'Public', 'User', '0', '0')
    END


-- Check if the G3STAFFS record exists in the for PUBLICUSER0 in this agency
SELECT @RCOUNT = COUNT(*) FROM G3STAFFS WHERE SERV_PROV_CODE = @SERVPROVCODE AND
                                              GA_USER_ID = '0'
IF @RCOUNT = 0
    BEGIN
    SET @PRINTVARIABLE = 'Inserting G3STAFFS record'
    PRINT @PRINTVARIABLE
    INSERT
      INTO G3STAFFS(GA_USER_ID, SERV_PROV_CODE, GA_FNAME, GA_LNAME,
                    GA_BUREAU_CODE, GA_DIVISION_CODE, GA_OFFICE_CODE,
                    GA_SECTION_CODE, GA_GROUP_CODE, REC_DATE, REC_FUL_NAM,
                    REC_STATUS, USER_NAME, GA_EMAIL, GA_AGENCY_CODE, GA_IVR_SEQ)
      VALUES ('0', @SERVPROVCODE, 'Public', 'User', 'NA', 'NA', 'NA', 'NA',
              'NA', GETDATE(), 'ADMIN', 'A', 'PUBLICUSER0',
              'anonymous@anonymous.com', SUBSTRING(@SERVPROVCODE, 0, 7), 145)
    END


-- Check if the XPUBLICUSER_SERVPROV record exists in the for PUBLICUSER0 in this agency

SELECT @RCOUNT = COUNT(*)
FROM XPUBLICUSER_SERVPROV
WHERE SERV_PROV_CODE = @SERVPROVCODE AND USER_NAME = 'PUBLICUSER0'
IF @RCOUNT = 0
    BEGIN
    SELECT @PRINTVARIABLE = 'Inserting XPUBLICUSER_SERVPROV record'
    PRINT @PRINTVARIABLE
    INSERT
      INTO XPUBLICUSER_SERVPROV(USER_SEQ_NBR, SERV_PROV_CODE, USER_NAME, STATUS,
                                REC_DATE, REC_FUL_NAM, REC_STATUS, AGENCY_PIN)
      VALUES (0, @SERVPROVCODE, 'PUBLICUSER0', 'ACTIVE', GETDATE(), 'ADMIN',
              'A', '9876')
    END

DECLARE GET_MODULES CURSOR LOCAL FOR
        SELECT DISTINCT (MODULE_NAME) MODULE
          FROM PPROV_GROUP
         WHERE SERV_PROV_CODE = @SERVPROVCODE AND REC_STATUS = 'A'
DECLARE @MODULES VARCHAR(2000)
OPEN GET_MODULES
FETCH NEXT FROM GET_MODULES INTO @MODULES
WHILE (@@FETCH_STATUS <> -1)
BEGIN
SELECT @RCOUNT = COUNT(*) FROM PPROV_GROUP WHERE SERV_PROV_CODE = @SERVPROVCODE AND
                                                 DISP_TEXT = ISNULL(@MODULES,
                                                                    '') +
                                                             'PublicUser'
IF @RCOUNT = 0
    BEGIN
    SET @PRINTVARIABLE = 'Inserting PPROV_GROUP record for group' + '+' +
                         'PublcUser'
    PRINT @PRINTVARIABLE
    SELECT @SEQ = MAX(GROUP_SEQ_NBR) + 1 FROM PPROV_GROUP
    INSERT
      INTO PPROV_GROUP(GROUP_SEQ_NBR, SERV_PROV_CODE, DISP_TEXT, STATUS,
                       REC_DATE, REC_STATUS, REC_FUL_NAM, MODULE_NAME)
      VALUES (@SEQ, @SERVPROVCODE, ISNULL(@MODULES, '') + 'PublicUser',
              'ENABLE', GETDATE(), 'A', 'ADMIN', @MODULES)
    END

FETCH NEXT FROM GET_MODULES INTO @MODULES
END
CLOSE GET_MODULES
DEALLOCATE GET_MODULES

-- Create default record
SELECT @RCOUNT = COUNT(*)
FROM R1SERVER_CONSTANT
WHERE SERV_PROV_CODE = @SERVPROVCODE AND CONSTANT_NAME = 'PA_DEFAULT_MODULE'
IF @RCOUNT = 0
    BEGIN
    INSERT
      INTO R1SERVER_CONSTANT(SERV_PROV_CODE, CONSTANT_NAME, CONSTANT_VALUE,
                             VISIBLE, REC_DATE, REC_FUL_NAM, REC_STATUS)
      VALUES (@SERVPROVCODE, 'PA_DEFAULT_MODULE', @DEFAULTMODULE, 'Y',
              GETDATE(), 'ADMIN', 'A')
    END


-- Create the ACA_CONFIGS Standard Choice Entry
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN
                WHERE SERV_PROV_CODE = @SERVPROVCODE AND
                      BIZDOMAIN = 'ACA_CONFIGS')
    BEGIN
    INSERT
      INTO RBIZDOMAIN(SERV_PROV_CODE, BIZDOMAIN, DESCRIPTION, VALUE_SIZE,
                      REC_DATE, REC_FUL_NAM, REC_STATUS)
      VALUES (@SERVPROVCODE, 'ACA_CONFIGS', 'ACA Configuration', 0, GETDATE(),
              'ADMIN', 'A')
    END


-- Set the TRANSACTION_LOG_INTERVAL - Default is 120
SELECT @SEQ = MAX(BDV_SEQ_NBR) + 1
FROM RBIZDOMAIN_VALUE
WHERE SERV_PROV_CODE = @SERVPROVCODE
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERVPROVCODE AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'TRANSACTION_LOG_INTERVAL')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      VALUES (@SEQ, @SERVPROVCODE, 'ACA_CONFIGS', 'TRANSACTION_LOG_INTERVAL',
              GETDATE(), 'ADMIN', 'A', '120')
    END


-- Password Reset Emails
SELECT @SEQ = ISNULL(MAX(TEMPLATE_ID),0) + 1
FROM BCUSTOMIZED_CONTENT
WHERE SERV_PROV_CODE = @SERVPROVCODE
IF NOT EXISTS (SELECT 1
                 FROM BCUSTOMIZED_CONTENT
                WHERE SERV_PROV_CODE = @SERVPROVCODE AND
                      CONTENT_TYPE = 'ACA_EMAIL_SENDPASSWORD_SUBJECT')
    BEGIN
    INSERT
      INTO BCUSTOMIZED_CONTENT(SERV_PROV_CODE,TEMPLATE_ID,CONTENT_TYPE,CONTENT_TEXT,
        BRIEF_DESC,REC_DATE,REC_FUL_NAM,REC_STATUS)
      VALUES (@SERVPROVCODE, @SEQ, 'ACA_EMAIL_SENDPASSWORD_SUBJECT', '1', 'Default config',
              GETDATE(), 'ADMIN', 'A')
    END

SELECT @SEQ = ISNULL(MAX(TEMPLATE_ID),0) + 1
FROM BCUSTOMIZED_CONTENT
WHERE SERV_PROV_CODE = @SERVPROVCODE
IF NOT EXISTS (SELECT 1
                 FROM BCUSTOMIZED_CONTENT
                WHERE SERV_PROV_CODE = @SERVPROVCODE AND
                      CONTENT_TYPE = 'ACA_EMAIL_SENDPASSWORD_CONTENT')
    BEGIN
    INSERT
      INTO BCUSTOMIZED_CONTENT(SERV_PROV_CODE,TEMPLATE_ID,CONTENT_TYPE,CONTENT_TEXT,
        BRIEF_DESC,REC_DATE,REC_FUL_NAM,REC_STATUS)
      VALUES (@SERVPROVCODE, @SEQ, 'ACA_EMAIL_SENDPASSWORD_CONTENT', '1', 'Default config',
              GETDATE(), 'ADMIN', 'A')
    END

-- Password Reset Emails
UPDATE BCUSTOMIZED_CONTENT
   SET CONTENT_TEXT = 'Reset Password For $$firstName$$ $$lastName$$'
 WHERE SERV_PROV_CODE = @SERVPROVCODE AND
       CONTENT_TYPE = 'ACA_EMAIL_SENDPASSWORD_SUBJECT'
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
 WHERE SERV_PROV_CODE = @SERVPROVCODE AND
       CONTENT_TYPE = 'ACA_EMAIL_SENDPASSWORD_CONTENT'
END
GO

EXEC dbo.STANDARD_CONFIG 'XXXXX'
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[STANDARD_CONFIG]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
   drop procedure [dbo].[STANDARD_CONFIG]
END
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'SPC_EXISTS_CHECK' AND XTYPE = 'P')
  DROP PROCEDURE dbo.SPC_EXISTS_CHECK
GO


--reset sequence
UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  ISNULL(MAX(BDV_SEQ_NBR),0) + 1 FROM RBIZDOMAIN_VALUE) WHERE SEQUENCE_NAME='RBIZDOMAIN_VALUE_SEQ'

UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  ISNULL(MAX(TEMPLATE_ID),0) + 1 FROM BCUSTOMIZED_CONTENT) WHERE SEQUENCE_NAME='BCUSTOMIZED_CONTENT_SEQ'
GO

