using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SotckAnalyzer.data
{
    public class RateData
    {
        FilterData data1, data2;

        public RateData(FilterData data1, FilterData data2)
        {
            this.data1 = data1;
            this.data2 = data2;
        }


        #region Share Rate to individual
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

        #endregion
        #region Money Rate to individual
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

        #region Rate of money
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

        #endregion

        #region Rate of share
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
