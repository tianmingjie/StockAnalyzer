using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DownloadData
{
    public class DownloadModule:IStockModule
    {
        public bool Execute(StockInfo info)
        {
            string startDate;
            if (Constant.DOWNLOAD_ALL.Equals("0"))
            {
                startDate = StockUtil.ReadUpdateFile(info.stock);
            }
            else
            {
                startDate = "2014-01-01";
            }
            string endDate = StockUtil.FormatDate(DateTime.Now);
            DataDownload.DownloadDataToCsv(info.stock, startDate, endDate);
            StockUtil.UpdateDownloadTimeStamp(info.stock, endDate);
            StockLog.Log.Info(info.stock + " updated " + startDate + " " + endDate);
            
            return true;
        }
    }
}
