using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class StockUtil
    {
        public static string FormatDate(DateTime date)
        {
            string d = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
            string m = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();
            return date.Year + "-" + m + "-" + d;
        }

        public static string FormatTime(DateTime date)
        {
            string h = date.Hour < 10 ? "0" + date.Hour.ToString() : date.Hour.ToString();
            string m = date.Minute < 10 ? "0" + date.Minute.ToString() : date.Minute.ToString();
            string s = date.Second < 10 ? "0" + date.Second.ToString() : date.Second.ToString();
            return h + ":" + m + ":" + s;
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


    }
}
