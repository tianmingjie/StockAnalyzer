using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SotckAnalyzer.data
{
    public class DailyDataSet
    {
        public List<DailyData> set;
        public string stock;
        public DateTime date;

        public decimal average
        {
            get
            {
                return Math.Round(totalMoney / (100*totalShare),2);
            }
        }

        public decimal averageSell
        {
            get
            {
                return Math.Round(totalSellMoney / (totalSellShare*100),2);
            }
        }

        public decimal averageBuy
        {
            get
            {
                return Math.Round(totalBuyMoney /( totalBuyShare*100),2);
            }
        }

        public decimal totalMoney
        {
            get
            {
                decimal money = 0;
                foreach (DailyData d in set)
                {
                        money += d.money;
                }
                return money;
            }
        }

        public decimal totalShare
        {
            get
            {
                decimal share = 0;
                foreach (DailyData d in set)
                {
                        share += d.share;
                }
                return share;
            }
        }


        public decimal totalBuyMoney
        {
            get
            {
                decimal money = 0;
                foreach (DailyData d in set)
                {
                    if(d.type=="B")
                         money += d.money;
                }
                return money;
            }
        }

        public decimal totalBuyShare
        {
            get
            {
                decimal share = 0;
                foreach (DailyData d in set)
                {
                    if (d.type == "B")
                    share += d.share;
                }
                return share;
            }
        }

        public decimal totalSellMoney
        {
            get
            {
                decimal money = 0;
                foreach (DailyData d in set)
                {
                    if (d.type == "S")
                    money += d.money;
                }
                return money;
            }
        }

        public decimal totalSellShare
        {
            get
            {
                decimal share = 0;
                foreach (DailyData d in set)
                {
                    if (d.type == "S")
                    share += d.share;
                }
                return share;
            }
        }


    }

    public class FilterData
       
    {
        public string stock;
        public DateTime date;
        public DailyDataSet set;

        public FilterData(DailyDataSet set)
        {
            this.set = set;
        }

        public FilterData(string stock,DateTime date)
        {
            this.stock = stock;
            this.date = date;
        }
        public List<DailyData> setByBigDeal;


#region total filtered
        public decimal totalBuyMoneyByBigDeal
        {
            get
            {
                decimal money = 0;
                foreach (DailyData d in setByBigDeal)
                {
                    if (d.type == "B")
                    money+=d.money;
                }
                return money;
            }
        }

        public decimal totalBuyShareByBigDeal
        {
            get
            {
                decimal share = 0;
                foreach (DailyData d in setByBigDeal)
                {
                    if (d.type == "B")
                    share += d.share;
                }
                return share;
            }
        }

              public decimal totalSellMoneyByBigDeal
        {
            get
            {
                decimal money = 0;
                foreach (DailyData d in setByBigDeal)
                {
                    if(d.type=="S")
                    money += d.money;
                }
                return money;
            }
        }

        public decimal totalSellShareByBigDeal
        {
            get
            {
                decimal share = 0;
                foreach (DailyData d in setByBigDeal)
                {
                    if (d.type == "S")
                    share += d.share;
                }
                return share;
            }
        }

# endregion
# region evergage
        public decimal avergaeBuyMoneyByBigDeal
        {
            get
            {
                return Math.Round(totalBuyMoneyByBigDeal / totalBuyShareByBigDeal / 100, 2);
            }
        }

        public decimal avergaeSellMoneyByBigDeal
        {
            get
            {
                return Math.Round(totalSellMoneyByBigDeal / totalSellShareByBigDeal / 100, 2);
            }
        }

#endregion

        #region Share rate
        public decimal rateOfSellShareByTotal
        {
            get
            {
                return Math.Round(totalSellShareByBigDeal / set.totalShare,2);
            }
        }
        public decimal rateOfBuyShareByTotal
        {
            get
            {
                return Math.Round(totalBuyShareByBigDeal / set.totalShare,2);
            }
        }
        public decimal rateOfSellShareByTotalSell
        {
            get
            {
                return Math.Round(totalSellShareByBigDeal / set.totalSellShare,2);
            }
        }
        public decimal rateOfBuyShareByTotalBuy
        {
            get
            {
                return Math.Round(totalBuyShareByBigDeal / set.totalBuyShare,2);
            }
        }
        #endregion

        #region Rate of money
        public decimal rateOfSellMoneyByTotal
        {
            get
            {
                return Math.Round(totalSellMoneyByBigDeal / set.totalMoney,2);
            }
        }
        public decimal rateOfBuyMoneyByTotal
        {
            get
            {
                return Math.Round(totalBuyMoneyByBigDeal / set.totalMoney,2);
            }
        }
        public decimal rateOfSellMoneyByTotalSell
        {
            get
            {
                return Math.Round(totalSellMoneyByBigDeal / set.totalSellMoney,2);
            }
        }
        public decimal rateOfBuyMoneyByTotalBuy
        {
            get
            {
                return Math.Round(totalBuyMoneyByBigDeal / set.totalBuyMoney,2);
            }
        }
        #endregion 
        string filter;
    }
}
