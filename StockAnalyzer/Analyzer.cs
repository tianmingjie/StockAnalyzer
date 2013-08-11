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
using SotckAnalyzer.type;
using Spring.Context;
using Spring.Context.Support;

namespace SotckAnalyzer
{
    public static class Analyzer
    {

        //static string Constant.ROOT_FOLDER = Constant.ROOT_FOLDER;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Dictionary<string, string> dic = ProcessParam(args);
            string stock = dic["-stock"]; //"002693";
            string startDate = dic["-start"];//"2013-05-01";
            string endDate, filter;
            if (dic.ContainsKey("-end"))
            {
                endDate = dic["-end"];
            }
            else
            {
                endDate = StockUtil.FormatDate(DateTime.Now);
            }

            if (dic.ContainsKey("-filter"))
            {
                filter = dic["-filter"];
            }
            else
            {
                filter = "500";
            }

            StockLog.Log.Info(stock + " " + startDate + " " + endDate + " " + filter);

            Analyze(stock, startDate, endDate, filter);
        }

        public static void Analyze(String stock, string startDate, string endDate, string filter)
        {
            //IApplicationContext ctx = ContextRegistry.GetContext();
            StockLog.Log.Info("start to create stock data");
            StockData data1 = new StockData(stock, startDate, endDate, true);//(StockData)ctx.GetObject("StockData");
            StockLog.Log.Info("start to create normal data");
            NormalData gd = new NormalData(data1);//(NormalData)ctx.GetObject("NormalData");
            StockLog.Log.Info("start to create big deal data");
            BigDealData bdd = new BigDealData(data1, filter);//(BigDealData)ctx.GetObject("BigDealData");
            //RangeData bigdeal = new RangeData(bdd, type); //(RangeData)ctx.GetObject("BigRangeData");
            // RangeData alldeal = new RangeData(gd, type);//(RangeData)ctx.GetObject("AllRangeData");
        
                if (!Directory.Exists(Constant.ANALYZE_FOLDER + stock))
                {
                    // Directory.Delete(Constant.ANALYZE_FOLDER + stock, true); 
                    Directory.CreateDirectory(Constant.ANALYZE_FOLDER + stock);
                }
                else
                {
                    if (Constant.CLEAN)
                    {
                        Directory.Delete(Constant.ANALYZE_FOLDER + stock, true);
                        Directory.CreateDirectory(Constant.ANALYZE_FOLDER + stock);
                    }
                }

            foreach (int type in Enum.GetValues(typeof(RangeType)))
            {
                String filePath = string.Format(@"{0}{1}\{1}_{2}_{3}_{4}_{5}.csv", Constant.ANALYZE_FOLDER, stock, startDate, endDate, filter, (RangeType)type);
                StockLog.Log.Info("start to analyze " + (RangeType)type);
                if (!File.Exists(filePath))
                {
                    
                    RangeData bigdeal = new RangeData(bdd, type); //(RangeData)ctx.GetObject("BigRangeData");
                    RangeData alldeal = new RangeData(gd, type);//(RangeData)ctx.GetObject("AllRangeData");
                    Dictionary<string, FilterData> big = bigdeal.DataList;
                    Dictionary<string, FilterData> all = alldeal.DataList;
                    FileUtil.WriteFile(filePath, DataUtil.Compare(all, big));
                }
                StockLog.Log.Info("End to analyze " + (RangeType)type);
            }
        }
        public static Dictionary<string, string> ProcessParam(string[] list)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //string[] list=a.Split(' ');
            for (int i = 0; i < list.Length - 1; i++)
            {
                if (list[i].StartsWith("-") && !list[i + 1].StartsWith("-"))
                {
                    dic.Add(list[i], list[i + 1]);
                }
            }
            return dic;
        }

    }
}
