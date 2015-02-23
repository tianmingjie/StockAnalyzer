using big;
using big.entity;
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
                Console.WriteLine(sid + " start ");
                DateTime beginning = DateTime.Now;
                BizApi.CreateDataTable(sid);

                try
                {
                    int weight = BizApi.QueryWeight(sid);
                    List<BasicData> list = ReadCsvFolder(folders[i], sid, new int[] { 500 * weight, 1000 * weight, 2000 * weight });

                    foreach (BasicData bd in list)
                    {
                        //Console.WriteLine(bd.time+" "+bd.sellshare);
                        BizApi.InsertBasicData(bd);
                    }
                    TimeSpan end = DateTime.Now - beginning;
                    Console.WriteLine(sid + " complete at " + end);
                    Console.WriteLine("==========================");
                }
                catch (Exception e) { throw e; }
            }

        }
        public static BasicData ReadCsvFile(string fileName, int big)
        {
            FileInfo f = new FileInfo(fileName);
            //如果文件不存在就返回null
            if (!f.Exists) return null;

            BasicData bd = new BasicData();
            //String fileName = "sz000830_2012-09-03.csv";
            string sid = f.Name.Substring(0, 8);
            string time = f.Name.Substring(9, 10);

            bd.time = new DateTime(Int32.Parse(time.Substring(0, 4)), Int32.Parse(time.Substring(5, 2)), Int32.Parse(time.Substring(8, 2)));
            bd.sid = sid;
            bd.c_type = "d";
            bd.big = big;

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(
                                   new StreamReader(fileName), true))
            {

                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();

                //颠倒记录
                IEnumerable<string[]> hi = csv.Reverse<string[]>();
                {

                    foreach (String[] record in hi)
                    {
                        Decimal price = Decimal.Parse(record[1].IndexOf("\0") > 0 ? record[1].Remove(record[1].IndexOf("\0")) : record[1]);
                        Double share = Double.Parse(record[3].IndexOf("\0") > 0 ? record[1].Remove(record[3].IndexOf("\0")) : record[3]);

                        bd.totalshare += share;
                        bd.totalmoney += Decimal.Multiply(price, (Decimal)share);

                        if (Int32.Parse(record[3]) > big)
                        {

                            if (record[5] == "S")
                            {
                                bd.sellshare += share;
                                bd.sellmoney += Decimal.Multiply(price, (Decimal)share);
                            }
                            if (record[5] == "B")
                            {
                                bd.buyshare += share;
                                bd.buymoney += Decimal.Multiply(price, (Decimal)share);
                            }

                        }

                    }
                }
                return bd;
            }
        }

        public static List<BasicData> ReadCsvFile(string fileName, string sid, int[] bigs)
        {
            List<BasicData> list = new List<BasicData>();
            FileInfo f = new FileInfo(fileName);
            //如果文件不存在就返回null
            if (!f.Exists) return null;


            //String fileName = "sz000830_2012-09-03.csv";
            //string sid = f.Name.Substring(0, 8);
            string time = f.Name.Substring(9, 10);

            DateTime t = new DateTime(Int32.Parse(time.Substring(0, 4)), Int32.Parse(time.Substring(5, 2)), Int32.Parse(time.Substring(8, 2)));



            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(
                                   new StreamReader(fileName), true))
            {
                foreach (int big in bigs)
                {
                    BasicData bd = new BasicData();
                    bd.time = t;
                    bd.sid = sid;
                    bd.c_type = "d";
                    bd.big = big;
                    int fieldCount = csv.FieldCount;
                    string[] headers = csv.GetFieldHeaders();

                    //颠倒记录
                    IEnumerable<string[]> hi = csv.Reverse<string[]>();
                    {

                        foreach (String[] record in hi)
                        {
                            Decimal price = Decimal.Parse(record[1].IndexOf("\0") > 0 ? record[1].Remove(record[1].IndexOf("\0")) : record[1]);
                            Double share = Double.Parse(record[3].IndexOf("\0") > 0 ? record[1].Remove(record[3].IndexOf("\0")) : record[3]);

                            bd.totalshare += share;
                            bd.totalmoney += Decimal.Multiply(price, (Decimal)share);

                            if (Int32.Parse(record[3]) > big)
                            {

                                if (record[5] == "S")
                                {
                                    bd.sellshare += share;
                                    bd.sellmoney += Decimal.Multiply(price, (Decimal)share);
                                }
                                if (record[5] == "B")
                                {
                                    bd.buyshare += share;
                                    bd.buymoney += Decimal.Multiply(price, (Decimal)share);
                                }

                            }

                        }
                    }
                    list.Add(bd);
                }
                return list;
            }
        }

        public static List<BasicData> ReadCsvFolder(string folder, string sid, int[] bigs)
        {
            if (!Directory.Exists(folder)) throw new Exception(folder + " not exist!!!");

            List<BasicData> list = new List<BasicData>();

            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith(".csv"))
                {
                    list = list.Concat(ReadCsvFile(file, sid, bigs)).ToList();
                }
            }
            return list;
        }

        public static List<BasicData> ReadCsvFolder(string folder, int big)
        {
            if (!Directory.Exists(folder)) throw new Exception(folder + " not exist!!!");

            List<BasicData> list = new List<BasicData>();

            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith(".csv"))
                {
                    list.Add(ReadCsvFile(file, big));
                }
            }
            return list;
        }
    }


}
