using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using SotckAnalyzer.data;
using SotckAnalyzer.reader;
using System.IO;

namespace SotckAnalyzer.analyzer
{
    public class BigDealAnalyzer
    {
        public StockData stockData;
        public BigDealAnalyzer(StockData stockData)
        {
            this.stockData = stockData;
        }
        public static string Analyze(string stock, string startDate, string endDate)
        {
            return Analyze(stock, Constant.BIG_DEAL, startDate, StockUtil.FormatDate(DateTime.Now));

        }
         public static string Analyze(StockData stockData,string filter)
        {

            string analyzePath;
            List<DailyData> dds = stockData.Dataset;
            StringBuilder str = new StringBuilder();
            str.Append("date,bigBuyShare,bigSellShare,toalShare,bigBuyMoney,bigSellMoney,toalMoney,Open,Close,Average,Hightest,WhenHighest,Lowest,WhenLowest,BigBuyShareRate,BigSellShareRate,BigBuyMoneyRate,BigSellMoneyRate\n");
            foreach (DailyData ds in dds)
            {

                BigDeal fd = new BigDeal(ds, filter);
                fd.Analye();
                str.Append(StockUtil.FormatDate(fd.set.date) + ",");
                str.Append(fd.TotalBuyShareByBigDeal + ",");
                str.Append(fd.TotalSellShareByBigDeal + ",");
                str.Append(fd.set.TotalShare + ",");
                str.Append(fd.TotalBuyMoneyByBigDeal + ",");
                str.Append(fd.TotalSellMoneyByBigDeal + ",");
                str.Append(fd.set.TotalMoney + ",");
                str.Append(fd.set.OpenPrice + ",");
                str.Append(fd.set.ClosePrice + ",");
                str.Append(fd.set.Average + ",");
                str.Append(fd.set.HighestPrice + ",");
                str.Append(StockUtil.FormatTime(fd.set.TimeWhenHighest) + ",");
                str.Append(fd.set.LowestPrice + ",");
                str.Append(StockUtil.FormatTime(fd.set.TimeWhenLowest) + ",");
                str.Append(fd.RateOfBuyShareByTotal + ",");
                str.Append(fd.RateOfSellShareByTotal + ",");
                str.Append(fd.RateOfBuyMoneyByTotal + ",");
                str.Append(fd.RateOfSellMoneyByTotal + ",");
                str.Append("\n");

            }

            analyzePath = Constant.ANALYZE_FOLDER + stockData.stock + "_" + stockData.startDate + "_" + stockData.endDate + "_" + filter + ".csv";

            if (File.Exists(analyzePath))
            {
                File.Delete(analyzePath);
            }
            using (StreamWriter outfile = new StreamWriter(Constant.ANALYZE_FOLDER + stockData.stock + "_" + stockData.startDate + "_" + stockData.endDate + "_" + filter + ".csv"))
            {
                outfile.Write(str);
                Console.WriteLine("Analyzed: " + Constant.ANALYZE_FOLDER + stockData.stock + "_" + stockData.startDate + "_" + stockData.endDate + "_" + filter + ".csv");
            }

            return analyzePath;

        }

        public static string Analyze(string stock, string filter, string startDate,string endDate)
        {
            stock = StockUtil.FormatStock(stock);
            string analyzePath;
            List<DailyData> dds = Csv.ReadCsv(stock, startDate, endDate,true);
            StringBuilder str = new StringBuilder();
            str.Append("date,bigBuyShare,bigSellShare,toalShare,bigBuyMoney,bigSellMoney,toalMoney,Open,Close,Average,Hightest,WhenHighest,Lowest,WhenLowest,BigBuyShareRate,BigSellShareRate,BigBuyMoneyRate,BigSellMoneyRate\n");
            foreach (DailyData ds in dds)
            {

                BigDeal fd = new BigDeal(ds, filter);
                fd.Analye();
                str.Append(StockUtil.FormatDate(fd.set.date) + ",");
                str.Append(fd.TotalBuyShareByBigDeal + ",");
                str.Append(fd.TotalSellShareByBigDeal + ",");
                str.Append(fd.set.TotalShare + ",");
                str.Append(fd.TotalBuyMoneyByBigDeal + ",");
                str.Append(fd.TotalSellMoneyByBigDeal + ",");
                str.Append(fd.set.TotalMoney + ",");
                str.Append(fd.set.OpenPrice + ",");
                str.Append(fd.set.ClosePrice + ",");
                str.Append(fd.set.Average + ",");
                str.Append(fd.set.HighestPrice + ",");
                str.Append(StockUtil.FormatTime(fd.set.TimeWhenHighest) + ",");
                str.Append(fd.set.LowestPrice + ",");
                str.Append(StockUtil.FormatTime(fd.set.TimeWhenLowest) + ",");
                str.Append(fd.RateOfBuyShareByTotal + ",");
                str.Append(fd.RateOfSellShareByTotal + ",");
                str.Append(fd.RateOfBuyMoneyByTotal + ",");
                str.Append(fd.RateOfSellMoneyByTotal + ",");
                str.Append("\n");

            }

            analyzePath = Constant.ANALYZE_FOLDER + stock + "_" + startDate + "_" + endDate + "_" + filter + ".csv";

            if (File.Exists(analyzePath))
            {
                File.Delete(analyzePath);
            }
            using (StreamWriter outfile = new StreamWriter(Constant.ANALYZE_FOLDER + stock + "_" + startDate + "_" + endDate + "_" + filter + ".csv"))
            {
                outfile.Write(str);
                Console.WriteLine("Analyzed: " + Constant.ANALYZE_FOLDER + stock + "_" + startDate + "_" + endDate + "_" + filter + ".csv");
            }

            return analyzePath;

        }
            
    }
}
