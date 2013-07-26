using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Common;
using log4net;

namespace DownloadData
{
    public class DataDownload
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(DataDownload));

        /// <summary>
        /// 生成csv 文件
        /// </summary>
        /// <param name="date"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        public static String DownloadDataToCsv(string stock, string date)
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
                        LOG.Debug("Downloaded: " + filePath);
                    }

                }
                else
                {
                    LOG.Debug("None: " + filePath);
                }
                
            }
            else
            {
                LOG.Debug("Skipped: " + filePath);
            }

            return filePath;
        }
        /// <summary>
        /// 生成csv 文件
        /// </summary>
        /// <param name="date"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        public static string DownloadDataToCsv(string stock, string startDate, string endDate)
        {
            if (!Directory.Exists(Constant.ROOT_FOLDER + stock)) Directory.CreateDirectory(Constant.ROOT_FOLDER + stock);

            for (DateTime dateTime = DateTime.Parse(startDate);
                 dateTime <= DateTime.Parse(endDate);
                 dateTime += TimeSpan.FromDays(1))
            {
                if (!(dateTime.Date.DayOfWeek == DayOfWeek.Saturday || dateTime.Date.DayOfWeek == DayOfWeek.Sunday))
                    //LOG.Info(toDate(dateTime.Date));
                    DownloadDataToCsv(stock, StockUtil.FormatDate(dateTime.Date));
            }

            return Constant.ROOT_FOLDER + stock;
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
