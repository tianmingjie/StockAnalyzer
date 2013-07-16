using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SotckAnalyzer.data
{
    public class RangeData
    {
        //minutes
        string _interval = "30m";
        FilterData _filterData;

        List<FilterData> list;
        public RangeData(FilterData filterData, string interval)
        {
            this._filterData = filterData;
            this._interval = interval;
        }

        public List<FilterData> Analyze()
        {

            if (list == null)
            {
                list = new List<FilterData>();


                if (GetIntervalType().Equals("d"))
                {
                    for (DateTime dateTime = GetTradeStartTime(_filterData.StartTime); dateTime <= GetTradeEndTime(_filterData.EndTime); dateTime += TimeSpan.FromDays(GetInterval()))
                    {
                        AddList(list,dateTime,dateTime.AddDays(GetInterval()));
                    }
                }

                if (GetIntervalType().Equals("w"))
                {
                    DateTime first = GetNextMonday(GetTradeStartTime(_filterData.StartTime));
                    DateTime last = GetThisMonday(GetTradeStartTime(_filterData.EndTime));

                    AddList(list, _filterData.StartTime, first);


                    for (DateTime dateTime = first; dateTime < last; dateTime += TimeSpan.FromDays(7 * GetInterval()))
                    {
                        AddList(list,dateTime,dateTime.AddDays(7 * GetInterval()));
                    }

                    AddList(list, last, _filterData.EndTime);
                }

                if (GetIntervalType().Equals("M"))
                {
                    DateTime first = GetNextFirstDate(GetTradeStartTime(_filterData.StartTime));
                    DateTime last = GetThisFirstDate(GetTradeStartTime(_filterData.EndTime));

                    AddList(list,_filterData.StartTime,first);

                    for (DateTime dateTime = first; dateTime < last; dateTime += TimeSpan.FromDays(GetMonthNumber(dateTime.AddDays(15))))
                    {
                        AddList(list,dateTime,dateTime.AddDays(GetMonthNumber(dateTime.AddDays(15))));
                    }

                    AddList(list,last,_filterData.EndTime);
                }

                if (GetIntervalType().Equals("m"))
                {
                    for (DateTime dateTime = GetTradeStartTime(_filterData.StartTime); dateTime <= GetTradeEndTime(_filterData.EndTime); dateTime += TimeSpan.FromDays(1))
                    {
                        for (DateTime d1 = GetTradeStartTime(dateTime); d1 <= GetTradeEndTime(dateTime); d1 += TimeSpan.FromMinutes(GetInterval()))
                        {
                            AddList(list,d1,d1.AddMinutes(GetInterval()));
                        }
                    }

                }


            }
            return list;
        }

        private void AddList(List<FilterData> list, DateTime start, DateTime end)
        {
            FilterData f1 = new FilterData();
            IEnumerable<EntryData> querySet = from d in _filterData.EntryDataList where d.time > start && d.time < end select d;
            if (querySet.Count<EntryData>() > 0)
            {
                f1.EntryDataList = querySet.ToList<EntryData>();
                list.Add(f1);
            }
        }
        private DateTime GetNextFirstDate(DateTime d)
        {
            return d.AddDays(GetMonthNumber(d)+1 - d.Day); 
        }
        private DateTime GetThisFirstDate(DateTime d)
        {
            return d.AddDays(1- d.Day);
        }
        private int GetMonthNumber(DateTime d)
        {
            int[] t = { 1, 3, 5, 7, 8, 10, 12 };
            int[] t1 = { 4, 6, 10, 11 };
            int dt = 0;
            if (t.Contains<int>(d.Month))
                dt = 31;
            if (t1.Contains<int>(d.Month))
                dt = 30;
            if (d.Month == 2)
            {
                if (d.Year % 4 == 0)
                {
                    dt = 29;
                }
                else
                {
                    dt = 28;
                }
            }
            return dt;
        }
        private DateTime GetThisMonday(DateTime d)
        {
            return d.AddDays(1 - (int)d.DayOfWeek);
        }
        private DateTime GetNextMonday(DateTime d)
        {
            return d.AddDays(8-(int)d.DayOfWeek);
        }

        private DateTime GetTradeStartTime(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 9, 25, 0);
        }

        private DateTime GetTradeEndTime(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 15, 0, 5);
        }
        private int GetInterval()
        {
            return Int32.Parse(_interval.Substring(0, _interval.Length - 1));
        }

        private string GetIntervalType()
        {
            return _interval.Substring(_interval.Length - 1);
        }
    }
}
