using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace big.entity
{
    [Serializable]
    [DataContract]
    public class BigData
    {
        [DataMember]
        public string sid;
        [DataMember]
        public int rateToOpen;
        [DataMember]
        public int rateToChange;
        [DataMember]
        public int minutes;
        [DataMember]
        public int shares;
        [DataMember]
        public string type; //1-B,0-S
        [DataMember]
        public DateTime time;
        [DataMember]
        public int big;
    }
}
