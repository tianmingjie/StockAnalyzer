using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using SotckAnalyzer;

namespace AutoAnalyzer
{
    class AutoAnalyzer
    {
        public static void Main(string[] args)
        {
            try
            {
                foreach (StockInfo stockInfo in StockUtil.AnalyzeList.Reverse<StockInfo>())
                {
                    if (stockInfo.isAnalyze)
                    {
                        Analyzer.Analyze(stockInfo.stock, "2013-02-01", StockUtil.FormatDate(DateTime.Now), stockInfo.filter);
                        Console.WriteLine(stockInfo.stock + " is done");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
