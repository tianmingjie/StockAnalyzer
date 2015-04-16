set log=c:\log\infoext.txt
cd c:\work\infoext

date /t > %log%
time /t >> %log%
infoext.exe >> %log%
date /t >> %log%
time /t >> %log%
echo ==================== >>  %log%
