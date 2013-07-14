using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SotckAnalyzer.data;

namespace SotckAnalyzer.analyzer
{
    interface IAnalyzer
    {
         StockData Analyze(StockData data);
    }
}
