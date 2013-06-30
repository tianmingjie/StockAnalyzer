using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using SotckAnalyzer.data;

namespace SotckAnalyzer.analyzer
{
    public class CsvAnalyzer
    {
        public static DailyDataSet ReadCsv(string fileName)
        {
            DailyDataSet data = new DailyDataSet();
            data.set = new List<DailyData>();


            FileInfo fi = new FileInfo(fileName);

            string fName = fi.Name;
            data.stock = fName.Substring(2, 6);
            string date = fName.Substring(9, 10);
            data.date = DateTime.Parse(date);

            if (!File.Exists(fileName)) throw new Exception(fileName + " not exist!");
            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(
                                   new StreamReader(fileName), true))
            {

                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
               
                while (csv.ReadNextRecord())
                {
                    DailyData dd = new DailyData();
                    //for (int i = 0; i < fieldCount; i++)
                    //    Console.Write(string.Format("{0} = {1};",
                    //                  headers[i], csv[i]));
                    //Console.WriteLine();
                    dd.time = DateTime.Parse(date + " " + csv[0]);
                    dd.price = Decimal.Parse(csv[1]);
                    dd.change = Decimal.Parse(csv[2]);
                    dd.share = long.Parse(csv[3]);
                    dd.money = long.Parse(csv[4]);
                    dd.type = csv[5];

                    data.set.Add(dd);
                }
            }

            return data;

        }


        public static FilterData Filter(DailyDataSet set, string filter)
        {
            FilterData fd = new FilterData(set);

            IEnumerable<DailyData> querySet = from data in set.set where data.share > 400 select data;
            fd.setByBigDeal = querySet.ToList<DailyData>();
            //foreach (DailyData aa in querySet.ToList<DailyData>())
            //{
            //    Console.WriteLine(aa.share);
            //}
            return fd;
        }
        //   public static void ReadCsv(string fileName)
        //{


        //    if (!File.Exists(fileName)) throw new Exception(fileName + " not exist!");
        //    // open the file "data.csv" which is a CSV file with headers
        //    using (CsvReader csv = new CsvReader(
        //                           new StreamReader(fileName), true))
        //    {

        //        int fieldCount = csv.FieldCount;

        //        string[] headers = csv.GetFieldHeaders();
        //        while (csv.ReadNextRecord())
        //        {
        //            for (int i = 0; i < fieldCount; i++)
        //                Console.Write(string.Format("{0} = {1};",
        //                              headers[i], csv[i]));
        //            Console.WriteLine();
        //        }

        //        //TradeRecord tr = new TradeRecord();
        //        //tr.stockId = id;
        //        //tr.recordDate = date;
        //        //tr.recordTime = str[0];
        //        ////tr.recordTime.
        //        //tr.price = Decimal.Parse(str[1]);
        //        //tr.priceChange = Decimal.Parse(str[2] == "--" ? "0" : str[2]);
        //        //tr.number = long.Parse(str[3]);
        //        //tr.money = long.Parse(str[4]);
        //        //string t = str[5];
        //    }

        //    return data;

        //}
    }
}