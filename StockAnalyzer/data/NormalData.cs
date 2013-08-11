using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace SotckAnalyzer.data
{
    public class NormalData : FilterData
    {
        public StockData data;
        public NormalData(StockData data)
        {
            this.data = data;
        }

        // private List<EntryData> EntryDataList;
        public override List<EntryData> EntryList
        {
            get
            {
                return data.EntryList;
            }
        }
        public override List<DailyData> DailyList
        {
            get
            {
                return data.DailyList;
            }
        }

    }
}

