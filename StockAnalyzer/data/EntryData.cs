using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SotckAnalyzer.data
{
    /// <summary>
    /// 一条记录
    /// </summary>
    public class EntryData
    {

        //public string stock;
        public DateTime time;
        public decimal price;
        public decimal change;
        public long share;
        public long money;
        public string type;
    }
}
