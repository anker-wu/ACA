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
		SET @ERR_MSG = 'Please specify the agency code in EXEC ' + COALESCE(@P_PROCEDURE_NAME, '') + '(''' + UPPER(@P_SPC) + ''') of file 04.aca_inspection_calendar_mssql.sql'
      RAISERROR(@ERR_MSG, 16, 1) with nowait 
      --exit when the agency not exists
      RAISERROR('interrupt', 20, 127, '1', '1') with log  
    END
END
GO


------------------------------------------------
--do aca inspection calendar part  (begin)
------------------------------------------------
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'INSPECTION_CALENDAR' AND XTYPE = 'P')
  DROP PROCEDURE dbo.INSPECTION_CALENDAR
GO

CREATE PROCEDURE dbo.inspection_calendar
   @p_spc             VARCHAR(30)
AS 
declare	  @v_serv_prov_code				varchar(50) 
declare	  @v_inspe_calendar				varchar(200)   
declare   @v_schedule_enable   			varchar(100)
declare   @v_last_seq_number   			int
declare   @v_pointIndex        			int
declare   @v_calendar_name     			varchar(100)
declare   @v_calendar_priority 			int
declare   @v_calendar_event_id 			int
declare   @v_event_recurrence_id      	int
declare   @v_calendar_type            	varchar(100)      
declare   @v_calendar_attempts        	int
declare   @v_calendar_unit_per_day    	int
declare   @i                          	int
declare   @v_max_schedule_day         	int
declare   @v_today                    	varchar(15)
declare   @v_found                    	int
declare   @v_event_type               	varchar(100)
declare   @v_am_start                 	varchar(100)
declare   @v_am_end                   	varchar(100)
declare   @v_pm_start                 	varchar(100)
declare   @v_pm_end                   	varchar(100)
declare   @v_allocated_units          	int
declare   @v_calendar_block_size      	varchar(100)
declare   @v_calendar_block_unit      	varchar(100)
declare   @start_date      				datetime
declare   @end_date      				datetime
declare   @rec_date      				datetime
declare   @rec_ful_nam      			varchar(70)
declare   @rec_status      				varchar(1)
declare   @event_name      				varchar(40)
declare   @event_comment    			varchar(4000)
declare   @max_units      				numeric(5)
declare   @allocated_units  			numeric(5)
declare   @event_type               	varchar(70)
declare	  @v_event_rec_idFor6           numeric(15)
declare   @v_event_rec_idFor7           numeric(15)
declare   @v_date_loop                  datetime
declare   @v_weekend_start              varchar(100)
declare   @v_weekend_end                varchar(100)
declare   @v_weeken_type                varchar(100)
begin
	--set @v_serv_prov_code					= 'SACCO'	
	set @v_serv_prov_code					= @p_spc
	-- check agency code
   DECLARE @P_PROCEDURE_NAME VARCHAR(1000)
   SET @P_PROCEDURE_NAME= object_name(@@procid)
   exec DBO.SPC_EXISTS_CHECK @P_PROCEDURE_NAME, @v_serv_prov_code
	set @v_calendar_name       				= 'ACA_DEFAULT_INSPECTION_CALENDAR'  
	set @v_calendar_priority   				= 99
	set @v_calendar_block_size 				= 1
	set @v_calendar_block_unit 				= 'Day'
	set @v_calendar_type       				= 'INSPECTION'
	set @v_calendar_attempts   				= 3
	set @v_calendar_unit_per_day 			= 32 
	--defaule 32, ACA don't use this option
	set @v_max_schedule_day      			= 5 * 365 
	-- 5 years, 
	set @v_event_type            			= 'Inspection'
	set @v_am_start              			= '08:00:00'
	set @v_am_end                			= '12:00:00'
	set @v_pm_start              			= '13:00:00'
	set @v_pm_end                			= '16:00:00'
	set @v_weekend_start         			= '00:00:00'
	set @v_weekend_end           			= '23:59:00'
	set @v_weeken_type           			= 'Weekend'
	set @v_allocated_units       			= 1
	select @v_found = count(*)
	from  calendar  c
	where c.serv_prov_code    = @v_serv_prov_code
	and   c.calendar_name     = @v_calendar_name
	and   c.rec_status        = 'A'
	if @v_found = 0 
	begin
		--get inspection calendar for ACA
		set @v_inspe_calendar = null
        select top 1 @v_inspe_calendar = t.value_desc
	        from  rbizdomain_value t, calendar c
	        where t.serv_prov_code  = @v_serv_prov_code
	        and   t.serv_prov_code  = c.serv_prov_code
	        and   c.calendar_name   = t.value_desc
	        and   c.rec_status      = 'A'
	        and   t.bizdomain       = 'ACA_CONFIGS'
	        and   t.bizdomain_value = 'INSPECTION_CALENDAR_NAME'
	        and   t.rec_status      = 'A' 
		select @v_last_seq_number = t.last_number + 1 from aa_sys_seq t where t.sequence_name = 'CALENDAR_SEQ'
		select @v_event_recurrence_id = t.last_number + 1 from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ'
		--for 1~5 fo week
		insert into calendar_event_recurrence(serv_prov_code, event_recurrence_id,    frequency,    interval,              day_of_month, 
											week_of_month,          day_of_week,  start_date,            end_date, 
											rec_date,               rec_ful_nam,  rec_status)
		values(@v_serv_prov_code, @v_event_recurrence_id,       0,                      1,            0, 
			   0,                           0,                      getdate(),      DATEADD(day,@v_max_schedule_day, getdate()),
			   getdate(),                     'ACA Admin',            'A')
		update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ'
		--for saturday
		select @v_event_rec_idFor6 = t.last_number + 1 from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ'
		insert into calendar_event_recurrence(serv_prov_code, event_recurrence_id,    frequency,    interval,              day_of_month, 
											week_of_month,          day_of_week,  start_date,            end_date, 
											rec_date,               rec_ful_nam,  rec_status)
		values(@v_serv_prov_code, @v_event_rec_idFor6   ,        1,                      1,            0, 
			   0,                           6,                      getdate(),      DATEADD(day,@v_max_schedule_day, getdate()),
			   getdate(),                     'ACA Admin',            'A')
		update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ'
		--for saturday
		select @v_event_rec_idFor7 = t.last_number + 1 from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ'
		insert into calendar_event_recurrence(serv_prov_code, event_recurrence_id,    frequency,    interval,              day_of_month, 
											week_of_month,          day_of_week,  start_date,            end_date, 
											rec_date,               rec_ful_nam,  rec_status)
		values(@v_serv_prov_code, @v_event_rec_idFor7   ,        1,                      1,            0, 
			   0,                           0,                      getdate(),      DATEADD(day,@v_max_schedule_day, getdate()),
			   getdate(),                     'ACA Admin',            'A')
		update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ'
		if  @v_inspe_calendar  is not  null
		begin 
			--copy 6.5 inspection calendar to 6.6
			print 'Copy inspection calendar ' + @v_inspe_calendar + ' to ' + @v_calendar_name
			insert into calendar(serv_prov_code,            calendar_id,                   calendar_name, 
								 calendar_type,             rec_date,                      rec_ful_nam, 
								 rec_status,                calendar_attempts,             calendar_priority,
								 calendar_comment,          calendar_unit_per_day,         calendar_block_size, 
								 calendar_block_unit,       enable_for_aca,                cut_off_time, 
								 after_cut_off)
			  select             serv_prov_code,            @v_last_seq_number,             @v_calendar_name, 
								 @v_calendar_type,             getdate(),                       rec_ful_nam, 
								 rec_status,                calendar_attempts,             @v_calendar_priority,
								 calendar_comment,          calendar_unit_per_day,         @v_calendar_block_size, 
								 @v_calendar_block_unit,     'Y',                           cut_off_time, 
								 after_cut_off
			  from         calendar t            
			  where        t.serv_prov_code = @v_serv_prov_code
			  and          t.calendar_name  = @v_inspe_calendar
			  and          t.rec_status     = 'A'
			declare cur_calendar_event cursor for
				select t.event_type,t.start_date,t.end_date, 
					  t.rec_date,t.rec_ful_nam, t.rec_status, 
					  t.event_name,t.event_comment, 
					  t.max_units,t.allocated_units
				from  calendar_event t,  calendar  c
				where t.serv_prov_code    = c.serv_prov_code
				and   t.calendar_id			= c.calendar_id
				and   t.serv_prov_code    = @v_serv_prov_code          
				and   convert(varchar(10),t.start_date,121) >=  convert(varchar(10),getdate(),121)  
				and   c.calendar_name		= @v_inspe_calendar
				and   t.rec_status			= 'A'
				and   c.rec_status			= 'A'
			open cur_calendar_event
			fetch NEXT from cur_calendar_event into @event_type,@start_date,@end_date,@rec_date,@rec_ful_nam,@rec_status,@event_name,@event_comment,@max_units,@allocated_units
			WHILE @@fetch_status = 0
			begin
				select @v_calendar_event_id = t.last_number + 1 from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_SEQ'
				if DATENAME(dw, @start_date) = 'Saturday' or DATENAME(dw, @start_date) = 'Sunday' 
				begin
					insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
											 event_type,                    start_date,                     end_date, 
											 rec_date,                      rec_ful_nam,                    rec_status, 
											 event_recurrence_id,           event_name,                     event_comment, 
											 max_units,                     allocated_units)
					values (@v_serv_prov_code,@v_calendar_event_id,@v_last_seq_number, 
						  @event_type,@start_date,@end_date, 
						  @rec_date,@rec_ful_nam, @rec_status, 
						  null,@event_name,@event_comment, 
						  @max_units,@allocated_units)
				end
				else
				begin
					insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
											 event_type,                    start_date,                     end_date, 
											 rec_date,                      rec_ful_nam,                    rec_status, 
											 event_recurrence_id,           event_name,                     event_comment, 
											 max_units,                     allocated_units)
					values (@v_serv_prov_code,@v_calendar_event_id,@v_last_seq_number, 
						  @event_type,@start_date,@end_date, 
						  @rec_date,@rec_ful_nam, @rec_status, 
						  @v_event_recurrence_id,@event_name,@event_comment, 
						  @max_units,@allocated_units)
				end
				update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_SEQ'
				fetch NEXT from cur_calendar_event into @event_type,@start_date,@end_date,@rec_date,@rec_ful_nam,@rec_status,@event_name,@event_comment,@max_units,@allocated_units
			end 
			close cur_calendar_event
			DEALLOCATE cur_calendar_event
		end  
		else 
		begin
			print 'inset inspection calendar ' + @v_calendar_name
			insert into calendar(serv_prov_code,            calendar_id,                   calendar_name, 
								 calendar_type,             rec_date,                      rec_ful_nam, 
								 rec_status,                calendar_attempts,             calendar_priority,
								 calendar_comment,          calendar_unit_per_day,         calendar_block_size, 
								 calendar_block_unit,       enable_for_aca,                cut_off_time, 
								 after_cut_off)
			  values(            @v_serv_prov_code,          @v_last_seq_number,             @v_calendar_name,
								 @v_calendar_type,           getdate(),                       'ACA Admin',
								 'A',                       @v_calendar_attempts,           @v_calendar_priority,
								 null,                      @v_calendar_unit_per_day,       @v_calendar_block_size,
								 @v_calendar_block_unit,     'Y',                           null,
								 null)
		end  
		set @i = 1
		while @i <= @v_max_schedule_day  
		begin
			set @v_date_loop = DATEADD(day,@i-1, getdate())
			set @v_today = convert(varchar(10),@v_date_loop, 101)
			select @v_found = count(*) 
			from   calendar_event t 
			where  t.serv_prov_code = @v_serv_prov_code 
			and    t.calendar_id    = @v_last_seq_number
			and    convert(varchar(10),t.start_date,121) =  convert(varchar(10),@v_date_loop,121)  
			and    t.rec_status           = 'A'
			if @v_found = 0 
			begin
			  -- add AM daily item
			  select @v_calendar_event_id = t.last_number + 1 from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_SEQ'
			  if DATENAME(dw, @v_date_loop) = 'Saturday'
			  begin
				insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
										   event_type,                    start_date,                     end_date, 
										   rec_date,                      rec_ful_nam,                    rec_status, 
										   event_recurrence_id,           event_name,                     event_comment, 
										   max_units,                     allocated_units)
				values (@v_serv_prov_code,                @v_calendar_event_id,            @v_last_seq_number, 
						@v_weeken_type,                    convert(datetime,@v_today + ' ' + @v_weekend_start),  convert(datetime,@v_today + ' ' + @v_weekend_end), 
						getdate(),                         'ACA Admin',                    'A', 
						@v_event_rec_idFor6,           'Saturday',                           null, 
						1,                            @v_allocated_units)
			  end
			  else if  DATENAME(dw, @v_date_loop) = 'Sunday' 
			  begin
				insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
										   event_type,                    start_date,                     end_date, 
										   rec_date,                      rec_ful_nam,                    rec_status, 
										   event_recurrence_id,           event_name,                     event_comment, 
										   max_units,                     allocated_units)
				values (@v_serv_prov_code,                @v_calendar_event_id,            @v_last_seq_number, 
						@v_weeken_type,                    convert(datetime,@v_today + ' ' + @v_weekend_start),  convert(datetime,@v_today + ' ' + @v_weekend_end), 
						getdate(),                         'ACA Admin',                    'A', 
						@v_event_rec_idFor7,           'Sunday',                           null, 
						1,                            @v_allocated_units)
			  end
			  else    
			  begin  	
				  insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
											 event_type,                    start_date,                     end_date, 
											 rec_date,                      rec_ful_nam,                    rec_status, 
											 event_recurrence_id,           event_name,                     event_comment, 
											 max_units,                     allocated_units)
				  values (@v_serv_prov_code,                @v_calendar_event_id,            @v_last_seq_number, 
						  @v_event_type,                    convert(datetime,@v_today + ' ' + @v_am_start),  convert(datetime,@v_today + ' ' + @v_am_end), 
						  getdate(),                         'ACA Admin',                    'A', 
						  @v_event_recurrence_id,           'AM',                           null, 
						  1,                            @v_allocated_units) 
				  update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_SEQ'
				  -- add PM daily item
				  select @v_calendar_event_id = t.last_number + 1 from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_SEQ'
				  insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
											 event_type,                    start_date,                     end_date, 
											 rec_date,                      rec_ful_nam,                    rec_status, 
											 event_recurrence_id,           event_name,                     event_comment, 
											 max_units,                     allocated_units)
				  values (@v_serv_prov_code,                @v_calendar_event_id,            @v_last_seq_number, 
						  @v_event_type,                    convert(datetime,@v_today + ' ' + @v_pm_start),  convert(datetime,@v_today + ' ' + @v_pm_end), 
						  getdate(),                         'ACA Admin',                    'A', 
						  @v_event_recurrence_id,           'PM',                           null, 
						  1,                            @v_allocated_units)
				end
				update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_SEQ'
			end
			set @i = @i + 1
		end 
		--assign inspection type to this calendar
		insert into xcalendar_insptyp(serv_prov_code, calendar_id, insp_seq_nbr, rec_date, rec_ful_nam, rec_status)
		select @v_serv_prov_code, @v_last_seq_number, t.insp_seq_nbr, getdate(), 'ACA Admin', 'A'  
		   from rinsptyp t 
		   where t.serv_prov_code = @v_serv_prov_code 
		   and   t.rec_status     = 'A'   
		update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_SEQ'
	end
end
GO

exec DBO.inspection_calendar 'XXXXX'
GO

DROP PROCEDURE DBO.inspection_calendar
GO

------------------------------------------------
--do aca inspection calendar part  (end)
------------------------------------------------
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'SPC_EXISTS_CHECK' AND XTYPE = 'P')
  DROP PROCEDURE dbo.SPC_EXISTS_CHECK
GO

