using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace SotckAnalyzer.data
{
    /// <summary>
    /// 一天的记录
    /// </summary>
    public class DailyData
    {
        //Start the 12:00 am
        private DateTime _date;

        private DateTime _startTime;
        private DateTime _endTime;
        private decimal _highest;
        private decimal _lowest;
        private decimal _open;
        private decimal _close;
        private DateTime _timeWhenHighest;
        private DateTime _timeWhenLowest;


        public List<EntryData> entryList;
        //public string stock;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }


        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        
        public decimal HighestPrice
        {
            get { return _highest; }
            set { _highest = value; }
        }

        

        public decimal LowestPrice
        {
            get { return _lowest; }
            set { _lowest = value; }
        }

        

        public decimal OpenPrice
        {
            get { return _open; }
            set { _open = value; }
        }

        

        public decimal ClosePrice
        {
            get { return _close; }
            set { _close = value; }
        }

        

        public DateTime TimeWhenHighest
        {
            get { return _timeWhenHighest; }
            set { _timeWhenHighest = value; }
        }
 
       

        public DateTime TimeWhenLowest
        {
            get { return _timeWhenLowest; }
            set { this._timeWhenLowest = value; }
        }

        public void Init()
        {
            _startTime = entryList[0].time;
            _endTime = entryList[entryList.Count - 1].time;
            _date = DateTime.Parse(StockUtil.FormatDate(_startTime));
            int index=0;
            decimal current;
            if (entryList.Count==0) { return; }
            foreach(EntryData data in entryList)
            {

                current = data.price ;

                if (index == 0)
                {
                    _close = current;
                    _highest = current;
                    _lowest = current;
                }

                if (_highest < current)
                {
                    _highest = current;
                    _timeWhenHighest = data.time;
                }
                if (_lowest > current)
                {
                    _lowest = current;
                    _timeWhenLowest = data.time;
                }
                _open= current;
                index++;
            }
        }

        public decimal Average
        {
            get
            {
                return Math.Round(TotalMoney / (100 * TotalShare), 2);
            }
        }

        public decimal AverageSell
        {
            get
            {
                return Math.Round(TotalSellMoney / (TotalSellShare * 100), 2);
            }
        }

        public decimal AverageBuy
        {
            get
            {
                return Math.Round(TotalBuyMoney / (TotalBuyShare * 100), 2);
            }
        }

        public decimal TotalMoney
        {
            get
            {
                decimal money = 0;
                foreach (EntryData d in entryList)
                {
                    money += d.money;
                }
                return money;
            }
        }

        public decimal TotalShare
        {
            get
            {
                decimal share = 0;
                foreach (EntryData d in entryList)
                {
                    share += d.share;
                }
                return share;
            }
        }


        public decimal TotalBuyMoney
        {
            get
            {
                decimal money = 0;
                foreach (EntryData d in entryList)
                {
                    if (d.type == "B")
                        money += d.money;
                }
                return money;
            }
        }

        public decimal TotalBuyShare
        {
            get
            {
                decimal share = 0;
                foreach (EntryData d in entryList)
                {
                    if (d.type == "B")
                        share += d.share;
                }
                return share;
            }
        }

        public decimal TotalSellMoney
        {
            get
            {
                decimal money = 0;
                foreach (EntryData d in entryList)
                {
                    if (d.type == "S")
                        money += d.money;
                }
                return money;
            }
        }

        public decimal TotalSellShare
        {
            get
            {
                decimal share = 0;
                foreach (EntryData d in entryList)
                {
                    if (d.type == "S")
                        share += d.share;
                }
                return share;
            }
        }


    }

    
}
