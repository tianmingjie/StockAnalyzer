set log=c:\log\analyze_log.txt
cd c:\work\analyze

date /t > %log%
time /t >> %log%
analyze.exe 0 >> %log%
REM 30days before
REM analyze.exe -30 >> %log%
date /t >> %log%
time /t >> %log%
echo ==================== >>  %log%

set log1=c:\log\generate_log.txt
cd c:\work\generatehtml

date /t > %log1%
time /t >> %log1%
generatehtml >> %log1%

date /t >> %log1%
time /t >> %log1%
echo ==================== >>  %log1%
