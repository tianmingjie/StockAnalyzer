using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using big;
using big.entity;
using Common;
using LumenWorks.Framework.IO.Csv;

namespace UpdateBigDeal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DownloadData("sz000153", new DateTime(2015, 4, 3).ToString("yyyy-MM-dd"), new DateTime(2015, 4, 3).ToString("yyyy-MM-dd"));
        }

        public static string DownloadData(string stock, string startDate, string endDate)
        {
            for (DateTime dateTime = DateTime.Parse(startDate);
                 dateTime <= DateTime.Parse(endDate);
                 dateTime += TimeSpan.FromDays(1))
            {
                if (!(dateTime.Date.DayOfWeek == DayOfWeek.Saturday || dateTime.Date.DayOfWeek == DayOfWeek.Sunday))
                    //LOG.Info(toDate(dateTime.Date));

                    DownloadDataToCsvByReader(stock, StockUtil.FormatDate(dateTime.Date));
            }

            return stock;
        }


        public static String DownloadDataToCsvByReader(string stock, string date)
        {
            try
            {
                WebClient client = new WebClient();
                string url = "http://market.finance.sina.com.cn/downxls.php?date=" + date + "&symbol=" + stock;
                string data = System.Text.Encoding.GetEncoding(936).GetString(client.DownloadData(url));
                data = ReplaceChinese(data);

                if (data != null && data.Length > 2048)
                {
                    TextReader stream = new StringReader(data);
                    ImportFileByReader(stock, date, stream);
                }
                else
                {
                    StockLog.Log.Debug("None: " + stock + "-" + date);
                }
            }
            catch
            {
                StockLog.Log.Error("Error" + stock + "-" + date);
            }


            return stock + "-" + date;
        }

        public static void ImportFileByReader(string sid, string date, TextReader reader)
        {
            //string sid = file.Substring(file.LastIndexOf("\\") + 1);
            StockLog.Log.Debug(sid + " start ");
            DateTime beginning = DateTime.Now;
            //BizApi.CreateDataTable(sid);
            string file = sid + "_" + date;
            try
            {
                //decimal weight = BizApi.QueryWeight(sid);
                decimal[] extractlist = BizApi.QueryExtractList(sid);
                DateTime lastupdate = DateTime.MinValue;//BizApi.QueryExtractLastUpdate(sid);

                List<BasicData> list = ReadCsvByReader(sid, date, reader, extractlist, lastupdate);

                foreach (BasicData bd in list)
                {
                    //Console.WriteLine(bd.time+" "+bd.sellshare);
                    //BizApi.InsertBasicData(bd);
                }
                TimeSpan end = DateTime.Now - beginning;

                StockLog.Log.Debug(file + " complete at " + end);
            }
            catch
            {
                StockLog.Log.Error(file + " import fail");
            }

        }

        public static List<BasicData> ReadCsvByReader(string sid, string date, TextReader stream, decimal[] bigs, DateTime lastupdate)
        {
            int index = 0;

            //bigs=null;
            //bigs=new decimal[1];
            //bigs[0]=2000M;
            List<BasicData> list = new List<BasicData>();
            BasicData[] array = new BasicData[bigs.Length];

            if (date.Length != 10) throw new Exception("time format is wrong -" + date);
            string time = date;
            DateTime t = new DateTime(Int32.Parse(time.Substring(0, 4)), Int32.Parse(time.Substring(5, 2)), Int32.Parse(time.Substring(8, 2)));

            //处理过了就忽略
            if (t < lastupdate.AddDays(1)) return null;

            Dictionary<decimal, string> haha = new Dictionary<decimal, string>();

            for (int j = 0; j < bigs.Length; j++)
            {
                array[j] = new BasicData();
                array[j].big = (int)bigs[j];
                array[j].time = t;
                array[j].sid = sid;
                array[j].c_type = "d";
                haha[bigs[j]] = "";
            }

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(stream, true))
            {  //颠倒记录
                IEnumerable<string[]> hi = csv.Reverse<string[]>();

                decimal current;

                decimal open = 0, close = 0, high = 0, low = 0;
                string extstring = "";


                foreach (String[] record in hi)
                {
                    string price_str = record[1];
                    string share_str = record[3];
                    string type_str = record[5];
                    string time_str = record[0];
                    try
                    {

                        Decimal price = Decimal.Parse(price_str.IndexOf("\0") > 0 ? price_str.Remove(price_str.IndexOf("\0")) : price_str);
                        Double share = Double.Parse(share_str.IndexOf("\0") > 0 ? share_str.Remove(share_str.IndexOf("\0")) : share_str);

                        //count close,open,low,high
                        current = price;
                        if (index == 0)
                        {
                            open = current;
                            high = current;
                            low = current;
                        }
                        if (high < current) high = current;
                        if (low > current) low = current;
                        close = current;
                        index++;

                        for (int k = 0; k < bigs.Length; k++)
                        {

                            int fieldCount = csv.FieldCount;
                            string[] headers = csv.GetFieldHeaders();

                            array[k].open = open;
                            array[k].close = close;
                            array[k].high = high;
                            array[k].low = low;
                            array[k].totalshare += share;
                            array[k].totalmoney += Decimal.Multiply(price, (Decimal)share);

                            if (Int32.Parse(share_str) >= bigs[k])
                            {
                               //11位编码编码，
                                //第一位买单1卖单0,后三位股数（百手）
                                //后三位时间数（相对于9：25的分钟数）
                                //后4位位股价（1正0负，后两位是相对开盘的百分比
                                Console.WriteLine(time_str + ": " + share + " " + type_str);
                                if (type_str == "S")
                                {
                                    haha[bigs[k]] += "-" + p(share) + p1(time_str) + p2(current, open);
                                    array[k].sellshare += share;
                                    array[k].sellmoney += Decimal.Multiply(price, (Decimal)share);
                                }
                                if (type_str == "B")
                                {
                                    haha[bigs[k]] += "+" + p(share) + p1(time_str) + p2(current, open);
                                    array[k].buyshare += share;
                                    array[k].buymoney += Decimal.Multiply(price, (Decimal)share);
                                }

                                array[k].extstring1 = haha[bigs[k]];
                            }
                        }
                    }
                    catch
                    {
                        StockLog.Log.Error(sid + " import fail");
                    }
                }
            }
            for (int j = 0; j < bigs.Length; j++)
            {
                list.Add(array[j]);
            }
            return list;
        }

        
        /// <summary>
        /// 股数编码，3位，百手
        /// </summary>
        /// <param name="bb"></param>
        /// <returns></returns>
        public static string p(double bb)
        {
            int aa = (int)Math.Floor(bb / 100);

            if (aa < 10) return "00" + aa.ToString();
            else if (aa < 100) return "0" + aa.ToString();
            else return aa.ToString();

        }

        /// <summary>
        /// 处理股价百分比，4位第一位是1正，0负，后面三位相对于开盘价的百分比
        /// </summary>
        /// <param name="open"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static string p2(decimal open, decimal current)
        {
            decimal cc = (current - open) / open * 1000;

            int bb = (int)Math.Floor(cc);
           

            if (bb > 0)
            {
                if (bb < 10) return "100" + bb;
                if (bb >= 10 &&bb<100) return "10"+ bb;
                return "1"+bb;

            }
            else
            {
                if (-bb < 10) return "000" +(-bb);
                if (-bb >= 10 && -bb < 100) return "00"+(-bb);
                return "0" + (-bb);
            }
        }

        /// <summary>
        /// 处理时间，也是3位，相对于9：25分的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string p1(string time)
        {
            TimeSpan ts = TimeSpan.Parse(time);
            TimeSpan start = new TimeSpan(9, 25, 00);
            TimeSpan noon = new TimeSpan(11, 30, 01);
            TimeSpan noon1 = new TimeSpan(13, 00, 00);
            TimeSpan end = new TimeSpan(15, 00, 01);
            if (ts < noon)
            {
                int a = (ts - start).Minutes;
                return p((double)a * 100);
            }
            else
            {
                double a1 = (ts - noon1).TotalMinutes + 125;
                return p(a1 * 100);
            }
        }

        /// <summary>
        /// 处理中文字符，减少文件大小
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ReplaceChinese(string data)
        {
            data = data.Replace('\t', ',');
            data = data.Replace("买盘", "B");
            data = data.Replace("卖盘", "S");
            data = data.Replace("中性盘", "Z");
            data = data.Replace("--", "0");
            data = data.Replace("成交时间", "time");
            data = data.Replace("成交价", "price");
            data = data.Replace("价格变动", "change");
            data = data.Replace("成交量(手)", "share");
            data = data.Replace("成交额(元)", "money");
            data = data.Replace("性质", "type");
            return data;
        }
    }
}
