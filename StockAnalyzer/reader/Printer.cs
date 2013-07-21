using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.data;
using Common;

namespace SotckAnalyzer.reader
{
    public class Printer
    {
         public static string PrintEntryData(EntryData e,bool includeHeader=false)
        {
            StringBuilder sb = new StringBuilder();
            if(includeHeader)sb.Append("time,price,change,share,money,type");
            sb.Append(String.Format("{0},{1},{2},{3},{4},{5}",e.time,e.price,e.money,e.change,e.share,e.type));

#if DEBUG
            Console.WriteLine(sb.ToString());
#endif
            return sb.ToString();
        }

         public static string CompareDailyData(List<DailyData> dailyData1, List<DailyData> dailyData2)
         {
             if (dailyData1.Count != dailyData2.Count) return null;

             StringBuilder sb = new StringBuilder();
             sb.Append(String.Format("{0},{1},{2},{3}\n", "time", "bigSellShareRate", "bigBuyShareRate", "Average"));
             for (int i = 0; i < dailyData1.Count; i++)
             {
                 Console.WriteLine(dailyData1[i].entryList.Count + " " + dailyData2[i].entryList.Count);
                 //sb.Append(String.Format("{0},{1},{2},{3}\n", StockUtil.FormatDate(dailyData1[i].date), dailyData1[i].TotalBuyMoney / dailyData2[i].TotalBuyMoney, dailyData1[i].TotalSellMoney / dailyData2[i].TotalSellMoney, dailyData1[i].TotalMoney / dailyData2[i].TotalMoney));
             }
             return sb.ToString();
         }
    }
}
