using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DownloadData
{
    public class Download
    {
        public static void Main(string[] args)
        {
           FileUtil.WriteFile(@"D:\project\stock\n.txt",StockUtil.FormatDate(DateTime.Now));

        }



        public static void PrintHelp()
        {
            Console.WriteLine("Usgae: DownloadData stock [StartDate] [EndDate]\n\r:StartDate format:YYYY-MM-DD");

        }
    }
}
