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
            string filename = "";
            if (args.Length == 0)
            {
                filename = "stock.csv";
            }
            else
            {
                filename = args[0];
            }
            
            Dictionary<string,string> d=GenerateStockList();
            StringBuilder sb = new StringBuilder();
            sb.Append("code,name\r\n");
            foreach (string a in d.Keys)
            {
                //Console.WriteLine(a, d[a]);
                sb.Append(a+","+d[a]+"\r\n");
            }
            if (File.Exists(filename)) File.Delete(filename);
            File.WriteAllText(filename, sb.ToString(), Encoding.UTF8);
        }

        public static Dictionary<string,string> GenerateStockList()
        {
            Dictionary<string, string> dict = new Dictionary<string,string>();

            
            for (int i = 1600000; i < 1605000; i++)
            {   
                string name="";
                if (InValidStock(TrimStock(i), out name))
                {   
                    dict.Add("sh"+i.ToString(),name);
                }
            }

            for (int i = 1000000; i < 1004000; i++)
            {
                string name="";
                if (InValidStock(TrimStock(i), out name))
                {
                    dict.Add("sz" + i.ToString(), name);
                }
            }

            for (int i = 1300000; i < 1301000; i++)
            {
                string name="";
                if (InValidStock(TrimStock(i).ToString(), out name))
                {
                    dict.Add("sz" + i.ToString(), name);
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
                //Console.WriteLine(stock+":"+name);
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
