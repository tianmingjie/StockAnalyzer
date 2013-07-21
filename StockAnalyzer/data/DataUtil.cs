using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace SotckAnalyzer.data
{
    public class DataUtil
    {

        public static List<DailyData> ConvertDailyList(List<EntryData> entryList)
        {
            if (entryList.Count == 0) return null;
            List<DailyData> list = new List<DailyData>();

            for (DateTime dateTime = DateTime.Parse(StockUtil.FormatDate(entryList[0].time));
                dateTime <= DateTime.Parse(StockUtil.FormatDate(entryList[entryList.Count - 1].time));
                dateTime += TimeSpan.FromDays(1))
            {
                IEnumerable<EntryData> querySet = from d in entryList where StockUtil.FormatDate(d.time) == StockUtil.FormatDate(dateTime) select d;
                if (querySet.Count<EntryData>() > 0)
                {
                    DailyData dd = new DailyData()
                    {
                        Date=dateTime,
                        entryList = querySet.ToList<EntryData>()
                    };
                    dd.Init();
                    list.Add(dd);
                }
            }
            return list;

        }

        public static string Compare(Dictionary<string,FilterData> big, Dictionary<string,FilterData> small)
        {
            StringBuilder sb=new StringBuilder();
            sb.Append("Date,Total,Buy,Sell\n");
            foreach(string a in big.Keys)
            {
                if (small.ContainsKey(a))
                {
                    sb.Append(a+",");
                    sb.Append(StockUtil.FormatRate(small[a].TotalMoney / big[a].TotalMoney)+",");
                    sb.Append(StockUtil.FormatRate(small[a].TotalBuyMoney / big[a].TotalMoney) + ",");
                    sb.Append(StockUtil.FormatRate(small[a].TotalSellMoney / big[a].TotalMoney) + "\n");
                }
                else
                {
                    sb.Append(a + ",");
                    sb.Append(0 + ",");
                    sb.Append(0 + ",");
                    sb.Append(0 + "\n");
                }
            }
            return sb.ToString();
      }
    }
}
