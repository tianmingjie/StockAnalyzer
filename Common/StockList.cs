using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using System.IO;

namespace Common
{
    public class StockList
    {

        public static List<string> StockList
        {
            get
            {
                // open the file "data.csv" which is a CSV file with headers
                using (CsvReader csv = new CsvReader(
                                       new StreamReader(Constant.STOCK_FILE), true))
                {
                    List<string> l = new List<string>();
                    while (csv.ReadNextRecord())
                    {
                        //Console.WriteLine(csv[0]);
                        l.Add(csv[0]);
                    }
                    return l;
                }
            }

        }


        public static List<string> AnalyzeList
        {
            get
            {
                // open the file "data.csv" which is a CSV file with headers
                using (CsvReader csv = new CsvReader(
                                       new StreamReader(Constant.ANALYZE_FILE), true))
                {
                    List<string> l = new List<string>();
                    while (csv.ReadNextRecord())
                    {
                        //Console.WriteLine(csv[0]);
                        l.Add(csv[0]);
                    }
                    return l;
                }
            }
        }


    }
}
