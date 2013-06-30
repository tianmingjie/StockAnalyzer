using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;

namespace SotckAnalyzer
{
    public static class Constant
    {
        public static string ROOT_FOLDER = ConfigurationManager.AppSettings["Constant.ROOT_FOLDER"];

    }
}
