using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace SotckAnalyzer
{
    public class AnalyzeModule :IStockModule
    {
        public bool Execute(ModuleInfo info)
        {
            Analyzer.Analyze(info.stock, Constant.ANALYZE_START_DATE, StockUtil.FormatDate(DateTime.Now), info.filter);
            return true;
        }
    }
}
