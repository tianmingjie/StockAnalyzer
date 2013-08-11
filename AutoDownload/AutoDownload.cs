using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.IO;
using DownloadData;
using log4net;
using System.Threading;

namespace AutoDownload
{
    public class AutoDownload
    {
        public static void Main(string[] args)
        {
            try
            {
                Auto();
            } catch(Exception e){
                StockLog.Log.Error(e);
            }
        }

        public static void Auto()
        {
            
            List<StockInfo> total = StockUtil.StockList;
            int count = total.Count;
            int range = total.Count / 10;
            List<StockInfo> list1 = StockUtil.StockList.GetRange(0, range);
            List<StockInfo> list2 = total.GetRange(range + 1, range);
            List<StockInfo> list3 = total.GetRange((range) * 2 + 1, range);
            List<StockInfo> list4 = total.GetRange((StockUtil.StockList.Count / 10) * 3 + 1, range);
            List<StockInfo> list5 = total.GetRange((range) * 4 + 1, range);
            List<StockInfo> list6 = total.GetRange((range) * 5 + 1, range);
            List<StockInfo> list7 = total.GetRange((range) * 6 + 1, range);
            List<StockInfo> list8 = total.GetRange((range) * 7 + 1, range);
            List<StockInfo> list9 = total.GetRange((range) * 8 + 1, range);
            List<StockInfo> list10 = total.GetRange((range) * 9 + 1, total.Count % 10);

            Thread thread1 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread2 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread3 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread4 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread5 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread6 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread7 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread8 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread9 = new Thread(new ParameterizedThreadStart(Process));
            Thread thread10 = new Thread(new ParameterizedThreadStart(Process));
            thread1.Start(list1);
            thread2.Start(list2);
            thread3.Start(list3);
            thread4.Start(list4);
            thread5.Start(list5);
            thread6.Start(list6);
            thread7.Start(list7);
            thread8.Start(list8);
            thread9.Start(list9);
            thread10.Start(list10);
        }

        public static void Process(Object o)
        {

            List<StockInfo> list = (List<StockInfo>)o;
            foreach (StockInfo stock in list)
            {
                string path = Constant.ROOT_FOLDER + @"\" + stock;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string startDate;
                if (Constant.DOWNLOAD_ALL.Equals("0"))
                {
                    startDate = StockUtil.ReadUpdateFile(stock.stock);
                }
                else
                {
                    startDate = "2012-09-01";
                }
                string endDate = StockUtil.FormatDate(DateTime.Now);

                //string startDate = "2012-09-01";
                // string endDate="2013-07-28";

                //string stock = f.Substring(f.LastIndexOf(@"\")+1);
                //if (!list.Contains<string>(stock))
                //{
                DataDownload.DownloadDataToCsv(stock.stock, startDate, endDate);
                StockUtil.UpdateDownloadTimeStamp(stock.stock, endDate);

               // File.AppendAllText(Constant.AUTO_DOWNLOAD_FILE, stock + ",");

                StockLog.Log.Info(stock.stock + " updated " + startDate + " " + endDate);

                //}
            }
        }
    }


}
