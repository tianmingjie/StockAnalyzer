echo off
net stop "memcached Server"
net start "memcached Server"
echo 服务已启动！

pause