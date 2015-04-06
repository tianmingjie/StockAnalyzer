using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace big.entity
{
    public class BigData
    {
        public string sid;
        public int rateToOpen;
        public int rateToChange;
        public int minutes;
        public int shares;
        public string type; //1-B,0-S
        public DateTime time;
        public int big;
    }
}
