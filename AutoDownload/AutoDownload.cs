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

            //StringBuilder sb = new StringBuilder();
            if (StockUtil.FormatDateToNumber(File.GetLastAccessTime(Constant.ROOT_FOLDER + @"\file.txt")) < StockUtil.FormatDateToNumber(DateTime.Now))
            {
                File.Delete(Constant.ROOT_FOLDER + @"\file.txt");
            }
            if (!File.Exists(Constant.ROOT_FOLDER + @"\file.txt")) File.Create(Constant.ROOT_FOLDER + @"\file.txt");
            string fileList = FileUtil.ReadFile(Constant.ROOT_FOLDER+@"\file.txt");
            List<string> list = fileList.Split(',').ToList<string>();
            foreach (String f in Directory.EnumerateDirectories(Constant.ROOT_FOLDER))
            {
               
                string startDate = StockUtil.ReadUpdateFile(f);
                string endDate=StockUtil.FormatDate(DateTime.Now);

                //string startDate = "2013-02-01";
                //string endDate="2013-07-01";

                string stock = f.Substring(f.LastIndexOf(@"\")+1);
                if (!list.Contains<string>(stock))
                {
                    DataDownload.DownloadDataToCsv(stock, startDate, endDate);
                    StockUtil.UpdateDownloadTimeStamp(f, endDate);

                    //sb.Append(stock+",");

                    File.AppendAllText(Constant.ROOT_FOLDER + @"\file.txt", stock+",");

                    LOG.Debug(stock + " updated "+startDate + " "+endDate);

                }
            }

           // FileUtil.WriteFile(Constant.ROOT_FOLDER + @"\file.txt",sb.ToString());
        }
    }
}
