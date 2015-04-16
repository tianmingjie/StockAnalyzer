set log=c:\log\UpdateBigDeal_log.txt
cd c:\work\UpdateBigDeal

date /t >> %log%
time /t >> %log%
UpdateBigDeal.exe 0 >> %log%
REM 30days before
REM analyze.exe -30 >> %log%
REM date /t >> %log%
REM time /t >> %log%
echo ==================== >>  %log%
