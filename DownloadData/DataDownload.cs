using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Common;


namespace DownloadData
{
    public class DataDownload
    {

        #region don't save local file
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
                        Import.ImportRawData.ImportFileByReader(stock, date, stream);
                    }
                    else
                    {
                        StockLog.Log.Debug("None: " + stock+"-"+date);
                    }

                    Random ran = new Random();
                    int RandKey = ran.Next(1000, 6000);
                    System.Threading.Thread.Sleep(RandKey);
            }
            catch
            {
                StockLog.Log.Error("Error" + stock + "-" + date);
            }


            return stock + "-" + date;
        }


        ////download from one csv file
        //public static void DownloadDataToCsvByReaderFromCsvFile(string csvFile, string startDate, string endDate)
        //{
        //    List<string> list = Common.StockUtil.ParseListFromCsvFile(csvFile);
        //    foreach (string stock in list)
        //    {
        //        if (!Directory.Exists(Constant.ROOT_FOLDER + stock)) Directory.CreateDirectory(Constant.ROOT_FOLDER + stock);

        //        for (DateTime dateTime = DateTime.Parse(startDate);
        //             dateTime <= DateTime.Parse(endDate);
        //             dateTime += TimeSpan.FromDays(1))
        //        {
        //            if (!(dateTime.Date.DayOfWeek == DayOfWeek.Saturday || dateTime.Date.DayOfWeek == DayOfWeek.Sunday))
        //                DownloadDataToCsvByReader(stock, StockUtil.FormatDate(dateTime.Date));
        //        }
        //    }
        //}

        public static string DownloadDataToCsvByReader(string stock, string startDate, string endDate)
        {

            for (DateTime dateTime = DateTime.Parse(startDate);
                 dateTime <= DateTime.Parse(endDate);
                 dateTime += TimeSpan.FromDays(1))
            {
                if (!(dateTime.Date.DayOfWeek == DayOfWeek.Saturday || dateTime.Date.DayOfWeek == DayOfWeek.Sunday))
                    //LOG.Info(toDate(dateTime.Date));

                    DownloadDataToCsvByReader(stock, StockUtil.FormatDate(dateTime.Date));
            }

            return Constant.ROOT_FOLDER + stock;
        }
        #endregion
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
