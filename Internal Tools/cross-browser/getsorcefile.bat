xcopy *.manifest cross-browser\ /y
xcopy *.rdf cross-browser\ /y
xcopy defaults\preferences\*.* cross-browser\defaults\preferences\ /y
xcopy modules\*.* cross-browser\modules\ /y
xcopy chrome\content\*.* cross-browser\chrome\content\ /y
xcopy chrome\content\options\*.* cross-browser\chrome\content\options\ /y
xcopy chrome\content\options\pages\*.* cross-browser\chrome\content\options\pages\ /y
xcopy chrome\content\common\*.* cross-browser\chrome\content\common\ /y
xcopy chrome\skin\*.* cross-browser\chrome\skin\ /y
xcopy chrome\locale\en-US\*.* cross-browser\chrome\locale\en-US\ /y

cd cross-browser
C:\Progra~1\7-Zip\7z.exe a -tzip ..\cross-browser.xpi
cd ..
rd cross-browser /s/q