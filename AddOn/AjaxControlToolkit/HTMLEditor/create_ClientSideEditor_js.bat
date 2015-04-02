del AjaxControlToolkit.HTMLEditor.ClientSideEditor.js
rem copy /b /y Toolbar_buttons\*.js+Popups\*.js+*.js ClientSideEditorTemp.js
cscript //NoLogo WScript\copyFromList.js < filelist.txt > ClientSideEditorTemp.js
C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe ClientSideEditor.csproj
del ClientSideEditorTemp.js
cscript //NoLogo WScript\RemoveExtraRegisterNamespace.js < ClientSideEditorTempNew.js > AjaxControlToolkit.HTMLEditor.ClientSideEditor.js
del ClientSideEditorTempNew.js
