        using System;
using System.Data;
using System.IO;
using System.Web;
using System.Text;
using System.Configuration;
using System.Collections.Generic;

namespace Common.excel
{
    class ExcelEngine
    {



        //public static List<TradeRecord> ReadData(string filename)
        //{

        //    // check the input parameters
        //    if (filename == null) return null;

        //    if (!File.Exists(filename)) throw new Exception(filename + " not exist");

        //    FileInfo fi = new FileInfo(filename);

        //    string fName = fi.Name;
        //    string id = fName.Substring(2, 6);
        //    string date = fName.Substring(9, 10);
 
        //    List<TradeRecord> s = new List<TradeRecord>();

        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Open(filename, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    int max = xlWorkSheet.UsedRange.Count;
        //    Excel.Range r = xlWorkSheet.get_Range("A2", "A" + max);

        //    foreach (Excel.Range a in r.Cells)
        //    {
        //        String v = "";
        //        if (a.Value2 != null)
        //        {
        //            v = a.Value2 as string;

        //            //String[] str = v.Trim().Split(' ');

        //            v = System.Text.RegularExpressions.Regex.Replace(v.Trim(), "\\s+", " ");
        //            string[] str = System.Text.RegularExpressions.Regex.Split(v, " ");

        //            TradeRecord tr = new TradeRecord();
        //            tr.stockId = id;
        //            tr.recordDate=date;
        //            tr.recordTime =str[0];
        //            //tr.recordTime.
        //            tr.price = Decimal.Parse(str[1]);
        //            tr.priceChange = Decimal.Parse(str[2] == "--" ? "0" : str[2]);
        //            tr.number = decimal.Parse(str[3]);
        //            tr.money = decimal.Parse(str[4]);
        //            string t = str[5];
        //            switch (t)
        //            {
        //                case "买盘":
        //                    tr.type = "B";
        //                    break;
        //                case "卖盘":
        //                    tr.type = "S";
        //                    break;
        //                default:
        //                    tr.type = "Z";
        //                    break;

        //            }
        //            s.Add(tr);
        //        }
        //    }

        //    xlWorkBook.Close(true, misValue, misValue);
        //    xlApp.Quit();


        //    return s;
        //    //releaseObject(xlWorkSheet);
        //    //releaseObject(xlWorkBook);
        //    //releaseObject(xlApp);
        //}

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                StockLog.Log.Error(ex);
            }
            finally
            {
                GC.Collect();
            }
        }


    }
}

