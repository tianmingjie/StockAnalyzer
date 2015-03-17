using big;
using big.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using BizCommon = big.BizCommon;

namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Stock" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Stock.svc or Stock.svc.cs at the Solution Explorer and start debugging.
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class StockResource
    {
        [WebGet(UriTemplate = "test", ResponseFormat = WebMessageFormat.Json)]
        public string version()
        {
            return "1.0";
        }


        [WebGet(UriTemplate = "query/id/{id}?big={big}&type={type}&start={start}&end={end}", ResponseFormat = WebMessageFormat.Json)]
        public IList<BasicData> QueryData(string id, string big, string type, string start, string end)
        {
            DateTime endDate, startDate;
            IList<BasicData> list;
            if (string.IsNullOrEmpty(end)) endDate = DateTime.Now; else endDate = BizCommon.ParseToDate(end);
            if (string.IsNullOrEmpty(big)) big = "1000";
            if (string.IsNullOrEmpty(type)) type = "w";
            if (string.IsNullOrEmpty(start)) startDate = new DateTime(2014, 1, 1); else startDate = BizCommon.ParseToDate(start);

            switch (type)
            {
                case "m":
                    list = BizApi.QueryByMonth(id, Int32.Parse(big), startDate, endDate);
                    break;
                case "d":
                    list = BizApi.QueryByDay(id, Int32.Parse(big), startDate, endDate);
                    break;
                default:
                    list = BizApi.QueryByWeek(id, Int32.Parse(big), startDate, endDate);
                    break;
            }

            return list;
        }

        [WebGet(UriTemplate = "line/id/{id}?type={type}&start={start}&end={end}", ResponseFormat = WebMessageFormat.Json)]
        public IList<LineData> QueryLineData(string id, string type, string start, string end)
        {
            DateTime endDate, startDate;
            IList<LineData> list;
            if (string.IsNullOrEmpty(end)) endDate = DateTime.Now; else endDate = BizCommon.ParseToDate(end);
            if (string.IsNullOrEmpty(type)) type = "w";
            if (string.IsNullOrEmpty(start)) startDate = new DateTime(2014, 1, 1); else startDate = BizCommon.ParseToDate(start);

            switch (type)
            {
                case "m":
                    list = BizApi.QueryLineByMonth(id, startDate, endDate);
                    break;
                case "d":
                    list = BizApi.QueryLineByDay(id, startDate, endDate);
                    break;
                default:
                    list = BizApi.QueryLineByWeek(id, startDate, endDate);
                    break;
            }

            return list;
        }

        [WebGet(UriTemplate = "infoext/id/{id}", ResponseFormat = WebMessageFormat.Json)]
        public InfoExtData QueryInfoExtById(string id)
        {
            return BizApi.QueryInfoExtById(id);
        }

        [WebGet(UriTemplate = "info/id/{id}", ResponseFormat = WebMessageFormat.Json)]
        public InfoData QueryInfoById(string id)
        {
            return BizApi.QueryInfoById(id);
        }

        [WebInvoke(UriTemplate = "info/all?type={type}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebGet(UriTemplate = "info/industry", ResponseFormat = WebMessageFormat.Json)]
        public string[] QueryInfoByAllIndustry(string type)
        {
            if (type.Equals("L"))
            {
                return BizApi.QueryAllLocation();
            }
            else
            {
                return BizApi.QueryAllIndustry();
            }

        }

        [WebGet(UriTemplate = "info/industry1/{industry1}", ResponseFormat = WebMessageFormat.Json)]
        public List<InfoData> QueryInfoByIndutry(string industry1)
        {
            return BizApi.QueryInfoByIndustry(industry1);
        }

        [WebGet(UriTemplate = "info/industry1/{industry1}/industry2/{industry2}", ResponseFormat = WebMessageFormat.Json)]
        public List<InfoData> QueryInfoByIndutry2(string industry1, string industry2)
        {
            return BizApi.QueryInfoByIndustry2(industry1, industry2);
        }

        [WebGet(UriTemplate = "info/location/{location}", ResponseFormat = WebMessageFormat.Json)]
        public List<InfoData> QueryInfoByLocation(string location)
        {
            return BizApi.QueryInfoByLocation(location);
        }


        [WebGet(UriTemplate = "analyze?level={level}&tag={tag}&old={old}", ResponseFormat = WebMessageFormat.Json)]
        public List<AnalyzeData> QueryAnalyze(string level,string tag, string old)
        {
            int level_val = 1;
            DateTime now = DateTime.Now;
            int o = -12;
            if (string.IsNullOrEmpty(tag)) tag = BizCommon.ParseToString(now);
            if (!string.IsNullOrEmpty(level)) level_val = Int32.Parse(level);
            if (!string.IsNullOrEmpty(old)) o = -Int32.Parse(old);

            DateTime end_date = now.AddMonths(-1);
            DateTime start_date = end_date.AddMonths(o);


            return BizApi.QueryAnalyzeData(tag, start_date,end_date, level_val);
        }


        [WebGet(UriTemplate = "statistics?level={level}&tag={tag}&type={type}", ResponseFormat = WebMessageFormat.Json)]
        public List<AnalyzeData> QueryAnalyzeStatistics(string level, string tag, string type)
        {
            int level_val = 1;
            //DateTime end_date = DateTime.Now;
            if (!string.IsNullOrEmpty(level)) level_val = Int32.Parse(level);
            if (!string.IsNullOrEmpty(tag)) tag = BizCommon.ParseToString(DateTime.Now);

            if (string.IsNullOrEmpty(type))
                return BizApi.QueryAnalyzeStatisticsByName(tag, level_val);
            if (type.Equals("name"))
                return BizApi.QueryAnalyzeStatisticsByName(tag, level_val);
            if (type.Equals("industry"))
                return BizApi.QueryAnalyzeStatisticsByIndustry(tag, level_val);
            return null;
        }

        //[WebGet(UriTemplate = "analyze/date/{date}", ResponseFormat = WebMessageFormat.Json)]
        //public List<AnalyzeData> QueryAnalyze(string date)
        //{
        //        return BizApi.QueryAnalyzeData(Common.ParseToDate(date), 1);
        //}

        [WebGet(UriTemplate = "lastupdate/id/{id}", ResponseFormat = WebMessageFormat.Json)]
        public string QueryLastUpdate(string id)
        {
            return BizCommon.ParseToString(BizApi.QueryExtractLastUpdate(id));
        }


    }
}
