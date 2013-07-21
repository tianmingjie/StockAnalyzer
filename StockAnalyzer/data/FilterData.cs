using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.data;
using Common;

namespace SotckAnalyzer.data
{
    public class FilterData
    {
        public FilterData()
        {
        }

       // private List<EntryData> EntryDataList;
        public virtual List<EntryData> EntryList
        {
            get;
            set;
            //set
            //{
            //    if (EntryDataList == null)
            //    {
            //        value = EntryDataList;
            //    }
            //}
            //get
            //{
            //    return EntryDataList;
            //}
        }

        public virtual List<DailyData> DailyList
        {
            get;
            set;
            //set
            //{
            //    if (EntryDataList == null)
            //    {
            //        value = EntryDataList;
            //    }
            //}
            //get
            //{
            //    return EntryDataList;
            //}
        }

        #region individual
        public decimal Open
        {
            get
            {
                if (EntryList.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return EntryList[0].price;
                }
            }
        }
        public decimal Close
        {
            get
            {
                if (EntryList.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return EntryList[EntryList.Count-1].price;
                }
            }
        }

        #endregion
        #region total filtered
        public DateTime StartTime
        {
            get{
                if (EntryList.Count == 0) {
                    return new DateTime();
                }
                else
                {
                    return EntryList[0].time;
                }
            }
            set
            {
            }
        }

        public DateTime EndTime
        {
            get
            {
                if (EntryList.Count == 0)
                {
                    return new DateTime();
                }
                else
                {
                    return EntryList[EntryList.Count - 1].time;
                }
            }
            set
            {
            }
        }

        public decimal TotalMoney
        {
            get
            {
                decimal money = 0.00M;
                foreach (EntryData d in EntryList)
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
                foreach (EntryData d in EntryList)
                {
                    if (d.type == "B")
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
                foreach (EntryData d in EntryList)
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
                foreach (EntryData d in EntryList)
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
                foreach (EntryData d in EntryList)
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
                foreach (EntryData d in EntryList)
                {
                    if (d.type == "S")
                        share += d.share;
                }
                return share;
            }
        }

        # endregion

        # region evergage
        public decimal AvergaeBuyMoney
        {
            get
            {
                return StockUtil.FormatRate(TotalBuyMoney / TotalBuyShare / 100);
            }
        }

        public decimal AvergaeSellMoney
        {
            get
            {
                return StockUtil.FormatRate(TotalSellMoney / TotalSellShare / 100);
            }
        }

        #endregion

    }
}
