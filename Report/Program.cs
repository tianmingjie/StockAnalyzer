using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleChartSharp;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Common;

namespace Report
{
    class Program
    {
        static void Main(string[] args)
        {
            //c1(@"D:\project\stock\analyzeData\sh600789\sh600789_2012-09-01_2013-08-08_500_Weekly.csv", "increDiffMoney");
            //c2(@"D:\project\stock\analyzeData\sh600789\sh600789_2012-09-01_2013-08-08_500_Weekly.csv", new String[] { "BuyShare", "SellShare" },"BuySellShare",ChartType.COLUMN);
            //c2(@"D:\project\stock\analyzeData\sh600789\sh600789_2012-09-01_2013-08-08_500_Weekly.csv", new String[] { "increDiffMoney" }, "increDiffMoney",ChartType.LINE);
            Console.WriteLine(GetString(1));
            //Console.WriteLine(Convert.ToChar(Convert.ToInt32('A')+3));

            DirectoryInfo directory=new DirectoryInfo(Constant.ANALYZE_FOLDER);
            foreach(DirectoryInfo d in directory.GetDirectories()){
                foreach(FileInfo f in d.GetFiles())
                {
                    if (f.Name.Contains("Daily") && f.Name.Contains("08-08") && f.Name.Contains("csv"))
                    {
                        ReportExcelToImage(f.FullName, new String[] { "BuyShare", "SellShare" },"BuySellShare",ChartType.COLUMN);
                        ReportExcelToImage(f.FullName, new String[] { "increDiffMoney" }, "increDiffMoney", ChartType.LINE);
                    }
                }

            }
        }

        private static void ReportExcelToImage(String filePath, String[] columnList,String title,ChartType chartType=ChartType.COLUMN)
        {
            FileInfo file = new FileInfo(filePath);
            String directory="";

            if (Constant.ANALYZE_CHART_DIR.Equals(""))
            {
                directory = file.DirectoryName;
            }
            else
            {
                directory = Constant.ANALYZE_CHART_DIR;
            }

            String fileName = file.Name.Substring(0,file.Name.IndexOf('.'));
            String imagePath = directory + @"\" + fileName +"-" +title + ".jpg";

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(filePath);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Excel.Range chartRange;

            Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(10, 80, 1000, 850);
            Excel.Chart chartPage = myChart.Chart;

            //non-empty count of csv file
            int count = Convert.ToInt32(xlApp.WorksheetFunction.CountA(xlWorkSheet.get_Range("A:A")));
            //non-empty count of header
            int headerCount = Convert.ToInt32(xlApp.WorksheetFunction.CountA(xlWorkSheet.Rows[1]));

            string chartString="A1:A"+count;
            foreach (var column in columnList)
            {
                int columnIndex = GetColumnIndex(xlWorkSheet, headerCount, column);
                chartString += "," + GetString(columnIndex) + "1:" + GetString(columnIndex) + count;
            }
            chartRange = xlWorkSheet.get_Range(chartString);

            chartPage.SetSourceData(chartRange, misValue);
            //chartPage.ChartType = Excel.XlChartType.xlColumnClustered;
            chartPage.ChartType = GetChartType(chartType);

            chartPage.Axes(Excel.XlAxisType.xlValue);
            //export chart as picture file
            chartPage.Export(imagePath, "JPG", misValue);

            xlWorkBook.Close(false, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }

        //private static void c1(String fileName,String column)
        //{
        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Open(fileName);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        //    Excel.Range chartRange;

        //    Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
        //    Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(10, 80, 600, 550);
        //    Excel.Chart chartPage = myChart.Chart;

        //    //non-empty count of csv file
        //    int count = Convert.ToInt32(xlApp.WorksheetFunction.CountA(xlWorkSheet.get_Range("A:A")));
        //    //non-empty count of header
        //    int line1 = Convert.ToInt32(xlApp.WorksheetFunction.CountA(xlWorkSheet.Rows[1]));


        //    //Excel.Range r = xlWorkSheet.get_Range(xlWorkSheet.Cells[1,1], xlWorkSheet.Cells[1,line1]);
        //    int columnIndex=GetColumnIndex(xlWorkSheet, line1, column);

        //    //chartRange = xlWorkSheet.get_Range("A1:A27, B1:B27, C1:C27");
        //    //chartRange = xlWorkSheet.get_Range(xlWorkSheet.Cells[1, columnIndex], xlWorkSheet.Cells[line, columnIndex]);
        //    chartRange = xlWorkSheet.get_Range("A1:A"+count+","+GetString(columnIndex)+"1:"+GetString(columnIndex)+count);

        //    chartPage.SetSourceData(chartRange, misValue);
        //    //chartPage.ChartType = Excel.XlChartType.xlColumnClustered;
        //    chartPage.ChartType = Excel.XlChartType.xlLine;

        //    chartPage.Axes(Excel.XlAxisType.xlValue);
        //    //export chart as picture file
        //    chartPage.Export(@"C:\excel_chart_export.jpg", "JPG", misValue);

        //    xlWorkBook.Close(false, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlWorkSheet);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlApp);

        //    Console.WriteLine("Excel file created , you can find the file");
        //}

        //private static void c(String fileName)
        //{
        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Open(fileName);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        //    Excel.Range chartRange;

        //    Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
        //    Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(10, 80, 600, 550);
        //    Excel.Chart chartPage = myChart.Chart;

        //    int line=Convert.ToInt32(xlApp.WorksheetFunction.CountA(xlWorkSheet.get_Range("A:A")));

        //    int line1 =Convert.ToInt32( xlApp.WorksheetFunction.CountA(xlWorkSheet.Rows[1]));

        //    //Excel.Range r = xlWorkSheet.get_Range(xlWorkSheet.Cells[1,1], xlWorkSheet.Cells[1,line1]);
        //    Console.WriteLine(GetColumnIndex(xlWorkSheet, line1, "Date"));
            
        //    chartRange = xlWorkSheet.get_Range("C1", "C27");

        //    chartPage.SetSourceData(chartRange, misValue);
        //    chartPage.ChartType = Excel.XlChartType.xlColumnClustered;

        //    chartPage.Axes(Excel.XlAxisType.xlValue);
        //    //export chart as picture file
        //    chartPage.Export(@"C:\excel_chart_export.jpg", "JPG", misValue);

        //    xlWorkBook.Close(false, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlWorkSheet);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlApp);

        //    Console.WriteLine("Excel file created , you can find the file");
        //}

        private static int GetColumnIndex(Excel.Worksheet xlWorkSheet, int size, String columnName)
        {
            for (int i = 1; i < size + 1; i++)
            {
                String name = xlWorkSheet.Cells[1, i].Value;
                Console.WriteLine(name+" "+i);
                if (columnName.Trim().Equals(name.ToString().Trim()))
                    return i;          
            }
            return -1;
        }

        private static Excel.XlChartType GetChartType(ChartType type)
        {
            if (type == ChartType.LINE)
            {
                return Excel.XlChartType.xlLine;
            }
            if (type == ChartType.COLUMN)
            {
                return Excel.XlChartType.xlColumnClustered;
            }
            return Excel.XlChartType.xlColumnClustered;
        }
        private static String GetString(int index)
        {
            return Convert.ToChar(Convert.ToInt32('A') + index-1).ToString() ;
        }
        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

    }

    enum ChartType
    {
        LINE=0,
        COLUMN=1
    }
}

