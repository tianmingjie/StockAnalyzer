using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace SotckAnalyzer.data
{
    public class RateData
    {
        public FilterData data1, data2;

        public RateData(FilterData data1, FilterData data2)
        {
            this.data1 = data1;
            this.data2 = data2;
        }

        public string CompareDaily()
        {
            if (data1.DailyList.Count != data2.DailyList.Count) return null;

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0},{1},{2},{3}\n", "time", "bigSellShareRate", "bigBuyShareRate", "Average"));
            for (int i = 0; i < data1.DailyList.Count; i++)
            {
                sb.Append(String.Format("{0},{1},{2},{3}\n", StockUtil.FormatDate(data1.DailyList[i].Date), StockUtil.FormatRate(data1.DailyList[i].TotalBuyMoney / data2.DailyList[i].TotalBuyMoney), StockUtil.FormatRate(data1.DailyList[i].TotalSellMoney / data2.DailyList[i].TotalSellMoney), StockUtil.FormatRate(data1.DailyList[i].TotalMoney / data2.DailyList[i].TotalMoney)));
            }
            return sb.ToString();
        }


        #region Rate to individual
        public decimal RateOfSellShare2Sell
        {
            get
            {
                return Math.Round((decimal)data1.TotalSellShare / data2.TotalSellShare, 2);
            }
        }
        public decimal RateOfBuyShare2Buy
        {
            get
            {
                return Math.Round((decimal)data1.TotalBuyShare / data2.TotalBuyShare, 2);
            }
        }
        public decimal RateOfSellMoney2Sell
        {
            get
            {
                return Math.Round((decimal)data1.TotalSellMoney / data2.TotalSellMoney, 2);
            }
        }
        public decimal RateOfBuyMoney2Buy
        {
            get
            {
                return Math.Round((decimal)data1.TotalBuyMoney / data2.TotalBuyMoney, 2);
            }
        }

        #endregion

        #region Rate of total
        public decimal RateOfMoney2Total
        {
            get
            {
                return Math.Round((decimal)data1.TotalMoney / data2.TotalMoney, 2);
            }
        }
        public decimal RateOfSellMoney2Total
        {
            get
            {
                return Math.Round((decimal)data1.TotalSellMoney / data2.TotalMoney, 2);
            }
        }
        public decimal RateOfBuyMoney2Total
        {
            get
            {
                return Math.Round((decimal)data1.TotalBuyMoney / data2.TotalMoney, 2);
            }
        }
        public decimal RateOfShare2Total
        {
            get
            {
                return Math.Round((decimal)data1.TotalShare / data2.TotalShare, 2);
            }
        }
        public decimal RateOfSellShare2Total
        {
            get
            {
                return Math.Round((decimal)data1.TotalBuyShare / data2.TotalShare, 2);
            }
        }
        public decimal RateOfBuyShare2Total
        {
            get
            {
                return Math.Round((decimal)data1.TotalBuyShare / data2.TotalShare, 2);
            }
        }

        #endregion
    }
}
