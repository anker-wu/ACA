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
		SET @ERR_MSG = 'Please specify the agency code in EXEC ' + COALESCE(@P_PROCEDURE_NAME, '') + '(''' + UPPER(@P_SPC) + ''') of file 03.permits-config_mssql.sql'
      RAISERROR(@ERR_MSG, 16, 1) with nowait 
      --exit when the agency not exists
      RAISERROR('interrupt', 20, 127, '1', '1') with log  
    END
END
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PERMITS_CONFIG]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
   drop procedure [dbo].[PERMITS_CONFIG]
END
GO

CREATE PROCEDURE DBO.PERMITS_CONFIG
   @SERV_PROV_CODE_ VARCHAR(15)
AS
BEGIN
   -- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @SERV_PROV_CODE_

---- Enable Job Value
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'JOB_VALUE_ENABLED')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'JOB_VALUE_ENABLED', GETDATE(), 'ADMIN', 'A', 'YES'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end


---- Enable Auto Populate State
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'AUTO_POPULATE_STATE')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'AUTO_POPULATE_STATE', GETDATE(), 'ADMIN', 'A', 'YES'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end


---- In the code, but didn't do anything?
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'DESCRIPTION_REQUIRED_FLAG')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'DESCRIPTION_REQUIRED_FLAG', GETDATE(), 'ADMIN', 'A', 'YES'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end


------ Inspection Features Configuration
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'ALL_USER_SCHEDULE_INSPECTION_ENABLED')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'ALL_USER_SCHEDULE_INSPECTION_ENABLED', GETDATE(), 'ADMIN', 'A',
             'YES'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end

IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'DISPLAY_OPTIONAL_INSPECTIONS')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'DISPLAY_OPTIONAL_INSPECTIONS', GETDATE(), 'ADMIN', 'A', 'Yes'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end

IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'INSPECTION_CALENDAR_NAME')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'INSPECTION_CALENDAR_NAME', GETDATE(), 'ADMIN', 'A',
             'Inspection Blockout'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end


------ Reporting Features Configuration
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'V360_WEB_ACTION_URL')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'V360_WEB_ACTION_URL', GETDATE(), 'ADMIN', 'A',
             'http://av.beta.accela.com/portlets/'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end

IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'V360_WEB_ACTION_USERNAME')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'V360_WEB_ACTION_USERNAME', GETDATE(), 'ADMIN', 'A', 'admin'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end

IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'ACA_CONFIGS' AND
                      BIZDOMAIN_VALUE = 'V360_WEB_ACTION_PASSWORD')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_, 'ACA_CONFIGS',
             'V360_WEB_ACTION_PASSWORD', GETDATE(), 'ADMIN', 'A', 'testok'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    end


-------- Print Permit Report
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'PRINT_PERMIT_REPORT_BUILDING')
    BEGIN
    INSERT
      INTO RBIZDOMAIN(SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE,
                      REC_FUL_NAM, REC_STATUS)
      VALUES (@SERV_PROV_CODE_, 'PRINT_PERMIT_REPORT_BUILDING', 0, GETDATE(),
              'ADMIN', 'A')
    END

IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'PRINT_PERMIT_REPORT_BUILDING' AND
                      BIZDOMAIN_VALUE = 'REPORT_NAME')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_,
             'PRINT_PERMIT_REPORT_BUILDING', 'REPORT_NAME', GETDATE(), 'ADMIN',
             'A', 'Print Permit'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    END


-------- Print Permit Summary Report
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'PRINT_PERMIT_SUMMARY_REPORT_BUILDING')
    BEGIN
    INSERT
      INTO RBIZDOMAIN(SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE,
                      REC_FUL_NAM, REC_STATUS)
      VALUES (@SERV_PROV_CODE_, 'PRINT_PERMIT_SUMMARY_REPORT_BUILDING', 0,
              GETDATE(), 'ADMIN', 'A')
    END

IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'PRINT_PERMIT_SUMMARY_REPORT_BUILDING' AND
                      BIZDOMAIN_VALUE = 'REPORT_NAME')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_,
             'PRINT_PERMIT_SUMMARY_REPORT_BUILDING', 'REPORT_NAME', GETDATE(),
             'ADMIN', 'A', 'Print Permit Summary'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    END


-------- Print Receipt Report
IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'PRINT_PAYMENT_RECEIPT_REPORT')
    BEGIN
    INSERT
      INTO RBIZDOMAIN(SERV_PROV_CODE, BIZDOMAIN, VALUE_SIZE, REC_DATE,
                      REC_FUL_NAM, REC_STATUS)
      VALUES (@SERV_PROV_CODE_, 'PRINT_PAYMENT_RECEIPT_REPORT', 0, GETDATE(),
              'ADMIN', 'A')
    END

IF NOT EXISTS (SELECT 1
                 FROM RBIZDOMAIN_VALUE
                WHERE SERV_PROV_CODE = @SERV_PROV_CODE_ AND
                      BIZDOMAIN = 'PRINT_PAYMENT_RECEIPT_REPORT' AND
                      BIZDOMAIN_VALUE = 'Receipt Report')
    BEGIN
    INSERT
      INTO RBIZDOMAIN_VALUE(BDV_SEQ_NBR, SERV_PROV_CODE, BIZDOMAIN,
                            BIZDOMAIN_VALUE, REC_DATE, REC_FUL_NAM, REC_STATUS,
                            VALUE_DESC)
      SELECT MAX(BDV_SEQ_NBR) + 1, @SERV_PROV_CODE_,
             'PRINT_PAYMENT_RECEIPT_REPORT', 'Receipt Report', GETDATE(),
             'ADMIN', 'A', 'Receipt Report'
        FROM RBIZDOMAIN_VALUE
       WHERE SERV_PROV_CODE = @SERV_PROV_CODE_
    END

END
GO

EXEC DBO.PERMITS_CONFIG 'XXXXX'
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PERMITS_CONFIG]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
   drop procedure [dbo].[PERMITS_CONFIG]
END
GO

--reset sequence
UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  ISNULL(MAX(BDV_SEQ_NBR),0) + 1 FROM RBIZDOMAIN_VALUE) WHERE SEQUENCE_NAME='RBIZDOMAIN_VALUE_SEQ'

UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT  ISNULL(MAX(TEMPLATE_ID),0) + 1 FROM BCUSTOMIZED_CONTENT) WHERE SEQUENCE_NAME='BCUSTOMIZED_CONTENT_SEQ'
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'SPC_EXISTS_CHECK' AND XTYPE = 'P')
  DROP PROCEDURE dbo.SPC_EXISTS_CHECK
GO
