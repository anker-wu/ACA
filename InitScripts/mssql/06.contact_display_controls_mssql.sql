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
		SET @ERR_MSG = 'Please specify the agency code in EXEC ' + COALESCE(@P_PROCEDURE_NAME, '') + '(''' + UPPER(@P_SPC) + ''') of file 06.contact_display_controls_mssql.sql.sql'
      RAISERROR(@ERR_MSG, 16, 1) with nowait 
      --exit when the agency not exists
      RAISERROR('interrupt', 20, 127, '1', '1') with log  
    END
END
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ACA_BIZ_TO_POLICY_CONTACT_TYPE' AND XTYPE = 'P')
  DROP PROCEDURE dbo.ACA_BIZ_TO_POLICY_CONTACT_TYPE
GO

CREATE PROCEDURE DBO.ACA_BIZ_TO_POLICY_CONTACT_TYPE @agency_code VARCHAR(100)  
AS  
BEGIN
	-- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @agency_code
DECLARE @module_name varchar(25),
		@user_role varchar(2500),
		@v_policyseq BIGINT,
		@v_count BIGINT

DECLARE cur_biz_domain CURSOR LOCAL FOR 
    SELECT B.BIZDOMAIN_VALUE AS MODULE_NAME,
			 USER_ROLE = 
			CASE B.VALUE_DESC 
             WHEN '1' THEN
              '1000000000'
             WHEN '2' THEN
              '0100000000'
             WHEN '3' THEN
              '0110000000'
           END 
      FROM RBIZDOMAIN_VALUE B, RBIZDOMAIN A
     WHERE A.SERV_PROV_CODE = B.SERV_PROV_CODE
       AND A.BIZDOMAIN = B.BIZDOMAIN
       AND A.REC_STATUS = 'A'
       AND UPPER(B.BIZDOMAIN) = 'ACA_APPLICANT_DISPLAY_RULE'
       AND B.SERV_PROV_CODE = @agency_code
	   AND NOT EXISTS (SELECT 1 FROM XPOLICY 
			WHERE SERV_PROV_CODE = @agency_code
				AND POLICY_NAME = 'ACA_CONTACT_TYPE_USER_ROLES' 
				AND UPPER(LEVEL_TYPE)='MODULE'
				AND LEVEL_DATA = B.BIZDOMAIN_VALUE
				AND RIGHT_GRANTED = 'ACA'
				AND DATA1='Applicant')

SELECT @v_policyseq = LAST_NUMBER from AA_SYS_SEQ where SEQUENCE_NAME ='XPOLICY_SEQ'


OPEN cur_biz_domain 
FETCH NEXT FROM  cur_biz_domain INTO @module_name, @user_role
WHILE (@@FETCH_STATUS <> -1)
        BEGIN
        
			SET @v_policyseq = @v_policyseq + 1

			INSERT INTO XPOLICY
			  (SERV_PROV_CODE,
			   POLICY_NAME, --ACA_CONTACT_TYPE_USER_ROLES
			   LEVEL_TYPE, --'MODULE'
			   LEVEL_DATA, --Module Name
			   POLICY_SEQ,
			   DATA1, --Contact Typ
			   DATA3, --User Role Code
			   RIGHT_GRANTED, --'ACA'
			   STATUS, --'Y'
			   REC_DATE, --GETDATE()
			   REC_FUL_NAM,
			   REC_STATUS,
			   MENU_LEVEL)
				VALUES
			  (@agency_code,
			   'ACA_CONTACT_TYPE_USER_ROLES',
			   'MODULE',
			   @module_name,
			   @v_policyseq,
			   'Applicant',
			   @user_role,
			   'ACA',
			   'Y',
			   GETDATE(),
			   'ADMIN',
			   'A',
			   '0')
			FETCH NEXT FROM  cur_biz_domain INTO @module_name, @user_role
		END
		UPDATE AA_SYS_SEQ SET LAST_NUMBER = @v_policyseq WHERE SEQUENCE_NAME ='XPOLICY_SEQ'
 CLOSE cur_biz_domain
 DEALLOCATE cur_biz_domain
END
GO

exec dbo.ACA_BIZ_TO_POLICY_CONTACT_TYPE 'XXXXX'
GO

DROP PROCEDURE DBO.ACA_BIZ_TO_POLICY_CONTACT_TYPE
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ACA_BIZ_TO_POLICY_CONTACT_TYPE' AND XTYPE = 'P')
  DROP PROCEDURE dbo.ACA_BIZ_TO_POLICY_CONTACT_TYPE
GO

CREATE PROCEDURE DBO.ACA_BIZ_TO_POLICY_CONTACT_TYPE @agency_code VARCHAR(100)  
AS  
BEGIN
	-- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @agency_code
DECLARE @module_name varchar(25),
		@contact_type varchar(2500),
		@v_policyseq BIGINT,
		@v_count BIGINT

DECLARE cur_contact_type_module CURSOR LOCAL FOR 
	SELECT C.CONSTANT_NAME AS MODULE_NAME, B.BIZDOMAIN_VALUE AS CONTACT_TYPE
      FROM RBIZDOMAIN A, RBIZDOMAIN_VALUE B, R1SERVER_CONSTANT C
     WHERE A.SERV_PROV_CODE = B.SERV_PROV_CODE
       AND C.SERV_PROV_CODE = B.SERV_PROV_CODE
       AND A.BIZDOMAIN = B.BIZDOMAIN
       AND A.REC_STATUS = 'A'
       AND UPPER(B.BIZDOMAIN) = 'CONTACT TYPE'
       AND C.DESCRIPTION  = 'Module'
       AND C.REC_STATUS = 'A'
       AND B.SERV_PROV_CODE = @agency_code
	   AND NOT EXISTS (SELECT 1 FROM XPOLICY
	         WHERE SERV_PROV_CODE = @agency_code
	           AND POLICY_NAME = 'ACA_CONTACT_TYPE_USER_ROLES'
	           AND UPPER(LEVEL_TYPE) = 'MODULE'
	           AND LEVEL_DATA = C.CONSTANT_NAME
	           AND DATA1 = B.BIZDOMAIN_VALUE
	           AND RIGHT_GRANTED = 'ACA')
		
SELECT @v_policyseq = LAST_NUMBER from AA_SYS_SEQ where SEQUENCE_NAME ='XPOLICY_SEQ'

OPEN cur_contact_type_module 
FETCH NEXT FROM  cur_contact_type_module INTO @module_name, @contact_type
WHILE (@@FETCH_STATUS <> -1)
        BEGIN
			 
			SET @v_policyseq = @v_policyseq + 1

			INSERT INTO XPOLICY
			  (SERV_PROV_CODE,
			   POLICY_NAME, --ACA_CONTACT_TYPE_USER_ROLES
			   LEVEL_TYPE, --'MODULE'
			   LEVEL_DATA, --Module Name
			   POLICY_SEQ,
			   DATA1, --Contact Typ
			   DATA3, --User Role Code
			   RIGHT_GRANTED, --'ACA'
			   STATUS, --'Y'
			   REC_DATE, --GETDATE()
			   REC_FUL_NAM,
			   REC_STATUS,
			   MENU_LEVEL)
				VALUES
			  (@agency_code,
			   'ACA_CONTACT_TYPE_USER_ROLES',
			   'MODULE',
			   @module_name,
			   @v_policyseq,
			   @contact_type,
			   '1000000000',
			   'ACA',
			   'Y',
			   GETDATE(),
			   'ADMIN',
			   'A',
			   '0')
			FETCH NEXT FROM  cur_contact_type_module INTO @module_name, @contact_type
		END
        UPDATE AA_SYS_SEQ SET LAST_NUMBER = @v_policyseq WHERE SEQUENCE_NAME ='XPOLICY_SEQ'
 CLOSE cur_contact_type_module
 DEALLOCATE cur_contact_type_module
END
GO

exec dbo.ACA_BIZ_TO_POLICY_CONTACT_TYPE 'XXXXX'
GO

DROP PROCEDURE DBO.ACA_BIZ_TO_POLICY_CONTACT_TYPE
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'SPC_EXISTS_CHECK' AND XTYPE = 'P')
  DROP PROCEDURE dbo.SPC_EXISTS_CHECK
GO

