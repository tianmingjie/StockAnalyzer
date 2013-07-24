using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.data;
using Common;

namespace SotckAnalyzer.reader
{
    public class Printer
    {
         public static string PrintEntryData(EntryData e,bool includeHeader=false)
        {
            StringBuilder sb = new StringBuilder();
            if(includeHeader)sb.Append("time,price,change,share,money,type");
            sb.Append(String.Format("{0},{1},{2},{3},{4},{5}",e.time,e.price,e.money,e.change,e.share,e.type));

#if DEBUG
            Console.WriteLine(sb.ToString());
#endif
            return sb.ToString();
        }
    }
}
