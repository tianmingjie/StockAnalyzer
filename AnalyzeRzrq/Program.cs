using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using big;
using big.entity;

namespace AnalyzeRzrq
{
    public class Program
    {

        public static int duration = 30;

        //上升过程涨幅２０％
        public static decimal shangzhang_zhangfu = 0.2M;

        //最高最低的涨幅３％
        public static decimal adjust = 0.02M;

        //下跌缩量６０％
        public static decimal xiedie_suoliang = 0.6M;

        //下跌的时候，最低比最高跌幅
        public static decimal xiadie_diefu = 0.05M;

        //下跌调整天数
        public static int xiadie_tiaozhengtianshu = 10;

        public static DateTime start = new DateTime(2015, 02, 03);
        public static DateTime end = new DateTime(2015, 04, 03);

        public static void Main(string[] args)
        {

            List<InfoData> list = BizApi.QueryInfoRzrq();
            //List<InfoData> list = new List<InfoData>() { BizApi.QueryInfoById("sh600157") };
            foreach (InfoData id in list)
            {
                //Console.WriteLine(id.sid + " processed");
                try
                {
                    if (Analyze(id.sid, start, end))
                    {
                        Console.WriteLine(id.sid + " selected!!!");
                    }
                }
                catch
                {
                    //Console.WriteLine(id.sid + " failed");
                    //throw;
                }
            }

        }

        public static bool Analyze(string sid, DateTime start, DateTime end)
        {

            List<LineData> list = BizApi.QueryLineByDay(sid, start, end);
            //停盘
            if (list.Count == 0)
            {
                //Console.WriteLine(sid + " close");
                return false;
            }
            int peekindexclose = 0;
            LineData peek = GetClosePeek(list, out peekindexclose);
            int valleyindexclose = 0;
            LineData vally = GetCloseValley(list, out valleyindexclose);
            int valleyindexlow2 = 0;
            LineData vally2 = GetLowValley2(list, peekindexclose, out valleyindexlow2);

            //如果时间太短，调整不够
            //if (peekindexclose < valleyindexclose || peekindexclose > valleyindexlow2) return false;
            //if (peekindexclose - valleyindexclose < 3 || valleyindexlow2 - peekindexclose < 3) return false;

            bool shangzhang = judgeShangzhang(list, peekindexclose, valleyindexclose);

            bool xiadie = judgeXiajiang(list, peekindexclose, valleyindexlow2);

            bool liangneng = judgeSuoliang(list, peekindexclose, valleyindexclose);
            Console.WriteLine("上涨，下跌，缩量"+shangzhang+","+xiadie+","+ liangneng);
            return shangzhang && xiadie && liangneng;
        }
        /// <summary>
        /// 下跌过程
        /// </summary>
        /// <param name="list"></param>
        /// <param name="peekindexclose"></param>
        /// <param name="valleyindexlow2"></param>
        /// <returns></returns>
        public static bool judgeXiajiang(List<LineData> list, int peekindexclose, int valleyindexlow2)
        {
            LineData peek = list[peekindexclose];
            LineData valley2 = list[valleyindexlow2];

            //阳线
            int b1 = (valley2.close > valley2.open) ? 1 : 0;

            //收盘3%的涨幅
            int b2 = ((decimal)((valley2.close - list[valleyindexlow2 - 1].close) / list[valleyindexlow2 - 1].close)) > adjust ? 1 : 0;

            //最低点比最高点10%的跌幅
            int b3 = ((decimal)(valley2.close - peek.close) / peek.close) < -xiadie_diefu ? 1 : 0;

            //调整小于10天
            int b4 = (valleyindexlow2 - peekindexclose) <=xiadie_tiaozhengtianshu ? 1 : 0;

            //最低点比前一天还低
            //int b5 = valley2.low < list[valleyindexlow2 - 1].low ? 1 : 0;

            return (b1 + b2 + b3 + b4 ) > 3;
        }

