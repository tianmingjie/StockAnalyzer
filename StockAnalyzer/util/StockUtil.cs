using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SotckAnalyzer.util
{
   public  class StockUtil
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
    }
}
