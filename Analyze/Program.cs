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
            if(args.Length==0)
            BizApi.InsertAnalyzeData(new DateTime(2014, 1, 1), DateTime.Now);
            else
            BizApi.InsertAnalyzeData(BizCommon.ParseToDate(args[0]), DateTime.Now);
        }
    }
}
