using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using big;
using big.entity;
using HtmlAgilityPack;

namespace InfoExt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InfoExtData ifd = QueryInfoExt("000809");

            Console.WriteLine(ifd.meiguweifenpeilirun);
        }

        public static  InfoExtData QueryInfoExt(string sid)
        {
            Console.WriteLine("start");
            string url = "http://stockpage.10jqka.com.cn/" + BizCommon.ProcessStockId(sid);
            WebClient client = new WebClient();
            byte[] gg = client.DownloadData(url);
            string haha = System.Text.Encoding.UTF8.GetString(gg);
            TextReader stream = new StringReader(haha);
            HtmlDocument document = new HtmlDocument();
            document.Load(stream);

            HtmlNode rootNode = document.DocumentNode;
            HtmlNodeCollection c = rootNode.SelectNodes("//dl[@class='company_details']");


            string innerString = c[0].InnerText;
            innerString = innerString.Replace(System.Environment.NewLine, "");


            innerString = innerString.Replace("\t", " ");


            innerString = System.Text.RegularExpressions.Regex.Replace(innerString, @"\s{1,}", " ", RegexOptions.IgnoreCase);
            innerString = innerString.Replace("： ", ":");
            string[] array = innerString.Split(' ');



            InfoExtData ifd = new InfoExtData();
            ifd.sid = sid;

            foreach (string item in array)
            {
                if (item.Trim().Length > 0 && item.IndexOf(":") > 0)
                {
                    string[] bb = item.Split(':');
                    switch (bb[0].Trim())
                    {
                        case "总股本": //亿股
                            ifd.zongguben = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "流通股"://亿股
                            ifd.liutonggu = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "营业收入增长率":
                            ifd.yingyeshouruzengzhanglv = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "营业收入": //亿元
                            ifd.yingyeshouru = decimal.Parse(bb[1].Substring(0, bb[1].Length - 2));
                            break;
                        case "净利润"://亿元
                            ifd.jinglirun = decimal.Parse(bb[1].Substring(0, bb[1].Length - 2));
                            break;
                        case "净利润增长率":
                            ifd.jinglirunzengzhanglv = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "每股收益"://元
                            ifd.meigujingzichan= decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "每股净资产"://元
                            ifd.meigujingzichan = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "净资产收益率":
                            ifd.jingzichanshouyilv = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "每股现金流"://元
                            ifd.meiguxianjinliu = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "每股公积金"://元
                            ifd.meigugongjijin = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                        case "每股未分配利润"://元
                            ifd.meiguweifenpeilirun = decimal.Parse(bb[1].Substring(0, bb[1].Length - 1));
                            break;
                    }


                }
            }

            return ifd;
        }

    }
}
