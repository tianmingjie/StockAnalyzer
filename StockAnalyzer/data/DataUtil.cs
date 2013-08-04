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
            sb.Append("Date,BuyShare,SellShare,BuyMoney,SellMoney,increDiffShare,increDiffMoney, BuyRate,SellRate, Change\n");

            decimal incrementalComShare = 0, incrementalComMoney = 0; 
            
            foreach (string a in big.Keys)
            {
                decimal rateOfBuy=0, rateOfSell=0, rateOfChange=0;
                decimal rateOfBuySell=0;
                decimal totalBuyShare=0,totalSellShare=0,totalBuyMoney=0,totalSellMoney=0;
                
                if (small.ContainsKey(a))
                {
                    rateOfBuy = StockUtil.FormatRate(small[a].TotalBuyMoney / big[a].TotalMoney);
                    rateOfSell = StockUtil.FormatRate(small[a].TotalSellMoney / big[a].TotalMoney);
                    totalBuyShare = small[a].TotalBuyShare;
                    totalSellShare=small[a].TotalSellShare;
                    totalBuyMoney=small[a].TotalBuyMoney;
                    totalSellMoney=small[a].TotalSellMoney;
                    incrementalComShare += totalBuyShare - totalSellShare;
                    incrementalComMoney += totalBuyMoney - totalSellMoney;
                    //incrementalBuyMoney += totalBuyMoney;
                    //incrementalSellMoney += totalSellMoney;
                }

                rateOfChange = StockUtil.FormatRate((big[a].Close - big[a].Open) / big[a].Open);
                rateOfBuySell = StockUtil.FormatRate((big[a].TotalBuyMoney - big[a].TotalSellMoney) / big[a].TotalSellMoney);
                sb.Append(a + ",");
                sb.Append(totalBuyShare+ ",");
                sb.Append(totalSellShare + ",");
                sb.Append(totalBuyMoney/10000 + ",");
                sb.Append(totalSellMoney / 10000 + ",");
                sb.Append(incrementalComShare + ",");
                sb.Append(incrementalComMoney / 10000 + ",");
                //sb.Append(0 + ",");
                sb.Append(rateOfBuy + ",");
                sb.Append(rateOfSell + ",");
               // sb.Append(rateOfBuySell + ",");
                sb.Append(rateOfChange + "\n");

            }
            return sb.ToString();
      }
    }
}
