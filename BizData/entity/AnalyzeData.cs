using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace big.entity
{
    [DataContract]
    public class AnalyzeData
    {
        [DataMember]
        public string sid;

        [DataMember]
        public decimal value;

        [DataMember]
        public string name;

        [DataMember]
        public string firstlevel;

        [DataMember]
        public string secondlevel;

        [DataMember]
        public DateTime lastupdate;

        [DataMember]
        public DateTime startdate;

        [DataMember]
        public int rank=-1;

        [DataMember]
        public int level;

        [DataMember]
        public int big;
    }
}
