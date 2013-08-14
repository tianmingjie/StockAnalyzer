using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using SotckAnalyzer;
using Modulizer;

namespace AutoAnalyzer
{
    class AutoAnalyzer
    {
        public static void Main(string[] args)
        {
            ModulizerMain modulizer = new ModulizerMain();
            String stock = "" ;
            try
            {
                
                foreach (StockInfo stockInfo in StockUtil.AnalyzeList.Reverse<StockInfo>())
                {
                    stock= stockInfo.stock;
                    StockLog.Log.Info(stock + " start...");
                    
                    if (stockInfo.isAnalyze)
                    {
                        //Analyzer.Analyze(stock,Constant.ANALYZE_START_DATE, StockUtil.FormatDate(DateTime.Now), stockInfo.filter);
                        modulizer.Execute(stockInfo);
                        StockLog.Log.Info(stock + " is done");
                    }
                }
            }
            catch (Exception e)
            {
                StockLog.Log.Info(stock + " fail to analyze",e);
            }
        }
    }
}
