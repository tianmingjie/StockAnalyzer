using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SotckAnalyzer.data
{
    public class BigDealData:FilterData
    {
        public string filter = "";

        public StockData data;

        public BigDealData(StockData data, String filter)
        {
            this.data = data;
            this.filter = filter;
        }

        public override List<EntryData> EntryDataList
        {
            get
            {
                IEnumerable<EntryData> querySet = from d in data.EntryDataList where d.share > Int32.Parse(filter) select d;
                return querySet.ToList<EntryData>();
            }
            set
            {
            }
        }
        }
    }
