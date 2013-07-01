using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using SotckAnalyzer.data;
using SotckAnalyzer.analyzer;
using Common;

namespace SotckAnalyzer
{
    static class Program
    {

        //static string Constant.ROOT_FOLDER = Constant.ROOT_FOLDER;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            string stock = args[0];// "600200";

            string startDate = args[1]; //"2012-09-01";
            string endDate = args[2]; //;"2013-07-01";
            string filter = args[3];// "1000";
            stock = StockUtil.FormatStock(stock);
            List<DailyDataSet> dds = CsvAnalyzer.ReadCsv(stock, startDate, endDate);
            StringBuilder str = new StringBuilder();
            str.Append("date,bigBuyShare,bigSellShare,toalShare,bigBuyMoney,bigSellMoney,toalMoney,Open,Close,Average,Hightest,WhenHighest,Lowest,WhenLowest,BigBuyShareRate,BigSellShareRate,BigBuyMoneyRate,BigSellMoneyRate\n");
            foreach (DailyDataSet ds in dds)
            {

                FilterData fd = new FilterData(ds, filter);
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

            string analyzePath = Constant.ANALYZE_FOLDER + stock + "_" + startDate + "_" + endDate + "_" + filter + ".csv";

            if (File.Exists(analyzePath))
            {
                File.Delete(analyzePath);
            }
            using (StreamWriter outfile = new StreamWriter(Constant.ANALYZE_FOLDER + stock + "_" + startDate+"_"+ endDate+"_"+filter+ ".csv"))
            {
                outfile.Write(str);
                Console.WriteLine("Analyzed: " + Constant.ANALYZE_FOLDER + stock + "_" + startDate + "_" + endDate + "_" + filter + ".csv");
            }

            
        }

    }
}
