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

        public decimal HighestPrice
        {
            get;
            set;
        }
        public decimal LowestPrice
        {
            get;
            set;
        }
        public decimal OpenPrice
        {
            get;
            set;
        }
        public decimal ClosePrice
        {
            get;
            set;
        }
        public DateTime TimeWhenHighest
        {
            get;
            set;
        }
        public DateTime TimeWhenLowest
        {
            get;
            set;
        }

        public decimal Average
        {
            get
            {
                return Math.Round(TotalMoney / (100 * TotalShare), 2);
            }
        }

        public decimal AverageSell
        {
            get
            {
                return Math.Round(TotalSellMoney / (TotalSellShare * 100), 2);
            }
        }

        public decimal AverageBuy
        {
            get
            {
                return Math.Round(TotalBuyMoney / (TotalBuyShare * 100), 2);
            }
        }

        public decimal TotalMoney
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

        public decimal TotalShare
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


        public decimal TotalBuyMoney
        {
            get
            {
                decimal money = 0;
                foreach (DailyData d in set)
                {
                    if (d.type == "B")
                        money += d.money;
                }
                return money;
            }
        }

        public decimal TotalBuyShare
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

        public decimal TotalSellMoney
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

        public decimal TotalSellShare
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

    
}
