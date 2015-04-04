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
            string dt = DateTime.Now.ToString("yyyy-MM-dd");
            //http://data.eastmoney.com/rzrq/sh.html
            string url = string.Format("http://vip.stock.finance.sina.com.cn/q/go.php/vInvestConsult/kind/rzrq/index.phtml?tradedate="+dt);
            //string url = string.Format("http://vip.stock.finance.sina.com.cn/q/go.php/vInvestConsult/kind/rzrq/index.phtml");
            WebClient client = new WebClient();
            byte[] gg = client.DownloadData(url);
            string jj = System.Text.Encoding.GetEncoding("gb2312").GetString(gg);


            TextReader stream = new StringReader(jj);
            HtmlDocument document = new HtmlDocument();
            document.Load(stream);

            HtmlNode rootNode = document.DocumentNode;
            HtmlNodeCollection c = rootNode.SelectNodes("//table[@id='dataTable']");

            string haha = c[1].InnerText;
            haha = haha.Replace(",", "");
            haha=haha.Replace("\n", ",");
            haha=haha.Replace(" ", "");
            haha = haha.Replace(",,,,", "|");
            haha=haha.Replace("|,", "|");
            if (haha.Length < 1000) return; 

            string start_str = "|1";
            int start = haha.IndexOf(start_str)+start_str.Length-1;
            int end = haha.LastIndexOf(",,,");
            string aa = haha.Substring(start, end - start);

            string[] cc = aa.Split('|');


            //股票代码	股票名称	余额(元)	买入额(元)	偿还额(元)	余量金额(元)	余量(股)	卖出量(股)	偿还量(股)	融券余额(元)

            //510010,融资融券_沪证,治理ETF,44315624,2015/4/1 0:00:00,114417(rongquanchanghuangliang),474500(rongquanmaichuliang),5136454.851(rongquanyue),4283949,2293344(rongzichanghuane),943569(rongzimairue),0,49452079(rongziyue)
            foreach (string bb in cc)
            {
                string[] dd = bb.Split(',');
                if (!dd[1].StartsWith("5"))
                {
                    list.Add(new RzrqData()
                    {
                        sid=StockUtil.FormatStock(dd[1]),
                        name=dd[2],
                        rongziyue = p(dd[3]),
                        rongzimairue = p(dd[4]),
                        rongzichanghuane=p(dd[5]),        
                        time=DateTime.Parse(dt),
                        rongquanyuliangjine = p(dd[6]),
                        rongquanyuliang = p(dd[7]),
                        rongquanmaichuliang = p(dd[8]),
                        rongquanchanghuanliang=p(dd[9]),
                        rongquanyue = p(dd[10])                     
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

        public static decimal p(string str)
        {
            return str == "--" ? 0 : decimal.Parse(str);
        }
    }
}
