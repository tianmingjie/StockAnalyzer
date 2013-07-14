using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.data;

namespace SotckAnalyzer.analyzer
{
    /// <summary>
    /// Print the data, save to data to csv
    /// </summary>
    interface ISave
    {
         bool Save(StockData data, bool SaveToFile = true);
    }
}
