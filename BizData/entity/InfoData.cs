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
    public class InfoData
    {
        
        //public int id;
        [DataMember]
        public string sid;
        [DataMember]
        public string name;
        [DataMember]
        public DateTime lastupdate;
        [DataMember]
        public double totalshare;
        [DataMember]
        public double floatshare;
        [DataMember]
        public decimal top10total;
        [DataMember]
        public decimal top10float;
        [DataMember]
        public string list;
        [DataMember]
        public decimal weight;
        [DataMember]
        public string firstlevel;
        [DataMember]
        public string secondlevel;
        [DataMember]
        public string location;
        [DataMember]
        public int valid;

    }
}
