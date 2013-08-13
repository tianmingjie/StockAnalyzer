using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.type;

namespace SotckAnalyzer.data
{
    public class RangeData
    {
        //minutes
        private int _type = 0;
        private FilterData _filterData;

        public RangeData(FilterData filterData, int type=0)
        {
            this._filterData = filterData;
            this._type = type;
        }

        public Dictionary<String, FilterData> DataList
        {
            get
            {
                Dictionary<String, FilterData> dic;

                switch (_type)
                {
                    case  0:
                        dic=GetDailyList();
                        break;
                    case 2:
                        dic = GetWeeklyList();
                        break;
                    case 1:
                        dic = GetMonthlyList();
                        break;
                    case 3:
                        dic = GetHourlyList();
                        break;
                    case 4:
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
            //if (_type == (int)RangeType.Hourly)
            //{
           //     dic.Add(StockUtil.FormatAllTime(unit.Start, true) + "_" + StockUtil.FormatAllTime(unit.End, true), f1);
           // }
            //else
            //{
            dic.Add(StockUtil.FormatAllTime(unit.Start) + "_" + StockUtil.FormatAllTime(unit.End), f1);
           // }
            }

        }

        private int IntervalType
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
