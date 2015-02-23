using System;
using System.Collections;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using big.entity;

namespace big
{
    /// <summary
    ///MYSQLHelper 的摘要说明
    /// </summary>
    public class MainClass
    {

        // 用于缓存参数的HASH表
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());


        public static void Main(string[] args)
        {
           // BizData.QueryByWeek("sz000830", 500, new DateTime(2015, 1, 1), new DateTime(2015, 3, 3));


            //List<BasicData> list = ImportRawData.ReadCsvFolder(@"D:\workspace\myproject\sz000830", 2000);
            //foreach (BasicData bd in list)
            //{
            //    //Console.WriteLine(bd.time+" "+bd.sellshare);
            //    BizData.InsertTable(bd);
            //}

            IList<BasicData> list = BizApi.QueryByWeek("sz000830", 2000, new DateTime(2014, 7, 1), new DateTime(2014, 12, 12)); ;

            Console.WriteLine(BizApi.QueryExtractLastUpdate("sz000830"));
        }

    }
}