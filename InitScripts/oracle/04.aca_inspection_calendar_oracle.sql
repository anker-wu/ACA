-- Comment: Please input your actual ID to replace XXXXX.
CREATE OR REPLACE PROCEDURE SPC_EXISTS_CHECK(P_PROCEDURE_NAME IN VARCHAR2, P_SPC IN VARCHAR2) IS
   L_COUNT_1 NUMBER;
BEGIN
   SELECT COUNT(*) INTO L_COUNT_1 FROM RSERV_PROV WHERE SERV_PROV_CODE = UPPER(P_SPC);
   IF (L_COUNT_1 <= 0) THEN
      
      --When agency does not exists, it will raise the error message
      RAISE_APPLICATION_ERROR((-20000 - 224), 'Please specify the agency code in EXEC ' || P_PROCEDURE_NAME || '(''' ||
                               UPPER(P_SPC) || ''') of file 04.aca_inspection_calendar_oracle.sql');
   END IF;
END SPC_EXISTS_CHECK;
/

------------------------------------------------------------------------------------------------
--do aca inspection calendar content  (begin)
------------------------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE inspection_calendar
(  p_spc             IN VARCHAR2
)
IS
  --v_serv_prov_code  varchar2(50) := 'FLAGSTAFF';
  v_serv_prov_code  varchar2(50) := p_spc;
  cursor cur_calendar_event(iv_serv_prov_code varchar2, iv_inspe_calendar varchar2)  is
    select t.*
    from  calendar_event t,  calendar  c
    where t.serv_prov_code    = c.serv_prov_code
    and   t.calendar_id       = c.calendar_id
    and   t.serv_prov_code    = iv_serv_prov_code  
    and   t.start_date        >= trunc(sysdate)
    and   c.calendar_name  = iv_inspe_calendar
    and   t.rec_status        = 'A'
    and   c.rec_status        = 'A';
  row_calendar_event  cur_calendar_event%rowtype;
  v_inspe_calendar    varchar2(100);
  v_schedule_enable   varchar2(100);
  v_last_seq_number   number(15);
  v_pointIndex        number(15);
  v_calendar_name     varchar2(100);
  v_calendar_priority number(15);
  v_calendar_event_id number(15);
  v_event_recurrence_id          number(15);
  v_calendar_type                varchar2(100);      
  v_calendar_attempts            number(15);
  v_calendar_unit_per_day        number(15);
  i                              number(15);
  v_max_schedule_day             number(15);
  v_today                        varchar2(15);
  v_found                        number(15);
  v_event_type                   varchar2(100);
  v_am_start                     varchar2(100);
  v_am_end                       varchar2(100);
  v_pm_start                     varchar2(100);
  v_pm_end                       varchar2(100);
  v_allocated_units              number(1);
  v_calendar_block_size          varchar2(100);
  v_calendar_block_unit          varchar2(100);
  v_event_rec_idFor6             number(15);
  v_event_rec_idFor7             number(15);
  v_date_loop                    date;
  v_weekend_start                varchar2(100);
  v_weekend_end                  varchar2(100);
  v_weeken_type                  varchar2(100);
begin
   -- check agency code
   SPC_EXISTS_CHECK('inspection_calendar', v_serv_prov_code);  
  v_calendar_name       := 'ACA_DEFAULT_INSPECTION_CALENDAR';  
  v_calendar_priority   := 99;
  v_calendar_block_size := 1;
  v_calendar_block_unit := 'Day';
  v_calendar_type       := 'INSPECTION';
  v_calendar_attempts   := 3;
  v_calendar_unit_per_day := 32; 
  --defaule 32, ACA don't use this option
  v_max_schedule_day      := 5 * 365; 
  -- 5 years, 
  v_event_type            := 'Inspection';
  v_am_start              := '08:00';
  v_am_end                := '12:00';
  v_pm_start              := '13:00';
  v_pm_end                := '16:00';
  v_weekend_start         := '00:00';
  v_weekend_end           := '23:59';
  v_weeken_type           := 'Weekend';
  v_allocated_units       := 1;
  select count(*) into v_found
  from  calendar  c
  where c.serv_prov_code    = v_serv_prov_code
  and   c.calendar_name     = v_calendar_name
  and   c.rec_status        = 'A';
  if v_found = 0 then
    begin
      --get inspection calendar for ACA
      select  t.value_desc into v_inspe_calendar
        from  rbizdomain_value t, calendar c
        where t.serv_prov_code  = v_serv_prov_code
        and   t.serv_prov_code  = c.serv_prov_code
        and   c.calendar_name   = trim(t.value_desc)
        and   c.rec_status      = 'A'
        and   t.bizdomain       = 'ACA_CONFIGS'
        and   t.bizdomain_value = 'INSPECTION_CALENDAR_NAME'
        and   t.rec_status      = 'A'
        and   rownum            = 1;    
    exception
      when no_data_found then
        v_inspe_calendar := null;
    end;
    select t.last_number + 1 into v_last_seq_number from aa_sys_seq t where t.sequence_name = 'CALENDAR_SEQ';
    select t.last_number + 1 into v_event_recurrence_id from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ';
    --for 1~5 fo week
    insert into calendar_event_recurrence(serv_prov_code, event_recurrence_id,    frequency,    interval,              day_of_month, 
                                          week_of_month,          day_of_week,  start_date,            end_date, 
                                          rec_date,               rec_ful_nam,  rec_status)
      values(v_serv_prov_code, v_event_recurrence_id,       0,                      1,            0, 
             0,                           0,                      sysdate,      sysdate + v_max_schedule_day,
             sysdate,                     'ACA Admin',            'A');
    update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ';
    --for saturday
  	select t.last_number + 1 into v_event_rec_idFor6 from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ';
  	insert into calendar_event_recurrence(serv_prov_code, event_recurrence_id,    frequency,    interval,              day_of_month, 
                      										week_of_month,          day_of_week,  start_date,            end_date, 
                      										rec_date,               rec_ful_nam,  rec_status)
    	values(v_serv_prov_code, v_event_rec_idFor6   ,        1,                      1,            0, 
      		   0,                           6,                      sysdate,      sysdate + v_max_schedule_day,
      		   sysdate,                     'ACA Admin',            'A');
  	update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ';
  	--for saturday
  	select t.last_number + 1 into v_event_rec_idFor7 from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ';
  	insert into calendar_event_recurrence(serv_prov_code, event_recurrence_id,    frequency,    interval,              day_of_month, 
                      										week_of_month,          day_of_week,  start_date,            end_date, 
                      										rec_date,               rec_ful_nam,  rec_status)
    	values(v_serv_prov_code, v_event_rec_idFor7   ,        1,                      1,            0, 
      		   0,                            0,                      sysdate,      sysdate + v_max_schedule_day,
      		   sysdate,                      'ACA Admin',            'A');
  	update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_RECURRENCE_SEQ';
    if v_inspe_calendar is not null then
      --copy 6.5 inspection calendar to 6.6
      dbms_output.put_line('Copy inspection calendar ' || v_inspe_calendar || ' to ' || v_calendar_name);
      insert into calendar(serv_prov_code,            calendar_id,                   calendar_name, 
                           calendar_type,             rec_date,                      rec_ful_nam, 
                           rec_status,                calendar_attempts,             calendar_priority,
                           calendar_comment,          calendar_unit_per_day,         calendar_block_size, 
                           calendar_block_unit,       enable_for_aca,                cut_off_time, 
                           after_cut_off)
        select             serv_prov_code,            v_last_seq_number,             v_calendar_name, 
                           v_calendar_type,           sysdate,                       rec_ful_nam, 
                           rec_status,                calendar_attempts,             v_calendar_priority,
                           calendar_comment,          calendar_unit_per_day,         v_calendar_block_size, 
                           v_calendar_block_unit,     'Y',                           cut_off_time, 
                           after_cut_off
        from         calendar t            
        where        t.serv_prov_code = v_serv_prov_code
        and          t.calendar_name  = v_inspe_calendar
        and          t.rec_status     = 'A';
      open cur_calendar_event(v_serv_prov_code, v_inspe_calendar);
      loop
        fetch cur_calendar_event into  row_calendar_event;
        exit when cur_calendar_event%notfound;
        select t.last_number + 1 into v_calendar_event_id from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_SEQ';
  			if to_char( row_calendar_event.start_date, 'D') = '1' or to_char( row_calendar_event.start_date, 'D') = '1' then
          insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
                                     event_type,                    start_date,                     end_date, 
                                     rec_date,                      rec_ful_nam,                    rec_status, 
                                     event_recurrence_id,           event_name,                     event_comment, 
                                     max_units,                     allocated_units)
          values (row_calendar_event.serv_prov_code,                v_calendar_event_id,            v_last_seq_number, 
                  row_calendar_event.event_type,                    row_calendar_event.start_date,  row_calendar_event.end_date, 
                  row_calendar_event.rec_date,                      row_calendar_event.rec_ful_nam, row_calendar_event.rec_status, 
                  null,                            row_calendar_event.event_name,  row_calendar_event.event_comment, 
                  row_calendar_event.max_units,                     row_calendar_event.allocated_units);
        else                
          insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
                                     event_type,                    start_date,                     end_date, 
                                     rec_date,                      rec_ful_nam,                    rec_status, 
                                     event_recurrence_id,           event_name,                     event_comment, 
                                     max_units,                     allocated_units)
          values (row_calendar_event.serv_prov_code,                v_calendar_event_id,            v_last_seq_number, 
                  row_calendar_event.event_type,                    row_calendar_event.start_date,  row_calendar_event.end_date, 
                  row_calendar_event.rec_date,                      row_calendar_event.rec_ful_nam, row_calendar_event.rec_status, 
                  v_event_recurrence_id,                            row_calendar_event.event_name,  row_calendar_event.event_comment, 
                  row_calendar_event.max_units,                     row_calendar_event.allocated_units);
        end if;
        update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_SEQ';
      end loop;
      close cur_calendar_event;
    else 
      dbms_output.put_line('inset inspection calendar ' || v_calendar_name);
      insert into calendar(serv_prov_code,            calendar_id,                   calendar_name, 
                           calendar_type,             rec_date,                      rec_ful_nam, 
                           rec_status,                calendar_attempts,             calendar_priority,
                           calendar_comment,          calendar_unit_per_day,         calendar_block_size, 
                           calendar_block_unit,       enable_for_aca,                cut_off_time, 
                           after_cut_off)
        values(            v_serv_prov_code,          v_last_seq_number,             v_calendar_name,
                           v_calendar_type,           sysdate,                       'ACA Admin',
                           'A',                       v_calendar_attempts,           v_calendar_priority,
                           null,                      v_calendar_unit_per_day,       v_calendar_block_size,
                           v_calendar_block_unit,     'Y',                           null,
                           null);
    end if;
    i := 1;
    for i in 1..v_max_schedule_day loop
      v_date_loop := sysdate + i - 1;
      v_today     := to_char(v_date_loop, 'yyyy-mm-dd');
      select count(*) into v_found 
      from   calendar_event t 
      where  t.serv_prov_code = v_serv_prov_code 
      and    t.calendar_id    = v_last_seq_number
      and    trunc(t.start_date) =  trunc(v_date_loop)
      and    t.rec_status           = 'A';
      if v_found = 0 then
        select t.last_number + 1 into v_calendar_event_id from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_SEQ';
        --Saturday
  		  if to_char(v_date_loop, 'D') = '7' then
    			insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
                									   event_type,                    start_date,                     end_date, 
                									   rec_date,                      rec_ful_nam,                    rec_status, 
                									   event_recurrence_id,           event_name,                     event_comment, 
                									   max_units,                     allocated_units)
    			values (v_serv_prov_code,                v_calendar_event_id,            v_last_seq_number, 
        					v_weeken_type,                   to_date(v_today || ' ' || v_weekend_start, 'yyyy-mm-dd hh24:mi'),  to_date(v_today || ' ' || v_weekend_end, 'yyyy-mm-dd hh24:mi'), 
        					sysdate,                         'ACA Admin',                    'A', 
        					v_event_rec_idFor6,              'Saturday',                     null, 
        					1,                               v_allocated_units);
    		--Sunday
        elsif to_char(v_date_loop, 'D') = '1' then
    			insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
                									   event_type,                    start_date,                     end_date, 
                									   rec_date,                      rec_ful_nam,                    rec_status, 
                									   event_recurrence_id,           event_name,                     event_comment, 
                									   max_units,                     allocated_units)
    			values (v_serv_prov_code,                v_calendar_event_id,            v_last_seq_number, 
        					v_weeken_type,                    to_date(v_today || ' ' || v_weekend_start, 'yyyy-mm-dd hh24:mi'),  to_date(v_today || ' ' || v_weekend_end, 'yyyy-mm-dd hh24:mi'), 
        					sysdate,                         'ACA Admin',                    'A', 
        					v_event_rec_idFor7,              'Sunday',                       null, 
        					1,                               v_allocated_units);
  		  else 
          -- add AM daily item
          insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
                                     event_type,                    start_date,                     end_date, 
                                     rec_date,                      rec_ful_nam,                    rec_status, 
                                     event_recurrence_id,           event_name,                     event_comment, 
                                     max_units,                     allocated_units)
          values (v_serv_prov_code,                v_calendar_event_id,            v_last_seq_number, 
                  v_event_type,                    to_date(v_today || ' ' || v_am_start, 'yyyy-mm-dd hh24:mi'),  to_date(v_today || ' ' || v_am_end, 'yyyy-mm-dd hh24:mi'), 
                  sysdate,                         'ACA Admin',                    'A', 
                  v_event_recurrence_id,           'AM',                           null, 
                  1,                               v_allocated_units);
          update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_SEQ';
          -- add PM daily item
          select t.last_number + 1 into v_calendar_event_id from aa_sys_seq t where t.sequence_name = 'CALENDAR_EVENT_SEQ';
          insert into calendar_event(serv_prov_code,                event_id,                       calendar_id, 
                                     event_type,                    start_date,                     end_date, 
                                     rec_date,                      rec_ful_nam,                    rec_status, 
                                     event_recurrence_id,           event_name,                     event_comment, 
                                     max_units,                     allocated_units)
          values (v_serv_prov_code,                v_calendar_event_id,            v_last_seq_number, 
                  v_event_type,                    to_date(v_today || ' ' || v_pm_start, 'yyyy-mm-dd hh24:mi'),  to_date(v_today || ' ' || v_pm_end, 'yyyy-mm-dd hh24:mi'), 
                  sysdate,                         'ACA Admin',                    'A', 
                  v_event_recurrence_id,           'PM',                           null, 
                  1,                               v_allocated_units);
        end if;
        update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_EVENT_SEQ';
      end if;
    end loop;
    --assign inspection type to this calendar
    insert into xcalendar_insptyp(serv_prov_code, calendar_id, insp_seq_nbr, rec_date, rec_ful_nam, rec_status)
      select v_serv_prov_code, v_last_seq_number, t.insp_seq_nbr, sysdate, 'ACA Admin', 'A'  
         from rinsptyp t 
         where t.serv_prov_code = v_serv_prov_code 
         and   t.rec_status     = 'A';
    update aa_sys_seq set last_number = last_number + 1 where sequence_name = 'CALENDAR_SEQ';
    COMMIT;
  end if;
end;
/

exec inspection_calendar('XXXXX');

DROP PROCEDURE inspection_calendar;



------------------------------------------------------------------------------------------------
--do aca inspection calendar content  (end)
------------------------------------------------------------------------------------------------
drop procedure SPC_EXISTS_CHECK;

