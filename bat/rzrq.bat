set log=c:\log\rzrq_log.txt
cd c:\work\rzrq

date /t >> %log%
time /t >> %log%
rzrq.exe  >> %log%
date /t >> %log%
time /t >> %log%
echo ==================== >>  %log%
