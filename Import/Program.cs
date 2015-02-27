using big;
using big.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //Console.WriteLine(BizApi.QueryExtractLastUpdate("sz000830", 2000));
            // decimal aaa=12345.6789999M;
            //string folder = @"D:\temp\test\data";

            //string[] folders=Directory.GetDirectories(folder);
            //for (int i=0;i<folders.Length;i++)
            //{
            //    Console.WriteLine(folders[i].Substring(folders[i].LastIndexOf("\\")+1));
            //}
            //Console.WriteLine(BizApi.QueryWeight("sz000831"));
            //Console.WriteLine(BizApi.QueryWeight("sz000001"));
            //List<BasicData> list = ImportRawData.ReadCsvFolder(@"D:\workspace\myproject\sz000830", 2000);
            //foreach (BasicData bd in list)
            //{
            //    //Console.WriteLine(bd.time+" "+bd.sellshare);
            //    BizApi.InsertBasicData(bd);
            //}

             Import.ImportRawData.Import(args[0]);
            //List<BasicData> list=Import.ImportRawData.ReadCsvFile(@"D:\temp\test\data\bad\sh600000_2012-09-03.csv", "sh600000", new decimal[] {  1000,2000 });
            //Console.WriteLine(list.Count);
            //BizApi.CreateDataTable("sz000005");
           
        }
    }
}
