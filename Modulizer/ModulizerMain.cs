using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using SotckAnalyzer;
using DownloadData;
using Report;
using Common.type;

namespace Modulizer
{
    public class ModulizerMain :IStockModule
    {
        public static void Main(string[] args)
        {

            //List<DateUnit> u=DateUtil.ConvertMonthlyDateUnit(DateTime.Parse("2012-09-03"), DateTime.Now);

            StockInfo info =new StockInfo();

            info.stock = StockUtil.FormatStock(GetPara(args,"-stock"));
            info.startDate = GetPara(args, "-start");
            if(info.startDate.Equals(""))
            {
                info.startDate = "2013-03-01";
            }
            //info.startDate = "2013-03-01";
            String[] bigDeal = GetPara(args, "-big").Split(',');
            if (bigDeal.Length <=1)
            {
                bigDeal = new String[] { "500", "1000", "2000" };
            }
            info.filterList = bigDeal;
            //info.filterList = new String[] { "500", "1000", "2000" };
            List <IStockModule> list= new List<IStockModule>();
            list.Add(new DownloadModule());
            list.Add(new AnalyzeModule());
            list.Add(new ReportModule());

            foreach (IStockModule module in list)
            {
                module.Execute(info);
            }

        }

        public static string GetPara(String[] args, String key)
        {
            String ret = "";
            for (int i = 0; i < args.Length; i = i + 2)
            {
                if (args[i].Equals(key))
                {
                    ret = args[i + 1];
                }
            }

            return ret;
        }

        public bool Execute(StockInfo info)
        {
            List<IStockModule> list = new List<IStockModule>();
            list.Add(new DownloadModule());
            list.Add(new AnalyzeModule());
            list.Add(new ReportModule());

            foreach (IStockModule module in list)
            {
                module.Execute(info);
            }
            return true;
        }
    }
}
