using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace big.entity
{
    [DataContract]
    public class LineData
    {
        //[DataMember]
        //public string sid;
        [DataMember]
        public string tag;
        [DataMember]
        public decimal open;
        [DataMember]
        public decimal close;
        [DataMember]
        public decimal high;
        [DataMember]
        public decimal low;

        [DataMember]
        public double totalshare;

        [DataMember]
        public double totalmoney;

        [DataMember]
        public double buyshare;

        [DataMember]
        public double sellshare;
    }
}
