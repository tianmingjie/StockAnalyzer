using big;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyze
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                int[] list=new int[] { 3, 6, 12,24 };
                exec(list);
            }
            else
            {
                string[] strlist = args[0].Split(',');
                int[] list=new int[strlist.Length];
                for(int i=0;i<strlist.Length;i++){
                    list[i]=int.Parse(strlist[i]);
                }
                exec(list);
            }
            

        }
        public static void exec(int[] a)
        {
            DateTime now = DateTime.Now;
            string tag = now.ToString("yyyyMMdd");
            DateTime end = now.AddMonths(-1);
            DateTime start = new DateTime();
            foreach (int i in a)
            {
                start = end.AddMonths(-i);
                BizApi.InsertAnalyzeData(tag, start, end);
            }
        }
    }
}
