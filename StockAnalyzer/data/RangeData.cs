using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace SotckAnalyzer.data
{
    public class RangeData
    {
        //minutes
        string _type = "d";
        FilterData _filterData;

        List<FilterData> list;
        public RangeData(FilterData filterData, string type)
        {
            this._filterData = filterData;
            this._type = type;
        }

        public Dictionary<String, FilterData> DataList
        {
            get
            {
                Dictionary<String, FilterData> dic;

                switch (_type.ToLower())
                {
                    case  "d":
                        dic=GetDailyList();
                        break;
                    case "w":
                        dic = GetWeeklyList();
                        break;
                    case "m":
                        dic = GetMonthlyList();
                        break;
                    case "h":
                        dic = GetHourlyList();
                        break;
                    case "2h":
                        dic = GetBiHourlyList();
                        break;
                    default:
                        dic=GetDailyList();
                        break;
                }
                return dic;
            }
        }

        private Dictionary<String, FilterData> GetDailyList()
        {
            Dictionary<string, FilterData> dic = new Dictionary<string, FilterData>();
            foreach(DateUnit unit in DateUtil.ConvertDailyDateUnit(_filterData.StartTime,_filterData.EndTime) ){
                AddList(dic, unit);
            }

            return dic;
        }

        private Dictionary<String, FilterData> GetMonthlyList()
        {
            Dictionary<string, FilterData> dic = new Dictionary<string, FilterData>();
            foreach (DateUnit unit in DateUtil.ConvertMonthlyDateUnit(_filterData.StartTime, _filterData.EndTime))
            {
                AddList(dic, unit);
            }

            return dic;
        }

        private Dictionary<String, FilterData> GetWeeklyList()
        {
            Dictionary<string, FilterData> dic = new Dictionary<string, FilterData>();
            foreach (DateUnit unit in DateUtil.ConvertWeeklyDateUnit(_filterData.StartTime, _filterData.EndTime))
            {
                AddList(dic, unit);
            }

            return dic;
        }

        private Dictionary<String, FilterData> GetBiHourlyList()
        {
            Dictionary<string, FilterData> dic = new Dictionary<string, FilterData>();
            foreach (DateUnit unit in DateUtil.ConvertBiHourlyDateUnit(_filterData.StartTime, _filterData.EndTime))
            {
                AddList(dic, unit);
            }

            return dic;
        }

        private Dictionary<String, FilterData> GetHourlyList()
        {
            Dictionary<string, FilterData> dic = new Dictionary<string, FilterData>();
            foreach (DateUnit unit in DateUtil.ConvertHourlyDateUnit(_filterData.StartTime, _filterData.EndTime))
            {
                AddList(dic, unit);
            }

            return dic;
        }

        private void AddList(Dictionary<String, FilterData> dic,DateUnit unit)
        {
            FilterData f1 = new FilterData();
            IEnumerable<EntryData> querySet = from d in _filterData.EntryList where d.time > unit.Start && d.time < unit.End select d;
            if (querySet.Count<EntryData>() > 0)
            {
            f1.EntryList = querySet.ToList<EntryData>();
            f1.DailyList = DataUtil.ConvertDailyList(f1.EntryList);

            dic.Add(StockUtil.FormatAllTime(unit.Start) + "_" + StockUtil.FormatAllTime(unit.End), f1);
            }

        }

        private string IntervalType
        {
            get
            {
                return _type;
            }
            set
            {
                _type=value;
            }
        }
    }
}
