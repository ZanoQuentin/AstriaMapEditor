@echo off
set /p MapID=MapID :
echo Compilation...
Flasm\flasm.exe -a Maps\%MapID%\temp.flm
set /p Name=Date :
echo Copie du fichier "Flasm\blank.swf" vers Maps\%MapID%\%MapID%_%Name%.swf
COPY Flasm\blank.swf Maps\%MapID%\%MapID%_%Name%.swf
echo Renommage du fichier "Flasm\blank.$wf" en "Flasm\blank.swf"
COPY Flasm\blank.$wf Flasm\blank.swf
DEL Flasm\blank.$wf
pause