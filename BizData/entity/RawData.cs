using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace big.entity
{
    [Serializable]
    public class RawData
    {
        public decimal price;
        public TimeSpan time;
        public decimal change;
        public double share;
        public double money;
        public string type;
    }
}
