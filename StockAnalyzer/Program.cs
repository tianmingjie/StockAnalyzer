using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using SotckAnalyzer.data;
using SotckAnalyzer.analyzer;
using Common;

namespace SotckAnalyzer
{
    static class Program
    {


        static String connectionString = "server=127.0.0.1;user id=root; password=as; database=albertsong; pooling=false;charset=utf8";
        //static string Constant.ROOT_FOLDER = Constant.ROOT_FOLDER;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //generateAll();
            //queryDailyData();

            //
            //MySqlConnection conn = MySQL.GetConnection();
            //conn.Open();
            //foreach (DirectoryInfo dir in new DirectoryInfo(@"D:\project\stock\data").GetDirectories())
            //{
            //    foreach (FileInfo fi in dir.GetFiles())
            //    {
            //        Console.WriteLine(fi.FullName);
            //        List<TradeRecord> l = ExcelEngine.ReadData(fi.FullName);
            //        MySQL.InsertDailyRecord(conn, l);
            //    }
            //}

            //MySQL mysql = new MySQL();


            //conn.Close();

            // conectMySQL();
            //for (int i = 600000; i < 600210; i++)
            //{
 
            //    DataDownload.downloadDataToCsv("sh"+i, "2013-05-01", "2013-06-28");
            //}
           // DataDownload.downloadDataToCsv("sh600038", "2013-05-20", "2013-06-28");
            //DailyDataSet set = CsvAnalyzer.ReadCsv(@"D:\project\stock\data\sh600038\sh600038-2013-05-20.csv");
            //FilterData fd = new FilterData(set, "<6");
            //fd.Analye();
            //Console.WriteLine(fd.setByBigDeal.Count);

            string stock = "sh600808";
            string startDate = "2013-01-01";
            string endDate = "2013-06-30";
            string filter = "1000";
            List<DailyDataSet> dds = CsvAnalyzer.ReadCsv(stock, startDate, endDate);
            StringBuilder str = new StringBuilder();
            str.Append("date,bigBuyShare,bigSellShare,toalShare,bigBuyMoney,bigSellMoney,toalMoney,Open,Close,Average,Hightest,WhenHighest,Lowest,WhenLowest,BigBuyShareRate,BigSellShareRate,BigBuyMoneyRate,BigSellMoneyRate\n");
            foreach (DailyDataSet ds in dds)
            {

                FilterData fd = new FilterData(ds, filter);
                fd.Analye();
                str.Append(StockUtil.FormatDate(fd.set.date) + ",");
                str.Append(fd.TotalBuyShareByBigDeal + ",");
                str.Append(fd.TotalSellShareByBigDeal + ",");
                str.Append(fd.set.TotalShare + ",");
                str.Append(fd.TotalBuyMoneyByBigDeal + ",");
                str.Append(fd.TotalSellMoneyByBigDeal + ",");
                str.Append(fd.set.TotalMoney + ",");
                str.Append(fd.set.OpenPrice + ",");
                str.Append(fd.set.ClosePrice + ",");
                str.Append(fd.set.Average + ",");
                str.Append(fd.set.HighestPrice + ",");
                str.Append(StockUtil.FormatTime(fd.set.TimeWhenHighest) + ",");
                str.Append(fd.set.LowestPrice + ",");
                str.Append(StockUtil.FormatTime(fd.set.TimeWhenLowest) + ",");
                str.Append(fd.RateOfBuyShareByTotal + ",");
                str.Append(fd.RateOfSellShareByTotal + ",");
                str.Append(fd.RateOfBuyMoneyByTotal + ",");
                str.Append(fd.RateOfSellMoneyByTotal + ",");
                str.Append("\n");

            }

            string analyzePath = Constant.ANALYZE_FOLDER + stock + "_" + startDate + "_" + endDate + "_" + filter + ".csv";

            if (File.Exists(analyzePath))
            {
                File.Delete(analyzePath);
            }
            using (StreamWriter outfile = new StreamWriter(Constant.ANALYZE_FOLDER + stock + "_" + startDate+"_"+ endDate+"_"+filter+ ".csv"))
            {
                outfile.Write(str);
                Console.WriteLine("Analyzed: " + Constant.ANALYZE_FOLDER + stock + "_" + startDate + "_" + endDate + "_" + filter + ".csv");
            }

            
        }

        public static void conectMySQL()
        {
            string connStr = "server=localhost;user=root;database=test;port=3306;password=Password1;";

            MySqlConnection conn = new MySqlConnection(connStr);

            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();

            try
            {
                conn.Open();
                cmd.Connection = conn;

                cmd.CommandText = "insert into test(id,data) values(?id,?data1)";


                cmd.Parameters.AddWithValue("?id", "1");
                cmd.Parameters.AddWithValue("?data1", "0.01");

                for (int i = 0; i < 10; i++)
                {

                    cmd.Parameters["?id"].Value = i;
                    cmd.Parameters["?data1"].Value = i + 0.01;
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("nDone.");
        }

        public static void queryDailyData()
        {
            WebClient client = new WebClient();
            byte[] data = client.DownloadData("http://hq.sinajs.cn/list=sh601006");

            string[] dataArray = System.Text.Encoding.GetEncoding(936).GetString(data).Replace('"', ',').Split(',');
            //dataArray[0]=
            String[] retData = new String[32];
            for (int i = 0; i < 32; i++)
                retData[i] = dataArray[i + 1];
            Console.WriteLine(retData[0]);

            //return retData;
        }





    }
}
