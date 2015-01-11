using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.type;

namespace Report
{
    public class ReportModule:IStockModule
    {
        public bool Execute(StockInfo info)
        {
            foreach (string filter in info.filterList)
            {
                foreach (int type in Enum.GetValues(typeof(RangeType)))
                {

                    //String filePath = string.Format(@"{0}{1}\{1}_{2}_{3}_{4}_{5}.csv", Constant.ANALYZE_FOLDER, stock, startDate, endDate, filter, (RangeType)type);
                    String filePath = string.Format(@"{0}{1}\{1}_{3}_{2}.csv", Constant.ANALYZE_FOLDER, info.stock, (RangeType)type, filter);
                    Program.ReportExcelToImage(filePath, new String[] { "BuyShare", "SellShare" }, "BuySellShare", ChartType.COLUMN);
                    Program.ReportExcelToImage(filePath, new String[] { "increDiffMoney" }, "increDiffMoney", ChartType.LINE);
                }
            }
            return true;
        }
    }
}
