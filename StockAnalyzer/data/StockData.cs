using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.reader;
using Common;

namespace SotckAnalyzer.data
{
    public class StockData:FilterData
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
        }
        public string stock;

        public string startDate;

        public bool isDownload=true;
        public string endDate;

        public List<DailyData> _dataset;
        private List<EntryData> _entrydata;


        public List<DailyData> Dataset
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

        public override List<EntryData> EntryDataList
        {
            get
            {
                if (_entrydata == null)
                {
                    Console.WriteLine("stock data");
                    _entrydata = new List<EntryData>();

                    foreach (DailyData daily in Dataset)
                    {
                        foreach (EntryData entry in daily.set)
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
