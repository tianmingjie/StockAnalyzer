using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace SotckAnalyzer.data
{
    public static class GeneralData
    {

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
