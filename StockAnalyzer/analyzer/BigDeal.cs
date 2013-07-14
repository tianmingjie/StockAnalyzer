using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.data;

namespace SotckAnalyzer.analyzer
{
    public class BigDeal
    {
        //public string stock;
        //public DateTime date;
        public DailyData set;
        public string filter;

        public List<EntryData> setByBigDeal;

        /// <summary>
        /// Filter is ">400" "<1000", "400-1000"
        /// </summary>
        /// <param name="set"></param>
        /// <param name="filter"></param>
        public BigDeal(DailyData set, string filter)
        {
            this.set = set;
            this.filter = filter;
        }

        public void Analye()
        {
            IEnumerable<EntryData> querySet = null;

            if (filter.StartsWith(">"))
            {
                querySet = from data in set.set where data.share > Int32.Parse(filter.Substring(1)) select data;
            }

            else if (filter.StartsWith("<"))
            {
                querySet = from data in set.set where data.share < Int32.Parse(filter.Substring(1)) select data;
            }

            else if (filter.Contains("-"))
            {
                String[] a = filter.Split('-');
                querySet = from data in set.set where data.share > Int32.Parse(a[0]) && data.share < Int32.Parse(a[1]) select data;
            }
            else
            {
                querySet = from data in set.set where data.share > Int32.Parse(filter) select data;
            }
            setByBigDeal = querySet.ToList<EntryData>();
        }


        #region total filtered
        public decimal TotalBuyMoneyByBigDeal
        {
            get
            {
                decimal money = 0;
                foreach (EntryData d in setByBigDeal)
                {
                    if (d.type == "B")
                        money += d.money;
                }
                return money;
            }
        }

        public decimal TotalBuyShareByBigDeal
        {
            get
            {
                decimal share = 0;
                foreach (EntryData d in setByBigDeal)
                {
                    if (d.type == "B")
                        share += d.share;
                }
                return share;
            }
        }

        public decimal TotalSellMoneyByBigDeal
        {
            get
            {
                decimal money = 0;
                foreach (EntryData d in setByBigDeal)
                {
                    if (d.type == "S")
                        money += d.money;
                }
                return money;
            }
        }

        public decimal TotalSellShareByBigDeal
        {
            get
            {
                decimal share = 0;
                foreach (EntryData d in setByBigDeal)
                {
                    if (d.type == "S")
                        share += d.share;
                }
                return share;
            }
        }

        # endregion

        # region evergage
        public decimal AvergaeBuyMoneyByBigDeal
        {
            get
            {
                return Math.Round(TotalBuyMoneyByBigDeal / TotalBuyShareByBigDeal / 100, 2);
            }
        }

        public decimal avergaeSellMoneyByBigDeal
        {
            get
            {
                return Math.Round(TotalSellMoneyByBigDeal / TotalSellShareByBigDeal / 100, 2);
            }
        }

        #endregion

        #region Share Rate
        public decimal RateOfSellShareByTotal
        {
            get
            {
                return Math.Round(TotalSellShareByBigDeal / set.TotalShare, 2);
            }
        }
        public decimal RateOfBuyShareByTotal
        {
            get
            {
                return Math.Round(TotalBuyShareByBigDeal / set.TotalShare, 2);
            }
        }
        public decimal RateOfSellShareByTotalSell
        {
            get
            {
                return Math.Round(TotalSellShareByBigDeal / set.TotalSellShare, 2);
            }
        }
        public decimal RateOfBuyShareByTotalBuy
        {
            get
            {
                return Math.Round(TotalBuyShareByBigDeal / set.TotalBuyShare, 2);
            }
        }
        #endregion

        #region Rate of money
        public decimal RateOfSellMoneyByTotal
        {
            get
            {
                return Math.Round(TotalSellMoneyByBigDeal / set.TotalMoney, 2);
            }
        }
        public decimal RateOfBuyMoneyByTotal
        {
            get
            {
                return Math.Round(TotalBuyMoneyByBigDeal / set.TotalMoney, 2);
            }
        }
        public decimal RateOfSellMoneyByTotalSell
        {
            get
            {
                return Math.Round(TotalSellMoneyByBigDeal / set.TotalSellMoney, 2);
            }
        }
        public decimal RateOfBuyMoneyByTotalBuy
        {
            get
            {
                return Math.Round(TotalBuyMoneyByBigDeal / set.TotalBuyMoney, 2);
            }
        }
        #endregion

    }
}
