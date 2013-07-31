using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.IO;
using DownloadData;
using log4net;

namespace AutoDownload
{
    public class AutoDownload
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(AutoDownload));

        public static void Main(string[] args)
        {

                log4net.Config.XmlConfigurator.Configure();

            try
            {
                //
                if (StockUtil.FormatDateToNumber(File.GetLastAccessTime(Constant.AUTO_DOWNLOAD_FILE))+1 < StockUtil.FormatDateToNumber(DateTime.Now))
                {
                    System.IO.File.WriteAllText(Constant.AUTO_DOWNLOAD_FILE, string.Empty);
                }
                //get updated list
                List<string> list = StockUtil.UpdateList;

                foreach (string stock in StockUtil.StockList)
                {
                    //foreach (String f in Directory.EnumerateDirectories(Constant.ROOT_FOLDER))
                    //{
                    string path = Constant.ROOT_FOLDER + @"\" + stock;
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    //string startDate = StockUtil.ReadUpdateFile(stock);
                    string endDate = StockUtil.FormatDate(DateTime.Now);

                   string startDate = "2012-09-01";
                   // string endDate="2013-07-28";

                    //string stock = f.Substring(f.LastIndexOf(@"\")+1);
                    if (!list.Contains<string>(stock))
                    {
                        DataDownload.DownloadDataToCsv(stock, startDate, endDate);
                        StockUtil.UpdateDownloadTimeStamp(stock, endDate);

                        File.AppendAllText(Constant.AUTO_DOWNLOAD_FILE, stock + ",");

                        LOG.Info(stock + " updated " + startDate + " " + endDate);

                    }
                }
            }
            catch (Exception e)
            {
                LOG.Error(e);
            }
        }
    }
}
