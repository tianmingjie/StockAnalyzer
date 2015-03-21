using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace big
{
    public class BizCommon
    {

        //dateforme YYYYMMDD

        public static DateTime ParseToDate(string format)
        {
            if (format.Length != 8) throw new Exception("Time format is wrong!!!");
            return new DateTime(Int32.Parse(format.Substring(0, 4)), Int32.Parse(format.Substring(4, 2)), Int32.Parse(format.Substring(6, 2)));
        }

        public static string ParseToString(DateTime format)
        {
            return format.ToString("yyyyMMdd");
        }

        public static string ProcessStockId(string sid)
        {
            return sid.Length == 8 ? sid.Substring(2, 6) : sid;
        }

        public static string ProcessWeekend(string tag)
        {
            DateTime current = ParseToDate(tag);
            if (current.DayOfWeek == DayOfWeek.Saturday)
            {
                current=current.AddDays(-1);
            }
            if(current.DayOfWeek==DayOfWeek.Sunday){
                current = current.AddDays(-2);
            }
            return ParseToString(current);
        }
    }
}
