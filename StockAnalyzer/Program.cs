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


            try
            {
                IApplicationContext ctx = ContextRegistry.GetContext();

                StockData data1 = (StockData)ctx.GetObject("StockData");
                // data.money = 1;
                Console.WriteLine(data1.DailyList.Count);

                NormalData gd = (NormalData)ctx.GetObject("NormalData");
                //List<EntryData> l=bdd.EntryDataList;
                Console.WriteLine(gd.DailyList.Count);

                //foreach (DailyData e in gd.DailyList)
                //{
                //    Console.WriteLine(e.date + " " + e.entryList.Count);
                //}

                BigDealData bdd = (BigDealData)ctx.GetObject("BigDealData");
                //List<EntryData> l=bdd.EntryDataList;
                Console.WriteLine(bdd.DailyList.Count);

                ////foreach (DailyData e in bdd.DailyList)
                ////{
                ////    Console.WriteLine(e.date + " " + e.entryList.Count);
                ////}


                RangeData bigdeal = (RangeData)ctx.GetObject("BigRangeData");
                RangeData alldeal = (RangeData)ctx.GetObject("AllRangeData");


                Dictionary<string, FilterData> big = bigdeal.DataList;
                Dictionary<string, FilterData> all = alldeal.DataList;

                //foreach (KeyValuePair<string, FilterData> a in big)
                //{
                //    // Console.WriteLine(e.TotalBuyMoney + " " + e.StartTime + " " + e.EndTime);
                //    Console.WriteLine(a.Key + " " + a.Value.TotalMoney);
                //}
               

                //foreach (KeyValuePair<string, FilterData> a in all)
                //{
                //    // Console.WriteLine(e.TotalBuyMoney + " " + e.StartTime + " " + e.EndTime);
                //    Console.WriteLine(a.Key + " " + a.Value.TotalMoney);
                //}


                Persistent.WriteFile(@"D:\project\stock\analyzeData\test.csv",DataUtil.Compare(all, big));


                //RateData rate = (RateData)ctx.GetObject("RateData");
                //Persistent.WriteFile
                //    (@"D:\project\stock\analyzeData\test.csv", rate.CompareDaily());


                //foreach (DateUnit d in DateUtil.ConvertBiHourlyDateUnit(DateTime.Parse("2013-07-03"), DateTime.Parse("2013-07-24")))
                //{
                //    Console.WriteLine(d.Start + " " + d.End);
                //}

            }
            catch (Exception e)
            {
                LOG.Error("Movie Finder is broken.", e);
            }
            finally
            {
                Console.WriteLine();
            }
            
        }

    }
}
