using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Common
{
    public class StockUtil
    {

        //public static List<string> StockList
        //{
        //    get
        //    {
        //        // open the file "data.csv" which is a CSV file with headers
        //        using (CsvReader csv = new CsvReader(
        //                               new StreamReader(Constant.STOCK_FILE), true))
        //        {
        //            List<string> l = new List<string>();
        //            while (csv.ReadNextRecord())
        //            {
        //                //Console.WriteLine(csv[0]);
        //                l.Add(csv[0]);
        //            }
        //            return l;
        //        }
        //    }

        //}

        public static List<StockInfo> StockList
        {
            get
            {
                // open the file "data.csv" which is a CSV file with headers
                using (CsvReader csv = new CsvReader(
                                       new StreamReader(Constant.STOCK_FILE), true))
                {
                    List<StockInfo> l = new List<StockInfo>();
                    while (csv.ReadNextRecord())
                    {
                        //Console.WriteLine(csv[0]);
                        l.Add(new StockInfo(){ 
                            stock=csv[0]});
                    }
                    return l;
                }
            }

        }

        public static List<string> UpdateList
        {
            get
            {
              return  FileUtil.ReadFile(Constant.AUTO_DOWNLOAD_FILE).Split(',').ToList<string>();
            }

        }

        //public static List<string> AnalyzeList
        //{
        //    get
        //    {
        //        // open the file "data.csv" which is a CSV file with headers
        //        using (CsvReader csv = new CsvReader(
        //                               new StreamReader(Constant.ANALYZE_FILE), true))
        //        {
        //            List<string> l = new List<string>();
        //            while (csv.ReadNextRecord())
        //            {
        //                //Console.WriteLine(csv[0]);
        //                l.Add(csv[0]);
        //            }
        //            return l;
        //        }
        //    }
        //}

        public static List<StockInfo> AnalyzeList
        {
            get
            {
                // open the file "data.csv" which is a CSV file with headers
                using (CsvReader csv = new CsvReader(
                                       new StreamReader(Constant.ANALYZE_FILE), true))
                {
                    List<StockInfo> l = new List<StockInfo>();
                    while (csv.ReadNextRecord())
                    {
                        //Console.WriteLine(csv[0]);
                        l.Add(new StockInfo(){ 
                            isAnalyze=csv[0].Trim().Equals("0")?false:true,
                            stock=csv[1],
                            filter=csv[3].Equals("")?Constant.BIG_DEAL:csv[3]
                        });
                    }
                    return l;
                }
            }
        }


        public static decimal FormatRate(decimal t)
        {
            return Math.Round(t, 4);
        }
        public static string FormatDate(DateTime date)
        {
            string d = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
            string m = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();
            return date.Year + "-" + m + "-" + d;
        }

        public static int FormatDateToNumber(DateTime date)
        {
            string d = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
            string m = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();
            return Int32.Parse(date.Year + "" + m + "" + d);
        }

        public static string FormatTime(DateTime date)
        {
            string h = date.Hour < 10 ? "0" + date.Hour.ToString() : date.Hour.ToString();
            string m = date.Minute < 10 ? "0" + date.Minute.ToString() : date.Minute.ToString();
            string s = date.Second < 10 ? "0" + date.Second.ToString() : date.Second.ToString();
            return h + ":" + m + ":" + s;
        }
        public static string FormateNumber(int a)
        {
            return a > 9 ? a.ToString() : "0" + a.ToString();
        }
        public static string FormatAllTime(DateTime date,bool isHour=false)
        {
            if (isHour)
            {
                return date.Year.ToString().Substring(2) + FormateNumber(date.Month) + FormateNumber(date.Day) + FormateNumber(date.Hour) + FormateNumber(date.Minute);

            }
            else
            {
                return date.Year.ToString().Substring(2) + FormateNumber(date.Month) + FormateNumber(date.Day);
            }
        }

        public static string FormatStock(string stock)
        {
            if (stock.StartsWith("sh") || stock.StartsWith("sz"))
            {
                return stock;
            }
            else
            { return Int32.Parse(stock) > 599999 ? "sh" + stock : "sz" + stock; }
        }
        /// <summary>
        ///  filename is sh600836-2013-10-10
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string RetrieveStock(string fileName)
        {
            return fileName.ToLower().StartsWith("sh")|| fileName.ToLower().StartsWith("sz")?fileName.ToLower().Substring(2,6):fileName.ToLower().Substring(0,6);
        }

        /// <summary>
        /// filename is sh600836-2013-10-10
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string RetrieveDate(string fileName)
        {
            return fileName.ToLower().StartsWith("sh") || fileName.ToLower().StartsWith("sz") ? fileName.ToLower().Substring(9, 10) : fileName.ToLower().Substring(7, 10);
        }

        public static decimal FormatChange(decimal change)
        {
            return change > 0 ? change : -change ;
        }

        public static bool UpdateDownloadTimeStamp(string stock,string updateDate)
        {
            string fullPath = String.Format(Constant.UPDATE_FILE, stock);
            if (!File.Exists(fullPath)) File.Create(fullPath).Close();
            FileUtil.WriteFile(fullPath, updateDate);
            return true;
        }

        public static string ReadUpdateFile(string stock)
        {
            string fullPath = String.Format(Constant.UPDATE_FILE, stock);
            if (File.Exists(fullPath))
            {
                string t = FileUtil.ReadFile(fullPath);
                return t.Trim().Equals("") ? Constant.ANALYZE_START_DATE: t ;
            }
            else
                return Constant.ANALYZE_START_DATE;
        }


    }
}
