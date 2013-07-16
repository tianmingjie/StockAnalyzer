using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using SotckAnalyzer.data;
using SotckAnalyzer.reader;
using Common;
using SotckAnalyzer.analyzer;
using Spring.Context;
using Spring.Context.Support;
using log4net;

namespace SotckAnalyzer
{
    static class Program
    {
         private static readonly ILog LOG = LogManager.GetLogger(typeof(Program));

        //static string Constant.ROOT_FOLDER = Constant.ROOT_FOLDER;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {


           //try
           // {
                IApplicationContext ctx = ContextRegistry.GetContext();

                StockData data1 = (StockData)ctx.GetObject("StockData");
                // data.money = 1;
                Console.WriteLine(data1.EntryDataList.Count);

                BigDealData bdd = (BigDealData)ctx.GetObject("BigDealData");
                //List<EntryData> l=bdd.EntryDataList;
                Console.WriteLine(bdd.TotalShare);

                //RateData rate = (RateData)ctx.GetObject("RateData");
                //Console.WriteLine(rate.RateOfShare2Total);

                RangeData range = (RangeData)ctx.GetObject("RangeData");

                List<FilterData> ff = range.Analyze();
                foreach (FilterData e in ff)
                {
                    Console.WriteLine(e.EntryDataList.Count+" "+e.StartTime+" "+e.EndTime);
                }
                
            //    List<FilterData> fl = new List<FilterData>();

            //    long interval = 3600 * 24;
                
            //foreach(EntryData e in bdd.EntryDataList){
            //    //if (e.time < bdd.StartTime.AddDays(30))
            //    //{
            //        Printer.PrintEntryData(e);
            //    //}
           // }
                

            //}
            //catch (Exception e)
            //{
            //    LOG.Error("Movie Finder is broken.", e);
            //}
            //finally
            //{
            //    Console.WriteLine();
            //}

            //string stock = args[0];// "600200";

            //string startDate = args[1]; //"2012-09-01";
            //string endDate = args[2]; //;"2013-07-01";
            //string filter = args[3];// "1000";

           // string stock = "002691";

           // string startDate = "2013-06-27";
           // string endDate = "2013-07-01";
           // string filter = "200";

           // stock = StockUtil.FormatStock(stock);

           //BigDealAnalyzer.Analyze(stock, filter, startDate, endDate);

           //List<DailyData> dds = Csv.ReadCsv(stock, startDate, endDate,true);

           //StringBuilder str = new StringBuilder();
           ////str.Append("date,bigBuyShare,bigSellShare,toalShare,bigBuyMoney,bigSellMoney,toalMoney,Open,Close,Average,Hightest,WhenHighest,Lowest,WhenLowest,BigBuyShareRate,BigSellShareRate,BigBuyMoneyRate,BigSellMoneyRate\n");
           //foreach (DailyData ds in dds)
           //{

           //    MeanChange mc = new MeanChange(ds, filter);
           //    List<EntryData> d = mc.Analye();
           //    // Console.WriteLine(d.Count);
           //    foreach (EntryData dd in d)
           //    {
           //        Console.WriteLine(dd.time + "," + dd.share + "," + dd.price + "," + dd.change + "," + dd.type);
           //    }
          // }
            
        }

    }
}
