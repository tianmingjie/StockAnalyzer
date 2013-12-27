using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace SotckAnalyzer
{
    public class AnalyzeModule :IStockModule
    {
        public bool Execute(StockInfo info)
        {
            string startDate = info.startDate.Equals("") ? Constant.ANALYZE_START_DATE : info.startDate;
            //string endDate = info.endDate.Equals("") ? StockUtil.FormatDate(DateTime.Now) : info.endDate;
            Analyzer.Analyze(info.stock, startDate, StockUtil.FormatDate(DateTime.Now), info.filterList);
            return true;
        }
    }
}
