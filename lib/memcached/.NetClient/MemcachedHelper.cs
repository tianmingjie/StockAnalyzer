using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Memcached.ClientLibrary;

namespace Tools.Common
{
    public class MemcachedHelper:IDisposable
    {
        private MemcachedClient _mc =null; 
        protected MemcachedClient mc
        {
            get
            {
                if(_mc==null) _mc=new MemcachedClient();//初始化一个客户端 
                return _mc;
            }
        }
        /// <summary>
        /// 如果默认不是本地服务，可以额外指定memcached服务器地址
        /// </summary>
        public static string[] serverList
        {
            get;
            set;
        }
        private static MemcachedHelper _instance = null;
        /// <summary>
        /// 单例实例对象，外部只能通过MemcachedHelper.instance使用memcached
        /// </summary>
        public static MemcachedHelper instance
        {
            get
            {
                if (_instance == null)
                {
                    if (serverList != null && serverList.Length > 0)
                        _instance = new MemcachedHelper(serverList);
                    else _instance = new MemcachedHelper();
                }
                
                return _instance;
            }
        }
        SockIOPool pool;
        private void start(params string[]servers)
        {
            string[] serverlist;
            if (servers == null || servers.Length < 1)
            {
                serverlist = new string[] { "127.0.0.1:11011" }; //服务器列表，可多个
            }
            else
            {
                serverlist=servers;
            }
            pool = SockIOPool.GetInstance();

            //根据实际情况修改下面参数
            pool.SetServers(serverlist);
            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize(); // initialize the pool for memcache servers      
        }
        private MemcachedHelper(string[] servers)
        {
            start(servers);
        }
        private MemcachedHelper()
        {
            start();
        }
        ~MemcachedHelper()
        {
            if (pool != null) pool.Shutdown();
        }

        public object Get(string key)
        {
            return mc.Get(key);
        }
        public T Get<T>(string key)
        {
            object data=mc.Get(key);
            if (data is T) return (T)data;
            else return default(T);
        }
        public bool Delete(string key)
        {
            return mc.Delete(key);
        }
        public bool Set(string key,object data)
        {
           return mc.Set(key, data);
        }
        //在应用程序退出之前，调用Dispose释放memcached客户端连接
        public void Dispose()
        {
            if (pool != null) pool.Shutdown();
        }
    }
}
