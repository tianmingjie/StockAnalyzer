using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.IO;
using DownloadData;
using log4net;
using System.Threading;
using big.entity;
using big;

namespace AutoDownload
{
    public class AutoDownload
    {
        public static void Main(string[] args)
        {
            try
            {

                Auto(Constant.DOWNLOAD_THREAD_NUMBER);

            } catch(Exception e){
                StockLog.Log.Error(e);
            }
        }

        public static void Auto(int totalThread = 2)
        {

            //List<InfoData> total = new List<InfoData>();

            //total.Add(new InfoData() { sid = "sh600009" });
            //total.Add(new InfoData() { sid = "sh600010" });
            List<InfoData> total =BizApi.QueryInfoAll();
            int count = total.Count;

            //int totalThread = 10;
            int range = total.Count / totalThread;
            int start = -1;

            for (int i = 0; i < totalThread; i++)
            {
                new Thread(new ParameterizedThreadStart(Process)).Start(total.GetRange(start + 1, range));
                start = range * i;
            }
        }
        public static void Process(Object o)
        {

            List<InfoData> list = (List<InfoData>)o;
            foreach (InfoData id in list)
            {
                DownloadSingle(id);
            }
        }

        public static void DownloadSingle(InfoData id)
        {
            DownloadSingle(id.sid);
        }


        public static void DownloadSingle(string sid)
        {

            string startDate = BizApi.QueryExtractLastUpdate(sid).AddDays(1).ToString("yyyy-MM-dd");
            string endDate = StockUtil.FormatDate(DateTime.Now);

            DataDownload.DownloadDataToCsvByReader(sid, startDate, endDate);
            if(!Constant.CLEAN) StockUtil.UpdateDownloadTimeStamp(sid, endDate);
            StockLog.Log.Info(sid + " updated " + startDate + " " + endDate);
        }
    }


}
