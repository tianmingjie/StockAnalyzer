set log=c:\log\analyze_log.txt
cd c:\work\analyze

date /t >> %log%
time /t >> %log%
analyze.exe 20130601 >> %log%
analyze.exe 20140101 >> %log%
analyze.exe 20140601 >> %log%
analyze.exe 20140901 >> %log%
date /t >> %log%
time /t >> %log%
echo ==================== >>  %log%
