using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using SotckAnalyzer.data;
using Common;
using DownloadData;
using log4net;

namespace SotckAnalyzer.reader
{
    public class Csv
    {

        private static readonly ILog LOG = LogManager.GetLogger(typeof(Csv));

        public static List<DailyData> ReadCsv(string stock, string startDate, string endDate,bool isDownload)
        {
            List<DailyData> dds = new List<DailyData>(); ;
            for (DateTime dateTime = DateTime.Parse(startDate);
                 dateTime <= DateTime.Parse(endDate);
                 dateTime += TimeSpan.FromDays(1))
            {
                if (!(dateTime.Date.DayOfWeek == DayOfWeek.Saturday || dateTime.Date.DayOfWeek == DayOfWeek.Sunday))
                {
                    //Console.WriteLine(toDate(dateTime.Date));
                    DailyData s = ReadCsv(stock, StockUtil.FormatDate(dateTime.Date),isDownload);
                    if (s != null)
                    {
                        dds.Add(s);
                    }
                }
            }
            return dds;
        }
        /// <summary>
        /// 分析某一天的数据
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DailyData ReadCsv(string stock, string date,bool isDownload)
        {
            string filePath = Constant.ROOT_FOLDER + stock + @"\" + stock + "_" + date + ".csv";
            if (File.Exists(filePath))
            {
                return ReadCsv(filePath,isDownload);
            }
            else
            {
                if (isDownload)
                {
                    DataDownload.DownloadDataToCsv(stock, date,date);
                    if (!File.Exists(filePath))
                    {
                        LOG.Info("Not Exist:" + filePath);
                        return null;
                    }
                    return ReadCsv(filePath, isDownload);
                }
                else
                {
                    return null;
                }
            }

        }
        /// <summary>
        /// 分析某一天的数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DailyData ReadCsv(string fileName,bool isDownload)
        {
            DailyData data = new DailyData();
            data.set = new List<EntryData>();


            FileInfo fi = new FileInfo(fileName);

            string fName = fi.Name;
            data.stock = StockUtil.RetrieveStock(fName);
            string date = StockUtil.RetrieveDate(fName);
            data.date = DateTime.Parse(date);


            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(
                                   new StreamReader(fileName), true))
            {
                int index = 0;

                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();

                decimal current;

                while (csv.ReadNextRecord())
                {
                    try
                    {
                        EntryData dd = new EntryData();
                        
                        current = Decimal.Parse(csv[1].Replace("\0", ""));

                        if (index == 0)
                        {
                            data.ClosePrice = current;
                            data.HighestPrice = current;
                            data.LowestPrice = current;
                        }

                        if (data.HighestPrice < current)
                        {
                            data.HighestPrice = current;
                            data.TimeWhenHighest = DateTime.Parse(date + " " + csv[0]);
                        }
                        if (data.LowestPrice > current)
                        {
                            data.LowestPrice = current;
                            data.TimeWhenLowest = DateTime.Parse(date + " " + csv[0]);
                        }

                        dd.time = DateTime.Parse(date + " " + csv[0]);
                        dd.price = current;
                        dd.change = Decimal.Parse(csv[2]);
                        dd.share = long.Parse(csv[3]);
                        dd.money = long.Parse(csv[4]);
                        dd.type = csv[5];

                        data.set.Add(dd);
                        data.OpenPrice = current;
                        index++;
                    }
                    catch
                    {
                        ///TODO 吞掉异常
                        ///
                        Console.WriteLine("Can't parse at line "+index + "  "+fileName);
                    }
                }

                //Console.WriteLine(fileName+" "+fieldCount + " " + index);
            }

            return data;

        }

    }
}