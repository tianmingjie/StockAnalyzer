﻿using big;
using big.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

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
            if (string.IsNullOrEmpty(end)) endDate = DateTime.Now; else endDate = Common.ParseToDate(end);
            if (string.IsNullOrEmpty(big)) big = "1000";
            if (string.IsNullOrEmpty(type)) type = "w";
            if (string.IsNullOrEmpty(start)) startDate = new DateTime(2014, 1, 1); else startDate = Common.ParseToDate(start);

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
            if (string.IsNullOrEmpty(end)) endDate = DateTime.Now; else endDate = Common.ParseToDate(end);
            if (string.IsNullOrEmpty(type)) type = "w";
            if (string.IsNullOrEmpty(start)) startDate = new DateTime(2014, 1, 1); else startDate = Common.ParseToDate(start);

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


        [WebGet(UriTemplate = "analyze/date/{date}?level={level}", ResponseFormat = WebMessageFormat.Json)]
        public List<AnalyzeData> QueryAnalyze(string date,string level)
        {
            if(string.IsNullOrEmpty(level)) 
                return BizApi.QueryAnalyzeData(Common.ParseToDate(date),1);
            else  
                return BizApi.QueryAnalyzeData(Common.ParseToDate(date),Int32.Parse(level));
        }


        [WebGet(UriTemplate = "lastupdate/id/{id}", ResponseFormat = WebMessageFormat.Json)]
        public string QueryLastUpdate(string id)
        {
            return Common.ParseToString(BizApi.QueryExtractLastUpdate(id));
        }


    }
}
