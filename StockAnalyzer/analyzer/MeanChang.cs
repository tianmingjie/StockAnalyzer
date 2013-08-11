using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.data;
using Common;

namespace SotckAnalyzer.analyzer
{
    public  class MeanChange
    {
        public DailyData set;
        public string filter;

        /// <summary>
        /// Filter is ">400" "<1000", "400-1000"
        /// </summary>
        /// <param name="set"></param>
        /// <param name="filter"></param>
        public MeanChange(DailyData set, string filter)
        {
            this.set = set;
            this.filter = filter;
        }

        public List<EntryData> Analye()
        {
            IEnumerable<EntryData> querySet = null;

            //decimal threshold=0.005M;

            //querySet = from data in set where data.share > Int32.Parse(filter) && data. select data;
            return  querySet.ToList<EntryData>();
        }

    }
}
