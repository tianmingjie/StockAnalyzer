sc create "Memcached Server" binpath= "G:\github\StockAnalyzer\lib\memcached\memcached.exe -d runservice -l 127.0.0.1 -m 64 -c 2048 -p 11011" DisplayName= "Memcached Server" start= auto  