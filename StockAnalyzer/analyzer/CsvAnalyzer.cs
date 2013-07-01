using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using SotckAnalyzer.data;
using Common;

namespace SotckAnalyzer.analyzer
{
    public class CsvAnalyzer
    {
        /// <summary>
        /// 分析一段时间的数据
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<DailyDataSet> ReadCsv(string stock, string startDate,string endDate)
        {
            List<DailyDataSet> dds = new List<DailyDataSet>(); ;
            for (DateTime dateTime = DateTime.Parse(startDate);
                 dateTime <= DateTime.Parse(endDate);
                 dateTime += TimeSpan.FromDays(1))
            {
                if (!(dateTime.Date.DayOfWeek == DayOfWeek.Saturday || dateTime.Date.DayOfWeek == DayOfWeek.Sunday)){
                    //Console.WriteLine(toDate(dateTime.Date));
                    DailyDataSet s=ReadCsv(stock, StockUtil.FormatDate(dateTime.Date));
                    if(s!=null){
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
        public static DailyDataSet ReadCsv(string stock, string date)
        {
            string filePath = Constant.ROOT_FOLDER + stock + @"\" + stock + "_" + date + ".csv";
            if (File.Exists(filePath))
            {
                return ReadCsv(filePath);
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 分析某一天的数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DailyDataSet ReadCsv(string fileName)
        {
            DailyDataSet data = new DailyDataSet();
            data.set = new List<DailyData>();


            FileInfo fi = new FileInfo(fileName);

            string fName = fi.Name;
            data.stock = fName.Substring(2, 6);
            string date = fName.Substring(9, 10);
            data.date = DateTime.Parse(date);

            //if (!File.Exists(fileName)) throw new Exception(fileName + " not exist!");
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
                        DailyData dd = new DailyData();
                        //for (int i = 0; i < fieldCount; i++)
                        //    Console.Write(string.Format("{0} = {1};",
                        //                  headers[i], csv[i]));
                        //Console.WriteLine();
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