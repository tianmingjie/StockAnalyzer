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


        public List<DailyData> Dataset
        {
            get
            {
                return Csv.ReadCsv(StockUtil.FormatStock(stock), startDate, endDate, isDownload);
            }
            set
            {
            }
        }

        public override List<EntryData> EntryDataList
        {
            get
            {
                List<EntryData> list = new List<EntryData>();

                foreach (DailyData daily in Dataset)
                {
                    foreach (EntryData entry in daily.set)
                    {
                        list.Add(entry);
                    }
                }
                return list;
            }
            set
            {
            }
        }

    }
}
