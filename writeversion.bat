set script_start_path=D:\support-files\aca_system_time_script_start.txt
set version_info=%cd%\build.main.properties
set script_end_path=D:\support-files\aca_system_time_script_end.txt
set version_aspx=%cd%\PrecompiledWeb\version.aspx
set time_aspx=D:\support-files\time.aspx
copy /b %script_start_path%+%version_info%+%script_end_path% %version_aspx%
copy %time_aspx% %cd%\PrecompiledWeb\time.aspx
pause
