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

        public virtual List<EntryData> EntryDataList
        {
            set;
            get;
        }

        #region total filtered

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
