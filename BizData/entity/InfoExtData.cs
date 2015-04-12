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
    public class InfoExtData
    {
        [DataMember]
        public string sid;
        [DataMember]
        public string lastupdate;

        [DataMember]
        public decimal shouyi;
        [DataMember]
        public decimal shiyinglv;
        [DataMember]
        public decimal jingzichan;
        [DataMember]
        public decimal shijinglv;
        [DataMember]
        public decimal shouru;
        [DataMember]
        public decimal shourutongbi;
        [DataMember]
        public decimal jinglirun;
        [DataMember]
        public decimal jingliruntongbi;
        [DataMember]
        public decimal maolilv;
        [DataMember]
        public decimal jinglilv;
        [DataMember]
        public decimal ROE;
        [DataMember]
        public decimal fuzhailv;
        [DataMember] //亿
        public decimal zongguben;
        [DataMember]
        public decimal liutonggu;
        [DataMember]
        public decimal zongzhi;
        [DataMember]
        public decimal liuzhi;
        [DataMember]
        public decimal meiguweifenpeilirun;
        [DataMember]
        public string shangshishijian;


    }
}
