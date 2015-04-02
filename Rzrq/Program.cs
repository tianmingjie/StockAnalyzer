using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using big.entity;
using Common;
using HtmlAgilityPack;

namespace Rzrq
{
    public class Program
    {
        public static void Main(string[] args)
        {

            List<RzrqData> list = new List<RzrqData>();
            //http://data.eastmoney.com/rzrq/sh.html
            string url = string.Format("http://data.eastmoney.com/rzrq/sh.html");
            WebClient client = new WebClient();
            byte[] gg = client.DownloadData(url);
            string haha = System.Text.Encoding.GetEncoding("gb2312").GetString(gg);

            string start_str = "defjson:{pages:1,data:";
            int start = haha.IndexOf(start_str)+start_str.Length;
            int end = haha.IndexOf("beforedisplay:function");
            string aa = haha.Substring(start, end - start);
            aa = aa.Trim();
            aa=aa.Substring(2, aa.Length - 6);

            aa=aa.Replace("\",\"","-");
            string[] cc = aa.Split('-');

            //510010,融资融券_沪证,治理ETF,44315624,2015/4/1 0:00:00,114417(rongquanchanghuangliang),474500(rongquanmaichuliang),5136454.851(rongquanyue),4283949,2293344(rongzichanghuane),943569(rongzimairue),0,49452079(rongziyue)
            foreach (string bb in cc)
            {
                string[] dd = bb.Split(',');
                if (dd.Length == 13 && dd[0]!=""&& !dd[0].StartsWith("5"))
                {
                    list.Add(new RzrqData()
                    {
                        sid=StockUtil.FormatStock(dd[0]),
                        name=dd[2],
                        time=DateTime.Parse(dd[4]),
                        rongquanchanghuanliang=decimal.Parse(dd[5]),
                        rongquanmaichuliang=decimal.Parse(dd[6]),
                        rongquanyue=decimal.Parse(dd[7]),
                        rongzichanghuane=decimal.Parse(dd[9]),
                        rongzimairue=decimal.Parse(dd[10]),
                        rongziyue=decimal.Parse(dd[12])
                    });
                }
            }
            //TextReader stream = new StringReader(haha);
            //HtmlDocument document = new HtmlDocument();
            //document.Load(stream);

            //HtmlNode rootNode = document.DocumentNode;
            //HtmlNodeCollection c = rootNode.SelectNodes("//table[@id='dt_1']");
            //System.Threading.Thread.Sleep(5000);
            //string innerString = c[0].InnerText;

            foreach (RzrqData rd in list)
            {
                big.BizApi.InsertRzrq(rd);
            }
            Console.WriteLine();
        }
    }
}
