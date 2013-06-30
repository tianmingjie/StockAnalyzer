using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using SotckAnalyzer.util;

namespace SotckAnalyzer.data
{
    public class DataDownload
    {


        /// <summary>
        /// 生成所有的股票的
        /// </summary>
        public static void generateAll()
        {
            string stock = "sh600036";

            DateTime startDate = new DateTime(2013, 6, 01);
            DateTime stopDate = new DateTime(2013, 6, 28);
            int interval = 1;

            //上海股票
            for (int i = 600000; i < 602000; i++)
            {
                stock = "sh" + i;
                //Console.WriteLine(stock.Substring(2));
                //return;
                for (DateTime dateTime = startDate;
                     dateTime < stopDate;
                     dateTime += TimeSpan.FromDays(interval))
                {
                    if (!(dateTime.Date.DayOfWeek == DayOfWeek.Saturday || dateTime.Date.DayOfWeek == DayOfWeek.Sunday))
                        //Console.WriteLine(toDate(dateTime.Date));
                        generateFile(stock, StockUtil.FormatDate(dateTime.Date));
                }
            }
            return;
        }


        public static bool generateFile(string stock, string date)
        {
            bool isDone = false;

            string id = stock;// stock.Substring(2);

            string dir = Constant.ROOT_FOLDER + @"\" + id;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            try
            {

                WebClient client = new WebClient();
                string url = "http://market.finance.sina.com.cn/downxls.php?date=" + date + "&symbol=" + stock;
                byte[] data = client.DownloadData(url);


                // Writes a block of bytes to this stream using data from
                // a byte array.
                if (data != null && data.Length > 2048)
                {
                    System.IO.FileStream _FileStream =
           new System.IO.FileStream(Constant.ROOT_FOLDER + id + @"\" + stock + "." + date + ".xls", System.IO.FileMode.Create,
                                    System.IO.FileAccess.Write);



                    _FileStream.Write(data, 0, data.Length);

                    // close file stream
                    _FileStream.Close();
                }
                isDone = true;

                if (new DirectoryInfo(Constant.ROOT_FOLDER + id).GetFiles().Length == 0)
                {
                    Directory.Delete(Constant.ROOT_FOLDER + id);
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
            }

            return isDone;
        }

        /// <summary>
        /// 处理成byte[]
        /// </summary>
        /// <param name="date"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        public static byte[] downloadData(string date, string stock)
        {
            WebClient client = new WebClient();
            string url = "http://market.finance.sina.com.cn/downxls.php?date=" + date + "&symbol=" + stock;
            byte[] data = client.DownloadData(url);

            return data;
        }

        /// <summary>
        /// 生成csv 文件
        /// </summary>
        /// <param name="date"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        public static bool downloadDataToCsv(string stock, string date)
        {

            if (!Directory.Exists(Constant.ROOT_FOLDER + stock)) Directory.CreateDirectory(Constant.ROOT_FOLDER + stock);

            string filePath = Constant.ROOT_FOLDER + stock + @"\" + stock + "_" + date + ".csv";
            if (!File.Exists(filePath))
            {
                WebClient client = new WebClient();
                string url = "http://market.finance.sina.com.cn/downxls.php?date=" + date + "&symbol=" + stock;
                string data = System.Text.Encoding.GetEncoding(936).GetString(client.DownloadData(url));
                data = ReplaceChinese(data);

                if (data != null && data.Length > 2048)
                {

                    using (StreamWriter outfile = new StreamWriter(Constant.ROOT_FOLDER + stock + @"\" + stock + "_" + date + ".csv"))
                    {
                        outfile.Write(data.ToString());
                    }

                }
                Console.WriteLine("Downloaded: " + filePath);
            }
            else
            {
                Console.WriteLine("Download Skipped: " + filePath);
            }

            return true;
        }
        /// <summary>
        /// 生成csv 文件
        /// </summary>
        /// <param name="date"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        public static bool downloadDataToCsv(string stock, string startDate, string endDate)
        {
            if (!Directory.Exists(Constant.ROOT_FOLDER + stock)) Directory.CreateDirectory(Constant.ROOT_FOLDER + stock);

            for (DateTime dateTime = DateTime.Parse(startDate);
                 dateTime < DateTime.Parse(endDate);
                 dateTime += TimeSpan.FromDays(1))
            {
                if (!(dateTime.Date.DayOfWeek == DayOfWeek.Saturday || dateTime.Date.DayOfWeek == DayOfWeek.Sunday))
                    //Console.WriteLine(toDate(dateTime.Date));
                    downloadDataToCsv(stock, StockUtil.FormatDate(dateTime.Date));
            }

            return true;
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
