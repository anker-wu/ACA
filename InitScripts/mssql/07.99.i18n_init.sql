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

-- Format LANG_ID like xx_XX
If Exists ( SELECT name 
            FROM sysobjects  
            WHERE name = 'GET_LANG_ID'
            AND type = 'FN')
	DROP FUNCTION dbo.GET_LANG_ID
GO 

CREATE FUNCTION dbo.GET_LANG_ID(@P_LANG_ID VARCHAR(10)) 
RETURNS VARCHAR(10)
AS
BEGIN
   DECLARE @V_RESULT VARCHAR(10)
   DECLARE @V_LANG_ID VARCHAR(10)
   SET @V_LANG_ID = RTRIM(LTRIM(@P_LANG_ID))
   DECLARE @V_LEN SMALLINT
   SET @V_LEN = LEN(@V_LANG_ID)
   --
   IF (@V_LEN = 2)
   BEGIN
      SET @V_RESULT = LOWER(@V_LANG_ID)
   END
   ELSE IF (@V_LEN = 5 AND CHARINDEX('_', @V_LANG_ID, 1) = 3)
   BEGIN
      SET @V_RESULT = LOWER(LEFT(@P_LANG_ID, 2)) + '_' + UPPER(RIGHT(@P_LANG_ID, 2))
   END 
   ELSE
   BEGIN
      SET @V_RESULT = @V_LANG_ID
   END
   RETURN (@V_RESULT)
END
GO

If Exists ( SELECT name 
            FROM sysobjects  
            WHERE name = 'PD_INITIALIZE_I18N_TAB'
            AND type = 'P')
	DROP PROCEDURE dbo.PD_INITIALIZE_I18N_TAB
GO 

CREATE PROCEDURE dbo.PD_INITIALIZE_I18N_TAB
	@MAIN_TAB_NAME  VARCHAR(4000) ,
	@I18N_TAB_NAME  VARCHAR(4000) ,
	@SEQ_NAME       VARCHAR(4000) ,
	@P_SPC          VARCHAR(30),                             
	@LANG_CODE      VARCHAR(4000) ,
	@P_OVERWRITE    VARCHAR(16)                              