        /// <summary>
        /// 判断量能
        /// </summary>
        /// <param name="list"></param>
        /// <param name="valleyindexclose"></param>
        /// <param name="peekindexclose"></param>
        /// <param name="valleyindexclose2"></param>
        /// <returns></returns>
        public static bool judgeSuoliang(List<LineData> list, int peekindexclose, int valleyindexclose)
        {
            //下跌的时候，缩量50%

            //TODO:如果涨停，缩量，需要去掉
            double totalshare_peek = 0;
            double totalshare_valley = 0;

            double avg_share_valley = 0;
            double avg_share_peek = 0;
            int index_valley = 0;
            for (int i = peekindexclose + 1; i < list.Count; i++)
            {
                totalshare_valley += list[i].totalshare;

                index_valley++;
            }
            avg_share_valley = totalshare_valley / index_valley;


            int index_peek = 0;
            for (int i = 0; i <= peekindexclose; i++)
            {
                if ((decimal)((list[i].close - list[i - 1].close) / list[i - 1].close) > 0.098M)
                {
                    if (list[i].totalshare <= avg_share_valley) continue;
                }
                totalshare_peek += list[i].totalshare;
                index_peek++;
            }
            avg_share_peek = totalshare_peek / index_peek;

            return ((decimal)(avg_share_valley / avg_share_peek)) < xiedie_suoliang;
        }

        /// <summary>
        /// 上涨过程
        /// </summary>
        /// <param name="list"></param>
        /// <param name="peekindexclose"></param>
        /// <param name="vallyindexclose"></param>
        /// <returns></returns>
        public static bool judgeShangzhang(List<LineData> list, int peekindexclose, int vallyindexclose)
        {
            LineData peek = list[peekindexclose];
            LineData valley = list[vallyindexclose];

            //收盘价阳线
            int b1 = (peek.close > peek.open) && (peek.close == peek.high) ? 1 : 0; ;

            int b2 = ((decimal)(peek.close - list[peekindexclose - 1].close) / list[peekindexclose - 1].close) > adjust ? 1 : 0;

            //最高价比最低价过20%
            int b3 = ((decimal)((peek.close - valley.close) / valley.close)) > shangzhang_zhangfu ? 1 : 0;

            //有涨停
            bool bb = false;
            for (int i = 1; i <= peekindexclose; i++)
            {
                if ((decimal)((list[i].close - list[i - 1].close) / list[i - 1].close) > 0.098M)
                    bb = true;
            }

            int b4 = bb ? 1 : 0;
            return (b1 + b2 + b3 + b4) > 3;
        }

        /// <summary>
        /// 获得第二次底部
        /// </summary>
        /// <param name="list"></param>
        /// <param name="peek"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static LineData GetCloseValley2(List<LineData> list, int peek, out int index)
        {
            decimal price = list[peek].close;
            index = 0;
            for (int i = peek; i < list.Count; i++)
            {
                if (price > list[i].close)
                {
                    price = list[i].close;
                    index = i;
                }
            }

            return list[index];

        }

        //根据最低价来判断第二次底部
        public static LineData GetLowValley2(List<LineData> list, int peek, out int index)
        {
            decimal price = list[peek].close;
            index = 0;
            for (int i = peek; i < list.Count; i++)
            {
                if (price > list[i].low)
                {
                    price = list[i].low;
                    index = i;
                }
            }

            return list[index];

        }

        //根据收盘价来判断顶部
        public static LineData GetClosePeek(List<LineData> list, out int index)
        {
            //LineData ld = new LineData() ;

            decimal price = 0;
            index = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (price < list[i].close)
                {
                    price = list[i].close;
                    index = i;
                }
            }

            return list[index];
        }

        //根据最高价来判断顶部
        public static LineData GetHighPeek(List<LineData> list, out int index)
        {
            //LineData ld = new LineData() ;

            decimal price = 0;
            index = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (price < list[i].high)
                {
                    price = list[i].high;
                    index = i;
                }
            }

            return list[index];
        }

        //根据收盘价来判断底部
        public static LineData GetCloseValley(List<LineData> list, out int index)
        {
            //LineData ld = new LineData() ;

            decimal price = 99999M;
            index = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (price > list[i].close)
                {
                    price = list[i].close;
                    index = i;
                }
            }
            return list[index];
        }

        //根据最低价来判断底部
        public static LineData GetLowValley(List<LineData> list, out int index)
        {
            //LineData ld = new LineData() ;

            decimal price = 99999M;
            index = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (price > list[i].low)
                {
                    price = list[i].low;
                    index = i;
                }
            }
            return list[index];
        }
    }
}
