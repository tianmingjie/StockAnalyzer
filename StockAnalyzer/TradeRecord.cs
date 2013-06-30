using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SotckAnalyzer
{
    public class TradeRecord
    {
        
        public string stockId;
        public string recordDate;
        public string recordTime;
        public decimal price;
        public decimal priceChange;
        public long number;
        public long money;
        public string type;
    }
}
