using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using big;
using big.entity;
using Common;

namespace Analyze
{
    public class GenerateHtml
    {
        public static void GenerateAll(string path,string tag)
        {
            tag = "20150415";
            path = @"G:\github\StockAnalyzer\web\";
            string temp=path+@"temp\";
            if(!Directory.Exists(temp)) Directory.CreateDirectory(temp);
            string[] old_list = Constant.ANALYZE_TIME.Split('-');
            string[] type_list = {"CYB","ZXB","ZB","XPG" };
            string[] industry_list = BizApi.QueryAllIndustry();
            string[] location_list = BizApi.QueryAllLocation();

            foreach (string old in old_list)
            {
                foreach (string type in type_list)
                {
                    string haha = Generate(Constant.ANALYZE_LEVEL.ToString(), tag, old, Constant.DAYS_BEFORE.ToString(), "", "", type);
                    string template = Common.FileUtil.ReadFile(path + "analyzetemplate.html");
                    template = template.Replace("PLACEHOLDER", haha);
                    string filename = string.Format("analyze_{0}_{1}.html", old, type);
                    Common.FileUtil.WriteFile(temp + filename, template);
                    Console.WriteLine();
                }

                foreach (string industry in industry_list)
                {
                    string haha = Generate(Constant.ANALYZE_LEVEL.ToString(), tag, old, Constant.DAYS_BEFORE.ToString(), industry, "", "");
                    string template = Common.FileUtil.ReadFile(path + "analyzetemplate.html");
                    template = template.Replace("PLACEHOLDER", haha);
                    string filename = string.Format("analyze_{0}_{1}.html", old, industry);
                    Common.FileUtil.WriteFile(temp + filename, template);
                    Console.WriteLine();
                }

                foreach (string location in location_list)
                {
                    string haha = Generate(Constant.ANALYZE_LEVEL.ToString(), tag, old, Constant.DAYS_BEFORE.ToString(), "", location, "");
                    string template = Common.FileUtil.ReadFile(path + "analyzetemplate.html");
                    template = template.Replace("PLACEHOLDER", haha);
                    string filename = string.Format("analyze_{0}_{1}.html", old, location);
                    Common.FileUtil.WriteFile(temp + filename, template);
                    Console.WriteLine();
                }
            }

        }

       public static string Generate(string level, string tag, string old, string daybefore, string industry, string location, string type)
        {
            DateTime now = BizCommon.ParseToDate(tag);
            DateTime end_date = now.AddDays((double)(-Int32.Parse(daybefore)));
            DateTime start_date = end_date.AddMonths(-Int32.Parse(old));

            List<AnalyzeData> data = BizApi.QueryAnalyzeData(tag, start_date, end_date, Int32.Parse(level), industry, location, type);

            var newRow="";
            newRow += "Start:" + BizCommon.ParseToString(start_date) + "  End:" + BizCommon.ParseToString(end_date);

            for(int i=0;i<data.Count;i++){

                   newRow += "<tr>";
                    var big = data[i].big;
                    var sid = data[i].sid;


                    var link = "<a target='_parent' href='/web/single_dynamic.html?sid=" + sid + "&big=" + big + "'>" + sid + "</a>";
                    newRow += "<td>" + link + "</td>";
                    newRow += "<td>" + data[i].name + "</td>";

                    var ind1 = data[i].firstlevel;
                    var ind2 = data[i].secondlevel;

                    var ind_link1 = "<a target='_parent' href='/web/company.html?industry1=" + ind1 + "'>" + ind1 + "</a>";
                    newRow += "<td>" + ind_link1 + "</td>";
                    var ind_link2 = "<a target='_parent' href='/web/company.html?industry1=" + ind1 + "&industry2=" + ind2 + "'>" + ind2 + "</a>";
                    newRow += "<td>" + ind_link2 + "</td>";

                    var enddate = data[i].enddate;
                    var startdate = data[i].startdate;

                    

                    //newRow += "<td>" + start_date + "</td>";
                    //newRow += "<td>" + end_date + "</td>";
                    
                //排名情况
                    var y = "/rest/rest/analyzevalue?level=" + level + "&tag=" + tag + "&sid=" + sid + "&daybefore=" + daybefore;

                    newRow += "<td>" + QueryAnalyze1(sid,level,tag,old,daybefore)+ "</td>";
                    //newRow += "<td>" + data[i].rank + "</td>";
                    newRow += "<td>" + data[i].value + "</td>";
                    
                //财务信息

                    InfoExtData data1=BizApi.QueryInfoExtById(sid);

                        newRow += "<td>" + data1.shiyinglv + "</td>";
                        newRow += "<td>" + data1.shijinglv + "</td>";
                        newRow += "<td>" + data1.jingzichan + "</td>";
                        newRow += "<td>" + data1.shourutongbi + "</td>";
                        newRow += "<td>" + data1.jingliruntongbi + "</td>";
                        newRow += "<td>" + data1.meiguweifenpeilirun + "</td>";
                        newRow += "<td>" + data1.zongguben + "</td>";
                        newRow += "<td>" + data1.liutonggu + "</td>";

                //最新价格
                        newRow += "<td>" + BizApi.QueryLatestPrice(sid,tag) + "</td>";

                //价格范围
                        newRow += "<td>" + BizApi.QueryMaxMinPriceByRange(sid,12) + "</td>";

                }

            return newRow;
        }

         public static string QueryAnalyze1(string sid,string level, string tag, string old,string daybefore)
        {
            tag = BizCommon.ProcessWeekend(tag);
            int level_val = 1;
            DateTime now = DateTime.Now;
            int i_daybeofre = 0;
            if (string.IsNullOrEmpty(tag)) tag = BizCommon.ParseToString(now); else now = BizCommon.ParseToDate(tag);
            if (string.IsNullOrEmpty(daybefore)) i_daybeofre =0; else i_daybeofre = int.Parse(daybefore);
            if (!string.IsNullOrEmpty(level)) level_val = Int32.Parse(level);
            if (string.IsNullOrEmpty(old)) old = Constant.ANALYZE_TIME;

            string vv = "";
            string[] list = old.Split('-');
            foreach (string v in list)
            {

                int o =-Int32.Parse(v);


                DateTime end_date = now.AddDays(-i_daybeofre);
                DateTime start_date = end_date.AddMonths(o);


                vv+= BizApi.QueryAnalyzeDataValue(sid, tag, start_date, end_date, level_val)+",";
            }

            return vv.Substring(0, vv.Length - 1); ;
        }
    }
}
