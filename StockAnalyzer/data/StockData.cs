using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.reader;
using Common;
using log4net;

namespace SotckAnalyzer.data
{
    public class StockData
    {

        public StockData(string stock)
        {
            this.stock = stock;
        }
        public StockData(string stock, string startDate, string endDate,bool download=true)
        {
            this.stock = stock;
            this.startDate=startDate;
            this.endDate = endDate;
            isDownload = download;
            init();
        }
        public string stock;

        public string startDate;

        public bool isDownload=true;
        public string endDate;

        public List<DailyData> _dataset;
        private List<EntryData> _entrydata;


        private void init()
        {
            if (_dataset == null)
            {
                _dataset = Csv.ReadCsv(StockUtil.FormatStock(stock), startDate, endDate, isDownload);
            }
            if (_entrydata == null)
            {
                _entrydata = new List<EntryData>();

                foreach (DailyData daily in DailyList)
                {
                    foreach (EntryData entry in daily.entryList)
                    {
                        _entrydata.Add(entry);
                    }
                }

            }
        }
        public List<DailyData> DailyList
        {
            get
            {
                if (_dataset == null)
                {
                    _dataset = Csv.ReadCsv(StockUtil.FormatStock(stock), startDate, endDate, isDownload);
                }
                return _dataset;
            }
            set
            {
                value = _dataset;
            }
        }

        public List<EntryData> EntryList
        {
            get
            {
                if (_entrydata == null)
                {
                    _entrydata = new List<EntryData>();

                    foreach (DailyData daily in DailyList)
                    {
                        foreach (EntryData entry in daily.entryList)
                        {
                            _entrydata.Add(entry);
                        }
                    }
                    
                }
                return _entrydata;
                
            }
            set
            {
                value = _entrydata;
            }
        }

    }
}
