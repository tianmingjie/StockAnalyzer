using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace SotckAnalyzer.data
{
    public class BigDealData : FilterData
    {
        public string filter = "";

        public StockData data;

        private List<EntryData> _entryList;

        public BigDealData(StockData data, String filter)
        {
            this.data = data;
            this.filter = filter;
           // base.Stock = data.Stock;
        }

        public override List<EntryData> EntryList
        {
            get
            {
                if (_entryList == null)
                {
                    //Console.WriteLine("big deal data");
                    IEnumerable<EntryData> querySet = from d in data.EntryList where d.share > Int32.Parse(filter) select d;
                    _entryList = querySet.ToList<EntryData>();
                }
                return _entryList;
            }
            set
            {
                value = _entryList;
            }
        }

        public override List<DailyData> DailyList
        {
            
            get
            {
                return DataUtil.ConvertDailyList(EntryList);
                //List<DailyData> DailyDataList=new List<DailyData>();
                //foreach(DailyData dd in data.DailyList) {
                //    IEnumerable<EntryData> querySet = from d in dd.entryList where d.share > Int32.Parse(filter) select d;
                //    dd.entryList=querySet.ToList<EntryData>();
                //    DailyDataList.Add(dd);
                //}

                //return DailyDataList;
            }
        }
    }
}
