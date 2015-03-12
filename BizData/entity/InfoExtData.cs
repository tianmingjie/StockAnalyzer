using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace big.entity
{
    [DataContract]
    public class InfoExtData
    {
        [DataMember]
        public string sid;
        [DataMember]
        public string lastupdate;

        [DataMember]
        public decimal zongguben;
        [DataMember]
        public decimal liutonggu;
        [DataMember]
        public decimal yingyeshouruzengzhanglv;
        [DataMember]
        public decimal yingyeshouru;
        [DataMember]
        public decimal jinglirun;
        [DataMember]
        public decimal jinglirunzengzhanglv;
        [DataMember]
        public decimal meigushouyi;
        [DataMember]
        public decimal meigujingzichan;
        [DataMember]
        public decimal jingzichanshouyilv;
        [DataMember]
        public decimal meiguxianjinliu;
        [DataMember]
        public decimal meigugongjijin;
        [DataMember]
        public decimal meiguweifenpeilirun;
        [DataMember]
        public decimal shiyinglv;
        [DataMember]
        public decimal shijinglv;
    }
}
