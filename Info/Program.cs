using big;
using big.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Info
{
    class Program
    {
        public static void Main(string[] args)
        {
            //string stock = "sz000166";
            //InfoData id = BuildInfo(stock);
            //Console.WriteLine(id);
            //BizApi.InsertInfo(id);
            //GetInfo(stock);
            //GetShares(stock);
            IList<InfoData> list=GetList();
            foreach (InfoData id in list)
            {
                BizApi.InsertInfo(id);
            }
        }

        public static IList<InfoData> GetList()
        {
            //300000-301000
            //600000-605000
            //000001-003000
            WebClient client = new WebClient();
            IList<InfoData> sz = GetStock(client, 1000001, 1003000);
          
            IList<InfoData> chuangye = GetStock(client, 1300000, 1301000);
            IList<InfoData> sh = GetStock(client, 1600000, 1605000);
            sz=sz.Concat(chuangye).ToList();
            sz=sz.Concat(sh).ToList();
            return sz;

        }

        public static IList<InfoData> GetStock(WebClient client, int start, int end)
        {
            IList<InfoData> list = new List<InfoData>();
            //股票存在
            string stock;
            for (int i = start; i < end; i++)
            {
                if (start < 1600000)
                {

                    stock = string.Format("sz{0}", i.ToString().Substring(1));
                }
                else
                {
                    stock = string.Format("sh{0}", i.ToString().Substring(1));
                }

                InfoData id=BuildInfo(stock);
                if (id != null) list.Add(id);
            }
            return list;
        }

        public static InfoData BuildInfo(string sid)
        {
            WebClient client = new WebClient();
                byte[] page = client.DownloadData(string.Format("http://hq.sinajs.cn/list={0}", sid));
                string content = System.Text.Encoding.GetEncoding("GBK").GetString(page);
            InfoData id=null;
                if (content.Length > 40)
                {
                    string name = content.Substring(content.IndexOf("\"") + 1, content.IndexOf(",") - content.IndexOf("\"")).Trim();
  

                    id= new InfoData();
                    id.sid = sid;
                    id.name = name.EndsWith(",")?name.Substring(0,name.Length-1):name;
                    id=BuildBasicInfo(id);
                    id = BuildShare(id);
                }
                Console.WriteLine("build complete: " + sid);
            return id;
        }

        public static Dictionary<string, string> GetInfo(string stock)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            WebClient client = new WebClient();
            // Stream data = client.OpenRead(string.Format("http://stockpage.10jqka.com.cn/{0}/company/", stock));
            // StreamReader reader = new StreamReader(data);
            //string s = reader.ReadToEnd();
            byte[] page = client.DownloadData(string.Format("http://stockpage.10jqka.com.cn/{0}/company/", stock));

            string content = System.Text.Encoding.UTF8.GetString(page);
            //string content = "成交额：1.62 亿元";

            Dictionary<string, string> source = new Dictionary<string, string>();
            source.Add("location", @"所属地域：");
            source.Add("industry", @"所属行业：");
            //source.Add("name", @"公司名称：");
            string[] match = { @"所属地域：", @"所属行业：", @"公司名称：" };

            foreach (String a in source.Keys)
            {
                Regex re = new Regex(source[a] + "</strong><span>(.*)</span>");
                MatchCollection matches = re.Matches(content);
                string value = matches.Count > 0 ? matches[0].Groups[1].Value : "empty";

                dic.Add(a, value);
            }

            return dic;
        }
        public static Dictionary<string, string> GetShares(string stock)
        {

            Dictionary<string, string> dic = new Dictionary<string, string>();
            WebClient client = new WebClient();
            // Stream data = client.OpenRead(string.Format("http://stockpage.10jqka.com.cn/{0}/company/", stock));
            // StreamReader reader = new StreamReader(data);
            //string s = reader.ReadToEnd();
            byte[] page = client.DownloadData(string.Format("http://stockpage.10jqka.com.cn/{0}/holder/", stock));

            string content = System.Text.Encoding.UTF8.GetString(page);
            //string content = "成交额：1.62 亿元";

            string[] match = { @"前十大流通股东累计持有：<em>(.*)</em>万股", @"累计占流通股比：<em>(.*)%</em>", @"前十大股东累计持有：<em>(.*)</em>万股", @"累计占总股本比：<em>(.*)%</em>" };

            string[] values = new string[match.Length];
            for (int i = 0; i < match.Length; i++)
            {
                Regex re = new Regex(match[i]);
                MatchCollection matches = re.Matches(content);
                values[i] = matches.Count > 0 ? matches[0].Groups[1].Value : "empty";
            }

            decimal floatshare = (Decimal.Parse(values[0]) / Decimal.Parse(values[1]) * 100);
            dic.Add("floatshare", floatshare.ToString("0"));
            dic.Add("totalshare", (Decimal.Parse(values[2]) / Decimal.Parse(values[3]) * 100).ToString("0"));
            dic.Add("top10float", values[1]);
            dic.Add("top10total", values[3]);
            int w=GetWeight(floatshare);
            dic.Add("weight", w.ToString("0"));
            dic.Add("list", string.Format("{0},{1},{2}", 500 * w, 1000 * w, 2000 * w));
            return dic;
        }

        public static InfoData BuildBasicInfo(InfoData id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            WebClient client = new WebClient();

            byte[] page = client.DownloadData(string.Format("http://stockpage.10jqka.com.cn/{0}/company/", Common.ProcessStockId(id.sid)));

            string content = System.Text.Encoding.UTF8.GetString(page);
            //string content = "成交额：1.62 亿元";

            string[] match = { @"所属地域：", @"所属行业：" };


            Regex re = new Regex(match[0] + "</strong><span>(.*)</span>");
            MatchCollection matches = re.Matches(content);
            string value = matches.Count > 0 ? matches[0].Groups[1].Value : "empty";
            id.location = value;

            Regex re1 = new Regex(match[1] + "</strong><span>(.*)</span>");
            MatchCollection matches1 = re1.Matches(content);
            string value1 = matches1.Count > 0 ? matches1[0].Groups[1].Value : "empty";
            string[] list = value1.Split('—');

            id.firstlevel = string.IsNullOrEmpty(list[0]) ? "" : list[0].Trim() ;
            id.secondlevel = string.IsNullOrEmpty(list[1]) ? "" : list[1].Trim();
            return id;
        }
        public static InfoData BuildShare(InfoData id)
        {

            Dictionary<string, string> dic = new Dictionary<string, string>();
            WebClient client = new WebClient();
            // Stream data = client.OpenRead(string.Format("http://stockpage.10jqka.com.cn/{0}/company/", stock));
            // StreamReader reader = new StreamReader(data);
            //string s = reader.ReadToEnd();
            byte[] page = client.DownloadData(string.Format("http://stockpage.10jqka.com.cn/{0}/holder/", Common.ProcessStockId(id.sid)));

            string content = System.Text.Encoding.UTF8.GetString(page);
            //string content = "成交额：1.62 亿元";

            string[] match = { @"前十大流通股东累计持有：<em>(.*)</em>万股", @"累计占流通股比：<em>(.*)%</em>", @"前十大股东累计持有：<em>(.*)</em>万股", @"累计占总股本比：<em>(.*)%</em>" };

            string[] values = new string[match.Length];
            for (int i = 0; i < match.Length; i++)
            {
                Regex re = new Regex(match[i]);
                MatchCollection matches = re.Matches(content);
                values[i] = matches.Count > 0 ? matches[0].Groups[1].Value : "empty";
            }
            for(int j=0;j<values.Length;j++) {
                values[j] = values[j] == "empty" ? "1" : values[j];
            }
            decimal floatshare = (Decimal.Parse(values[0]) / Decimal.Parse(values[1]== "empty" ? "1" : values[1]) * 100);
            id.totalshare = (Double)(Decimal.Parse(values[2]) / Decimal.Parse( values[3]) * 100);
            id.floatshare = (Double)floatshare;
            id.top10total = Decimal.Parse(values[1]);
            id.top10float = Decimal.Parse(values[3]);
            id.weight = GetWeight(floatshare);
            if (id.totalshare == 100f || id.floatshare == 100f)
            {
                id.valid = 0;
            }
            else
            {
                id.valid = 1;
            }
            return id;
        }
        public static int GetWeight(decimal val)
        {
            if (val < 100000) return 1;
            else if (val > 500000) return 4;
            else return 2;
        }


    }
}
