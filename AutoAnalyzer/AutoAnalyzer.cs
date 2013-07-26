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
                foreach (string stock in StockUtil.AnalyzeList)
                {
                    Analyzer.Analyze(stock, "2013-02-01", StockUtil.FormatDate(DateTime.Now), "500");
                    Console.WriteLine(stock + " is done");
                }
            }catch(Exception e)
            {
            }
        }
    }
}
