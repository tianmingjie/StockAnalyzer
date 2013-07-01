set stock=000430
set startDate=2013-05-01
set endDate=2013-07-01
set filter=600

D:\project\github\StockAnalyzer\bin\DownloadData.exe %stock%  %startDate% %endDate%

D:\project\github\StockAnalyzer\bin\SotckAnalyzer.exe %stock%  %startDate% %endDate% %filter%