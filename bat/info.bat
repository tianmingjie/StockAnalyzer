set log=c:\log\info.txt
cd c:\work\info

date /t > %log%
time /t >> %log%
info.exe >> %log%
date /t >> %log%
time /t >> %log%
echo ==================== >>  %log%
