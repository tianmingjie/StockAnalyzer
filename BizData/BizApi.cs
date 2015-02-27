using big.entity;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace big
{
    enum SQLState
    {
        Init = -1,
        Select = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }
    public class BizApi
    {
        public static int create_table = -1;
        public static int extract_data = -1;
        public static int info = -1;
        public static string BASIC_TABLE = "basictemplate";
        public static string EXTRACT_TABLE_STATUS = "extractstatus";
        public static string CREATE_TABLE_STATUS = "createstatus";

        public static string INFO = "basicinfo";

        //public static DateTime DEFAULT_LASTUPDATE = new DateTime(2014, 1, 1);

        #region 插入更新
        /// <summary>
        /// if table exist,update status, if not exist, create table
        /// </summary>
        /// <param name="sid"></param>
        public static void CreateDataTable(string sid)
        {

            //if (create_table == -1)
            //{
            string sql0 = String.Format("select 1 as Count from {0} where tablename='{1}' ", CREATE_TABLE_STATUS, sid);
            DataSet ds = MySqlHelper.GetDataSet(sql0);
            //create_table = ds.Tables[0].Rows.Count;

            // }

            if (ds.Tables[0].Rows.Count == 0)
            {
                string sql = String.Format("create table {0} like {1}", sid, BASIC_TABLE);
                MySqlHelper.ExecuteNonQuery(sql);
                string sql1 = string.Format("insert {0}(tablename,lastupdate) value('{1}','{2}')", CREATE_TABLE_STATUS, sid, DateTime.Now.ToString());
                MySqlHelper.ExecuteNonQuery(sql1);
                string sq3 = string.Format("insert {0}(sid,lastupdate)values('{1}','{2}')", EXTRACT_TABLE_STATUS, sid, DateTime.MinValue);
                MySqlHelper.ExecuteNonQuery(sq3);
            }
            //if (create_table == 1)
            //{
            //    MySqlHelper.ExecuteNonQuery(string.Format("update {0} set lastupdate='{2}' where tablename='{1}'", CREATE_TABLE_STATUS, newname, DateTime.Now.ToString()));
            //    create_table = 2;
            //}
        }

        //插入分析好的数据
        public static void InsertBasicData(BasicData bd)
        {
            ValidateBasicData(bd);
            string sid = bd.sid;
            //CreateDataTable(sid);
            string sql = String.Format(
                "INSERT INTO {0}(time,c_type,big,buyshare,buymoney,sellshare,sellmoney,totalshare,totalmoney,open,close,high,low)VALUES('{1}','{2}',{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})",
                        sid, bd.time, bd.c_type, bd.big, bd.buyshare, bd.buymoney, bd.sellshare, bd.sellmoney, bd.totalshare, bd.totalmoney,bd.open,bd.close,bd.high,bd.low);
            MySqlHelper.ExecuteNonQuery(sql);
            UpdateExtractStatus(bd);
        }
        #region basicinfor
        public static void InsertInfo(InfoData id)
        {
            if (info == -1)
            {
                DataSet ds = MySqlHelper.GetDataSet(String.Format("select 1 as Count from {0} where sid='{1}' ", INFO, id.sid));
                info = ds.Tables[0].Rows.Count;
            }

            if (info == 0)
            {
                string sql =
                    string.Format("insert {0}(sid,name,lastupdate,totalshare,top10total,floatshare,top10float,list,weight,firstlevel,secondlevel,location,valid) value('{1}','{2}','{3}',{4},{5},{6},{7},'{8}',{9},'{10}','{11}','{12}',{13})", INFO, id.sid, id.name, DateTime.Now.ToString("yyyy-MM-dd"), id.totalshare, id.top10total, id.floatshare, id.top10float, id.list, id.weight, id.firstlevel, id.secondlevel, id.location, id.valid);

                MySqlHelper.ExecuteNonQuery(sql);
                info = 1;
            }
            else
            {
                string sql =
                    string.Format("update {0} set name='{2}',lastupdate={3},totalshare={4},top10total={5},floatshare={6},top10float={7},list='{8}',weight={9},firstlevel='{10}',secondlevel='{11}',location='{12}',valid={13}) where sid='{1}'", INFO, id.sid, id.name, DateTime.Now.ToString("yyyy-MM-dd"), id.totalshare, id.top10total, id.floatshare, id.top10float, id.list, id.weight, id.firstlevel, id.secondlevel, id.location, id.valid);
                MySqlHelper.ExecuteNonQuery(sql);
            }
        }


        #endregion
        //更新数据抽取的时间戳
        public static void UpdateExtractStatus(BasicData bd)
        {
            ValidateBasicData(bd);
            string sid = bd.sid;

            if (extract_data == -1)
            {
                string sql = String.Format("select 1 from {0} where sid='{1}'", EXTRACT_TABLE_STATUS, sid);
                DataSet ds = MySqlHelper.GetDataSet(sql);
                extract_data = ds.Tables[0].Rows.Count;
            }
            if (extract_data == 0)
            {
                string sql = string.Format("insert {0}(sid,lastupdate)values('{1}','{2}')", EXTRACT_TABLE_STATUS, sid, bd.time);
                MySqlHelper.ExecuteNonQuery(sql);
                extract_data = 1;
            }
            else
            {
                string sql = string.Format("update {0} set lastupdate='{2}' where sid='{1}'", EXTRACT_TABLE_STATUS, sid, bd.time);
                MySqlHelper.ExecuteNonQuery(sql);
            }
        }

        public static void ValidateBasicData(BasicData bd)
        {
            if (bd == null) return;
        }
        #endregion
        //bymonth 
        //select sum(sellshare),DATE_FORMAT(time ,'%X %V') as week from test.000830_500 GROUP BY week ORDER BY week
        //by week
        //select sum(sellshare),DATE_FORMAT(time ,'%X %m') as month from test.000830_500 GROUP BY month ORDER BY month

        #region 查询api
        public static IList<BasicData> QueryByMonth(string sid, int big, DateTime start, DateTime end)
        {
            IList<BasicData> list = new List<BasicData>();
            string sql = string.Format("select sum(buyshare) as buyshare,sum(buymoney) as buymoney,sum(sellshare) as sellshare,sum(sellmoney) as sellmoney,sum(totalshare) as totalshare,sum(totalmoney) as totalmoney, DATE_FORMAT(time ,'%X_%m') as tag from {0} where big={1} and time >'{2}' and time<'{3}' GROUP BY tag ORDER BY tag", sid, big, start, end);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];

            return BuildDataList(dt, "m", sid, big);

        }

        public static IList<BasicData> QueryByWeek(string sid, int big, DateTime start, DateTime end)
        {
            IList<BasicData> list = new List<BasicData>();
            string sql = string.Format("select sum(buyshare) as buyshare,sum(buymoney) as buymoney,sum(sellshare) as sellshare,sum(sellmoney) as sellmoney,sum(totalshare) as totalshare,sum(totalmoney) as totalmoney,DATE_FORMAT(time ,'%X_%V') as tag from {0} where big={1} and time >'{2}' and time<'{3}' GROUP BY tag ORDER BY tag", sid, big, start, end);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            return BuildDataList(dt, "w", sid, big);
        }


        public static IList<BasicData> QueryByDay(string sid, int big, DateTime start, DateTime end)
        {
            IList<BasicData> list = new List<BasicData>();
            string sql = string.Format("select sum(buyshare) as buyshare,sum(buymoney) as buymoney,sum(sellshare) as sellshare,sum(sellmoney) as sellmoney,sum(totalshare) as totalshare,sum(totalmoney) as totalmoney,DATE_FORMAT(time ,'%X%m%d') as tag from {0} where big={1} and time >'{2}' and time<'{3}' GROUP BY tag ORDER BY tag", sid, big, start, end);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            return BuildDataList(dt, "d", sid, big);
        }

        private static IList<BasicData> BuildDataList(DataTable dt, string type, string sid, int big)
        {
            IList<BasicData> list = new List<BasicData>();
            double incrementalTotalShare = 0;
            double incrementalBuyShare = 0;
            double incrementalSellShare = 0;
            Decimal incrementalTotalMoney = 0;
            Decimal incrementalBuyMoney = 0;
            Decimal incrementalSellMoney = 0;
            foreach (DataRow dr in dt.Rows)
            {
                incrementalTotalShare += (Double)dr["totalshare"];
                incrementalBuyShare += (Double)dr["buyshare"];
                incrementalSellShare += (Double)dr["sellshare"];
                incrementalTotalMoney += (Decimal)((Double)dr["totalmoney"]);
                incrementalBuyMoney += (Decimal)((Double)dr["buymoney"]);
                incrementalSellMoney += (Decimal)((Double)dr["sellmoney"]);
                BasicData bd = new BasicData()
                {


                    sid = sid,
                    big = big,
                    c_type = type,
                    buyshare = (Double)dr["buyshare"],
                    buymoney = (Decimal)((Double)dr["buymoney"]),
                    sellshare = (Double)dr["sellshare"],
                    sellmoney = (Decimal)((Double)dr["sellmoney"]),
                    totalshare = (Double)dr["totalshare"],
                    totalmoney = (Decimal)((Double)dr["totalmoney"]),
                    tag = (string)dr["tag"],
                    incrementalTotalShare = incrementalTotalShare,
                    incrementalBuyShare = incrementalBuyShare,
                    incrementalSellShare = incrementalSellShare,
                    incrementalTotalMoney = ProcessMoney(incrementalTotalMoney),
                    incrementalBuyMoney = ProcessMoney(incrementalBuyMoney),
                    incrementalSellMoney = ProcessMoney(incrementalSellMoney)

                };

                list.Add(bd);
            }

            return list;
        }

        public static double ProcessMoney(decimal money)
        {
            return Math.Floor((double)money * 100);
            //return (double)money * 100;
        }
        /// <summary>
        /// 查询最后一次插入数据的时间
        /// </summary>
        /// <param name="sid">stock id</param>
        /// <param name="big">big deal</param>
        /// <returns></returns>
        public static DateTime QueryExtractLastUpdate(string sid)
        {
            IList<BasicData> list = new List<BasicData>();
            string sql = string.Format("select lastupdate from {0} where sid='{1}' ", EXTRACT_TABLE_STATUS, sid);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DateTime dt;
            if (ds.Tables[0].Rows.Count > 0)
                dt = DateTime.Parse(ds.Tables[0].Rows[0][0].ToString());
            else
                dt = Constant.DEFAULT_LASTUPDATE;
            return dt;
        }

        public static decimal QueryWeight(string sid)
        {
            decimal weight = 1.0M;
            string sql = string.Format("select weight from {0} where sid='{1}'", INFO, sid);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
                weight = Decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

            return weight;
        }

        public static decimal[] QueryExtractList(string sid)
        {
            string str = "";
            string sql = string.Format("select list from {0} where sid='{1}'", INFO, sid);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                str = ds.Tables[0].Rows[0][0].ToString();
                string[] liststr=str.Split(',');
                decimal[] list=new decimal[liststr.Length];
                for(int i=0;i<liststr.Length;i++)
                    list[i]=decimal.Parse(liststr[i]);
                return list;
            }
            else
            {
                return new decimal[] { 500, 1000, 2000 };
            }

        }

        #endregion

        #region info

        public static List<InfoData> QueryInfoAll()
        {
            InfoData id = new InfoData();
            string sql = string.Format("select sid,name,lastupdate,totalshare,floatshare,location,firstlevel,secondlevel,weight from {0} order by sid", INFO);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            //DataTable dt = ds.Tables[0];
            return BuildInfoData(ds.Tables[0]);
        }

        public static InfoData QueryInfoById(string sid)
        {
            string sql = string.Format("select sid,name,lastupdate,totalshare,floatshare,location,firstlevel,secondlevel,weight from {0} where sid='{1}' ", INFO, sid);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            //DataTable dt = ds.Tables[0];
            return BuildInfoData(ds.Tables[0])[0];
        }

        public static List<InfoData> QueryInfoByIndustry(string insutry)
        {
            string sql = string.Format("select sid,name,lastupdate,totalshare,floatshare,location,firstlevel,secondlevel,weight from {0} where firstlevel='{1}' ", INFO, insutry);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            return BuildInfoData(ds.Tables[0]);
        }

        public static List<InfoData> QueryInfoByIndustry2(string insutry,string industry2)
        {
            string sql = string.Format("select sid,name,lastupdate,totalshare,floatshare,location,firstlevel,secondlevel,weight from {0} where firstlevel='{1}' and secondlevel='{2}' ", INFO, insutry,industry2);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            return BuildInfoData(ds.Tables[0]);
        }

        public static List<InfoData> QueryInfoByLocation(string location)
        {
            string sql = string.Format("select sid,name,lastupdate,totalshare,floatshare,location,firstlevel,secondlevel,weight from {0} where location='{1}' ", INFO, location);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            //DataTable dt = ds.Tables[0];
            return BuildInfoData(ds.Tables[0]);
        }

        public static List<InfoData> BuildInfoData(DataTable dt)
        {
            List<InfoData> list = new List<InfoData>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    InfoData id = new InfoData();
                    id.sid = dr["sid"].ToString();
                    id.name = dr["name"].ToString();
                    id.lastupdate = DateTime.Parse(dr["lastupdate"].ToString());
                    id.totalshare = Double.Parse(dr["totalshare"].ToString());
                    id.floatshare = Double.Parse(dr["floatshare"].ToString());
                    id.location = dr["location"].ToString();
                    id.firstlevel = dr["firstlevel"].ToString(); ;
                    id.secondlevel = dr["secondlevel"].ToString();
                    id.weight = Decimal.Parse(dr["weight"].ToString());
                    list.Add(id);
                }
            }

            return list;
        }
        #endregion
    }
}
