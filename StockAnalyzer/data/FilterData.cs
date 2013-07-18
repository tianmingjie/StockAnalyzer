using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.data;

namespace SotckAnalyzer.data
{
    public class FilterData
    {
        public FilterData()
        {
        }

       // private List<EntryData> EntryDataList;
        public virtual List<EntryData> EntryDataList
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

        #region total filtered
        public DateTime StartTime
        {
            get{
            return EntryDataList[0].time;
            }
            set
            {
            }
        }

        public DateTime EndTime
        {
            get
            {
                return EntryDataList[EntryDataList.Count-1].time;
            }
            set
            {
            }
        }

        public long TotalMoney
        {
            get
            {
                long money = 0;
                foreach (EntryData d in EntryDataList)
                {
                        money += d.money;
                }
                return money;
            }
        }

        public long TotalShare
        {
            get
            {
                long share = 0;
                foreach (EntryData d in EntryDataList)
                {
                    if (d.type == "B")
                        share += d.share;
                }
                return share;
            }
        }

        public long TotalBuyMoney
        {
            get
            {
                long money = 0;
                foreach (EntryData d in EntryDataList)
                {
                    if (d.type == "B")
                        money += d.money;
                }
                return money;
            }
        }

        public long TotalBuyShare
        {
            get
            {
                long share = 0;
                foreach (EntryData d in EntryDataList)
                {
                    if (d.type == "B")
                        share += d.share;
                }
                return share;
            }
        }

        public long TotalSellMoney
        {
            get
            {
                long money = 0;
                foreach (EntryData d in EntryDataList)
                {
                    if (d.type == "S")
                        money += d.money;
                }
                return money;
            }
        }

        public long TotalSellShare
        {
            get
            {
                long share = 0;
                foreach (EntryData d in EntryDataList)
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
                return Math.Round((decimal)TotalBuyMoney / TotalBuyShare / 100, 2);
            }
        }

        public decimal AvergaeSellMoney
        {
            get
            {
                return Math.Round((decimal)TotalSellMoney / TotalSellShare / 100, 2);
            }
        }

        #endregion

    }
}