AS 
	BEGIN
      -- Initialize interface variables
		DECLARE @MAIN_TAB_NAME_                           VARCHAR(100)                    		
		DECLARE @I18N_TAB_NAME_                           VARCHAR(100)                    		
		DECLARE @SEQ_NAME_                                VARCHAR(100)                    		
		DECLARE @LANG_CODE_                               VARCHAR(10)                     		
		SET @MAIN_TAB_NAME_ = UPPER(COALESCE(@MAIN_TAB_NAME, ''))
		SET @I18N_TAB_NAME_ = UPPER(COALESCE(@I18N_TAB_NAME, ''))
		SET @SEQ_NAME_ = UPPER(COALESCE(@SEQ_NAME, ''))
		SET @LANG_CODE_ = COALESCE(@LANG_CODE, '')
		-- Initialize variables		
		DECLARE @SQL_ADD_TMP_RES_ID_                      VARCHAR(4000)
		DECLARE @SQL_GEN_RES_ID_                          VARCHAR(4000)
		DECLARE @SQL_DROP_TMP_RES_ID_                     VARCHAR(4000)
		DECLARE @SQL_CLS_I18N_TAB_                        VARCHAR(4000) 
		DECLARE @SQL_INS_I18N_                            VARCHAR(4000) 
		DECLARE @SQL_UPD_SEQ_                             VARCHAR(4000) 
		DECLARE @I18N_TAB_INS_COLUMNS_                    VARCHAR(4000) 
		DECLARE @MAIN_TAB_SEL_COLUMNS_                    VARCHAR(4000) 
		DECLARE @MAIN_TAB_SEL_VALUE_                      VARCHAR(64)		
		SET @SQL_ADD_TMP_RES_ID_         = ''
		SET @SQL_GEN_RES_ID_             = ''
		SET @SQL_DROP_TMP_RES_ID_        = ''
		SET @SQL_CLS_I18N_TAB_           = ''
		SET @SQL_INS_I18N_               = ''
		SET @SQL_UPD_SEQ_                = ''
		SET @I18N_TAB_INS_COLUMNS_       = ''
		SET @MAIN_TAB_SEL_COLUMNS_       = ''
		SET @MAIN_TAB_SEL_VALUE_         = ''
		-- the other variables
		DECLARE @MAX_RES_ID_ BIGINT
		DECLARE @MAX_RES_ID__ VARCHAR(24)
		DECLARE @RES_ID_ BIGINT
		--
		SET @MAX_RES_ID_ = 1
      SET @MAX_RES_ID__ = '1'
      SET @RES_ID_ = 1
		DECLARE @L_INS_WHERE           VARCHAR(4000)
		-- Initialize variables	for cursor
		DECLARE @COLUMN_NAME_ VARCHAR(100)
		SET @COLUMN_NAME_ = ''
		-- Declare cursor for get tab columns' name
		DECLARE GET_COLUMNS CURSOR LOCAL FOR 
		SELECT C.NAME COLUMN_NAME
		FROM SYSOBJECTS O,
           SYSCOLUMNS C
      WHERE O.ID = C.ID
      AND O.XTYPE = 'U'
      AND O.NAME = UPPER (@I18N_TAB_NAME_)	
      --Genarate RES_ID
      SET @SQL_GEN_RES_ID_ = 'DECLARE INIT_RES_ID CURSOR FOR SELECT RES_ID FROM ' + @MAIN_TAB_NAME_ +  ' WHERE RES_ID IS NULL'
      SELECT @MAX_RES_ID_ = LAST_NUMBER FROM AA_SYS_SEQ WHERE SEQUENCE_NAME=@SEQ_NAME_
      SET @SQL_CLS_I18N_TAB_  = 'DELETE FROM ' + @I18N_TAB_NAME_ + ' WHERE LANG_ID = ' + '''' + @LANG_CODE_ + ''''
		--Genarate where... for insert ... select ...
      IF(UPPER(@P_OVERWRITE) != 'YES') 
      BEGIN
         SET @L_INS_WHERE = ' WHERE RES_ID > ' + CAST(@MAX_RES_ID_ AS VARCHAR)
      END
      ELSE
      BEGIN
         SET @L_INS_WHERE = ' WHERE RES_ID IS NOT NULL'
      END
		--Generate Insert SQL FOR I18N_TAB
		OPEN GET_COLUMNS 		
		FETCH NEXT FROM  GET_COLUMNS INTO @COLUMN_NAME_      
		WHILE (@@FETCH_STATUS <> -1)
		BEGIN
		   --INSERT INTO I18N_TAB (...)
			IF @I18N_TAB_INS_COLUMNS_ <> '' 
			BEGIN 
				SET @I18N_TAB_INS_COLUMNS_  = @I18N_TAB_INS_COLUMNS_ + ',' + @COLUMN_NAME_ 
			END
			ELSE
			BEGIN 
				SET @I18N_TAB_INS_COLUMNS_  = @COLUMN_NAME_ 
			END
         --Processes the special fields(like: SERV_PROV_CODE, LANG_ID, REC_DATE, REC_FUL_NAM, REC_STATUS)
			IF @COLUMN_NAME_ = 'SERV_PROV_CODE' 
			BEGIN 
				SET @MAIN_TAB_SEL_VALUE_  = 'SERV_PROV_CODE'
				SET @SQL_GEN_RES_ID_ =  @SQL_GEN_RES_ID_ + ' AND SERV_PROV_CODE = ''' + @P_SPC + ''''
				-- for table R2CHCKBOX start: only initialize R1_CHECKBOX_GROUP IN ('APPLICATION','WORKFLOW TASK','FEEATTACHEDTABLE')
            IF(@MAIN_TAB_NAME_ = 'R2CHCKBOX')
            BEGIN
               SET @SQL_GEN_RES_ID_ = @SQL_GEN_RES_ID_ + ' AND R1_CHECKBOX_GROUP IN (''APPLICATION'',''WORKFLOW TASK'',''FEEATTACHEDTABLE'')'     
            END
            -- for table R2CHCKBOX end
            SET @SQL_CLS_I18N_TAB_ = @SQL_CLS_I18N_TAB_ + ' AND SERV_PROV_CODE = ''' + @P_SPC + ''''
            SET @L_INS_WHERE = @L_INS_WHERE + ' AND SERV_PROV_CODE = ''' + @P_SPC + ''''
			END
			ELSE IF @COLUMN_NAME_ = 'LANG_ID'
			BEGIN 
				SET @MAIN_TAB_SEL_VALUE_  = '''' + @LANG_CODE_  +'''' 
			END
			ELSE IF @COLUMN_NAME_ = 'REC_DATE'
			BEGIN 
				SET @MAIN_TAB_SEL_VALUE_  = 'GETDATE()' 
			END
			ELSE IF @COLUMN_NAME_ = 'REC_FUL_NAM'
			BEGIN 
				SET @MAIN_TAB_SEL_VALUE_  = '''SYSTEM''' 
			END
			ELSE IF @COLUMN_NAME_ = 'REC_STATUS'
			BEGIN 
				SET @MAIN_TAB_SEL_VALUE_  = '''A''' 
			END
			ELSE
			BEGIN
			   SET @MAIN_TAB_SEL_VALUE_ = @COLUMN_NAME_
			END
			--SELECT ... FROM MAIN_TAB			 
			IF @MAIN_TAB_SEL_COLUMNS_ <> '' 
			BEGIN 
				SET @MAIN_TAB_SEL_COLUMNS_  = @MAIN_TAB_SEL_COLUMNS_ + ',' + @MAIN_TAB_SEL_VALUE_ 
			END
			ELSE
			BEGIN 
				SET @MAIN_TAB_SEL_COLUMNS_  = @MAIN_TAB_SEL_VALUE_ 
			END   			
		   FETCH NEXT FROM  GET_COLUMNS INTO @COLUMN_NAME_
		END
		CLOSE GET_COLUMNS
		DEALLOCATE GET_COLUMNS		
		--SQL Start: INSERT INTO ... SELECT ... FORM ...		
		SET @SQL_INS_I18N_  = 'INSERT INTO ' + @I18N_TAB_NAME_ + ' (' + @I18N_TAB_INS_COLUMNS_ + ')' 
		SET @SQL_INS_I18N_  = @SQL_INS_I18N_ + CHAR(10)+ ' SELECT ' + @MAIN_TAB_SEL_COLUMNS_ + ' FROM ' + @MAIN_TAB_NAME_
		SET @SQL_INS_I18N_  = @SQL_INS_I18N_ + CHAR(10) + @L_INS_WHERE
		--SQL End: INSERT INTO ... SELECT ... FORM ...
		SET @SQL_UPD_SEQ_  = 'UPDATE AA_SYS_SEQ SET LAST_NUMBER = (SELECT CASE WHEN COUNT(*) > COALESCE(MAX(RES_ID),1) THEN COUNT(*) ELSE COALESCE(MAX(RES_ID),1) END FROM ' + @MAIN_TAB_NAME_ + ') WHERE SEQUENCE_NAME =''' + @SEQ_NAME_ + '''' 		
		--execute generate resource id
		SET @SQL_GEN_RES_ID_ = @SQL_GEN_RES_ID_ + ' FOR UPDATE'		
		EXECUTE (@SQL_GEN_RES_ID_)
		PRINT @SQL_GEN_RES_ID_
		OPEN INIT_RES_ID
		FETCH NEXT FROM INIT_RES_ID INTO @RES_ID_
		WHILE @@FETCH_STATUS = 0
		BEGIN
		 SET @MAX_RES_ID_ = @MAX_RES_ID_ + 1
         SET @MAX_RES_ID__ = CAST(@MAX_RES_ID_ AS VARCHAR)
         EXECUTE ('UPDATE ' + @MAIN_TAB_NAME_ + ' SET RES_ID = ' + @MAX_RES_ID__ + ' WHERE CURRENT OF INIT_RES_ID ') 
         FETCH NEXT FROM INIT_RES_ID INTO @RES_ID_
		END
		CLOSE INIT_RES_ID
		DEALLOCATE INIT_RES_ID
		--execute clean main language resource table
		IF (UPPER(@P_OVERWRITE) = 'YES') 
		BEGIN
		   PRINT @SQL_CLS_I18N_TAB_
		   EXECUTE (@SQL_CLS_I18N_TAB_)
		END 
		--execute resource table data
		PRINT @SQL_INS_I18N_
		EXECUTE (@SQL_INS_I18N_)
		--reset sequence
		PRINT @SQL_UPD_SEQ_
		EXECUTE (@SQL_UPD_SEQ_)
	END
GO

--------------------------------------------------------
---- I18N initial migration for special tables (begin)
--------------------------------------------------------

IF EXISTS (SELECT NAME
             FROM SYSOBJECTS
            WHERE NAME = 'I18N_MIG_BACTIVITY_COMMENT'
                  AND TYPE = 'P')
   DROP PROCEDURE DBO.I18N_MIG_BACTIVITY_COMMENT
GO

CREATE PROCEDURE DBO.I18N_MIG_BACTIVITY_COMMENT
   @P_SERV_PROV_CODE VARCHAR (30),
   @P_LANG_ID VARCHAR (10),
   @P_OVERWRITE VARCHAR (16)
AS
BEGIN
   PRINT ('BACTIVITY_COMMENT begin.')
   SET  NOCOUNT ON
   DECLARE @MAX_RES_ID_ BIGINT
   DECLARE @SQL_CREATE_TEMP_TAB VARCHAR (4000)
   --
   SELECT @MAX_RES_ID_ = LAST_NUMBER + 1
   FROM AA_SYS_SEQ
   WHERE SEQUENCE_NAME = 'BACTIVITY_COMMENT_RES_SEQ'
   -- Drop and create temporary table
   IF EXISTS ( SELECT name
         FROM sysobjects
         WHERE name = 'TEMP_BACTIVITY_COMMENT'
         AND type = 'U')
   BEGIN
      DROP TABLE DBO.TEMP_BACTIVITY_COMMENT
   END
   --
   SET @SQL_CREATE_TEMP_TAB =
            'SELECT SERV_PROV_CODE,
            B1_PER_ID1,
            B1_PER_ID2,
            B1_PER_ID3,
            G6_ACT_NUM,
            COMMENT_TYPE,
            IDENTITY (BIGINT, '
            + CAST (@MAX_RES_ID_ AS VARCHAR)
            + ', 1) RES_ID
      INTO DBO.TEMP_BACTIVITY_COMMENT
      FROM BACTIVITY_COMMENT
      WHERE SERV_PROV_CODE = '''
            + @P_SERV_PROV_CODE
            + '''
            AND RES_ID IS NULL'
   --
   EXECUTE (@SQL_CREATE_TEMP_TAB)
   -- Create index
   CREATE INDEX TEMP_BACTIVITY_COMMENT_IX
      ON TEMP_BACTIVITY_COMMENT
         (SERV_PROV_CODE,
            B1_PER_ID1,
            B1_PER_ID2,
            B1_PER_ID3,
            G6_ACT_NUM,
            COMMENT_TYPE)
   -- Set RES_ID
   UPDATE BACTIVITY_COMMENT
      SET RES_ID = A.RES_ID
      FROM TEMP_BACTIVITY_COMMENT A
      WHERE A.SERV_PROV_CODE = BACTIVITY_COMMENT.SERV_PROV_CODE
            AND A.B1_PER_ID1 = BACTIVITY_COMMENT.B1_PER_ID1
            AND A.B1_PER_ID2 = BACTIVITY_COMMENT.B1_PER_ID2
            AND A.B1_PER_ID3 = BACTIVITY_COMMENT.B1_PER_ID3
            AND A.G6_ACT_NUM = BACTIVITY_COMMENT.G6_ACT_NUM
            AND A.COMMENT_TYPE = BACTIVITY_COMMENT.COMMENT_TYPE
            AND BACTIVITY_COMMENT.SERV_PROV_CODE = @P_SERV_PROV_CODE
            AND BACTIVITY_COMMENT.RES_ID IS NULL
   -- if overwrite
   IF (UPPER (@P_OVERWRITE) = 'YES')
   BEGIN
      DELETE FROM BACTIVITY_COMMENT_I18N
         WHERE SERV_PROV_CODE = @P_SERV_PROV_CODE
               AND LANG_ID = @P_LANG_ID
      --
      SET @MAX_RES_ID_ = 1
   END
   -- insert data for default language
   INSERT   INTO BACTIVITY_COMMENT_I18N (SERV_PROV_CODE, RES_ID, LANG_ID,
                                          TEXT, REC_DATE, REC_FUL_NAM,
                                          REC_STATUS, CLOBTEXT)
      SELECT T.SERV_PROV_CODE,
               T.RES_ID,
               @P_LANG_ID,
               T.TEXT,
               GETDATE (),
               'SYSTEM',
               'A',
               T.CLOBTEXT
         FROM BACTIVITY_COMMENT T
         WHERE T.SERV_PROV_CODE = @P_SERV_PROV_CODE
               AND T.RES_ID >= @MAX_RES_ID_
   -- Reset sequence
   UPDATE AA_SYS_SEQ
      SET LAST_NUMBER =
               (SELECT CASE
                        WHEN COUNT ( * ) > COALESCE (MAX (RES_ID), 1) THEN
                           COUNT ( * )
                        ELSE
                           COALESCE (MAX (RES_ID), 1)
                     END
                  FROM BACTIVITY_COMMENT)
      WHERE SEQUENCE_NAME = 'BACTIVITY_COMMENT_RES_SEQ'
   -- Drop temporary table
   DROP TABLE DBO.TEMP_BACTIVITY_COMMENT
   SET  NOCOUNT OFF
   PRINT ('BACTIVITY_COMMENT end.')
END
GO

IF EXISTS (SELECT NAME
             FROM SYSOBJECTS
            WHERE NAME = 'I18N_MIG_GPROCESS'
                  AND TYPE = 'P')
   DROP PROCEDURE DBO.I18N_MIG_GPROCESS
GO

CREATE PROCEDURE DBO.I18N_MIG_GPROCESS
   @P_SERV_PROV_CODE VARCHAR (30),
   @P_LANG_ID VARCHAR (10),
   @P_OVERWRITE VARCHAR (16)
AS
BEGIN
   PRINT ('GPROCESS begin.')
   SET  NOCOUNT ON
   DECLARE @MAX_RES_ID_ BIGINT
   DECLARE @SQL_CREATE_TEMP_TAB VARCHAR (4000)
   --
   SELECT @MAX_RES_ID_ = LAST_NUMBER + 1
   FROM AA_SYS_SEQ
   WHERE SEQUENCE_NAME = 'GPROCESS_RES_SEQ'
   -- Drop and create temporary table
   IF EXISTS ( SELECT name
         FROM sysobjects
         WHERE name = 'TEMP_GPROCESS'
         AND type = 'U')
   BEGIN
      DROP TABLE DBO.TEMP_GPROCESS
   END
   --
   SET @SQL_CREATE_TEMP_TAB =
            'SELECT SERV_PROV_CODE,
            B1_PER_ID1,
            B1_PER_ID2,
            B1_PER_ID3,
            SD_STP_NUM,
            RELATION_SEQ_ID,
            IDENTITY (BIGINT, '
            + CAST (@MAX_RES_ID_ AS VARCHAR)
            + ', 1) RES_ID
      INTO DBO.TEMP_GPROCESS
      FROM GPROCESS
      WHERE SERV_PROV_CODE = '''
            + @P_SERV_PROV_CODE
            + '''
            AND RES_ID IS NULL'
   --
   EXECUTE (@SQL_CREATE_TEMP_TAB)
   -- Create index
   CREATE INDEX TEMP_GPROCESS_IX
      ON TEMP_GPROCESS
         (SERV_PROV_CODE,
            B1_PER_ID1,
            B1_PER_ID2,
            B1_PER_ID3,
            SD_STP_NUM,
            RELATION_SEQ_ID)
   -- Set RES_ID
   UPDATE GPROCESS
      SET RES_ID = A.RES_ID
      FROM TEMP_GPROCESS A
      WHERE A.SERV_PROV_CODE = GPROCESS.SERV_PROV_CODE
            AND A.B1_PER_ID1 = GPROCESS.B1_PER_ID1
            AND A.B1_PER_ID2 = GPROCESS.B1_PER_ID2
            AND A.B1_PER_ID3 = GPROCESS.B1_PER_ID3
            AND A.SD_STP_NUM = GPROCESS.SD_STP_NUM
            AND A.RELATION_SEQ_ID = GPROCESS.RELATION_SEQ_ID
            AND GPROCESS.SERV_PROV_CODE = @P_SERV_PROV_CODE
            AND GPROCESS.RES_ID IS NULL
   -- if overwrite
   IF (UPPER (@P_OVERWRITE) = 'YES')
   BEGIN
      DELETE FROM GPROCESS_I18N
         WHERE SERV_PROV_CODE = @P_SERV_PROV_CODE
               AND LANG_ID = @P_LANG_ID
      --
      SET @MAX_RES_ID_ = 1
   END
   -- insert data for default language
   INSERT   INTO GPROCESS_I18N (SERV_PROV_CODE, RES_ID, LANG_ID, SD_COMMENT
                                 , REC_DATE, REC_FUL_NAM, REC_STATUS,
                                 SD_PRO_DES, SD_APP_DES)
      SELECT T.SERV_PROV_CODE,
               T.RES_ID,
               @P_LANG_ID,
               T.SD_COMMENT,
               GETDATE (),
               'SYSTEM',
               'A',
               T.SD_PRO_DES,
               T.SD_APP_DES
         FROM GPROCESS T
         WHERE T.SERV_PROV_CODE = @P_SERV_PROV_CODE
               AND T.RES_ID >= @MAX_RES_ID_
   -- Reset sequence
   UPDATE AA_SYS_SEQ
      SET LAST_NUMBER =
               (SELECT CASE
                        WHEN COUNT ( * ) > COALESCE (MAX (RES_ID), 1) THEN
                           COUNT ( * )
                        ELSE
                           COALESCE (MAX (RES_ID), 1)
                     END
                  FROM GPROCESS)
      WHERE SEQUENCE_NAME = 'GPROCESS_RES_SEQ'
   -- Drop temporary table
   DROP TABLE DBO.TEMP_GPROCESS
   SET  NOCOUNT OFF
   PRINT ('GPROCESS end.')
END
GO



--------------------------------------------------------
---- I18N initial migration for special tables (end)
--------------------------------------------------------


--EXEC PD_INITIALIZE_I18N_TAB('G3STAFFS', 'G3STAFFS_I18N', 'G3STAFFS_RES_SEQ', 'QA', 'en', 'YES')
--DELETE FROM G3STAFFS_I18N WHERE SERV_PROV_CODE = 'QA'
--UPDATE G3STAFFS SET RES_ID = NULL WHERE SERV_PROV_CODE = 'QA'

If Exists ( SELECT name 
            FROM sysobjects  
            WHERE name = 'I18N_MIG'
            AND type = 'P')
	DROP PROCEDURE dbo.I18N_MIG
GO 

CREATE PROCEDURE dbo.I18N_MIG
	@P_OVERWRITE                              VARCHAR(4000),
	@P_MAIN_TABLE                             VARCHAR(128),
	@P_SPC                                     VARCHAR(32)
AS 	
	BEGIN
	   SET NOCOUNT ON
	   -- Declare variables
		DECLARE @L_CNT                                    BIGINT                           
		SET @L_CNT = 0 
		DECLARE @L_CNT2                                   BIGINT                           
		SET @L_CNT2 = 0 
		DECLARE @L_LANG                                   VARCHAR(10)                      
		SET @L_LANG = 'en_US' 
		DECLARE @L_MAIN_TAB                               VARCHAR(4000)                   
		SET @L_MAIN_TAB = '' 
		DECLARE @L_RES_NAM                                VARCHAR(4000)                   
		SET @L_RES_NAM = '' 
		DECLARE @P_MAIN_TABLE_                            VARCHAR(128)
		SET @P_MAIN_TABLE_ = UPPER(SUBSTRING(@P_MAIN_TABLE, 1, 25))
		DECLARE @P_SPC_                                   VARCHAR(32)
		SET @P_SPC_ = UPPER(@P_SPC)
      -- Declare cursor for get all I18N table name
		DECLARE CUR_I18N CURSOR LOCAL FOR 
		SELECT NAME TABLE_NAME
		FROM SYSOBJECTS 
      WHERE XTYPE='U'
      AND NAME LIKE (@P_MAIN_TABLE_ + '_I18N')
		-- Declare variable for cursor
      DECLARE @I18N_TAB_NAME VARCHAR(30)
      SET @I18N_TAB_NAME = ''
      -- Declare cursor for get agencies
		DECLARE CUR_SPC CURSOR LOCAL FOR 
		SELECT SERV_PROV_CODE
		FROM  RSERV_PROV 
		WHERE	 SERV_PROV_CODE  <> 'STANDARDDATA'
		AND    SERV_PROV_CODE  LIKE @P_SPC_
		-- Declare variable for cursor
		DECLARE @SERV_PROV_CODE_ VARCHAR(15)
		SET @SERV_PROV_CODE_ = ''
		-- do initialize		
		OPEN CUR_SPC 		
		FETCH NEXT FROM  CUR_SPC INTO @SERV_PROV_CODE_
		WHILE (@@FETCH_STATUS <> -1)
		BEGIN
			--check MULTI-LANGUAGE SUPPORT ENABLE switch
			SELECT @L_CNT  =  COUNT(*)
			FROM  RBIZDOMAIN_VALUE 
			WHERE	 SERV_PROV_CODE  = @SERV_PROV_CODE_
			 AND	BIZDOMAIN  = 'I18N_SETTINGS'
			 AND	BIZDOMAIN_VALUE  = 'MULTI-LANGUAGE SUPPORT ENABLE'
			 AND	UPPER(VALUE_DESC)  = 'YES'
			--check main language
			SELECT @L_CNT2  =  COUNT(*)
			FROM  RBIZDOMAIN_VALUE 
			WHERE	 SERV_PROV_CODE  = @SERV_PROV_CODE_
			 AND	BIZDOMAIN  = 'I18N_SETTINGS'
			 AND	BIZDOMAIN_VALUE  = 'I18N_DEFAULT_LANGUAGE'
			--get default main language
			IF ( @L_CNT2 <= 0 ) 
			BEGIN 
				SET @L_LANG  = 'en_US' 
			END
			ELSE
			BEGIN 
				SELECT @L_LANG  =  VALUE_DESC
				FROM  RBIZDOMAIN_VALUE 
				WHERE	 SERV_PROV_CODE  = @SERV_PROV_CODE_
				 AND	BIZDOMAIN  = 'I18N_SETTINGS'
				 AND	BIZDOMAIN_VALUE  = 'I18N_DEFAULT_LANGUAGE'
			END
			IF ( @L_CNT > 0 ) 
			BEGIN 
				OPEN CUR_I18N 
				FETCH NEXT FROM  CUR_I18N INTO @I18N_TAB_NAME
				WHILE (@@FETCH_STATUS <> -1)
				BEGIN
					IF ( @I18N_TAB_NAME = 'REXPRESSION_LOOKUP_CRITER_I18N' ) 
					BEGIN 
						SET @L_MAIN_TAB  = 'REXPRESSION_LOOKUP_CRITERIA' 
						SET @L_RES_NAM  = @L_MAIN_TAB  + '_RES_SEQ'  
					END
					ELSE IF ( @I18N_TAB_NAME = 'RRATING_CONDITION_CRITERI_I18N' )
					BEGIN 
						SET @L_MAIN_TAB  = 'RRATING_CONDITION_CRITERIA'
						SET @L_RES_NAM  = @L_MAIN_TAB  + '_RES_SEQ' 
					END
					ELSE IF ( @I18N_TAB_NAME = 'RRATING_EXPRESSION_CRITER_I18N' )
					BEGIN 
						SET @L_MAIN_TAB  = 'RRATING_EXPRESSION_CRITERIA' 
						SET @L_RES_NAM  = @L_MAIN_TAB  + '_RES_SEQ'  
					END
					ELSE IF ( @I18N_TAB_NAME = 'REXPRESSION_FUNCTION_CRIT_I18N' )
					BEGIN 
						SET @L_MAIN_TAB  = 'REXPRESSION_FUNCTION_CRITERIA' 
						SET @L_RES_NAM  = @L_MAIN_TAB  + '_RES_SEQ'  
					END
					ELSE
					BEGIN 
						SET @L_MAIN_TAB  = SUBSTRING(@I18N_TAB_NAME, 1, LEN(@I18N_TAB_NAME) - 4 - 1)
						SET @L_RES_NAM  = @L_MAIN_TAB  + '_RES_SEQ' 
					END
					SELECT @L_CNT2  =  COUNT(*)
					FROM  AA_SYS_SEQ 
					WHERE	 SEQUENCE_NAME  = @L_RES_NAM
					IF ( @L_CNT2 > 0 ) 
					BEGIN 
					   IF (@L_MAIN_TAB = 'BACTIVITY_COMMENT')
					   BEGIN
					      EXEC DBO.I18N_MIG_BACTIVITY_COMMENT @SERV_PROV_CODE_, @L_LANG, @P_OVERWRITE
					   END 
					   ELSE IF (@L_MAIN_TAB = 'GPROCESS')
					   BEGIN
					      EXEC DBO.I18N_MIG_GPROCESS @SERV_PROV_CODE_, @L_LANG, @P_OVERWRITE
					   END
					   ELSE
					   BEGIN
						   EXEC DBO.PD_INITIALIZE_I18N_TAB @L_MAIN_TAB  ,  @I18N_TAB_NAME  ,  @L_RES_NAM  ,  @SERV_PROV_CODE_ ,  @L_LANG  ,  @P_OVERWRITE 
						END
					END
					-- dbms_output.put_line (L_MAIN_TAB || ' ' || @I18N_TAB_NAME || ' ' || L_MAIN_TAB||'_RES_SEQ' || ' ' || @SERV_PROV_CODE_ || ' ' || L_LANG)
					PRINT ((ISNULL(CAST(@L_MAIN_TAB   AS VARCHAR), '' )  +  '_RES_SEQ'))
				   FETCH NEXT FROM  CUR_I18N INTO @I18N_TAB_NAME
				END
				CLOSE CUR_I18N
				--DEALLOCATE CUR_I18N
			END
		FETCH NEXT FROM  CUR_SPC INTO @SERV_PROV_CODE_
		END
		CLOSE CUR_SPC
		DEALLOCATE CUR_SPC
		DEALLOCATE CUR_I18N
		SET NOCOUNT OFF
	END
GO

If Exists ( SELECT name 
            FROM sysobjects  
            WHERE name = 'ACA_I18N_INIT'
            AND type = 'P')
	DROP PROCEDURE dbo.ACA_I18N_INIT
GO 

CREATE PROCEDURE dbo.ACA_I18N_INIT @P_SPC VARCHAR(15)
AS 
BEGIN
   SET NOCOUNT ON
   --
   DECLARE @V_SERVPROVCODE    VARCHAR(15)
   DECLARE @V_CNT_DFLT_LANG   BIGINT
   DECLARE @V_CNT_I18N_ENABLE BIGINT
   --
   SET @V_SERVPROVCODE = UPPER(@P_SPC)
   -- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @V_SERVPROVCODE
   --
   SELECT @V_CNT_DFLT_LANG = COUNT(*)
     FROM RBIZDOMAIN_VALUE
    WHERE SERV_PROV_CODE = @V_SERVPROVCODE
      AND BIZDOMAIN = 'I18N_SETTINGS'
      AND BIZDOMAIN_VALUE = 'I18N_DEFAULT_LANGUAGE'
      AND REC_STATUS = 'A'
   --
   SELECT @V_CNT_I18N_ENABLE = COUNT(*)
     FROM RBIZDOMAIN_VALUE
    WHERE SERV_PROV_CODE = @V_SERVPROVCODE
      AND BIZDOMAIN = 'I18N_SETTINGS'
      AND BIZDOMAIN_VALUE = 'MULTI-LANGUAGE SUPPORT ENABLE'
      AND UPPER(VALUE_DESC) = 'YES'
      AND REC_STATUS = 'A'
   --
   IF (@V_CNT_DFLT_LANG > 0 AND @V_CNT_I18N_ENABLE > 0)
   BEGIN
      EXEC DBO.I18N_MIG 'NO', 'G3STAFFS', @V_SERVPROVCODE
      EXEC DBO.I18N_MIG 'NO', 'RBIZDOMAIN_VALUE', @V_SERVPROVCODE
      EXEC DBO.I18N_MIG 'NO', 'BCUSTOMIZED_CONTENT', @V_SERVPROVCODE
      EXEC DBO.I18N_MIG 'NO', 'CALENDAR', @V_SERVPROVCODE
      EXEC DBO.I18N_MIG 'NO', 'CALENDAR_EVENT', @V_SERVPROVCODE
      EXEC DBO.I18N_MIG 'NO', 'XPOLICY', @V_SERVPROVCODE
   END
END
GO

exec dbo.ACA_I18N_INIT 'XXXXX'
GO

If Exists ( SELECT name 
            FROM sysobjects  
            WHERE name = 'ACA_I18N_INIT'
            AND type = 'P')
	DROP PROCEDURE dbo.ACA_I18N_INIT
GO 

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'SPC_EXISTS_CHECK' AND XTYPE = 'P')
  DROP PROCEDURE dbo.SPC_EXISTS_CHECK
GO

