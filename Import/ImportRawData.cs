using big;
using big.entity;
using Common;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    //http://market.finance.sina.com.cn/downxls.php?date=2011-07-08&symbol=sh600900


    public class ImportRawData
    {
        public static void Import(string folder)
        {

            string[] folders = Directory.GetDirectories(folder);
            for (int i = 0; i < folders.Length; i++)
            {
                string sid = folders[i].Substring(folders[i].LastIndexOf("\\") + 1);
                StockLog.Log.Debug(sid + " start ");
                DateTime beginning = DateTime.Now;
                BizApi.CreateDataTable(sid);

                try
                {
                    //decimal weight = BizApi.QueryWeight(sid);
                    decimal[] extractlist = BizApi.QueryExtractList(sid);
                    DateTime lastupdate = BizApi.QueryExtractLastUpdate(sid);
                    StockLog.Log.Debug(" last update is: " + lastupdate);
                    List<BasicData> list = ReadCsvFolder(folders[i], sid, extractlist, lastupdate);

                    foreach (BasicData bd in list)
                    {
                        //Console.WriteLine(bd.time+" "+bd.sellshare);
                        BizApi.InsertBasicData(bd);
                    }
                    TimeSpan end = DateTime.Now - beginning;
                    StockLog.Log.Debug(sid + " complete at " + end);
                }
                catch {
                    StockLog.Log.Error(sid + " import fail");
                }
            }

        }

        public static void ImportFileByReader(string sid, string date,TextReader reader)
        {
            //string sid = file.Substring(file.LastIndexOf("\\") + 1);
            StockLog.Log.Debug(sid + " start ");
            DateTime beginning = DateTime.Now;
            BizApi.CreateDataTable(sid);
            string file = sid + "_" + date;
            try
            {
                //decimal weight = BizApi.QueryWeight(sid);
                decimal[] extractlist = BizApi.QueryExtractList(sid);
                DateTime lastupdate = BizApi.QueryExtractLastUpdate(sid);

                List<BasicData> list = ReadCsvByReader(sid, date, reader, extractlist, lastupdate);

                foreach (BasicData bd in list)
                {
                    //Console.WriteLine(bd.time+" "+bd.sellshare);
                    BizApi.InsertBasicData(bd);
                }
                TimeSpan end = DateTime.Now - beginning;

                StockLog.Log.Debug(file + " complete at " + end);
            }
            catch {
                StockLog.Log.Error(file + " import fail"); 
            }

        }


        public static void ImportFile(string sid, string file)
        {
            //string sid = file.Substring(file.LastIndexOf("\\") + 1);
            StockLog.Log.Debug(sid + " start ");
            DateTime beginning = DateTime.Now;
            BizApi.CreateDataTable(sid);

            try
            {
                //decimal weight = BizApi.QueryWeight(sid);
                 decimal[] extractlist = BizApi.QueryExtractList(sid);
                DateTime lastupdate = BizApi.QueryExtractLastUpdate(sid);

                List<BasicData> list = ReadCsvFile(file, sid,  extractlist, lastupdate);

                foreach (BasicData bd in list)
                {
                    //Console.WriteLine(bd.time+" "+bd.sellshare);
                    BizApi.InsertBasicData(bd);
                }
                TimeSpan end = DateTime.Now - beginning;
                StockLog.Log.Debug(file + " complete at " + end);
            }
            catch
            {
                StockLog.Log.Error(file + " import fail");
            }
        
        }


        public static List<BasicData> ReadCsvByReader(string sid, string date, TextReader stream, decimal[] bigs, DateTime lastupdate)
        {
            int index = 0;

            List<BasicData> list = new List<BasicData>();
            BasicData[] array = new BasicData[bigs.Length];

            if (date.Length != 10) throw new Exception("time format is wrong -" + date);
            string time = date;
            DateTime t = new DateTime(Int32.Parse(time.Substring(0, 4)), Int32.Parse(time.Substring(5, 2)), Int32.Parse(time.Substring(8, 2)));

            //处理过了就忽略
            if (t < lastupdate.AddDays(1)) return null;

            for (int j = 0; j < bigs.Length; j++)
            {
                array[j] = new BasicData();
                array[j].big = (int)bigs[j];
                array[j].time = t;
                array[j].sid = sid;
                array[j].c_type = "d";
            }

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(stream, true))
            {  //颠倒记录
                IEnumerable<string[]> hi = csv.Reverse<string[]>();

                decimal current;

                decimal open = 0, close = 0, high = 0, low = 0;

                foreach (String[] record in hi)
                {
                    string price_str = record[1];
                    string share_str = record[3];
                    string type_str = record[5];
                    try
                    {

                        Decimal price = Decimal.Parse(price_str.IndexOf("\0") > 0 ? price_str.Remove(price_str.IndexOf("\0")) : price_str);
                        Double share = Double.Parse(share_str.IndexOf("\0") > 0 ? share_str.Remove(share_str.IndexOf("\0")) : share_str);

                        //count close,open,low,high
                        current = price;
                        if (index == 0)
                        {
                            open = current;
                            high = current;
                            low = current;
                        }
                        if (high < current) high = current;
                        if (low > current) low = current;
                        close = current;
                        index++;

                        for (int k = 0; k < bigs.Length; k++)
                        {

                            int fieldCount = csv.FieldCount;
                            string[] headers = csv.GetFieldHeaders();

                            array[k].open = open;
                            array[k].close = close;
                            array[k].high = high;
                            array[k].low = low;
                            array[k].totalshare += share;
                            array[k].totalmoney += Decimal.Multiply(price, (Decimal)share);

                            if (Int32.Parse(share_str) >= bigs[k])
                            {

                                if (type_str == "S")
                                {
                                    array[k].sellshare += share;
                                    array[k].sellmoney += Decimal.Multiply(price, (Decimal)share);
                                }
                                if (type_str == "B")
                                {
                                    array[k].buyshare += share;
                                    array[k].buymoney += Decimal.Multiply(price, (Decimal)share);
                                }
                            }
                        }
                    }
                    catch
                    {
                        StockLog.Log.Error(sid+"_"+date + " import fail");
                    }
                }
            }
            for (int j = 0; j < bigs.Length; j++)
            {
                list.Add(array[j]);
            }
            return list;
        }

        public static List<BasicData> ReadCsvFile(string fileName, string sid, decimal[] bigs, DateTime lastupdate)
        {
            int index = 0;

            List<BasicData> list = new List<BasicData>();
            BasicData[] array = new BasicData[bigs.Length];


            FileInfo f = new FileInfo(fileName);
            //如果文件不存在就返回null
            if (!f.Exists) return null;

            //String fileName = "sz000830_2012-09-03.csv";
            //string sid = f.Name.Substring(0, 8);
            string time = f.Name.Substring(9, 10);
            DateTime t = new DateTime(Int32.Parse(time.Substring(0, 4)), Int32.Parse(time.Substring(5, 2)), Int32.Parse(time.Substring(8, 2)));

            //处理过了就忽略
            if (t < lastupdate.AddDays(1)) return null;

            for (int j = 0; j < bigs.Length; j++)
            {
                array[j] = new BasicData();
                array[j].big = (int)bigs[j];
                array[j].time = t;
                array[j].sid = sid;
                array[j].c_type = "d";
            }

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
            {  //颠倒记录
                IEnumerable<string[]> hi = csv.Reverse<string[]>();

                decimal current;

                decimal open = 0, close = 0, high = 0, low = 0;

                foreach (String[] record in hi)
                {
                    string price_str = record[1];
                    string share_str = record[3];
                    string type_str = record[5];
                    try
                    {

                        Decimal price = Decimal.Parse(price_str.IndexOf("\0") > 0 ? price_str.Remove(price_str.IndexOf("\0")) : price_str);
                        Double share = Double.Parse(share_str.IndexOf("\0") > 0 ? share_str.Remove(share_str.IndexOf("\0")) : share_str);

                        //count close,open,low,high
                        current = price;
                        if (index == 0)
                        {
                            open = current;
                            high = current;
                            low = current;
                        }
                        if (high < current) high = current;
                        if (low > current) low = current;
                        close = current;
                        index++;

                        for (int k = 0; k < bigs.Length; k++)
                        {

                            int fieldCount = csv.FieldCount;
                            string[] headers = csv.GetFieldHeaders();

                            array[k].open = open;
                            array[k].close = close;
                            array[k].high = high;
                            array[k].low = low;
                            array[k].totalshare += share;
                            array[k].totalmoney += Decimal.Multiply(price, (Decimal)share);

                            if (Int32.Parse(share_str) >= bigs[k])
                            {

                                if (type_str == "S")
                                {
                                    array[k].sellshare += share;
                                    array[k].sellmoney += Decimal.Multiply(price, (Decimal)share);
                                }
                                if (type_str == "B")
                                {
                                    array[k].buyshare += share;
                                    array[k].buymoney += Decimal.Multiply(price, (Decimal)share);
                                }
                            }
                        }
                    }
                    catch {
                        StockLog.Log.Error(fileName + " import fail");
                    }
                }
            }
            for (int j = 0; j < bigs.Length; j++)
            {
                list.Add(array[j]);
            }
            return list;
        }

        public static List<BasicData> ReadCsvFolder(string folder, string sid, decimal[] bigs, DateTime lastupdate)
        {
            if (!Directory.Exists(folder)) throw new Exception(folder + " not exist!!!");

            List<BasicData> list = new List<BasicData>();

            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith(".csv"))
                {
                    List<BasicData> tmp = ReadCsvFile(file, sid, bigs, lastupdate);
                    if (tmp != null)
                        list = list.Concat(tmp).ToList();
                }
            }
            return list;
        }

    }


}
