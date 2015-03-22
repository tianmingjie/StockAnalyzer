set log=G:\log\analyze_log.txt
cd G:\github\StockAnalyzer\bin

date /t >> %log%
time /t >> %log%
analyze.exe 0 >> %log%
REM analyze.exe -30 >> %log%

date /t >> %log%
time /t >> %log%
echo ==================== >>  %log%
