set log=c:\log\generate_log.txt
cd c:\work\generatehtml

date /t > %log%
time /t >> %log%
generatehtml >> %log%

date /t >> %log%
time /t >> %log%
echo ==================== >>  %log%
