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
		SET @ERR_MSG = 'Please specify the agency code in EXEC ' + COALESCE(@P_PROCEDURE_NAME, '') + '(''' + UPPER(@P_SPC) + ''') of file 05.enhance_public_users_and_roles_for_registration_mssql.sql'
      RAISERROR(@ERR_MSG, 16, 1) with nowait 
      --exit when the agency not exists
      RAISERROR('interrupt', 20, 127, '1', '1') with log  
    END
END
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ACA_BIZ_TO_POLICY_CAPSEARCH' AND XTYPE = 'P')
  DROP PROCEDURE dbo.ACA_BIZ_TO_POLICY_CAPSEARCH
GO

CREATE PROCEDURE DBO.ACA_BIZ_TO_POLICY_CAPSEARCH @agency_code VARCHAR(100)  
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
    SELECT substring(B.BIZDOMAIN_VALUE,
                  1,
                  len(B.BIZDOMAIN_VALUE) - len('_CAP_FILTER_USERTYPE')) AS LEVEL_DATA,
			 USER_ROLE = 
			CASE b.VALUE_DESC 
             WHEN '1' THEN
              '1000000000'
             WHEN '2' THEN
              '0100000000'
             WHEN '3' THEN
              '0110000000'
           END 
      FROM RBIZDOMAIN_VALUE B, RBIZDOMAIN A
     WHERE A.SERV_PROV_CODE = @agency_code
       AND A.BIZDOMAIN = B.BIZDOMAIN
       AND A.REC_STATUS = 'A'
       AND UPPER(B.BIZDOMAIN) = 'ACA_CONFIGS'
       AND UPPER(B.BIZDOMAIN_VALUE) like '%_CAP_FILTER_USERTYPE'
       AND B.SERV_PROV_CODE = @agency_code
       AND NOT EXISTS (SELECT 1
                FROM XPOLICY
               WHERE SERV_PROV_CODE = B.SERV_PROV_CODE
                 AND POLICY_NAME = 'ACA_CAP_SEARCH_USER_ROLES'
                 AND UPPER(LEVEL_TYPE) = 'MODULE'
                 AND LEVEL_DATA =
                     substring(B.BIZDOMAIN_VALUE,
                                 1,
                                 len(B.BIZDOMAIN_VALUE) - len('_CAP_FILTER_USERTYPE'))
                 AND DATA1 = 'ACA_CAP_SEARCH_USER_ROLES'
                 AND RIGHT_GRANTED = 'ACA')

SELECT @v_policyseq = LAST_NUMBER from AA_SYS_SEQ where SEQUENCE_NAME ='XPOLICY_SEQ'

OPEN cur_biz_domain 
FETCH NEXT FROM  cur_biz_domain INTO @module_name, @user_role
WHILE (@@FETCH_STATUS <> -1)
        BEGIN
        
        SET @v_policyseq = @v_policyseq + 1
        
        INSERT INTO XPOLICY
         (SERV_PROV_CODE, POLICY_SEQ, POLICY_NAME, LEVEL_TYPE, LEVEL_DATA,
          DATA1, RIGHT_GRANTED, STATUS, DATA3, REC_STATUS, REC_DATE, REC_FUL_NAM,
          MENU_LEVEL)
         VALUES
         (@agency_code, @v_policyseq, 'ACA_CAP_SEARCH_USER_ROLES', 'MODULE',
          @module_name, 'ACA_CAP_SEARCH_USER_ROLES', 'ACA', 'Y',
          @user_role, 'A', GETDATE(), 'ADMIN', '0')

			FETCH NEXT FROM  cur_biz_domain INTO @module_name, @user_role
		END
   UPDATE AA_SYS_SEQ SET LAST_NUMBER = @v_policyseq WHERE SEQUENCE_NAME ='XPOLICY_SEQ'		
 CLOSE cur_biz_domain
 DEALLOCATE cur_biz_domain
END
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ACA_BIZ_TO_POLICY_INSPECTION_SCHEDULE' AND XTYPE = 'P')
  DROP PROCEDURE dbo.ACA_BIZ_TO_POLICY_INSPECTION_SCHEDULE
GO

CREATE PROCEDURE dbo.ACA_BIZ_TO_POLICY_INSPECTION_SCHEDULE @agency_code VARCHAR(100)  
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
    SELECT substring(B.BIZDOMAIN_VALUE,
                  1,
                  len(B.BIZDOMAIN_VALUE) - len('_INSPECTION_USERTYPE')) AS LEVEL_DATA,
			 USER_ROLE = 
			CASE b.VALUE_DESC 
             WHEN '1' THEN
              '1000000000'
             WHEN '2' THEN
              '0100000000'
             WHEN '3' THEN
              '0110000000'
           END 
      FROM RBIZDOMAIN_VALUE B, RBIZDOMAIN A
     WHERE A.SERV_PROV_CODE = @agency_code
       AND A.BIZDOMAIN = B.BIZDOMAIN
       AND A.REC_STATUS = 'A'
       AND UPPER(B.BIZDOMAIN) = 'ACA_CONFIGS'
       AND UPPER(B.BIZDOMAIN_VALUE) like '%_INSPECTION_USERTYPE'
       AND B.SERV_PROV_CODE = @agency_code
       AND NOT EXISTS (SELECT 1
                FROM XPOLICY
               WHERE SERV_PROV_CODE = B.SERV_PROV_CODE
                 AND POLICY_NAME = 'ACA_INSPECTION_USER_ROLES'
                 AND UPPER(LEVEL_TYPE) = 'MODULE'
                 AND LEVEL_DATA =
                     substring(B.BIZDOMAIN_VALUE,
                     1,
                     len(B.BIZDOMAIN_VALUE) - len('_INSPECTION_USERTYPE'))
                 AND DATA1 = 'ACA_INSPECTION_USER_ROLES'
                 AND RIGHT_GRANTED = 'ACA')

SELECT @v_policyseq = LAST_NUMBER from AA_SYS_SEQ where SEQUENCE_NAME ='XPOLICY_SEQ'

OPEN cur_biz_domain 
FETCH NEXT FROM  cur_biz_domain INTO @module_name, @user_role
WHILE (@@FETCH_STATUS <> -1)
        BEGIN
			
         SET @v_policyseq = @v_policyseq + 1
        
        INSERT INTO XPOLICY
         (SERV_PROV_CODE, POLICY_SEQ, POLICY_NAME, LEVEL_TYPE, LEVEL_DATA,
          DATA1, RIGHT_GRANTED, STATUS, DATA3, REC_STATUS, REC_DATE, REC_FUL_NAM,
          MENU_LEVEL)
         VALUES
         (@agency_code, @v_policyseq, 'ACA_INSPECTION_USER_ROLES', 'MODULE',
          @module_name, 'ACA_INSPECTION_USER_ROLES', 'ACA', 'Y',
          @user_role, 'A', GETDATE(), 'ADMIN', '0')


			FETCH NEXT FROM  cur_biz_domain INTO @module_name, @user_role
		END
   UPDATE AA_SYS_SEQ SET LAST_NUMBER = @v_policyseq WHERE SEQUENCE_NAME ='XPOLICY_SEQ'		
 CLOSE cur_biz_domain
 DEALLOCATE cur_biz_domain
END
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ACA_POLICY_INSPECTION_SCHEDULE' AND XTYPE = 'P')
  DROP PROCEDURE dbo.ACA_POLICY_INSPECTION_SCHEDULE
GO

CREATE PROCEDURE DBO.ACA_POLICY_INSPECTION_SCHEDULE @agency_code VARCHAR(100)  
AS  
BEGIN
	-- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @agency_code
DECLARE @module_name varchar(25),
		@v_policyseq BIGINT

DECLARE cur_module CURSOR LOCAL FOR 
	select constant_name from r1server_constant B
	where serv_prov_code=@agency_code And description = 'Module'
	AND NOT EXISTS (SELECT 1
                           FROM XPOLICY
                        WHERE SERV_PROV_CODE = B.SERV_PROV_CODE
                           AND POLICY_NAME = 'ACA_INSPECTION_USER_ROLES'
                           AND UPPER(LEVEL_TYPE) = 'MODULE'
                           AND LEVEL_DATA = B.CONSTANT_NAME
                           AND DATA1 = 'ACA_INSPECTION_USER_ROLES'
                           AND RIGHT_GRANTED = 'ACA')

--DELETE FROM XPOLICY WHERE POLICY_NAME ='ACA_INSPECTION_USER_ROLES' AND SERV_PROV_CODE=@agency_code

SELECT @v_policyseq = LAST_NUMBER from AA_SYS_SEQ where SEQUENCE_NAME ='XPOLICY_SEQ'

OPEN cur_module 
FETCH NEXT FROM  cur_module INTO @module_name 
WHILE (@@FETCH_STATUS <> -1)
        BEGIN
			SET @v_policyseq = @v_policyseq + 1

			INSERT INTO XPOLICY
			  (SERV_PROV_CODE,
			   POLICY_NAME, --ACA_CAP_SEARCH_USER_ROLES  ACA_INSPECTION_USER_ROLES
			   LEVEL_TYPE, --'MODULE'
			   LEVEL_DATA,
			   POLICY_SEQ,
			   DATA1, --'ACA_CAP_SEARCH_USER_ROLES'
			   DATA3,
			   RIGHT_GRANTED, --'ACA'
			   STATUS, --'Y'
			   REC_DATE, --GETDATE()
			   REC_FUL_NAM,
			   REC_STATUS,
			   MENU_LEVEL)
			VALUES
			  (@agency_code,
			   'ACA_INSPECTION_USER_ROLES',
			   'MODULE',
			   @module_name,
			   @v_policyseq,
			   'ACA_INSPECTION_USER_ROLES',
			   '1000000000',
			   'ACA',
			   'Y',
			   GETDATE(),
			   'ADMIN',
			   'A',
			   '0')

			FETCH NEXT FROM  cur_module INTO @module_name 
		END
 CLOSE cur_module
 DEALLOCATE cur_module

UPDATE AA_SYS_SEQ SET LAST_NUMBER = @v_policyseq WHERE SEQUENCE_NAME ='XPOLICY_SEQ'
END
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ACA_POLICY_CAP_SEARCH' AND XTYPE = 'P')
  DROP PROCEDURE dbo.ACA_POLICY_CAP_SEARCH
GO

CREATE PROCEDURE DBO.ACA_POLICY_CAP_SEARCH @agency_code VARCHAR(100)  
AS  
BEGIN
	-- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @agency_code
DECLARE @module_name varchar(25),
		@v_policyseq BIGINT

DECLARE cur_module CURSOR LOCAL FOR 
	select constant_name from r1server_constant B
	where serv_prov_code=@agency_code And description = 'Module'
	AND NOT EXISTS (SELECT 1
                           FROM XPOLICY
                        WHERE SERV_PROV_CODE = B.SERV_PROV_CODE
                           AND POLICY_NAME = 'ACA_CAP_SEARCH_USER_ROLES'
                           AND UPPER(LEVEL_TYPE) = 'MODULE'
                           AND LEVEL_DATA = B.CONSTANT_NAME
                           AND DATA1 = 'ACA_CAP_SEARCH_USER_ROLES'
                           AND RIGHT_GRANTED = 'ACA')

--DELETE FROM XPOLICY WHERE POLICY_NAME ='ACA_CAP_SEARCH_USER_ROLES' AND SERV_PROV_CODE=@agency_code

SELECT @v_policyseq = LAST_NUMBER from AA_SYS_SEQ where SEQUENCE_NAME ='XPOLICY_SEQ'

OPEN cur_module 
FETCH NEXT FROM  cur_module INTO @module_name 
WHILE (@@FETCH_STATUS <> -1)
        BEGIN
			SET @v_policyseq = @v_policyseq + 1

			INSERT INTO XPOLICY
			  (SERV_PROV_CODE,
			   POLICY_NAME, --ACA_CAP_SEARCH_USER_ROLES  ACA_INSPECTION_USER_ROLES
			   LEVEL_TYPE, --'MODULE'
			   LEVEL_DATA,
			   POLICY_SEQ,
			   DATA1, --'ACA_CAP_SEARCH_USER_ROLES'
			   DATA3,
			   RIGHT_GRANTED, --'ACA'
			   STATUS, --'Y'
			   REC_DATE, --GETDATE()
			   REC_FUL_NAM,
			   REC_STATUS,
			   MENU_LEVEL)
			VALUES
			  (@agency_code,
			   'ACA_CAP_SEARCH_USER_ROLES',
			   'MODULE',
			   @module_name,
			   @v_policyseq,
			   'ACA_CAP_SEARCH_USER_ROLES',
			   '1000000000',
			   'ACA',
			   'Y',
			   GETDATE(),
			   'ADMIN',
			   'A',
			   '0')

			FETCH NEXT FROM  cur_module INTO @module_name 
		END
 CLOSE cur_module
 DEALLOCATE cur_module

UPDATE AA_SYS_SEQ SET LAST_NUMBER = @v_policyseq WHERE SEQUENCE_NAME ='XPOLICY_SEQ'
END
GO

exec dbo.ACA_BIZ_TO_POLICY_CAPSEARCH 'XXXXX'
GO

exec dbo.ACA_BIZ_TO_POLICY_INSPECTION_SCHEDULE 'XXXXX'
GO

exec dbo.ACA_POLICY_INSPECTION_SCHEDULE 'XXXXX'
GO

exec dbo.ACA_POLICY_CAP_SEARCH 'XXXXX'
GO

DROP PROCEDURE DBO.ACA_BIZ_TO_POLICY_CAPSEARCH
GO

DROP PROCEDURE DBO.ACA_BIZ_TO_POLICY_INSPECTION_SCHEDULE
GO

DROP PROCEDURE DBO.ACA_POLICY_INSPECTION_SCHEDULE
GO

DROP PROCEDURE DBO.ACA_POLICY_CAP_SEARCH
GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'SPC_EXISTS_CHECK' AND XTYPE = 'P')
  DROP PROCEDURE dbo.SPC_EXISTS_CHECK
GO

