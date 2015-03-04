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

            InfoExtData ifd = BizApi.QueryInfoExtById("sh600000");
            Console.WriteLine();
        }


        public static Dictionary<string,string> GenerateStockList()
        {
            Dictionary<string, string> dict = new Dictionary<string,string>();

            
            for (int i = 1600000; i < 1605000; i++)
            {   
                string name="";
                if (InValidStock(TrimStock(i), out name))
                {
                    dict.Add("sh" + TrimStock(i), name);
                }
            }

            for (int i = 1000000; i < 1004000; i++)
            {
                string name="";
                if (InValidStock(TrimStock(i), out name))
                {
                    dict.Add("sz" + TrimStock(i), name);
                }
            }

            for (int i = 1300000; i < 1301000; i++)
            {
                string name="";
                if (InValidStock(TrimStock(i).ToString(), out name))
                {
                    dict.Add("sz" + TrimStock(i), name);
                }
            }
            return dict;
        }

        public static string TrimStock(int stock)
        {
            return stock.ToString().Substring(1);
        }

        public static bool InValidStock(string stock,out string name)
        {
            name = "";
            bool inValid=false;
            WebClient wc = new WebClient();
            Uri uri=new Uri("http://hq.sinajs.cn/list="+FormatStock(stock));
            byte[] bytes = wc.DownloadData(uri);
            string str = System.Text.Encoding.Default.GetString(bytes);
            
            if (!str.Contains("=\"\""))
            {
                inValid = true;
                int a=str.IndexOf("\"");
                int b=str.IndexOf(",");
                name = str.Substring(a+1, b - a-1);
                Console.WriteLine(stock+":"+name);
            }
            return inValid;
        }



        public static string FormatStock(string stock)
        {
            if (stock.StartsWith("sh") || stock.StartsWith("sz"))
            {
                return stock;
            }
            else
            { return Int32.Parse(stock) > 599999 ? "sh" + stock : "sz" + stock; }
        }
    }
}
