using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DownloadData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string stock;
            string startDate;
            string endDate;
            switch (args.Length)
            {
                case 0:
                    PrintHelp();
                    throw new Exception();
                    break;
                case 1:
                    stock = StockUtil.FormatStock(args[0]);
                    startDate = StockUtil.FormatDate(DateTime.Now);
                    DataDownload.DownloadDataToCsv(stock, startDate);
                    break;
                case 2:
                     stock = StockUtil.FormatStock(args[0]);
                     startDate = args[1];
                     DataDownload.DownloadDataToCsv(stock, startDate);
                    break;

                case 3:
                     stock = StockUtil.FormatStock(args[0]);
                     startDate = args[1];
                     endDate = args[2];
                     DataDownload.DownloadDataToCsv(stock, startDate,endDate);
                    break;
                default:
                    PrintHelp();
                    break;
            }
        }



        public static void PrintHelp()
        {
            Console.WriteLine("Usgae: DownloadData stock [StartDate] [EndDate]\n\r:StartDate format:YYYY-MM-DD");

        }
    }
}
