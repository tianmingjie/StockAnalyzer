using big;
using big.entity;
using Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //List<InfoData> list = BizApi.QueryInfoByIndustry2("金融服务","银行");

            //BizApi.InsertAnalyzeData(new DateTime(2014,1,1),DateTime.Now);
            //List<InfoData> list = BizApi.QueryInfoAll();
            ////decimal[] list = BizApi.QueryExtractList("sh600000");
            //List<AnalyzeData> list1 = BizApi.QueryAnalyzeData(DateTime.Now);


            //StockResource sr = new StockResource();
            //DateTime end=new DateTime(2015,3,12);
            //DateTime start=end.AddMonths(-1);
            //List<AnalyzeData> list1 = BizApi.ComputeAll(1,start,end);
            //foreach (AnalyzeData ad in list1)
            //    Console.WriteLine(ad.sid + "," + ad.value + "," + ad.name + "," + ad.firstlevel + "," + ad.secondlevel);
            //BizApi.InsertInfoExt(new InfoData() { sid = "sh600000" });

            //InfoExtData ifd = BizApi.QueryInfoExtById("sh600000");

            //AnalyzeData ad = BizApi.ComputeSingle3("sh600000", 1, 1000, new DateTime(2015, 1, 1), new DateTime(2015, 3, 1));
            //BizApi.ComputeAll_3(int.Parse(args[0]), int.Parse(args[1]));
            Console.WriteLine(BizApi.QueryLatestPrice("sh600272","20150319"));
            Console.WriteLine(BizApi.QueryMaxMinPriceByRange("sh600272",24));
            Console.WriteLine();
        }
    }
    
}
